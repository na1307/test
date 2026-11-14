use serde::Deserialize;

#[derive(Deserialize)]
pub(crate) struct AutorunJsonRoot {
    pub title: String,
    pub items: Vec<AutorunJsonItem>,
}

#[derive(Deserialize)]
pub(crate) struct AutorunJsonItem {
    pub text: String,
    pub exepath: String,
}
