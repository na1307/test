#![allow(non_snake_case)]

mod junction_creator;
mod junction_creator_factory;

use crate::junction_creator_factory::JunctionCreatorFactory;
use std::ffi::c_void;
use std::sync::atomic::{AtomicUsize, Ordering};
use windows::Win32::Foundation::*;
use windows::Win32::System::Com::StringFromCLSID;
use windows::Win32::System::LibraryLoader::GetModuleFileNameW;
use windows::Win32::System::Registry::*;
use windows::Win32::System::SystemServices::DLL_PROCESS_ATTACH;
use windows::Win32::UI::Shell::{SHCNE_ASSOCCHANGED, SHCNF_IDLIST, SHChangeNotify};
use windows::core::*;

static DLL_REF_COUNT: AtomicUsize = AtomicUsize::new(0);
static CURRENT_HINSTANCE: AtomicUsize = AtomicUsize::new(0);

const JUNCTIONCREATOR_CLSID: GUID = GUID::from_u128(0x5e19a787_ca63_4296_b092_1b8e6f48f3f6);

#[unsafe(no_mangle)]
extern "system" fn DllMain(hinstdll: HINSTANCE, fdwReason: u32, _lpvReserved: *const c_void) -> BOOL {
    if fdwReason == DLL_PROCESS_ATTACH {
        CURRENT_HINSTANCE.store(hinstdll.0 as usize, Ordering::Relaxed);
    }

    TRUE
}

#[unsafe(no_mangle)]
extern "system" fn DllGetClassObject(rclsid: *const GUID, riid: *const GUID, ppv: *mut *mut c_void) -> HRESULT {
    if ppv.is_null() {
        return E_POINTER;
    }

    if unsafe { (*rclsid).eq(&JUNCTIONCREATOR_CLSID) } {
        let instance = JunctionCreatorFactory {}.into_object();

        DLL_REF_COUNT.fetch_add(1, Ordering::Relaxed);

        // The Drop impl on JunctionCreatorFactory will decrement the count on QI failure
        unsafe { instance.QueryInterface(riid, ppv) }
    } else {
        CLASS_E_CLASSNOTAVAILABLE
    }
}

#[unsafe(no_mangle)]
extern "system" fn DllCanUnloadNow() -> HRESULT {
    if DLL_REF_COUNT.load(Ordering::Relaxed) == 0 { S_OK } else { S_FALSE }
}

#[unsafe(no_mangle)]
extern "system" fn DllRegisterServer() -> HRESULT {
    if let Err(e) = register_server() {
        return e.into();
    }

    if let Err(e) = register_shell_extension() {
        return e.into();
    }

    S_OK
}

fn register_server() -> Result<()> {
    let clsid_reg_path = get_clsid_reg_path()?;

    set_registry_key(HKEY_CLASSES_ROOT, &clsid_reg_path, None, "JunctionCreator")?;

    let mut inprocserver32 = clsid_reg_path.clone();

    inprocserver32.push_str("\\InProcServer32");

    let hmodule = HMODULE(CURRENT_HINSTANCE.load(Ordering::Relaxed) as *mut c_void);
    let mut buffer = [0u16; 261];
    let length: u32;

    unsafe {
        length = GetModuleFileNameW(Some(hmodule), &mut buffer);
    }

    if length == 0 {
        return Err(E_FAIL.into());
    }

    let module_path = String::from_utf16_lossy(&buffer[..length as usize]);

    set_registry_key(HKEY_CLASSES_ROOT, &inprocserver32, None, &module_path)?;
    set_registry_key(HKEY_CLASSES_ROOT, &inprocserver32, Some("ThreadingModel"), "Apartment")?;

    Ok(())
}

fn register_shell_extension() -> Result<()> {
    let clsid_string = get_clsid_string()?;

    set_registry_key(
        HKEY_CLASSES_ROOT,
        "Directory\\shellex\\DragDropHandlers\\JunctionCreator",
        None,
        &clsid_string,
    )?;

    set_registry_key(
        HKEY_CLASSES_ROOT,
        "Folder\\shellex\\DragDropHandlers\\JunctionCreator",
        None,
        &clsid_string,
    )?;

    set_registry_key(
        HKEY_CLASSES_ROOT,
        "Drive\\shellex\\DragDropHandlers\\JunctionCreator",
        None,
        &clsid_string,
    )?;

    unsafe {
        SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, None, None);
    }

    Ok(())
}

fn set_registry_key(hkey: HKEY, subkey: &str, value_name: Option<&str>, value: &str) -> Result<()> {
    let value_vec = to_utf16_null_terminated(value);

    let result = unsafe {
        RegSetKeyValueW(
            hkey,
            PCWSTR(to_utf16_null_terminated(subkey).as_ptr()),
            PCWSTR(to_utf16_null_terminated(value_name.unwrap_or_default()).as_ptr()),
            REG_SZ.0,
            Some(value_vec.as_ptr() as *const c_void),
            (value_vec.len() * 2usize) as u32,
        )
    };

    result.ok()
}

#[unsafe(no_mangle)]
extern "system" fn DllUnregisterServer() -> HRESULT {
    if let Err(e) = unregister_server() {
        return e.into();
    }

    if let Err(e) = unregister_shell_extension() {
        return e.into();
    }

    S_OK
}

fn unregister_server() -> Result<()> {
    let clsid_reg_path = get_clsid_reg_path()?;
    let result = unsafe { RegDeleteTreeW(HKEY_CLASSES_ROOT, PCWSTR(to_utf16_null_terminated(&clsid_reg_path).as_ptr())) };

    if result.is_err() && result != ERROR_FILE_NOT_FOUND {
        return Err(result.to_hresult().into());
    }

    Ok(())
}

fn unregister_shell_extension() -> Result<()> {
    let result = unsafe { RegDeleteTreeW(HKEY_CLASSES_ROOT, w!("Directory\\shellex\\DragDropHandlers\\JunctionCreator")) };

    if result.is_err() && result != ERROR_FILE_NOT_FOUND {
        return Err(result.to_hresult().into());
    }

    let result = unsafe { RegDeleteTreeW(HKEY_CLASSES_ROOT, w!("Folder\\shellex\\DragDropHandlers\\JunctionCreator")) };

    if result.is_err() && result != ERROR_FILE_NOT_FOUND {
        return Err(result.to_hresult().into());
    }

    let result = unsafe { RegDeleteTreeW(HKEY_CLASSES_ROOT, w!("Drive\\shellex\\DragDropHandlers\\JunctionCreator")) };

    if result.is_err() && result != ERROR_FILE_NOT_FOUND {
        return Err(result.to_hresult().into());
    }

    unsafe {
        SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, None, None);
    }

    Ok(())
}

fn to_utf16_null_terminated(s: &str) -> Vec<u16> {
    let mut utf16: Vec<u16> = s.encode_utf16().collect();

    utf16.push(0);

    utf16
}

fn get_clsid_reg_path() -> Result<String> {
    let mut clsid_string = get_clsid_string()?;

    clsid_string.insert_str(0, "CLSID\\");

    Ok(clsid_string)
}

fn get_clsid_string() -> Result<String> {
    Ok(unsafe { String::from_utf16_lossy(StringFromCLSID(&JUNCTIONCREATOR_CLSID)?.as_wide()) })
}
