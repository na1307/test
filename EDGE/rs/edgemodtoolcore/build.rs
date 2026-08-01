fn main() {
    embed_resource::compile("src/Resource_dll.rc", embed_resource::NONE)
        .manifest_optional()
        .unwrap();
}
