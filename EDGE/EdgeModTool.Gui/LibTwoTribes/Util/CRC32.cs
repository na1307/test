namespace EdgeModTool.LibTwoTribes.Util;

internal sealed class CRC32 {
    public const uint DefaultPolynomial = 0xEDB88320;
    private readonly uint[] crcTable = new uint[256];
    private uint polynomial;

    public CRC32(uint polynomial = DefaultPolynomial) {
        this.polynomial = polynomial;

        CalculateCRCTable();
    }

    public uint Polynomial {
        get => polynomial;

        set {
            polynomial = value;

            CalculateCRCTable();
        }
    }

    public uint CalculateCRC(byte[] data) => UpdateCRC(0x00000000, data);

    private void CalculateCRCTable() {
        uint c;
        uint n;

        for (n = 0; n < 256; n++) {
            c = n;

            for (var k = 0; k < 8; k++) {
                if ((c & 1) != 0) {
                    c = polynomial ^ (c >> 1);
                } else {
                    c >>= 1;
                }
            }

            crcTable[n] = c;
        }
    }

    private uint UpdateCRC(uint crc, byte[] data) => data.Aggregate(crc, (current, t) => crcTable[(current ^ t) & 0xff] ^ (current >> 8));
}
