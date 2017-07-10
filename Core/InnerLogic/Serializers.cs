using System;
using System.Security.Cryptography;
using System.Text;

namespace Core.InnerLogic
{
    public class Serializers
    {
        public string CalculateMD5Hash(string input)
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString().ToLower();
        }

        public string EncryptMd5(string toEncrypt)
        {
            try
            {
                var md5 = new MD5CryptoServiceProvider();
                md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(toEncrypt));
                var result = md5.Hash;
                var strBuilder = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                {
                    strBuilder.Append(result[i].ToString("x2"));
                }
                return strBuilder.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}