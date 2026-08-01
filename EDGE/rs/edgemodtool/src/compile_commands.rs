use crate::Result;
use std::ffi::{CString, c_char};
use std::path::PathBuf;

#[link(name = "edgemodtoolcore.dll")]
unsafe extern "C" {
    fn compile_text_loc(json_path: *const c_char) -> i32;
    fn decompile_text_loc(text_loc_path: *const c_char) -> i32;
}

pub fn compile_command(path: PathBuf) -> Result<()> {
    if !path.exists() {
        return Err(format!("The path {} does not exist.", path.display()));
    }

    if !path.is_file() {
        return Err(format!("The path {} is not a file.", path.display()));
    }

    match path.file_name().unwrap().to_str().unwrap() {
        "text.json" => {
            println!("Compiling text.loc");

            unsafe {
                let path = CString::new(path.to_str().unwrap()).unwrap();

                handle_error_code(compile_text_loc(path.as_ptr()))?;
            }
        }
        _ => {
            return Err(format!(
                "The file \"{}\" is the unknown file name for currently not supported.",
                path.display()
            ));
        }
    };

    println!("Successfully compiled the file.");

    Ok(())
}

pub fn decompile_command(path: PathBuf) -> Result<()> {
    if !path.exists() {
        return Err(format!("The path {} does not exist.", path.display()));
    }

    if !path.is_file() {
        return Err(format!("The path {} is not a file.", path.display()));
    }

    match path.file_name().unwrap().to_str().unwrap() {
        "text.loc" => {
            println!("Decompiling text.loc");

            unsafe {
                let path = CString::new(path.to_str().unwrap()).unwrap();

                handle_error_code(decompile_text_loc(path.as_ptr()))?;
            }
        }
        _ => {
            return Err(format!(
                "The file \"{}\" is the unknown file name for currently not supported.",
                path.display()
            ));
        }
    };

    println!("Successfully decompiled the file.");

    Ok(())
}

fn handle_error_code(code: i32) -> Result<()> {
    match code {
        0 => Ok(()),
        1 => Err("File Open Error.".to_string()),
        2 => Err("File Write Error.".to_string()),
        3 => Err("Serialization Error.".to_string()),
        4 => Err("(De)Compile Error.".to_string()),
        _ => Err("Unknown Error.".to_string()),
    }
}
