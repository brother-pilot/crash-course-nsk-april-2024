using System.Security.Cryptography;
using System.Text;

namespace Market.Helpers;

public static class PasswordHelper
{
    private static MD5 _md5 = MD5.Create();
    
    public static string GetPasswordHash(string password, string salt)
    {
        var bytes = Encoding.Default.GetBytes(password + salt);
        var computedHash = _md5.ComputeHash(bytes);
        return Convert.ToHexString(computedHash);
    }
}