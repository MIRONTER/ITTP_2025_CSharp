using System.Security.Cryptography;
using System.Text;

namespace ITTP_2025_C_
{
    internal static class Tools
    {
        internal static string CreateSHA256(string input)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = SHA256.HashData(inputBytes);
            return Convert.ToHexString(hashBytes);
        }
    }
}
