using Microsoft.Extensions.Options;
using System.Text.Json.Nodes;

namespace SimpleMC.Models;

internal sealed class SettingsService(IOptionsMonitor<Paths> pathsMonitor, IOptionsMonitor<Accounts> accountsMonitor) {
    public static string UserSettingsFilePath { get; } = Path.Combine(App.BaseDirectoryPath, "usersettings.json");

    public Paths Paths => pathsMonitor.CurrentValue;

    public Accounts Accounts => accountsMonitor.CurrentValue;

    public async Task SavePathsAsync(Paths paths) {
        var jo = await ParseUserSettingsJson();

        if (jo[nameof(Paths)] is not JsonObject pathsSection) {
            pathsSection = [];
            jo[nameof(Paths)] = pathsSection;
        }

        var ip = paths.InstancePath;

        if (ip != "instances") {
            pathsSection["InstancePath"] = ip;
        } else {
            pathsSection.Remove("InstancePath");
        }

        await SaveUserSettingsJson(jo);
    }

    public async Task SaveAccountsAsync(Accounts accounts) {
        var jo = await ParseUserSettingsJson();

        if (jo[nameof(Accounts)] is not JsonObject accountsSection) {
            accountsSection = [];
            jo[nameof(Accounts)] = accountsSection;
        }

        var oa = accounts.OfflineAccount;

        if (!string.IsNullOrWhiteSpace(oa)) {
            accountsSection["Offline"] = oa;
        } else {
            accountsSection.Remove("Offline");
        }

        await SaveUserSettingsJson(jo);
    }

    private static async Task<JsonObject> ParseUserSettingsJson() {
        Directory.CreateDirectory(App.BaseDirectoryPath);

        JsonObject jo;

        if (File.Exists(UserSettingsFilePath)) {
            var fileContent = await File.ReadAllBytesAsync(UserSettingsFilePath);
            jo = fileContent.Length > 0 ? JsonNode.Parse(fileContent)?.AsObject() ?? [] : [];
        } else {
            jo = [];
        }

        return jo;
    }

    private static async Task SaveUserSettingsJson(JsonObject jo)
        => await File.WriteAllTextAsync(UserSettingsFilePath, jo.ToJsonString(new() {
            WriteIndented = true
        }));
}
