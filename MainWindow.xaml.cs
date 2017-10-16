using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Drawing = System.Drawing;
using System.Security.Cryptography;
using System.IO;
using System.ComponentModel;
using System.Windows.Controls;
using CryptoExplorer.ImageCryptoLib;
using CryptoExplorer.PaddingOracleLib;

namespace CryptoExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static MainWindow mw;
        private string _bitwisePic1FilePath = String.Empty;
        private string _bitwisePic2FilePath = String.Empty;
        private string _blockFilePath = String.Empty;
        private string _streamSourceFilePath = String.Empty;
        private string _streamKeyfilePath = String.Empty;
        private Drawing.Bitmap _encryptedBitmap;
        private readonly BackgroundWorker bgWorker = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();
            EnableControls(false);
            bgWorker.DoWork += new DoWorkEventHandler(attack_DoWork);
            mw = this;
        }
        private void attack_DoWork(object sender, DoWorkEventArgs e)
        {
            StartAttack();
        }
        public void StartAttack()
        {
            string baseUrl = ReadMessage(txtBaseUrl);
            string cipherHex = ReadMessage(txtCipherHex);
            PrependToLog(txtOracleLogSummary, string.Format("Attack start time: {0}", DateTime.Now));
            PrependToLog(txtOracleLogSummary, String.Format("=================================\n"));
            OnlineCBCOracle cbcOracle = new OnlineCBCOracle(baseUrl);
            PaddingOracleAttacker attacker = new PaddingOracleAttacker(cbcOracle, mw);
            byte[] cipher = Helpers.ConvertHexStringToByteArray(cipherHex);
            
            string plainText = attacker.Decrypt(cipher);
            PrependToLog(txtOracleLogSummary, String.Format("\n\n>>>>>>>>> Decryption result <<<<<<<<<<<:\n{0}\n", plainText));
            PrependToLog(txtOracleLogSummary, String.Format("\n================================="));
            PrependToLog(txtOracleLogSummary, string.Format("Attack end time: {0}", DateTime.Now));
            //Dispatcher.Invoke((Action)(() => btnAttack.IsEnabled = true));
            Dispatcher.Invoke((Action)(UpdateUI));
        }
        void UpdateUI() {
            btnAttack.IsEnabled = true;
        }
        public void PrependToLog(TextBox MessageBox, string Message)
        {
            //Dispatcher.Invoke((Action)(() => MessageBox.AppendText(Message)));
            Dispatcher.Invoke((Action)(() => MessageBox.Text = Message + MessageBox.Text));
        }
        public void AppendToLog(TextBox MessageBox, string Message)
        {
            Dispatcher.Invoke((Action)(() => MessageBox.AppendText(Message)));
        }

        private string ReadMessage(TextBox MessageBox)
        {
            string value="";
            Dispatcher.Invoke((Action)(() => value = MessageBox.Text ));
            return value;
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
                _encryptedBitmap = BlockCipher.EncryptImage(_blockFilePath, mode);
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
                Drawing.Image decryptedImg = BlockCipher.DecryptImage(_encryptedBitmap, mode);
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
        private void EnableControls(bool flag)
        {
            btnComputeBO.IsEnabled = flag;
            btnDisplayBitwiseSource.IsEnabled = flag;
            btnEncrypt.IsEnabled = flag;
            btnDecrypt.IsEnabled = flag;
            btnEncryptDecryptStream.IsEnabled = flag;
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
                _encryptedBitmap = StreamCipher.EncryptImage(_streamSourceFilePath, _streamKeyfilePath, Operation.XOR);
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
                File.WriteAllBytes(saveFileDialog.FileName, BlockCipher.imageToByteArray(_encryptedBitmap));
        }

        private void btnAttack_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtBaseUrl.Text) || string.IsNullOrEmpty(txtCipherHex.Text)) {
                MessageBox.Show("Please enter a base URL and cipher hex");
                return;
            }
            btnAttack.IsEnabled = false;
            txtOracleLogSummary.Text = "";
            txtDecryptedText.Text = "";

            bgWorker.RunWorkerAsync();
        }

        private void btnComputeBO_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((bool)rbBitwiseAnd.IsChecked) {
                    _encryptedBitmap = StreamCipher.EncryptImage(_bitwisePic1FilePath, _bitwisePic2FilePath, Operation.AND);
                }
                else if ((bool)rbBitwiseOR.IsChecked) {
                    _encryptedBitmap = StreamCipher.EncryptImage(_bitwisePic1FilePath, _bitwisePic2FilePath, Operation.OR);
                }
                else if ((bool)rbBitwiseXOR.IsChecked) {
                    _encryptedBitmap = StreamCipher.EncryptImage(_bitwisePic1FilePath, _bitwisePic2FilePath, Operation.XOR);
                }
                SetWpfImageFromImage(_encryptedBitmap, imgBitwiseDisplay);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnBrowseBitwisePic1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
            openFileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;

            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                btnDisplayBitwiseSource.IsEnabled = true;
                _bitwisePic1FilePath = openFileDialog.FileName;
                txtBitwisePic1FileName.Text = _bitwisePic1FilePath;
            }
        }

        private void btnBrowseBitwisePic2_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
            openFileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;

            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                btnComputeBO.IsEnabled = true;
                _bitwisePic2FilePath = openFileDialog.FileName;
                txtBitwisePic2FileName.Text = _bitwisePic2FilePath;
            }
        }

        private void btnDisplayBitwiseSource_Click(object sender, RoutedEventArgs e)
        {
            SetWpfImageFromPath(_bitwisePic1FilePath, imgBitwiseDisplay);
        }
    }
}