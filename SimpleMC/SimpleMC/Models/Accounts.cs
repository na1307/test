using Microsoft.Extensions.Configuration;

namespace SimpleMC.Models;

internal sealed class Accounts {
    [ConfigurationKeyName("Microsoft")]
    public MicrosoftAccount[] MicrosoftAccounts { get; set; } = [];

    [ConfigurationKeyName("Offline")]
    public string? OfflineAccount { get; set; }
}
