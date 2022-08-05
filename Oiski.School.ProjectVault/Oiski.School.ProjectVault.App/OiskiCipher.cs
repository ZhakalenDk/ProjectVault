using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Oiski.School.ProjectVault.App
{
    public class OiskiCipher
    {
        /// <summary>
        /// Initialize a new instance of type <see cref="OiskiCipher"/>
        /// <br/>
        /// <strong>Note:</strong> Use this when no keys should be preloaded
        /// </summary>
        public OiskiCipher() { /*Empty*/ }
        /// <summary>
        /// Initialize a new instance of type <see cref="OiskiCipher"/> with key paramaters preloaded
        /// </summary>
        /// <param name="keysAsXml"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public OiskiCipher(string keysAsXml)
        {
            if (keysAsXml == null) { throw new ArgumentNullException(nameof(keysAsXml), "Key can't be null. If this was intended use OiskiCipher() instead"); }
            using var cry = new RSACryptoServiceProvider(2048);
            cry.PersistKeyInCsp = false;
            cry.FromXmlString(keysAsXml);
            _publicKey = cry.ExportParameters(false);
            _privateKey = cry.ExportParameters(true);
        }

        private RSAParameters _publicKey;
        private RSAParameters _privateKey;
        private UnicodeEncoding _byteConverter = new UnicodeEncoding();

        /// <summary>
        /// Generates a new set of keys and stores in memory
        /// </summary>
        public void GenerateKeys()
        {
            using var cry = new RSACryptoServiceProvider(2048);
            cry.PersistKeyInCsp = false;
            _publicKey = cry.ExportParameters(false);
            _privateKey = cry.ExportParameters(true);
        }

        /// <summary>
        /// Uses the <strong>public key</strong> to encrypt <paramref name="message"/>
        /// </summary>
        /// <param name="message"></param>
        /// <returns>The encrypted message in cipher form</returns>
        public byte[] Encrypt(string message)
        {
            using var cry = new RSACryptoServiceProvider(2048);
            cry.PersistKeyInCsp = false;
            cry.ImportParameters(_publicKey);

            var cipherText = cry.Encrypt((_byteConverter.GetBytes(message)), true);

            #region Test Code (Legacy)
            //File.WriteAllBytes(@"C:\Users\smukk\source\repos\ProjectVault\Oiski.School.ProjectVault\Oiski.School.ProjectVault.App\bin\Debug\net6.0\Encryption.txt", cipherText);

            //cry.ImportParameters(_privateKey);
            //var plaintext = _byteConverter.GetString(cry.Decrypt(File.ReadAllBytes(@"C:\Users\smukk\source\repos\ProjectVault\Oiski.School.ProjectVault\Oiski.School.ProjectVault.App\bin\Debug\net6.0\Encryption.txt"), true));
            #endregion

            return cipherText;
        }

        /// <summary>
        /// Uses the <strong>private key</strong> to decrypt <paramref name="cipherText"/>
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns>The decrypted cipher in plaintext</returns>
        public string Decrypt(byte[] cipherText)
        {
            using var cry = new RSACryptoServiceProvider(2048);
            cry.PersistKeyInCsp = false;
            cry.ImportParameters(_privateKey);

            var plaintext = cry.Decrypt(cipherText, true);

            return _byteConverter.GetString(plaintext);
        }

        public string GetPrivateKeyAsXml()
        {
            using var cry = new RSACryptoServiceProvider(2048);
            cry.ImportParameters(_privateKey);
            return cry.ToXmlString(true);
        }

        public string GetPublicKeyAsXml()
        {
            using var cry = new RSACryptoServiceProvider(2048);
            cry.ImportParameters(_publicKey);
            return cry.ToXmlString(false);
        }
    }
}
