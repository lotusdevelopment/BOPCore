using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Encrypt
{
    public class Encryption
    {
        private const string InitVector = "tu89geji340t89u2";

        private const int Keysize = 256;

        private const string Key = "$Lotus$%#!Enc1t7#";

        public string Encrypt(string text)
        {
            try
            {
                var initVectorBytes = Encoding.UTF8.GetBytes(InitVector);
                var plainTextBytes = Encoding.UTF8.GetBytes(text);
                var password = new PasswordDeriveBytes(Key, null);
#pragma warning disable 618
                var keyBytes = password.GetBytes(Keysize / 8);
#pragma warning restore 618
                var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC };
                var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
                var memoryStream = new MemoryStream();
                var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                var encrypted = memoryStream.ToArray();
                memoryStream.Close();
                cryptoStream.Close();
                return Convert.ToBase64String(encrypted);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public string Decrypt(string encryptedText)
        {
            try
            {
                var initVectorBytes = Encoding.ASCII.GetBytes(InitVector);
                var deEncryptedText = Convert.FromBase64String(encryptedText);
                var password = new PasswordDeriveBytes(Key, null);
#pragma warning disable 618
                var keyBytes = password.GetBytes(Keysize / 8);
#pragma warning restore 618
                var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC };
                var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
                var memoryStream = new MemoryStream(deEncryptedText);
                var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                var plainTextBytes = new byte[deEncryptedText.Length];
                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }

}
