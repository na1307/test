use clap::{Parser, Subcommand};
use std::path::PathBuf;

#[derive(Parser)]
#[command(name = "EdgeModTool", version, about, long_about = None)]
pub struct Cli {
    #[command(subcommand)]
    pub command: Commands,
}

#[derive(Subcommand)]
pub enum Commands {
    /// Compile a file
    Compile {
        /// File path
        path: PathBuf,
    },
    /// Decompile a file
    Decompile {
        /// File path
        path: PathBuf,
    },
    /// Install the mod loader
    InstallLoader {
        /// profile name
        profile: Option<String>,
    },
    /// Uninstall the mod loader
    UninstallLoader {
        /// profile name
        profile: Option<String>,
    },
    /// Install a mod
    Install {
        /// Mod path
        path: PathBuf,
        /// Profile name
        #[arg(long)]
        profile: Option<String>,
    },
    /// Uninstall a mod
    Uninstall {
        /// Mod name
        modname: String,
        /// Profile name
        #[arg(long)]
        profile: Option<String>,
    },
    /// Profile actions
    Profile {
        #[command(subcommand)]
        command: ProfileCommands,
    },
    /// Launch the game
    Launch {
        /// Profile name
        profile: Option<String>,
    },
}

#[derive(Subcommand)]
pub enum ProfileCommands {
    /// Add or Modify a profile
    Add {
        /// Profile name
        name: Option<String>,
        /// edge.exe path
        path: Option<PathBuf>,
    },
    /// Remove a profile
    Remove {
        /// Profile name
        name: String,
    },
    /// Change the default profile
    Default {
        /// Profile name
        name: String,
    },
}
