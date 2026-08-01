use crate::profiles::read_profile_json;
use std::env::current_exe;
use std::fs::{File, create_dir, remove_dir_all, remove_file};
use std::path::PathBuf;
use zip::ZipArchive;

define_errors_enum! {
    LoaderErrors;
    PathDoesNotExist = 1,
    NotAValidEdgeDirectory = 2,
    ArchiveDoesNotExist = 3,
    LoaderAlreadyInstalled = 4,
    LoaderNotInstalled = 5,
    IOError = 6,
    ProfileDoesNotExist = 7,
}

pub fn install_loader(profile: &str) -> Result<(), LoaderErrors> {
    let profiles = read_profile_json().map_err(|_| LoaderErrors::IOError)?;
    let edgeexe = profiles.get_path_for_profile(profile).map_err(|_| LoaderErrors::ProfileDoesNotExist)?;
    let edgepath = edgeexe.parent().unwrap();

    if !edgepath.exists() {
        return Err(LoaderErrors::PathDoesNotExist);
    }

    if !edgepath.is_dir() || !edgeexe.exists() {
        return Err(LoaderErrors::NotAValidEdgeDirectory);
    }

    let mut mods_path: PathBuf = edgepath.into();

    mods_path.push("mods");

    if mods_path.exists() && mods_path.is_dir() {
        return Err(LoaderErrors::LoaderAlreadyInstalled);
    }

    let loader_archive = current_exe().unwrap().with_file_name("loader.zip");

    if !loader_archive.exists() || !loader_archive.is_file() {
        return Err(LoaderErrors::ArchiveDoesNotExist);
    }

    let loader_archive = File::open(loader_archive).map_err(|_| LoaderErrors::IOError)?;
    let mut loader_archive = ZipArchive::new(loader_archive).map_err(|_| LoaderErrors::IOError)?;

    loader_archive.extract(edgepath).map_err(|_| LoaderErrors::IOError)?;
    create_dir(edgeexe.with_file_name("mods")).map_err(|_| LoaderErrors::IOError)?;

    Ok(())
}

pub fn uninstall_loader(profile: &str) -> Result<(), LoaderErrors> {
    let profiles = read_profile_json().map_err(|_| LoaderErrors::IOError)?;
    let edgeexe = profiles.get_path_for_profile(profile).map_err(|_| LoaderErrors::ProfileDoesNotExist)?;
    let edgepath = edgeexe.parent().unwrap();

    if !edgepath.exists() {
        return Err(LoaderErrors::PathDoesNotExist);
    }

    if !edgepath.is_dir() || !edgeexe.exists() {
        return Err(LoaderErrors::NotAValidEdgeDirectory);
    }

    let mods_path = edgeexe.with_file_name("mods");

    if !mods_path.exists() || !mods_path.is_dir() {
        return Err(LoaderErrors::LoaderNotInstalled);
    }

    remove_dir_all(mods_path).map_err(|_| LoaderErrors::IOError)?;
    remove_dir_all(edgeexe.with_file_name("plugins")).map_err(|_| LoaderErrors::IOError)?;
    remove_file(edgeexe.with_file_name("xinput1_3.dll")).map_err(|_| LoaderErrors::IOError)?;

    Ok(())
}
