using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Oiski.School.ProjectVault.App
{
    public static class UserManager
    {
        static string _root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        static FileHandler _userStore = new FileHandler(_root, "UserStorage.txt");

        public static bool SignInUser(string username, string password)
        {
            var hashedPassword = OiskiHasher.HashPassword(password, username);

            var user = _userStore.FindLine(username.ToLower())?.Split(",");

            if (user != null && user[1] == hashedPassword)
            {
                return true;
            }

            return false;
        }

        public static bool StoreUser(string username, string password)
        {
            var hashedPassword = OiskiHasher.HashPassword(password, username.ToLower());

            if (_userStore.FindLine(username.ToLower()) == null)
            {
                //  This is not secure at all, and only serves as a demonstration. A user should always be stored in a more secure way.
                _userStore.WriteLine($"{username.ToLower()},{hashedPassword}", append: true);

                return true;
            }

            return false;
        }

        public static bool StoreKey(string username, string password, string privateKey)
        {
            var hashedPassword = OiskiHasher.HashPassword(password, username.ToLower());

            if (SignInUser(username, password))
            {
                /*
                    This is not secure at all, and only serves as a demonstration. The private key should be stored more securely.
                    According to NCSC (National Cyber Security Centre) a private key for end entities should be stored in
                    Trusted Platform Modules (TPM) or a USB tamper-resistant security token.

                    Source: https://www.ncsc.gov.uk/collection/in-house-public-key-infrastructure/pki-principles/protect-your-private-keys#:~:text=A%20CA's%20private%20key%20should,USB%20tamper%2Dresistant%20security%20token.
                */

                int lineNumber = _userStore.GetLineNumber(_userStore.FindLine(username.ToLower()));

                _userStore.UpdateLine($"{username.ToLower()},{hashedPassword},{privateKey}", lineNumber);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the <strong>Private Key</strong> a user with <paramref name="username"/> used to encrypt a file
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>The <strong>Private Key</strong> if the <paramref name="username"/> and <paramref name="password"/> returns a match, and the user has a key associated; Otherwise, if not, <see langword="null"/></returns>
        public static string RetrieveKeys(string username, string password)
        {
            var hashedPassword = OiskiHasher.HashPassword(password, username.ToLower());

            if (SignInUser(username, password))
            {
                var userData = _userStore.FindLine(username.ToLower())?.Split(",");

                var key = ((userData.Length > 1) ? (userData[2]) : (null));

                return key;
            }

            return null;
        }
    }
}
