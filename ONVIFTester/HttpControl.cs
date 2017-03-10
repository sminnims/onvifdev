using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace ONVIFTester
{
    class HttpControl
    {
        private String deviceUrl;
        private HttpWebRequest httpReq;
        private HttpWebResponse httpRes;
        private Stream reqStream;
        private Stream resStream;
        public void Connect(String url)
        {
            deviceUrl = url;
            httpReq = (HttpWebRequest)WebRequest.Create(deviceUrl);            
        }

        public void setONVIFHeader(String contentType)
        {
            /* set HttpWenReq object */
            httpReq.Proxy = null;
            httpReq.Method = "POST";
            httpReq.ContentType = "application/soap+xml; charset=utf-8; action=" + contentType;            
            httpReq.Headers.Add("Accept-Encoding", "gzip, deflate");
            httpReq.ServicePoint.Expect100Continue = false;
            httpReq.KeepAlive = false;
        }

        public void Write(byte[] data)
        {
            httpReq.ContentLength = data.Length;

            reqStream = httpReq.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();
        }

        public String Read()
        {
            StringBuilder paramStr = new StringBuilder();

            httpRes = (HttpWebResponse)httpReq.GetResponse();
            resStream = httpRes.GetResponseStream();
            StreamReader streamRead = new StreamReader(resStream);
            
            paramStr.Append(streamRead.ReadToEnd());

            streamRead.Close();            
            resStream.Close();

            return paramStr.ToString();
        }

    }
}
