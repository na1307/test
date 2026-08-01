mod commands;
mod compile_commands;
mod loader_commands;
mod profile_commands;

use clap::Parser;
use commands::*;
use compile_commands::*;
use loader_commands::*;
use profile_commands::*;
use std::ffi::{CStr, CString, c_char};
use std::ptr::null_mut;

type Result<T> = std::result::Result<T, String>;

fn main() {
    let cli = Cli::parse();

    let result = match cli.command {
        Commands::Compile { path } => compile_command(path),
        Commands::Decompile { path } => decompile_command(path),
        Commands::InstallLoader { profile } => install_loader_command(profile),
        Commands::UninstallLoader { profile } => uninstall_loader_command(profile),
        Commands::Install { path, profile } => Ok(()),
        Commands::Uninstall { modname, profile } => Ok(()),
        Commands::Profile { command } => match command {
            ProfileCommands::Add { name, path } => add_profile_command(name, path),
            ProfileCommands::Remove { name } => remove_profile_command(name),
            ProfileCommands::Default { name } => default_profile_command(name),
        },
        Commands::Launch { profile } => launch_command(profile),
    };

    if let Err(error) = result {
        eprintln!("ERROR: {}", error);
    }
}

#[link(name = "edgemodtoolcore.dll")]
unsafe extern "C" {
    fn get_default_profile(name: *mut *mut c_char) -> i32;
    fn delete_string(ptr: *mut c_char) -> i32;
}

fn get_default_profile_name(profile: Option<String>) -> Result<String> {
    if let Some(profile) = profile {
        Ok(profile)
    } else {
        unsafe {
            let mut ptr: *mut c_char = null_mut();

            if get_default_profile(&mut ptr) != 0 {
                return Err("Unknown Error.".to_string());
            }

            let value = CString::from(CStr::from_ptr(ptr)).to_string_lossy().to_string();

            delete_string(ptr);

            Ok(value)
        }
    }
}
