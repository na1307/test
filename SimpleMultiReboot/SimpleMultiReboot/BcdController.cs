#pragma warning disable CA2201
using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Options;
using System.Collections.ObjectModel;

namespace SimpleMultiReboot;

internal sealed class BcdController : IDisposable {
    private readonly DComSessionOptions dcom;
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
        dcom = new();
        cimSession = CimSession.Create("localhost", dcom);
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

#pragma warning disable CA2208, S3928
        using CimMethodParametersCollection oop = [
            CimMethodParameter.Create("Id", entry.Type switch {
                EntryType.FirmwareApplication => "{a5a30fa2-3d06-4e9f-b5f4-a01df9d1fcba}",
                EntryType.WindowsBootLoader => "{9dea862c-5cdd-4e70-acc1-f32b344d4795}",
                _ => throw new ArgumentOutOfRangeException()
            }, CimType.String, CimFlags.None)
        ];
#pragma warning restore CA2208, S3928

        var result = cimSession.InvokeMethod(bcdStore, "OpenObject", oop);

        if (!(bool)result.ReturnValue.Value) {
            throw new("OpenObject failed.");
        }

        var managerObject = (CimInstance)result.OutParameters["Object"].Value;

        using CimMethodParametersCollection solep = [
            CimMethodParameter.Create("Type", BcdElementTypes.BootSequence, CimType.UInt32, CimFlags.None),
            CimMethodParameter.Create("Ids", (string[])[entry.Identifier], CimType.StringArray, CimFlags.None)
        ];

        result = cimSession.InvokeMethod(managerObject, "SetObjectListElement", solep);

        if (!(bool)result.ReturnValue.Value) {
            throw new("SetObjectListElement failed.");
        }
    }

    public void Dispose() {
        bcdStore.Dispose();
        cimSession.Dispose();
        dcom.Dispose();
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
