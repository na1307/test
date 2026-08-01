use crate::{Result, get_default_profile_name};
use std::ffi::{CString, c_char};

#[link(name = "edgemodtoolcore.dll")]
unsafe extern "C" {
    fn install_loader(profile: *const c_char) -> i32;
    fn uninstall_loader(profile: *const c_char) -> i32;
}

pub fn install_loader_command(profile: Option<String>) -> Result<()> {
    let profile = get_default_profile_name(profile)?;

    println!("Installing loader to profile '{}'", profile);

    unsafe {
        let profile = CString::new(profile).unwrap();

        handle_error_code(install_loader(profile.as_ptr()))?;
    }

    println!("Install successful");

    Ok(())
}

pub fn uninstall_loader_command(profile: Option<String>) -> Result<()> {
    let profile = get_default_profile_name(profile)?;

    println!("Uninstalling loader to profile '{}'", profile);

    unsafe {
        let profile = CString::new(profile).unwrap();

        handle_error_code(uninstall_loader(profile.as_ptr()))?;
    }

    println!("Uninstall successful");

    Ok(())
}

fn handle_error_code(code: i32) -> Result<()> {
    match code {
        0 => Ok(()),
        1 => Err("The provided path does not exist.".to_string()),
        2 => Err("The provided path is not a valid EDGE directory.".to_string()),
        3 => Err("The loader archive does not exist.".to_string()),
        4 => Err("The loader is already installed.".to_string()),
        5 => Err("The loader is not installed.".to_string()),
        6 => Err("IO Error.".to_string()),
        7 => Err("Profile does not exist.".to_string()),
        _ => Err("Unknown Error.".to_string()),
    }
}
