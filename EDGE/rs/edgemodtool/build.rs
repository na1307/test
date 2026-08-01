use std::path::PathBuf;

fn main() {
    let libpath = PathBuf::from(std::env::var("OUT_DIR").unwrap());
    let libpath = libpath.parent().unwrap().parent().unwrap().parent().unwrap();

    println!("cargo:rustc-link-arg=/libpath:{}", libpath.to_str().unwrap());

    embed_resource::compile("src/Resource_exe.rc", embed_resource::NONE)
        .manifest_optional()
        .unwrap();
}
