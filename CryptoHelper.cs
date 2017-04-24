using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptoExplorer
{
    public static class CryptoHelper
    {
        private static string _masterKeyStr = String.Empty;
        private static string _ivStr=String.Empty;
        private static byte[] _encryptedBytes = null;
        private static int _bytes;

        public static byte[] imageToByteArray(this System.Drawing.Bitmap image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public static Image byteArrayToImage(this byte[] byteArrayIn)
        {
            using (var ms = new MemoryStream(byteArrayIn))
            {
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }
        }
        /// <summary>
        /// Reads an image from filepath and encrypts its pixel content (This does not encrypt the image header!)
        /// Refer MSDN article on BitmapData class for details about reading & writing pixes in an image.
        /// Link: https://msdn.microsoft.com/en-us/library/system.drawing.imaging.bitmapdata.aspx#Examples
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Bitmap EncryptImage(this string filePath, CipherMode cipherMode) {
            try
            {
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

                byte[] modifiedBytes = null;
                // Modify the bytes
                
                switch (cipherMode) {
                    case CipherMode.CBC:
                        modifiedBytes = CryptoHelper.Encrypt(contentBytes, CipherMode.CBC);
                        break;
                    case CipherMode.ECB:
                        modifiedBytes = CryptoHelper.Encrypt(contentBytes, CipherMode.ECB);
                        break;
                    default: break;
                }
                                
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

        public static Image DecryptImage(this Bitmap bmp, CipherMode cipherMode)
        {
            try
            {
                // Lock the bitmap's bits.  
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                System.Drawing.Imaging.BitmapData bmpData =
                    bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                    bmp.PixelFormat);

                // Get the address of the first line.
                IntPtr ptr = bmpData.Scan0;
                
                // Fetch the bytes to modify from the content stored during encryption
                int bytes = _bytes;
                byte[] contentBytes = _encryptedBytes;

                // Copy the content bytes into the array (This does not copy the image header!).
                // System.Runtime.InteropServices.Marshal.Copy(ptr, contentBytes, 0, bytes);

                // Modify the bytes
                byte[] modifiedBytes = null;
                // Modify the bytes

                switch (cipherMode)
                {
                    case CipherMode.CBC:
                        modifiedBytes = CryptoHelper.Decrypt(contentBytes, CipherMode.CBC);
                        break;
                    case CipherMode.ECB:
                        modifiedBytes = CryptoHelper.Decrypt(contentBytes, CipherMode.ECB);
                        break;
                    default: break;
                }

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

        public static byte[] Encrypt(byte[] plainTextAsBytes, CipherMode cipherMode)
        {
            using (RijndaelManaged myRijndael = new RijndaelManaged { Mode = cipherMode, Padding=PaddingMode.PKCS7 })
            {
                myRijndael.GenerateKey();
                myRijndael.GenerateIV();
                // store key and IV for decryption
                _masterKeyStr = Convert.ToBase64String(myRijndael.Key);
                _ivStr = Convert.ToBase64String(myRijndael.IV);

                ICryptoTransform encryptor = myRijndael.CreateEncryptor(myRijndael.Key, myRijndael.IV);


                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(plainTextAsBytes, 0, plainTextAsBytes.Length);
                        csEncrypt.FlushFinalBlock();
                        return msEncrypt.ToArray();
                    }
                }
            }
        }

        public static byte[] Decrypt(byte[] cipherTextAsBytes, CipherMode cipherMode)
        {
            using (RijndaelManaged myRijndael = new RijndaelManaged { Mode = cipherMode, Padding = PaddingMode.PKCS7 })
            {
                myRijndael.Key = Convert.FromBase64String(_masterKeyStr);
                myRijndael.IV = Convert.FromBase64String(_ivStr);
                
                ICryptoTransform decryptor = myRijndael.CreateDecryptor(myRijndael.Key, myRijndael.IV);

                var outputStream = new MemoryStream();
                var cryptoStream = new CryptoStream(
                    outputStream,
                    decryptor,
                    CryptoStreamMode.Write);

                cryptoStream.Write(cipherTextAsBytes, 0, cipherTextAsBytes.Length);
                cryptoStream.FlushFinalBlock();

                byte[] plainTextAsBytes = outputStream.ToArray();
                return plainTextAsBytes;
            }
        }

        private static byte[] DeriveKey(string purpose, byte[] masterKey, int keySize)
        {
            var kdf = new Rfc2898DeriveBytes
                (masterKey,
                Encoding.Unicode.GetBytes(purpose),
                10);

            return kdf.GetBytes(keySize);
        }
    }
}
