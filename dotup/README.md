# dotup

`dotup` is a CLI tool designed to easily manage (install, update, uninstall) multiple versions of the .NET SDK. It allows for parallel management of different SDK versions and automatically detects and installs the version specified in a project's `global.json`.

## Platform Support

*   **Linux**: ✅ Fully Supported.
*   **Windows**: ⏳ Planned for future support.
*   **macOS**: ❌ Not planned (Hardware not available to the developer).

## Features

*   **Flexible Version Installation**: Support for specific versions, feature bands, major versions, LTS, and Current channels.
*   **`global.json` Integration**: Automatically detects `global.json` in the current directory tree to install the required SDK version.
*   **Version Management**: Easily list installed SDKs and remove unused versions.
*   **Auto-Update**: Keep your installed channels up-to-date with a single command.
*   **Environment Configuration**: Automatic `DOTNET_ROOT` and `PATH` configuration via helper scripts.

## Installation

### Arch Linux (AUR)

You can install `dotup` using an AUR helper like `yay`:

```bash
yay -S dotup-bin
```

Alternatively, you can manually build the package with `makepkg`.

```bash
git clone --depth 1 https://aur.archlinux.org/dotup-bin.git
cd dotup-bin
makepkg -sir
```

### Other Linux Distributions

For other Linux distributions, you can manually install the binary and configure the environment variables.

1.  **Download and Place Binary**:
    [Download the latest release binary](https://github.com/na1307/dotup/releases/latest) and move it to a system path (e.g., `/usr/local/bin` or `/usr/bin`).
    ```bash
    sudo mv dotup /usr/local/bin/
    sudo chmod +x /usr/local/bin/dotup
    ```

2.  **Configure Environment**:
    Copy the provided `dotup.sh` script to `/etc/profile.d/` to automatically load environment variables (`DOTNET_ROOT`, `PATH`) upon login.
    ```bash
    sudo cp dotup.sh /etc/profile.d/
    ```

    Alternatively, you can manually add the following line to your shell configuration file (e.g., `.bashrc`, `.zshrc`):
    ```bash
    eval "$(dotup env)"
    ```

## Usage

### 1. Install SDK

Install a specific version or channel of the .NET SDK.

```bash
dotup install [channel]
```

*   **Without arguments**: Looks for a `global.json` in the current or parent directories and installs the specified version.
*   **Channel Examples**:
    *   `10.0.100`: Exact version.
    *   `10.0.0`: Latest SDK for the specific release.
    *   `10.0.1xx`: Latest patch of the specified feature band.
    *   `10.0.x`: Latest feature band and patch for the major.minor version.
    *   `10`: Latest version for the major version.
    *   `lts`: Latest Long Term Support version.
    *   `latest`: Latest Stable version.
    *   `preview`: Latest Preview version.

### 2. List Installed SDKs

Display a list of .NET SDKs currently installed via `dotup`.

```bash
dotup list
```

### 3. Update SDKs

Update installed channels to their latest available versions.

```bash
dotup update [channel]
```

*   **[channel]**: Update a specific channel.
*   **Without arguments**: Checks and updates **all** installed channels.

### 4. Uninstall SDK

Remove an SDK version that is no longer needed.

```bash
dotup uninstall [channel]
```

*   If you remove the last remaining channel, `dotup` may ask if you want to delete the entire .NET installation folder to clean up.

## Development

This project is built with .NET. You can build it using standard `dotnet` commands.

```bash
dotnet build
dotnet test
dotnet publish
```

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
