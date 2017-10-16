using System;
using System.Net;

namespace CryptoExplorer.PaddingOracleLib
{
    public class OnlineCBCOracle
    {
        //This Oracle is the test Oracle from the Crypto class on Coursera https://www.coursera.org/course/crypto
        //Use at your own responsibility!
        //private string _baseUrl = "http://crypto-class.appspot.com/po?er=";
        private string _baseUrl = "";
        public OnlineCBCOracle(string baseUrl) {
            _baseUrl = baseUrl;
        }

        public bool RequestOracle(byte[] cipher)
        {
            //const string BASE_URL = "http://crypto-class.appspot.com/po?er=";
            //const string BASE_URL = "http://localhost.:1555/Home/compute?secret=";
            string urlData = Helpers.ConvertByteArrayToHexString(cipher);

            WebClient wc = new WebClient();
            WebProxy proxy = new WebProxy();
            proxy.Address = new Uri("http://127.0.0.1:8080");
            wc.Proxy = proxy;
            try
            {
                wc.DownloadData(_baseUrl + urlData);
            }
            catch (WebException e)
            {
                //Invalid padding
                if (e.Message.Contains("403"))
                    return false;

                //Valid padding, but wrong mac
                if (e.Message.Contains("404"))
                    return true;
            }

            //Failed, the oracle is not up!
            return false;
        }
    }
}
