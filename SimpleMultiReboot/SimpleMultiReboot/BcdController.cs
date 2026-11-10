#pragma warning disable CA2201
using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Options;
using System.Collections.ObjectModel;

namespace SimpleMultiReboot;

internal sealed class BcdController : IDisposable {
    private readonly CimSession cimSession;
    private readonly CimInstance bcdStore;
    private readonly List<BootEntry> entries;

    static BcdController() {
        var value = -1;

        NativeMethods.GetFirmwareType(ref value);

        IsUefi = value == 2;
    }

    public BcdController() {
        entries = [];
#pragma warning disable IDE0028, IDE0306
        Entries = new(entries);
#pragma warning restore IDE0028, IDE0306
        cimSession = CimSession.Create(null);
        using CimMethodParametersCollection osp = [CimMethodParameter.Create("File", string.Empty, CimType.String, CimFlags.None)];
        var result = cimSession.InvokeMethod("root/WMI", "BcdStore", "OpenStore", osp);

        if (!(bool)result.ReturnValue.Value) {
            throw new("Opening BcdStore failed!");
        }

        bcdStore = (CimInstance)result.OutParameters["Store"].Value;

        AddEntries(EntryType.WindowsBootLoader);

        if (IsUefi) {
            AddEntries(EntryType.FirmwareApplication);
        }
    }

    public static bool IsUefi { get; }

    public Collection<BootEntry> Entries { get; }

    public void SetBootSequence(BootEntry entry) {
        if (!entries.Contains(entry)) {
            throw new ArgumentException("Entry not found.");
        }

        using CimMethodParametersCollection oop1 = [
            CimMethodParameter.Create("Id", "{a5a30fa2-3d06-4e9f-b5f4-a01df9d1fcba}", CimType.String, CimFlags.None)
        ];

        var result = cimSession.InvokeMethod(bcdStore, "OpenObject", oop1);

        if (!(bool)result.ReturnValue.Value) {
            throw new("OpenObject failed.");
        }

        var fwbootmgr = (CimInstance)result.OutParameters["Object"].Value;

        using CimMethodParametersCollection solep1 = [
            CimMethodParameter.Create("Type", BcdElementTypes.BootSequence, CimType.UInt32, CimFlags.None),
            CimMethodParameter.Create("Ids", (string[])[entry.Type == EntryType.FirmwareApplication ? entry.Identifier : "{9dea862c-5cdd-4e70-acc1-f32b344d4795}"], CimType.StringArray, CimFlags.None)
        ];

        result = cimSession.InvokeMethod(fwbootmgr, "SetObjectListElement", solep1);

        if (!(bool)result.ReturnValue.Value) {
            throw new("SetObjectListElement failed.");
        }

        if (entry.Type == EntryType.WindowsBootLoader) {
            using CimMethodParametersCollection oop2 = [
                CimMethodParameter.Create("Id", "{9dea862c-5cdd-4e70-acc1-f32b344d4795}", CimType.String, CimFlags.None)
            ];

            result = cimSession.InvokeMethod(bcdStore, "OpenObject", oop2);

            if (!(bool)result.ReturnValue.Value) {
                throw new("OpenObject failed.");
            }

            var bootmgr = (CimInstance)result.OutParameters["Object"].Value;

            using CimMethodParametersCollection solep2 = [
                CimMethodParameter.Create("Type", BcdElementTypes.BootSequence, CimType.UInt32, CimFlags.None),
                CimMethodParameter.Create("Ids", (string[])[entry.Identifier], CimType.StringArray, CimFlags.None)
            ];

            result = cimSession.InvokeMethod(bootmgr, "SetObjectListElement", solep2);

            if (!(bool)result.ReturnValue.Value) {
                throw new("SetObjectListElement failed.");
            }
        }
    }

    public void Dispose() {
        bcdStore.Dispose();
        cimSession.Dispose();
    }

    private void AddEntries(EntryType type) {
        using CimMethodParametersCollection eop = [CimMethodParameter.Create("Type", type, CimType.UInt32, CimFlags.None)];
        var result = cimSession.InvokeMethod(bcdStore, "EnumerateObjects", eop);

        if (!(bool)result.ReturnValue.Value) {
            throw new("Enumerating BcdStore failed!");
        }

        var objects = (CimInstance[])result.OutParameters["Objects"].Value;

        foreach (var instance in objects) {
            using CimMethodParametersCollection eep = [];
            var innerResult = cimSession.InvokeMethod(instance, "EnumerateElements", eep);

            if (!(bool)innerResult.ReturnValue.Value) {
                throw new("Enumerating BcdObject failed!");
            }

            var elements = (CimInstance[])innerResult.OutParameters["Elements"].Value;

            var identifier = Guid.Parse((string)instance.CimInstanceProperties["Id"].Value);

            var description = (string?)elements
                .SingleOrDefault(e => (BcdElementTypes)e.CimInstanceProperties["Type"].Value == BcdElementTypes.Description)
                ?.CimInstanceProperties["String"].Value ?? "(No Description)";

            entries.Add(new(type, identifier, description));
        }
    }
}
