using System.Globalization;
using System.Text;

namespace EdgeModTool.LibTwoTribes.Util;

internal static class AssetUtil {
    private static readonly CRC32 Crc32Name = new();
    private static readonly CRC32 Crc32NameSpace = new(~CRC32.DefaultPolynomial);

    internal enum EngineVersion : ulong {
        Version1803Rush = 0x0018000000000003,
        Version1804Edge = 0x0018000000000004,
        VersionD003EdgeOld = 0x00D0000000000003,
        VersionD103EdgeOld = 0x00D1000000000003,
        VersionD603EdgeOld = 0x00D6000000000003,
        VersionDb03EdgeOld = 0x00DB000000000003,
        VersionDf03EdgeOld = 0x00DF000000000003,
    }

    public static string GetEngineVersionName(EngineVersion version)
        => version switch {
            EngineVersion.Version1803Rush => "18-03 [RUSH]",
            EngineVersion.Version1804Edge => "18-04 [EDGE]",
            EngineVersion.VersionD003EdgeOld => "D0-03 [EDGE OLD]",
            EngineVersion.VersionD103EdgeOld => "D1-03 [EDGE OLD]",
            EngineVersion.VersionD603EdgeOld => "D6-03 [EDGE OLD]",
            EngineVersion.VersionDb03EdgeOld => "DB-03 [EDGE OLD]",
            EngineVersion.VersionDf03EdgeOld => "DF-03 [EDGE OLD]",
            _ => string.Create(CultureInfo.InvariantCulture, $"[0x{(ulong)version:X16}]"),
        };

    public static string CrcFullName(string name, string nameSpace, bool stripExtension = true)
        => string.Create(CultureInfo.InvariantCulture, $"{CrcName(name, stripExtension):X8}{CrcNamespace(nameSpace):X8}");

    public static uint CrcName(string name, bool stripExtension = true) {
        if (stripExtension && name.Contains('.')) {
            name = name[..name.LastIndexOf('.')];
        }

        return Crc32Name.CalculateCRC(BinaryUtil.Reverse(Encoding.ASCII.GetBytes(name)));
    }

    public static uint CrcNamespace(string nameSpace) => Crc32NameSpace.CalculateCRC(BinaryUtil.Reverse(Encoding.ASCII.GetBytes(nameSpace)));
}
