using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace MiniFramework
{
    public static class Decrypt
    {
        public static byte[] AES(byte[] encryptData, string key)
        {
            try
            {
                SymmetricAlgorithm aes = Rijndael.Create();
                aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32, '0').Substring(0, 32));
                aes.IV = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
                using (MemoryStream ms = new MemoryStream())
                {
                    CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);
                    cs.Write(encryptData, 0, encryptData.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static string DES(byte[] encryptData, string key)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Key = Encoding.UTF8.GetBytes(key.PadRight(8, '0').Substring(0, 8));
                des.IV = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
                using (MemoryStream ms = new MemoryStream())
                {
                    CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                    cs.Write(encryptData, 0, encryptData.Length);
                    cs.FlushFinalBlock();
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static string RSA(byte[] encryptData, string xmlPrivateKey)
        {
            try
            {
                CspParameters cp = new CspParameters();
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cp);
                rsa.FromXmlString(xmlPrivateKey);
                var decryptBytes = rsa.Decrypt(encryptData, false);
                return Encoding.UTF8.GetString(decryptBytes);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}