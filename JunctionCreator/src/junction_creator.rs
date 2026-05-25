use crate::{DLL_REF_COUNT, to_utf16_null_terminated};
use std::ffi::{OsString, c_void};
use std::os::windows::ffi::OsStrExt;
use std::path::PathBuf;
use std::ptr::{copy_nonoverlapping, null_mut};
use std::str::FromStr;
use std::sync::Mutex;
use std::sync::atomic::Ordering;
use windows::Wdk::Storage::FileSystem::REPARSE_DATA_BUFFER;
use windows::Win32::Foundation::*;
use windows::Win32::Globalization::lstrcmpiW;
use windows::Win32::Storage::FileSystem::*;
use windows::Win32::System::Com::*;
use windows::Win32::System::IO::DeviceIoControl;
use windows::Win32::System::Ioctl::FSCTL_SET_REPARSE_POINT;
use windows::Win32::System::Ole::{CF_HDROP, ReleaseStgMedium};
use windows::Win32::System::Registry::HKEY;
use windows::Win32::System::SystemServices::IO_REPARSE_TAG_MOUNT_POINT;
use windows::Win32::UI::Shell::Common::ITEMIDLIST;
use windows::Win32::UI::Shell::*;
use windows::Win32::UI::WindowsAndMessaging::{HMENU, InsertMenuW, MF_BYPOSITION, MF_STRING};
use windows::core::*;

#[implement(IShellExtInit, IContextMenu)]
pub(crate) struct JunctionCreator {
    from: Mutex<PathBuf>,
    to: Mutex<PathBuf>,
}

impl JunctionCreator {
    pub(crate) fn new() -> Self {
        Self {
            from: Mutex::new(PathBuf::new()),
            to: Mutex::new(PathBuf::new()),
        }
    }
}

impl Drop for JunctionCreator {
    fn drop(&mut self) {
        DLL_REF_COUNT.fetch_sub(1, Ordering::Relaxed);
    }
}

impl IShellExtInit_Impl for JunctionCreator_Impl {
    fn Initialize(&self, pidlfolder: *const ITEMIDLIST, pdtobj: Ref<IDataObject>, _hkeyprogid: HKEY) -> Result<()> {
        let temp_from: String;
        let temp_to: String;

        // to
        unsafe {
            let mut buffer = [0u16; 260];

            if !SHGetPathFromIDListW(pidlfolder, &mut buffer).as_bool() {
                return Err(E_FAIL.into());
            }

            let mut buffer2 = buffer;
            let hr = PathCchStripToRoot(PWSTR(buffer2.as_mut_ptr()), 260);

            if hr != S_OK && hr != S_FALSE {
                return Err(hr.into());
            }

            let mut buffer3 = [0u16; 100];

            GetVolumeInformationW(PCWSTR(buffer2.as_ptr()), None, None, None, None, Some(buffer3.as_mut_slice()))?;

            let ntfs = lstrcmpiW(PCWSTR(buffer3.as_ptr()), w!("NTFS"));
            let refs = lstrcmpiW(PCWSTR(buffer3.as_ptr()), w!("ReFS"));

            if ntfs != 0 && refs != 0 {
                return Err(E_FAIL.into());
            }

            let len = buffer.iter().position(|&p| p == 0).unwrap_or(buffer.len());
            temp_to = String::from_utf16(&buffer[..len])?;
        }

        // from
        unsafe {
            let pdtobj = pdtobj.ok()?;

            let formatetc = FORMATETC {
                cfFormat: CF_HDROP.0,
                ptd: null_mut(),
                dwAspect: DVASPECT_CONTENT.0,
                lindex: -1,
                tymed: TYMED_HGLOBAL.0 as u32,
            };

            let stg = StgMediumGuard(pdtobj.GetData(&formatetc)?);
            let hdrop = HDROP(stg.0.u.hGlobal.0);
            let file_count = DragQueryFileW(hdrop, u32::MAX, None);

            if file_count != 1 {
                return Err(E_FAIL.into());
            }

            let filename_length = DragQueryFileW(hdrop, 0, None);

            if filename_length == 0 {
                return Err(E_FAIL.into());
            }

            let mut buffer = vec![0u16; (filename_length + 1) as usize];
            let path_length = DragQueryFileW(hdrop, 0, Some(&mut buffer));

            if path_length == 0 {
                return Err(E_FAIL.into());
            }

            if !PathIsDirectoryW(PCWSTR(buffer.as_ptr())).as_bool() {
                return Err(E_INVALIDARG.into());
            }

            temp_from = String::from_utf16(&buffer[..path_length as usize])?;
        }

        let fromPath = PathBuf::from_str(&temp_from).map_err(|_| E_FAIL)?;
        let toPath = PathBuf::from_str(&temp_to).map_err(|_| E_FAIL)?;

        if fromPath.parent().unwrap_or(&fromPath) == toPath {
            return Err(E_INVALIDARG.into());
        }

        if let Ok(mut from) = self.from.lock() {
            *from = temp_from.into();
        } else {
            return Err(E_FAIL.into());
        }

        if let Ok(mut to) = self.to.lock() {
            *to = temp_to.into();
        } else {
            return Err(E_FAIL.into());
        }

        Ok(())
    }
}

