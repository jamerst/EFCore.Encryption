using System.Security.Cryptography;
using System.Text;

namespace Sample.Utils;

public static class SecurityUtils {
    private static readonly byte[] _key = Encoding.UTF8.GetBytes("!p2yiy5kJQbSe*tB6LedLJkDE@Fp3*2v");

    public static byte[] Encrypt(byte[] bytes) {
        using (Aes aes = Aes.Create()) {
            aes.Key = _key;
            aes.GenerateIV();

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using (var msOut = new MemoryStream()) {
                msOut.Write(aes.IV);
                using (var cs = new CryptoStream(msOut, encryptor, CryptoStreamMode.Write)) {
                    using (var msIn = new MemoryStream(bytes)) {
                        msIn.CopyTo(cs);
                    }
                }
                return msOut.ToArray();
            }
        }
    }

    public static async Task<byte[]> EncryptAsync(byte[] bytes) {
        using (Aes aes = Aes.Create()) {
            aes.Key = _key;
            aes.GenerateIV();

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using (var msOut = new MemoryStream()) {
                msOut.Write(aes.IV); // prepend with IV to be extracted when decrypting
                using (var cs = new CryptoStream(msOut, encryptor, CryptoStreamMode.Write)) {
                    using (var msIn = new MemoryStream(bytes)) {
                        await msIn.CopyToAsync(cs);
                    }
                }
                return msOut.ToArray();
            }
        }
    }

    public static byte[] Decrypt(byte[] bytes) {
        if (bytes is null || bytes.Length == 0)
        {
            return new byte[0];
        }

        using (Aes aes = Aes.Create()) {
            aes.Key = _key;

            byte[] iv = new byte[16];
            Array.Copy(bytes, iv, iv.Length);
            aes.IV = iv;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using (var msIn = new MemoryStream(bytes)) {
                msIn.Seek(iv.Length, SeekOrigin.Current); // skip past IV
                using (var cs = new CryptoStream(msIn, decryptor, CryptoStreamMode.Read)) {
                    using (var msOut = new MemoryStream()) {
                        cs.CopyTo(msOut);
                        return msOut.ToArray();
                    }
                }
            }
        }
    }

    public static async Task<byte[]> DecryptAsync(byte[] bytes) {
        using (Aes aes = Aes.Create()) {
            aes.Key = _key;

            byte[] iv = new byte[16];
            Array.Copy(bytes, iv, iv.Length);
            aes.IV = iv;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using (var msIn = new MemoryStream(bytes)) {
                msIn.Seek(iv.Length, SeekOrigin.Current);
                using (var cs = new CryptoStream(msIn, decryptor, CryptoStreamMode.Read)) {
                    using (var msOut = new MemoryStream()) {
                        await cs.CopyToAsync(msOut);
                        return msOut.ToArray();
                    }
                }
            }
        }
    }

    public static string Hash(byte[] bytes) {
        using (var sha = SHA256.Create()) {
            return Convert.ToBase64String(sha.ComputeHash(bytes));
        }
    }

    public static async Task<string> HashAsync(byte[] bytes) {
        using (var sha = SHA256.Create()) {
            using (var ms = new MemoryStream(bytes)) {
                return Convert.ToBase64String(await sha.ComputeHashAsync(ms));
            }
        }
    }
}