use serde::{Deserialize, Serialize};
use std::collections::HashMap;
use std::env::current_exe;
use std::fmt::{Display, Formatter};
use std::fs::{File, write};
use std::io::{BufReader, BufWriter, Read, Write};
use std::path::{Path, PathBuf};
use std::process::Command;

#[derive(Debug, Serialize, Deserialize)]
pub struct Profiles {
    default: String,
    profiles: HashMap<String, PathBuf>,
}

impl Profiles {
    pub fn from_json(json_file: impl Read) -> serde_json::Result<Self> {
        serde_json::from_reader(json_file)
    }

    pub fn save_json(&self, json_file: impl Write) -> serde_json::Result<()> {
        serde_json::to_writer_pretty(json_file, self)
    }

    pub fn add_or_modify_profile(&mut self, name: &str, path: PathBuf) {
        self.profiles.insert(name.to_string(), path);
    }

    pub fn remove_profile(&mut self, name: &str) -> Result<(), ProfilesErrors> {
        if !self.profiles.contains_key(name) {
            return Err(ProfilesErrors::ProfileDoesNotExist);
        }

        self.profiles.remove(name);

        Ok(())
    }

    pub fn get_default(&self) -> &str {
        &self.default
    }

    pub fn set_default(&mut self, name: &str) -> Result<(), ProfilesErrors> {
        if !name.is_empty() && !self.profiles.contains_key(name) {
            return Err(ProfilesErrors::ProfileDoesNotExist);
        }

        self.default = name.to_string();

        Ok(())
    }

    pub fn get_count(&self) -> usize {
        self.profiles.len()
    }

    pub fn get_path_for_profile(&self, name: &str) -> Result<PathBuf, ProfilesErrors> {
        self.profiles.get(name).cloned().ok_or(ProfilesErrors::ProfileDoesNotExist)
    }
}

impl Display for Profiles {
    fn fmt(&self, f: &mut Formatter<'_>) -> std::fmt::Result {
        f.write_str(&serde_json::to_string_pretty(self).map_err(|_| std::fmt::Error)?)
    }
}

define_errors_enum! {
    ProfilesErrors;
    PathDoesNotExist = 1,
    NotAFile = 2,
    NotAValidEdgeExecutable = 3,
    IOError = 4,
    ProfileDoesNotExist = 5,
}

pub fn add_profile(name: &str, path: &Path) -> Result<(), ProfilesErrors> {
    if !path.exists() {
        return Err(ProfilesErrors::PathDoesNotExist);
    }

    if !path.is_file() {
        return Err(ProfilesErrors::NotAFile);
    }

    if !path.ends_with("edge.exe") {
        return Err(ProfilesErrors::NotAValidEdgeExecutable);
    }

    let profile_json = get_profile_json_path().map_err(|_| ProfilesErrors::IOError)?;

    if !profile_json.exists() {
        write(
            &profile_json,
            r#"{
    "default": "",
    "profiles": {}
}"#,
        )
        .map_err(|_| ProfilesErrors::IOError)?;
    }

    let mut profiles = read_profile_json().map_err(|_| ProfilesErrors::IOError)?;
    let current_count = profiles.get_count();

    profiles.add_or_modify_profile(name, path.into());

    if current_count == 0 {
        profiles.set_default(name)?;
    }

    write_profile_json(profiles).map_err(|_| ProfilesErrors::IOError)?;

    Ok(())
}

pub fn remove_profile(name: &str) -> Result<(), ProfilesErrors> {
    let mut profiles = read_profile_json().map_err(|_| ProfilesErrors::IOError)?;

    profiles.remove_profile(name)?;

    if profiles.get_count() == 0 {
        profiles.set_default("")?;
    }

    write_profile_json(profiles).map_err(|_| ProfilesErrors::IOError)?;

    Ok(())
}

pub fn default_profile(name: &str) -> Result<(), ProfilesErrors> {
    let mut profiles = read_profile_json().map_err(|_| ProfilesErrors::IOError)?;

    profiles.set_default(name)?;
    write_profile_json(profiles).map_err(|_| ProfilesErrors::IOError)?;

    Ok(())
}

pub fn launch(profile: &str) -> Result<(), ProfilesErrors> {
    let profiles = read_profile_json().map_err(|_| ProfilesErrors::IOError)?;
    let edgeexe = profiles.get_path_for_profile(profile)?;

    Command::new(edgeexe).spawn().map_err(|_| ProfilesErrors::IOError)?;

    Ok(())
}

fn get_profile_json_path() -> std::io::Result<PathBuf> {
    Ok(current_exe()?.with_file_name("profile.json"))
}

pub fn read_profile_json() -> std::io::Result<Profiles> {
    let file = File::open(&get_profile_json_path()?)?;
    let br = BufReader::new(file);
    let profiles = Profiles::from_json(br)?;

    Ok(profiles)
}

fn write_profile_json(profiles: Profiles) -> std::io::Result<()> {
    let file = File::create(&get_profile_json_path()?)?;
    let bw = BufWriter::new(file);

    profiles.save_json(bw)?;

    Ok(())
}
