using System;
using System.Security.Cryptography;

namespace UniversityZusha.funcions
{
    /// <summary>
    /// מחלקה לניהול אבטחת סיסמאות באמצעות Hashing.
    /// </summary>
    public static class PasswordHashHelper
    {
        public static string HashPasswordHash(string PasswordHash)
        {
            // יצירת Salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            // יצירת Hash
            var pbkdf2 = new Rfc2898DeriveBytes(PasswordHash, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            // שילוב ה-Salt וה-Hash
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            // המרת ה-Hash למחרוזת Base64
            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            return savedPasswordHash;
        }

        public static bool VerifyPasswordHash(string enteredPasswordHash, string storedHash)
        {
            // המרת ה-Hash ממחרוזת למערך בתים
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            // חילוץ ה-Salt
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // יצירת Hash לסיסמה שהוכנסה
            var pbkdf2 = new Rfc2898DeriveBytes(enteredPasswordHash, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            // השוואה בין ה-Hashים
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;

            return true;
        }
    }
}
