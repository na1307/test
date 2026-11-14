mod auto_raw_mode;
mod autorun_json;
mod win32;

use crate::auto_raw_mode::AutoRawMode;
use crate::autorun_json::AutorunJsonRoot;
use crate::win32::*;
use crossterm::Command;
use crossterm::cursor::{MoveTo, position};
use crossterm::style::{Color, ResetColor, SetBackgroundColor, SetForegroundColor};
use crossterm::terminal::{Clear, ClearType, SetTitle, size};
use serde_json::from_str;
use std::env::current_exe;
use std::fs::read;
use std::io::{Write, stdout};
use std::process::ExitCode;

const AUTORUN_CONFIG_FILE_NAME: &str = "autorun.jsn";
const CONSOLE_IS_TOO_SMALL: &str = "콘솔 크기가 너무 작은 것 같습니다.\r\n\r\n콘솔 크기를 늘리고 다시 시도하세요.";

macro_rules! write_stdout {
    ( $($arg:tt)* ) => {
        stdout().write_fmt(format_args!($($arg)*)).map_err(|err| format!("write_fmt: {}", err))?;
        stdout().flush().map_err(|err| format!("flush: {}", err))?;
    };
}

fn main() -> ExitCode {
    if let Err(message) = real_main() {
        println!("{}", message);
        println!();
        println!("계속하려면 아무 키를 누르십시오...");

        let arm = AutoRawMode::new();

        unsafe {
            let mut ir = INPUT_RECORD::default();
            let mut cc = 0;

            loop {
                if ReadConsoleInputA(GetStdHandle(STD_INPUT_HANDLE), &mut ir, 1, &mut cc) != 0 {
                    if ir.EventType == KEY_EVENT as u16 && ir.Event.KeyEvent.bKeyDown != 0 {
                        break;
                    }
                } else {
                    panic!("ReadConsoleInputA returned 0");
                }
            }
        }

        drop(arm);

        ExitCode::FAILURE
    } else {
        ExitCode::SUCCESS
    }
}

fn real_main() -> Result<(), String> {
    let path_autorun_ini = current_exe()
        .map_err(|err| format!("current_exe: {}", err))?
        .with_file_name(AUTORUN_CONFIG_FILE_NAME);

    if !path_autorun_ini.is_file() {
        return Err(format!("{}이(가) 없습니다!", AUTORUN_CONFIG_FILE_NAME));
    }

    let json_string = String::from_utf8(
        read(path_autorun_ini)
            .map_err(|err| format!("read_to_string: {}", err))?
            .strip_prefix(b"\xEF\xBB\xBF")
            .unwrap_or(&[])
            .to_vec(),
    )
    .map_err(|err| format!("read_to_string: {}", err))?;

    let autorun_json: AutorunJsonRoot = from_str(&json_string).map_err(|err| format!("from_str: {}", err))?;
    let arm = AutoRawMode::new();

    SetTitle(&autorun_json.title)
        .execute_winapi()
        .map_err(|err| format!("SetTitle: {}", err))?;

    let size = size().map_err(|err| format!("size: {}", err))?;
    let title_column = ((size.0 as usize).saturating_sub(autorun_json.title.chars().count()) / 2) as u16;
    let num = autorun_json.items.len();
    let mut selected = usize::from(num != 0);
    let stdin = unsafe { GetStdHandle(STD_INPUT_HANDLE) };
    let mut ir = INPUT_RECORD::default();
    let mut cc = 0u32;

    Clear(ClearType::All).execute_winapi().map_err(|err| format!("Clear: {}", err))?;
    MoveTo(title_column, 1).execute_winapi().map_err(|err| move_err_message("MoveTo", err))?;
    write_stdout!("{}", autorun_json.title);

    let mut rows = vec![0; num + 1];

    for index in (1..=num).chain(std::iter::once(0)) {
        let spacing = if index <= 1 { 3 } else { 2 };
        // Start below the previous label even if it wrapped onto multiple rows.
        let (_, previous_row) = position().map_err(|err| format!("position: {}", err))?;
        let row = previous_row + spacing;
        rows[index] = row;

        draw_menu_item(&autorun_json, row, index, index == selected)?;
    }

    loop {
        if unsafe { ReadConsoleInputA(stdin, &mut ir, 1, &mut cc) } == 0 {
            return Err("ReadConsoleInputA returned 0".into());
        }

        if ir.EventType != KEY_EVENT as u16 || unsafe { ir.Event.KeyEvent.bKeyDown } == 0 {
            continue;
        }

        let next = match unsafe { ir.Event.KeyEvent.wVirtualKeyCode } as i32 {
            VK_UP => selected.checked_sub(1).unwrap_or(num),
            VK_DOWN if selected == num => 0,
            VK_DOWN => selected + 1,
            VK_RETURN => {
                MoveTo(0, rows[0]).execute_winapi().map_err(|err| move_err_message("MoveTo", err))?;
                write_stdout!("\r\n");

                if selected != 0 {
                    std::process::Command::new(
                        current_exe()
                            .map_err(|err| format!("current_exe: {}", err))?
                            .with_file_name(&autorun_json.items[selected - 1].exepath),
                    )
                    .spawn()
                    .map_err(|err| format!("Command: {}", err))?;
                }

                break;
            }
            _ => continue,
        };

        if next != selected {
            draw_menu_item(&autorun_json, rows[selected], selected, false)?;
            draw_menu_item(&autorun_json, rows[next], next, true)?;
            selected = next;
        }
    }

    drop(arm);

    Ok(())
}

fn draw_menu_item(menu: &AutorunJsonRoot, row: u16, index: usize, highlighted: bool) -> Result<(), String> {
    MoveTo(2, row).execute_winapi().map_err(|err| move_err_message("MoveTo", err))?;

    if highlighted {
        SetForegroundColor(Color::Black).execute_winapi().map_err(|err| err.to_string())?;
        SetBackgroundColor(Color::White).execute_winapi().map_err(|err| err.to_string())?;
    }

    let text = if index == 0 { "종료" } else { &menu.items[index - 1].text };

    write_stdout!("{}. {}", index, text);

    if highlighted {
        ResetColor.execute_winapi().map_err(|err| err.to_string())?;
    }

    Ok(())
}

fn move_err_message(command: &str, err: std::io::Error) -> String {
    if err.raw_os_error() == Some(87) {
        CONSOLE_IS_TOO_SMALL.into()
    } else {
        format!("{}: {}", command, err)
    }
}
