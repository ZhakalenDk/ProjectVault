using Oiski.School.ProjectVault.App;

namespace Oiski.School.ProjectVault.Tests
{
    public class EncryptionTests
    {
        [Theory]
        [InlineData("Hello, World!")]
        [InlineData("I'm Awesome")]
        [InlineData("Hun sagde det med et kys i panden; Kærester, det er sådan nogen der går fra hinanden")]
        public void OiskiCipher_Encrypt_And_Decrypt(string plaintext)
        {
            //  ARRANGE
            OiskiCipher crypto = new OiskiCipher();
            crypto.GenerateKeys();

            //  ACT
            var ciphertext = crypto.Encrypt(plaintext);

            // ASSERT
            Assert.NotNull(ciphertext); //  Validating that the encryption returned a none null value

            //  ACT
            var decryption = crypto.Decrypt(ciphertext);

            //  ASSERT
            Assert.True(decryption == plaintext);
        }
    }
}