impl IContextMenu_Impl for JunctionCreator_Impl {
    fn QueryContextMenu(&self, hmenu: HMENU, indexmenu: u32, idcmdfirst: u32, _idcmdlast: u32, uflags: u32) -> HRESULT {
        if (uflags & CMF_DEFAULTONLY) == 1 {
            return HRESULT(0);
        }

        unsafe {
            if let Err(e) = InsertMenuW(
                hmenu,
                indexmenu,
                MF_STRING | MF_BYPOSITION,
                idcmdfirst as usize,
                w!("Create Directory Junction"),
            ) {
                return e.code();
            }
        }

        HRESULT(1)
    }

    fn InvokeCommand(&self, pici: *const CMINVOKECOMMANDINFO) -> Result<()> {
        unsafe {
            if !(*pici).lpVerb.0.is_null() {
                return Err(E_INVALIDARG.into());
            }
        }

        if let Ok(from) = self.from.lock()
            && let Ok(to) = self.to.lock()
        {
            let mut to = to.clone();

            to.push(from.file_name().ok_or(E_FAIL)?);

            let to_vec = to_utf16_null_terminated(to.to_str().ok_or(E_FAIL)?);

            unsafe {
                CreateDirectoryW(PCWSTR(to_vec.as_ptr()), None)?;

                let handle = SafeHandle(CreateFileW(
                    PCWSTR(to_vec.as_ptr()),
                    GENERIC_WRITE.0,
                    FILE_SHARE_NONE,
                    None,
                    OPEN_EXISTING,
                    FILE_FLAG_OPEN_REPARSE_POINT | FILE_FLAG_BACKUP_SEMANTICS,
                    None,
                )?);

                let mut substitute_string = OsString::from("\\??\\");

                substitute_string.push(from.as_os_str());

                let mut substitute_name: Vec<u16> = substitute_string.encode_wide().collect();

                substitute_name.push(0);

                let mut print_name: Vec<u16> = from.as_os_str().encode_wide().collect();

                print_name.push(0);

                let sub_len_bytes = ((substitute_name.len() - 1) * size_of::<u16>()) as u16;
                let print_len_bytes = ((print_name.len() - 1) * size_of::<u16>()) as u16;
                let sub_offset = 0u16;
                let print_offset = (substitute_name.len() * size_of::<u16>()) as u16;
                let mount_point_header_size = 16usize;
                let path_buffer_bytes = (substitute_name.len() + print_name.len()) * size_of::<u16>();
                let total_buffer_size = mount_point_header_size + path_buffer_bytes;
                let mut buffer = vec![0u8; total_buffer_size];
                let reparse_ptr = buffer.as_mut_ptr() as *mut REPARSE_DATA_BUFFER;
                (*reparse_ptr).ReparseTag = IO_REPARSE_TAG_MOUNT_POINT;
                (*reparse_ptr).ReparseDataLength = (total_buffer_size - 8) as u16;
                (*reparse_ptr).Reserved = 0;
                let mount_point = &mut (*reparse_ptr).Anonymous.MountPointReparseBuffer;
                mount_point.SubstituteNameOffset = sub_offset;
                mount_point.SubstituteNameLength = sub_len_bytes;
                mount_point.PrintNameOffset = print_offset;
                mount_point.PrintNameLength = print_len_bytes;
                let path_buffer_start = mount_point.PathBuffer.as_mut_ptr();

                copy_nonoverlapping(
                    substitute_name.as_ptr(),
                    path_buffer_start.offset((sub_offset / 2) as isize),
                    substitute_name.len(),
                );

                copy_nonoverlapping(
                    print_name.as_ptr(),
                    path_buffer_start.offset((print_offset / 2) as isize),
                    print_name.len(),
                );

                let mut br = 0;

                DeviceIoControl(
                    (&handle).into(),
                    FSCTL_SET_REPARSE_POINT,
                    Some(buffer.as_ptr() as *const c_void),
                    total_buffer_size as u32,
                    None,
                    0,
                    Some(&mut br),
                    None,
                )?;
            }
        } else {
            return Err(E_FAIL.into());
        }

        Ok(())
    }

    fn GetCommandString(&self, _idcmd: usize, _utype: u32, _preserved: *const u32, _pszname: PSTR, _cchmax: u32) -> Result<()> {
        Err(E_NOTIMPL.into())
    }
}

struct StgMediumGuard(pub(crate) STGMEDIUM);

impl Drop for StgMediumGuard {
    fn drop(&mut self) {
        unsafe {
            ReleaseStgMedium(&mut self.0);
        }
    }
}

struct SafeHandle(HANDLE);

impl Drop for SafeHandle {
    fn drop(&mut self) {
        unsafe {
            self.0.free();
        }
    }
}

impl From<&SafeHandle> for HANDLE {
    fn from(value: &SafeHandle) -> Self {
        value.0
    }
}
