using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Oiski.School.ProjectVault.App
{
    public static class OiskiHasher
    {
        public static string HashPassword(string passsword, string salt = null)
        {
            /*
                This is a poor implementation, however, it only serves as a demonstration.
                In reality you wouldn't iterate 10 times as I do here but (maybe) 128,000 iterations, according to Brock Allen in 2014.
                He also mentaion that iterations are hardware specific and that the real target should be milliseconds.
                So it should take 500 to 1000 milliseconds to compute a hashed password.

                This is according to this article form Brock Allon in 2014: https://brockallen.com/2014/02/09/how-membershipreboot-stores-passwords-properly/
            */
            byte[] saltKey = ((salt != null) ? (Encoding.UTF8.GetBytes(salt.ToLower())) : (Encoding.UTF8.GetBytes(new Random().Next().ToString())));
            using var rfc2898 = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(passsword), saltKey, 10);
            return Convert.ToBase64String(rfc2898.GetBytes(32));
        }
    }
}
