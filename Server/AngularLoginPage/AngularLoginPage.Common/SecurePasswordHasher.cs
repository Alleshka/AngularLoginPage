using System.Security.Cryptography;
using System.Text;

namespace AngularLoginPage.Common
{
    public static class SecurePasswordHasher
    {
        private const int _saltSize = 128;
        private const int _passwordSize = 24;
        private static RandomNumberGenerator _rng;

        static SecurePasswordHasher()
        {
            _rng = RandomNumberGenerator.Create();
        }

        public static PasswordParts ComputeHash(string password)
        {
            var bytesSalt = GenerateSalt();
            return ComputeHash(password, bytesSalt);
        }

        public static bool CheckPassword(string password, string salt, string hashed)
        {
            var bytesSalt = Convert.FromBase64String(salt);
            var inputedHash = ComputeHash(password, bytesSalt).Password;
            return inputedHash == hashed;
        }

        private static byte[] GenerateSalt()
        {
            var bytes = new byte[_saltSize / 8];
            _rng.GetBytes(bytes);
            return bytes;
        }

        private static PasswordParts ComputeHash(string password, byte[] salt)
        {
            var bytesToHash = Encoding.UTF8.GetBytes(password);
            var bytesResult = new Rfc2898DeriveBytes(bytesToHash, salt, 10000).GetBytes(_passwordSize);

            return new PasswordParts(bytesResult, salt);
        }
    }
}
