using System;
using System.Security.Cryptography;
using System.Collections;
using System.Text;


public static class MyCryptHelper
{

    public static Random random = new Random();

    public static string cryptographyKey = "e1k2r3e4m5";
    public static string Encrypt(string input)
    {
        Cryptography cryptography = new Cryptography(cryptographyKey);
        return cryptography.Encrypt(input);
    }

    public static string Decrypt(string input)
    {
        try
        {
            Cryptography cryptography = new Cryptography(cryptographyKey);
            return cryptography.Decrypt(input);
        }
        catch
        {
            return "-1";
        }
    }


    public static string GenerateRandomString(int length, Boolean unique)
    {
        string[] chars = {
                "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M",
                "N", "P", "Q", "R", "S", "T", "W", "X", "Y", "Z", "0", "1",
                "2", "3", "4", "5", "6", "7", "8", "9"};

        ArrayList arr = new ArrayList(chars.Length);
        for (int i = 0; i < chars.Length; i++)
            arr.Add(chars[i]);

        string outPut = "";
        int index = 0;
        if (length > 0 && length <= chars.Length)
        {
            for (int i = 0; i < length; i++)
            {
                if (unique == false)
                {
                    outPut += arr[random.Next(1, arr.Count) - 1].ToString();
                }
                else
                {
                    index = random.Next(0, arr.Count - 1);
                    outPut += arr[index].ToString();
                    arr.RemoveAt(index);
                }
            }
        }
        else
            outPut = "";

        return outPut;
    }


    class Cryptography : IDisposable
    {
        private TripleDESCryptoServiceProvider des;
        private MD5CryptoServiceProvider hashmd5;
        private byte[] buff;

        public Cryptography(string key)
        {
            hashmd5 = new MD5CryptoServiceProvider();
            des = new TripleDESCryptoServiceProvider();
            des.Key = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
            des.IV = Encoding.UTF8.GetBytes("NoRePeAt");
            des.Mode = CipherMode.ECB;
            hashmd5 = null;
        }

        public string Encrypt(string inputData)
        {
            buff = Encoding.UTF8.GetBytes(inputData);
            buff = des.CreateEncryptor().TransformFinalBlock(buff, 0, buff.Length);
            string r = Convert.ToBase64String(buff);

            r = r.Replace("=", "(eql)").Replace("+", "(sum)").Replace("&", "(amp)");

            return r;
        }

        public string Decrypt(string inputData)
        {
            string i = inputData.Replace("(eql)", "=").Replace("(sum)", "+").Replace("(amp)", "&");

            buff = Convert.FromBase64String(i);
            buff = des.CreateDecryptor().TransformFinalBlock(buff, 0, buff.Length);
            string encrypted = Encoding.UTF8.GetString(buff);

            return encrypted;
        }


        public static string ConvertStringToMD5(string ClearText)
        {
            byte[] ByteData = Encoding.ASCII.GetBytes(ClearText);

            MD5 oMd5 = MD5.Create();
            byte[] HashData = oMd5.ComputeHash(ByteData);

            StringBuilder oSb = new StringBuilder();
            for (int x = 0; x < HashData.Length; x++)
            {
                oSb.Append(HashData[x].ToString("x2"));
            }
            return oSb.ToString();
        }

       
        void IDisposable.Dispose()
        {
            des = null;
            hashmd5 = null;
            buff = null;
        }
      
    }
}