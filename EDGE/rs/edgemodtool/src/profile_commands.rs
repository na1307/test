use crate::{Result, get_default_profile_name};
use std::ffi::{CString, c_char};
use std::io::{Write, stdin, stdout};
use std::path::PathBuf;

#[link(name = "edgemodtoolcore.dll")]
unsafe extern "C" {
    fn add_profile(name: *const c_char, path: *const c_char) -> i32;
    fn remove_profile(name: *const c_char) -> i32;
    fn default_profile(name: *const c_char) -> i32;
    fn launch(profile: *const c_char) -> i32;
}

pub fn add_profile_command(name: Option<String>, path: Option<PathBuf>) -> Result<()> {
    let name = if let Some(name) = name {
        name
    } else {
        let mut buffer = String::new();

        print!("Profile name: ");

        stdout().flush().map_err(|err| format!("{}", err))?;
        stdin().read_line(&mut buffer).map_err(|err| format!("{}", err))?;

        buffer.trim().to_string()
    };

    if name.is_empty() {
        return Err("The name cannot be empty.".to_string());
    }

    let path = if let Some(path) = path {
        path
    } else {
        let mut buffer = String::new();

        print!("edge.exe path: ");

        stdout().flush().map_err(|err| format!("{}", err))?;
        stdin().read_line(&mut buffer).map_err(|err| format!("{}", err))?;

        buffer.trim().to_string().into()
    };

    unsafe {
        let name = CString::new(name.clone()).unwrap();
        let path = CString::new(path.to_str().unwrap()).unwrap();

        handle_error_code(add_profile(name.as_ptr(), path.as_ptr()))?;
    }

    println!("Successfully added/modified profile {}.", name);

    Ok(())
}

pub fn remove_profile_command(name: String) -> Result<()> {
    unsafe {
        let name = CString::new(name.clone()).unwrap();

        handle_error_code(remove_profile(name.as_ptr()))?;
    }

    println!("Successfully removed profile {}.", name);

    Ok(())
}

pub fn default_profile_command(name: String) -> Result<()> {
    unsafe {
        let name = CString::new(name.clone()).unwrap();

        handle_error_code(default_profile(name.as_ptr()))?;
    }

    println!("Setting default profile: {}", name);

    Ok(())
}

pub fn launch_command(profile: Option<String>) -> Result<()> {
    let profile = get_default_profile_name(profile)?;

    println!("Launching profile '{}'", profile);

    unsafe {
        let profile = CString::new(profile).unwrap();

        handle_error_code(launch(profile.as_ptr()))?;
    }

    Ok(())
}

fn handle_error_code(code: i32) -> Result<()> {
    match code {
        0 => Ok(()),
        1 => Err("The provided path does not exist.".to_string()),
        2 => Err("The provided path is not a file.".to_string()),
        3 => Err("The provided path is not a valid EDGE executable.".to_string()),
        4 => Err("IO Error.".to_string()),
        5 => Err("Profile does not exist.".to_string()),
        _ => Err(format!("Unknown Error ({}).", code)),
    }
}
