using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptoExplorer.PaddingOracleLib
{
    public static class Helpers
    {

        public static string ConvertByteArrayToHexString(byte[] data)
        {
            string hex = "";
            foreach (byte c in data)
            {
                int tmp = c;
                hex += String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));
            }
            return hex;
        }

        public static string ConvertByteArrayToUTF8String(byte[] data)
        {
            return System.Text.Encoding.UTF8.GetString(data);
        }

        public static string ConvertUTF8StringToHex(string asciiString)
        {
            string hex = "";
            foreach (char c in asciiString)
            {
                int tmp = c;
                hex += String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));
            }
            return hex;
        }

        public static byte[] ConvertUTF8StringToByteArray(string asciiString)
        {
            return System.Text.Encoding.UTF8.GetBytes(asciiString);
        }

        public static string ConvertHexStringToUTF8String(string hexValue)
        {
            string strValue = "";
            while (hexValue.Length > 0)
            {
                char c = System.Convert.ToChar(System.Convert.ToUInt32(hexValue.Substring(0, 2), 16));
                if (c > 255)
                    throw new Exception();

                strValue += c.ToString();
                hexValue = hexValue.Substring(2, hexValue.Length - 2);
            }
            return strValue;
        }

        public static byte[] ConvertHexStringToByteArray(string hexValue)
        {
            byte[] res = new byte[hexValue.Length / 2];
            int i = 0;

            while (hexValue.Length > 0)
            {
                char c = System.Convert.ToChar(System.Convert.ToUInt32(hexValue.Substring(0, 2), 16));
                if (c > 255)
                    throw new Exception();

                res[i++] = (byte)c;
                hexValue = hexValue.Substring(2, hexValue.Length - 2);
            }
            return res;
        }

        //Performs byte-wise XOR between elements of @a and @b
        public static byte[] Xor(byte[] a, byte[] b)
        {
            if (a == null || b == null)
                return null;

            int len = Math.Max(a.Length, b.Length);

            byte[] res = new byte[len];

            if (a.Length > b.Length)
            {
                a.CopyTo(res, 0);
                for (int i = 0; i < b.Length; i++)
                {
                    res[i] ^= b[i];
                }
            }
            else
            {
                b.CopyTo(res, 0);
                for (int i = 0; i < a.Length; i++)
                {
                    res[i] ^= a[i];
                }
            }

            return res;
        }

    }
}