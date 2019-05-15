using System;
using System.Security.Cryptography;
using System.Text;

namespace Jcr.Api.Helpers
{
    public static class CryptHelpers
    {
        public static string Decrypt(string base64Text, string encryptionKey)
        {
            string str;
            bool goodString = base64Text.Trim().Length > 10
                              || base64Text.Trim().Substring(base64Text.Trim().Length - 1, 1) == "=";

            if (goodString)
            {
                using (var provider = new TripleDESCryptoServiceProvider())
                {
                    provider.Key = new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(encryptionKey));
                    provider.Mode = CipherMode.ECB;
                    ICryptoTransform transform = provider.CreateDecryptor();
                    base64Text = base64Text.Replace(@" ", "+");
                    byte[] inputBuffer = Convert.FromBase64String(base64Text);
                    str = Encoding.ASCII.GetString(transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length));
                }
            }
            else
            {
                str = string.Empty;
            }

            return str;
        }

        public static string Encrypt(string plainText, string encryptionKey)
        {
            string str;
            if (plainText.Trim().Length > 0)
            {
                using (var provider = new TripleDESCryptoServiceProvider())
                {
                    provider.Key = new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(encryptionKey));
                    provider.Mode = CipherMode.ECB;
                    ICryptoTransform transform = provider.CreateEncryptor();
                    byte[] bytes = Encoding.ASCII.GetBytes(plainText);
                    str = Convert.ToBase64String(transform.TransformFinalBlock(bytes, 0, bytes.Length));
                }
            }
            else
            {
                str = string.Empty;
            }

            return str;
        }
    }
}