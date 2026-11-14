fn main() {
    windows_bindgen::builder()
        .output("src/win32.rs")
        .filters([
            "ReadConsoleInputA",
            "GetStdHandle",
            "STD_INPUT_HANDLE",
            "KEY_EVENT",
            "VK_RETURN",
            "VK_UP",
            "VK_DOWN",
        ])
        .flat()
        .sys()
        .write();

    embed_resource::compile("autorun.rc", embed_resource::NONE).manifest_optional().unwrap();
}
