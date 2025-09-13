using System.IO.Hashing;
using System.Security.Cryptography;

namespace SimpleHashTab;

internal sealed record class Hashed {
    public const int AlgorithmCount = 5;
    private readonly ReadOnlyMemory<byte> hashBytes;

    private Hashed(string algorithm, ReadOnlyMemory<byte> hashBytes) {
        Algorithm = algorithm;
        this.hashBytes = hashBytes;
    }

    public string Algorithm { get; }

    public string HashString => Convert.ToHexString(hashBytes.Span);

    public static async Task<Hashed> HashCrc32(string filePath, Action<long> progressCallback) {
        await using MemoryStream ms = new();

        await using (FileStream fs = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
        await using (ReadProgressStream rps = new(fs, progressCallback)) {
            await rps.CopyToAsync(ms);
        }

        var data = Crc32.Hash(ms.ToArray());

        return new("CRC32", data.Reverse().ToArray());
    }

    public static async Task<Hashed> HashMD5(string filePath, Action<long> progressCallback) {
        await using FileStream fs = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
        await using ReadProgressStream rps = new(fs, progressCallback);
        var data = await MD5.HashDataAsync(rps);

        return new("MD5", data);
    }

    public static async Task<Hashed> HashSha1(string filePath, Action<long> progressCallback) {
        await using FileStream fs = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
        await using ReadProgressStream rps = new(fs, progressCallback);
        var data = await SHA1.HashDataAsync(rps);

        return new("SHA-1", data);
    }

    public static async Task<Hashed> HashSha256(string filePath, Action<long> progressCallback) {
        await using FileStream fs = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
        await using ReadProgressStream rps = new(fs, progressCallback);
        var data = await SHA256.HashDataAsync(rps);

        return new("SHA-256", data);
    }

    public static async Task<Hashed> HashSha512(string filePath, Action<long> progressCallback) {
        await using FileStream fs = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
        await using ReadProgressStream rps = new(fs, progressCallback);
        var data = await SHA512.HashDataAsync(rps);

        return new("SHA-512", data);
    }
}
