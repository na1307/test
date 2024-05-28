using Microsoft.Win32;

const string regpath = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full";

using var reg = Registry.LocalMachine.OpenSubKey(regpath);

MessageBox.Show(".NET Framework 4." + reg.GetValue("Release") switch {
    >= 533320 => "8.1",
    >= 528040 => "8",
    >= 461808 => "7.2",
    >= 461308 => "7.1",
    >= 460798 => "7",
    >= 394802 => "6.2",
    >= 394254 => "6.1",
    >= 393295 => "6",
    >= 379893 => "5.2",
    >= 378675 => "5.1",
    >= 378389 => "5",
    _ => "0"
});
