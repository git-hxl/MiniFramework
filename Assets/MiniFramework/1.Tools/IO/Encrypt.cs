using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
public static class Encrypt
{
    public static byte[] AES(byte[] normalData, string key)
    {
        var bytes = normalData;
        SymmetricAlgorithm aes = Rijndael.Create();
        aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32, '0').Substring(0, 32));
        aes.IV = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
        using (MemoryStream ms = new MemoryStream())
        {
            CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(bytes, 0, bytes.Length);
            cs.FlushFinalBlock();
            return ms.ToArray();
        }
    }
    public static byte[] DES(byte[] normalData, string key)
    {
        var bytes = normalData;
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        des.Key = Encoding.UTF8.GetBytes(key.PadRight(8, '0').Substring(0, 8));
        des.IV = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
        using (MemoryStream ms = new MemoryStream())
        {
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(bytes, 0, bytes.Length);
            cs.FlushFinalBlock();
            return ms.ToArray();
        }
    }
    public static byte[] RSA(byte[] normalData, string xmlPublicKey)
    {
        var bytes = normalData;
        CspParameters cp = new CspParameters();
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cp);
        rsa.FromXmlString(xmlPublicKey);
        byte[] encryptBytes = rsa.Encrypt(bytes, false);
        return encryptBytes;
    }
    public static byte[] MD5(byte[] normalData)
    {
        byte[] bytes = normalData;
        byte[] md5 = new MD5CryptoServiceProvider().ComputeHash(bytes);
        return md5;
    }
    /// <summary>
    /// RSA产生密钥
    /// </summary>
    /// <param name="xmlPrivateKey">私钥</param>
    /// <param name="xmlPublicKey">公钥</param>
    public static void GenerateKey(out string xmlPrivateKey, out string xmlPublicKey)
    {
        try
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            xmlPrivateKey = rsa.ToXmlString(true);
            xmlPublicKey = rsa.ToXmlString(false);
        }
        catch (Exception e)
        {
            throw e;
        }
    }
}
