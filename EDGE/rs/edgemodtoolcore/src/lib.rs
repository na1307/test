#![allow(clippy::enum_variant_names)]

#[macro_use]
mod macros;
mod compiler;
mod loader;
mod loc;
mod mod_definition;
mod profiles;

use profiles::read_profile_json;
use std::ffi::{CStr, CString, c_char};
use std::path::PathBuf;
use std::str::FromStr;

#[unsafe(no_mangle)]
extern "C" fn compile_text_loc(json_path: *const c_char) -> i32 {
    if json_path.is_null() {
        return -1;
    }

    let Ok(json_path) = unsafe { CStr::from_ptr(json_path) }.to_str() else {
        return -2;
    };

    match compiler::compile_text_loc(json_path.into()) {
        Ok(_) => 0,
        Err(e) => e.into(),
    }
}

#[unsafe(no_mangle)]
extern "C" fn decompile_text_loc(text_loc_path: *const c_char) -> i32 {
    if text_loc_path.is_null() {
        return -1;
    }

    let Ok(text_loc_path) = unsafe { CStr::from_ptr(text_loc_path) }.to_str() else {
        return -2;
    };

    match compiler::decompile_text_loc(text_loc_path.into()) {
        Ok(_) => 0,
        Err(e) => e.into(),
    }
}

#[unsafe(no_mangle)]
extern "C" fn install_loader(profile: *const c_char) -> i32 {
    if profile.is_null() {
        return -1;
    }

    let Ok(profile) = unsafe { CStr::from_ptr(profile) }.to_str() else {
        return -2;
    };

    match loader::install_loader(profile) {
        Ok(_) => 0,
        Err(e) => e.into(),
    }
}

#[unsafe(no_mangle)]
extern "C" fn uninstall_loader(profile: *const c_char) -> i32 {
    if profile.is_null() {
        return -1;
    }

    let Ok(profile) = unsafe { CStr::from_ptr(profile) }.to_str() else {
        return -2;
    };

    match loader::uninstall_loader(profile) {
        Ok(_) => 0,
        Err(e) => e.into(),
    }
}

#[unsafe(no_mangle)]
extern "C" fn add_profile(name: *const c_char, path: *const c_char) -> i32 {
    if name.is_null() || path.is_null() {
        return -1;
    }

    let Ok(name) = unsafe { CStr::from_ptr(name) }.to_str() else {
        return -2;
    };

    let Ok(path) = unsafe { CStr::from_ptr(path) }.to_str() else {
        return -2;
    };

    let path = PathBuf::from_str(path).unwrap();

    match profiles::add_profile(name, &path) {
        Ok(_) => 0,
        Err(e) => e.into(),
    }
}

#[unsafe(no_mangle)]
extern "C" fn remove_profile(name: *const c_char) -> i32 {
    if name.is_null() {
        return -1;
    }

    let Ok(name) = unsafe { CStr::from_ptr(name) }.to_str() else {
        return -2;
    };

    match profiles::remove_profile(name) {
        Ok(_) => 0,
        Err(e) => e.into(),
    }
}

#[unsafe(no_mangle)]
extern "C" fn default_profile(name: *const c_char) -> i32 {
    if name.is_null() {
        return -1;
    }

    let Ok(name) = unsafe { CStr::from_ptr(name) }.to_str() else {
        return -2;
    };

    match profiles::default_profile(name) {
        Ok(_) => 0,
        Err(e) => e.into(),
    }
}

#[unsafe(no_mangle)]
extern "C" fn get_default_profile(name: *mut *mut c_char) -> i32 {
    if name.is_null() {
        return -1;
    }

    let Ok(profiles) = read_profile_json() else {
        return -2;
    };

    let default = profiles.get_default();

    let Ok(default) = CString::from_str(default) else {
        return -3;
    };

    unsafe {
        *name = default.into_raw();
    }

    0
}

#[unsafe(no_mangle)]
extern "C" fn launch(profile: *const c_char) -> i32 {
    if profile.is_null() {
        return -1;
    }

    let Ok(profile) = unsafe { CStr::from_ptr(profile) }.to_str() else {
        return -2;
    };

    match profiles::launch(profile) {
        Ok(_) => 0,
        Err(e) => e.into(),
    }
}

#[unsafe(no_mangle)]
extern "C" fn delete_string(ptr: *mut c_char) -> i32 {
    if ptr.is_null() {
        return -1;
    }

    unsafe {
        _ = CString::from_raw(ptr);
    }

    0
}
