use crate::DLL_REF_COUNT;
use std::os::windows::process::CommandExt;
use std::path::PathBuf;
use std::process::Command;
use std::ptr::null_mut;
use std::str::FromStr;
use std::sync::Mutex;
use std::sync::atomic::Ordering;
use windows::Win32::Foundation::*;
use windows::Win32::Globalization::lstrcmpiW;
use windows::Win32::Storage::FileSystem::GetVolumeInformationW;
use windows::Win32::System::Com::*;
use windows::Win32::System::Ole::{CF_HDROP, ReleaseStgMedium};
use windows::Win32::System::Registry::HKEY;
use windows::Win32::System::Threading::CREATE_NO_WINDOW;
use windows::Win32::UI::Shell::Common::ITEMIDLIST;
use windows::Win32::UI::Shell::*;
use windows::Win32::UI::WindowsAndMessaging::{HMENU, InsertMenuW, MF_BYPOSITION, MF_STRING};
use windows::core::*;

#[implement(IShellExtInit, IContextMenu)]
pub(crate) struct JunctionCreator {
    from: Mutex<String>,
    to: Mutex<String>,
}

impl JunctionCreator {
    pub(crate) fn new() -> Self {
        Self {
            from: Mutex::new(String::new()),
            to: Mutex::new(String::new()),
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

            if let Ok(mut to) = self.to.lock() {
                let len = buffer.iter().position(|&p| p == 0).unwrap_or(buffer.len());

                *to = String::from_utf16(&buffer[..len])?;
            } else {
                return Err(E_FAIL.into());
            }
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

            if let Ok(mut from) = self.from.lock() {
                *from = String::from_utf16(&buffer[..path_length as usize])?;
            } else {
                return Err(E_FAIL.into());
            }
        }

        if let Ok(from) = self.from.lock()
            && let Ok(to) = self.to.lock()
        {
            let fromPath = PathBuf::from_str(&from).map_err(|_| E_FAIL)?;
            let toPath = PathBuf::from_str(&to).map_err(|_| E_FAIL)?;

            if fromPath.parent().unwrap_or(&fromPath) == toPath {
                return Err(E_INVALIDARG.into());
            }
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

            let name = PathBuf::from_str(&from)
                .map_err(|_| E_FAIL)?
                .file_name()
                .ok_or(E_FAIL)?
                .to_str()
                .ok_or(E_FAIL)?
                .to_string();

            to.push_str(&name);

            Command::new("cmd")
                .args(["/c", "mklink", "/j", &to, &from])
                .creation_flags(CREATE_NO_WINDOW.0)
                .spawn()?;
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
