using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Drawing = System.Drawing;
using System.Security.Cryptography;
using System.IO;

namespace CryptoExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _blockFilePath = String.Empty;
        private string _streamSourceFilePath = String.Empty;
        private string _streamKeyfilePath = String.Empty;
        private Drawing.Bitmap _encryptedBitmap;
        public MainWindow()
        {
            InitializeComponent();
            EnableControls(false);
        }
        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
            openFileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;

            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true) {
                btnEncrypt.IsEnabled = true;
                _blockFilePath = openFileDialog.FileName; 
                txtFileName.Text = _blockFilePath;
                SetWpfImageFromPath(_blockFilePath, imgDisplay);
            }
        }        
        private void btnEncrypt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CipherMode mode= CipherMode.CBC;
                if ((bool)rbEncryptionModeCBC.IsChecked) { mode = CipherMode.CBC; }
                else if ((bool)rbEncryptionModeECB.IsChecked) { mode = CipherMode.ECB; }
                _encryptedBitmap = CryptoHelper.EncryptImage(_blockFilePath, mode);
                SetWpfImageFromImage(_encryptedBitmap, imgDisplay);
                btnDecrypt.IsEnabled = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void btnDecrypt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CipherMode mode = CipherMode.CBC;
                if ((bool)rbEncryptionModeCBC.IsChecked) { mode = CipherMode.CBC; }
                else if ((bool)rbEncryptionModeECB.IsChecked) { mode = CipherMode.ECB; }
                Drawing.Image decryptedImg = CryptoHelper.DecryptImage(_encryptedBitmap, mode);
                SetWpfImageFromImage(decryptedImg, imgDisplay);
                btnDecrypt.IsEnabled = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void SetWpfImageFromPath(string filePath, System.Windows.Controls.Image imgControl)
        {
            BitmapImage bitmapImg = new BitmapImage();
            bitmapImg.BeginInit();
            bitmapImg.UriSource = new Uri(filePath, UriKind.Absolute);
            bitmapImg.EndInit();
            imgControl.Source = bitmapImg;
        }
        private void SetWpfImageFromImage(Drawing.Image img, System.Windows.Controls.Image imgControl) {
            using (var ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                BitmapImage bitmapImg = new BitmapImage();
                bitmapImg.BeginInit();
                bitmapImg.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImg.StreamSource = ms;
                bitmapImg.EndInit();
                imgControl.Source = bitmapImg;
            }
        }
        private void EnableControls(bool isEnabled)
        {
            btnEncrypt.IsEnabled = isEnabled;
            btnDecrypt.IsEnabled = isEnabled;
            btnEncryptDecryptStream.IsEnabled = isEnabled;
        }

        private void btnBrowseSourcePic_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
            openFileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;

            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                //btnEncryptStream.IsEnabled = true;
                _streamSourceFilePath = openFileDialog.FileName;
                txtFileNameSourcePic.Text = _streamSourceFilePath;
                //SetWpfImageFromPath(_filePath, imgDisplayStream);
            }
        }

        private void btnBrowseKeyStream_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
            openFileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;

            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                btnEncryptDecryptStream.IsEnabled = true;
                _streamKeyfilePath = openFileDialog.FileName;
                txtFileNameKeyStream.Text = _streamKeyfilePath;
                //SetWpfImageFromPath(_filePath, imgDisplayStream);
            }
        }
        private void btnEncryptDecryptStream_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _encryptedBitmap = XorHelper.EncryptImage(_streamSourceFilePath, _streamKeyfilePath);
                SetWpfImageFromImage(_encryptedBitmap, imgDisplayStream);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void btnSaveStream_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
            saveFileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;

            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllBytes(saveFileDialog.FileName, CryptoHelper.imageToByteArray(_encryptedBitmap));
        }
    }
}