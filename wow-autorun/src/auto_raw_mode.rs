use crossterm::terminal::{disable_raw_mode, enable_raw_mode};

#[allow(unused)]
pub(crate) struct AutoRawMode(i32);

impl AutoRawMode {
    pub fn new() -> AutoRawMode {
        enable_raw_mode().expect("enable_raw_mode");

        AutoRawMode(0)
    }
}

impl Drop for AutoRawMode {
    fn drop(&mut self) {
        disable_raw_mode().expect("disable_raw_mode");
    }
}
