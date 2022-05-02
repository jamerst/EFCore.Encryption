using System.Text;

namespace EFCore.Encryption.Tests.Utils;

public static class BinaryConverter {
    public static byte[] ToBinary(string value) {
        return Encoding.UTF8.GetBytes(value);
    }

    public static byte[] ToBinary(DateOnly value) {
        return Encoding.UTF8.GetBytes(value.ToString("yyyy-MM-dd"));
    }

    public static byte[] ToBinary(DateTime value) {
        return BitConverter.GetBytes(value.ToBinary());
    }
}