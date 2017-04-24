using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptoExplorer
{
    public static class XorHelper
    {
        private static byte[] _encryptedBytes = null;
        private static int _bytes;
        private static byte[] _iv = DeriveKey();
        public static Bitmap EncryptImage(this string filePath, string keyStreamFilePath)
        {
            try
            {
                byte[] iv=GetKeyStreamBytes(keyStreamFilePath);
                Bitmap bmp = new Bitmap(Bitmap.FromFile(filePath)); // Pick image from file path

                // Lock the bitmap's bits.  
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                System.Drawing.Imaging.BitmapData bmpData =
                    bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                    bmp.PixelFormat);

                // Get the address of the first line.
                IntPtr ptr = bmpData.Scan0;

                // Declare an array to hold the bytes of the bitmap.
                int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
                byte[] contentBytes = new byte[bytes];

                // Copy the content bytes into the array (This does not copy the image header!).
                System.Runtime.InteropServices.Marshal.Copy(ptr, contentBytes, 0, bytes);

                byte[] modifiedBytes = new byte[contentBytes.Length];
                // Modify the bytes

                //for (int i = 0; i < contentBytes.Length; i++) {
                //    modifiedBytes[i] = (byte) (contentBytes[i] ^ _iv[i]);
                //}
                modifiedBytes = Xor(contentBytes, iv);

                _encryptedBytes = modifiedBytes; // Store the encrypted bytes in memory, for later use in decryption routine.
                _bytes = bytes;

                // Copy the modified values back to the bitmap
                System.Runtime.InteropServices.Marshal.Copy(modifiedBytes, 0, ptr, bytes);

                // Unlock the bits.
                bmp.UnlockBits(bmpData);
                return bmp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static byte[] GetKeyStreamBytes(string keyStreamFilePath) {
            Bitmap bmp = new Bitmap(Bitmap.FromFile(keyStreamFilePath)); // Pick image from file path

            // Lock the bitmap's bits.  
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                bmp.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] contentBytes = new byte[bytes];

            // Copy the content bytes into the array (This does not copy the image header!).
            System.Runtime.InteropServices.Marshal.Copy(ptr, contentBytes, 0, bytes);
            return contentBytes;
        }
        private static byte[] DeriveKey()
        {
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[3200000];
                rng.GetBytes(tokenData);
                return tokenData;
            }
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
