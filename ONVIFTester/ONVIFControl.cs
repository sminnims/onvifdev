using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace ONVIFTester
{
    class wsdUsernameToken
    {
        public String Username { get; set; }
        public String Nonce { get; set; }
        public String Create { get; set; }
        public String plainPassword { get; set; }
        public String Password { get; set; }
        public String PasswordType { get; set; }
    }

    class ONVIFDevice
    {
        public DateTime timeoffset { get; set; }
        public String model { get; set; }
        public String serial { get; set; }
        public String prefixSchema { get; set; }
        public XmlDocument capabilities { get; set; }
        public String analysticxaddr { get; set; }
        public String devicexaddr { get; set; }
        public String eventxaddr { get; set; }
        public String imagexaddr { get; set; }
        public String devioxaddr { get; set; }
        public String mediaxaddr { get; set; }
        public String recordxaddr { get; set; }
        public String searchxaddr { get; set; }
        public String replayxaddr { get; set; }
    }

    class ONVIFControl
    {
        private ONVIFDevice onvifdev;
        private wsdUsernameToken _wsdUsernameToken;
        private String deviceURL;
        private String onvifPrefixSchema;
        private Form1 parentForm;

        private HttpHandler _http;

        private DateTime startDate;
        private DateTime baseDateTime;
        public ONVIFControl(Form1 _form)
        {
            onvifdev = new ONVIFDevice();
            _wsdUsernameToken = new wsdUsernameToken();
            _http = new HttpHandler();
            parentForm = _form;
        }
        /* wsdUsernameToken I/F */
        #region wsdUsernameToken
        public void setUsername(String str)
        {
            if (str == null)
                str = "root";
            _wsdUsernameToken.Username = str;
        }
        public void setPassword(String str)
        {
            if (str == null)
                str = "password";
            _wsdUsernameToken.plainPassword = str;
        }
        public void setNonce()
        {
            _wsdUsernameToken.Nonce = EncodingHelper.Base64Encode(DigestPassword.getNonce(32));
        }
        #endregion
        /* wsdUsernameToken I/F */

        /* global value get/set I/F */
        #region valueSettingI/F
        public void setDeviceUrl(String str)
        {
            if (str == null)
                str = "127.0.0.1";
            deviceURL = str;
        }

        #endregion
        /* global value get/set I/F */

        /* ONVIF Control API */
        #region ONVIFControlAPI
        public void ONVIF_GetSystemDateAndTime()
        {
            XmlDocument soapEnvXml = new XmlDocument();

            /* set packet */
            StringBuilder reqMessage = new StringBuilder();
            reqMessage.Append("<s:Envelope xmlns:s=\"http://www.w3.org/2003/05/soap-envelope\">");
            reqMessage.Append("<s:Body xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
            reqMessage.Append("<GetSystemDateAndTime xmlns=\"http://www.onvif.org/ver10/device/wsdl\"/></s:Body></s:Envelope>");

            /* encoding */
            byte[] reqMessageBinary = EncodingHelper.String2Byte(reqMessage.ToString());

            /* Get Url */
            String Url = deviceURL + "/onvif/device_service";           

            _http.Connect(Url);

            _http.setONVIFHeader("\"http://www.onvif.org/ver10/device/wsdl/GetSystemDateAndTime\"");
            _http.Write(reqMessageBinary);

            String paramStr = _http.Read();
            
            /* parser soap response */
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(paramStr.ToString());
            XmlElement xelements = xdoc.DocumentElement;
            XmlAttributeCollection xAttribute = xdoc.DocumentElement.Attributes;
                       
            onvifPrefixSchema = "tds2"; /* set default to canon prefix */
            /* need to find prefix of namespace:/ver10/schema */
            foreach (XmlAttribute attr in xAttribute)
            {
                /* need to find  prefix schema of "http://www.onvif.org/ver10/schema" */
                bool result = attr.Value.Equals("http://www.onvif.org/ver10/schema", StringComparison.Ordinal);

                if (result == true)
                {
                    String[] split_word = attr.Name.Split(':');
                    onvifPrefixSchema = split_word[(split_word.Length) - 1];
                    break;
                }
            }
            onvifdev.prefixSchema = onvifPrefixSchema;
#if false
            /* it doesn't work.... why??? */
            XmlNamespaceManager xmlns = new XmlNamespaceManager(xdoc.NameTable);
            
            String tt = xmlns.LookupPrefix("http://www.onvif.org/ver10/schema");
            String schemaNs = xmlns.LookupNamespace("xmlns");            
#endif
            try
            {
                /* Onvif Application Programmers Guide - Created – The UtcTime when the request is made. */
                XmlNodeList xnode_list = xdoc.GetElementsByTagName(onvifPrefixSchema + ":UTCDateTime");

                XmlNode time_node = xnode_list[0].FirstChild;
                XmlNode date_node = xnode_list[0].LastChild;

                String _hour = time_node[onvifPrefixSchema + ":Hour"].InnerText;
                String _min = time_node[onvifPrefixSchema + ":Minute"].InnerText;
                String _sec = time_node[onvifPrefixSchema + ":Second"].InnerText;

                String _year = date_node[onvifPrefixSchema + ":Year"].InnerText;
                String _month = date_node[onvifPrefixSchema + ":Month"].InnerText;
                String _day = date_node[onvifPrefixSchema + ":Day"].InnerText;

                DateTime currentTime = new DateTime(Int32.Parse(_year), Int32.Parse(_month), Int32.Parse(_day)
                                               , Int32.Parse(_hour), Int32.Parse(_min), Int32.Parse(_sec));

                /* deviceTime is UTC/GMT time */
                baseDateTime = currentTime;
                onvifdev.timeoffset = baseDateTime;
                /* save current time for calcurate passedTime */
                startDate = DateTime.Now;
                parentForm.settbLogBox("\r\nFunction OK : " + System.Reflection.MethodBase.GetCurrentMethod().Name + "\n");
            }
            catch (NullReferenceException nre)
            {
                parentForm.settbLogBox("\r\nNull 참조 에러 : " + nre.Message + "\n" + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public void ONVIF_GetCapabilities()
        {
            /* get synced-time using base time */
            baseDateTime = DigestPassword.getCurrentTime(startDate, baseDateTime);
            /* make createTime string */
            _wsdUsernameToken.Create = DigestPassword.getCreatedTimeString(baseDateTime);
            /* make digest password */
            //_wsdUsernameToken.Password = DigestPassword.getPasswordDigest("LKqI6G/AikKCQrN0zqZFlg==", "2010-09-16T07:50:45Z", "userpassword");            
            _wsdUsernameToken.Password = DigestPassword.getPasswordDigest(_wsdUsernameToken.Nonce,
                                                                          _wsdUsernameToken.Create,
                                                                          _wsdUsernameToken.plainPassword);
            /* set packet */
            StringBuilder reqMessage = new StringBuilder();
            reqMessage.Append("<s:Envelope xmlns:s=\"http://www.w3.org/2003/05/soap-envelope\">");
            reqMessage.Append("<s:Header>");
            reqMessage.Append("<Security ");
            reqMessage.Append("s:mustUnderstand=\"1\" ");
            reqMessage.Append("xmlns=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">");
            reqMessage.Append("<UsernameToken><Username>" + _wsdUsernameToken.Username + "</Username>");
            reqMessage.Append("<Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordDigest\">" + _wsdUsernameToken.Password + "</Password>");
            reqMessage.Append("<Nonce EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\">" + _wsdUsernameToken.Nonce + "</Nonce>");
            reqMessage.Append("<Created xmlns=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\">" + _wsdUsernameToken.Create + "</Created>");
            reqMessage.Append("</UsernameToken></Security></s:Header>");
            reqMessage.Append("<s:Body xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
            reqMessage.Append("<GetCapabilities xmlns=\"http://www.onvif.org/ver10/device/wsdl\">");
            reqMessage.Append("<Category>All</Category></GetCapabilities></s:Body></s:Envelope>");

            /* encoding */
            byte[] reqMessageBinary = EncodingHelper.String2Byte(reqMessage.ToString());

            /* Get Url */
            String Url = deviceURL + "/onvif/device_service";
            try
            {
                _http.Connect(Url);
                _http.setONVIFHeader("\"http://www.onvif.org/ver10/device/wsdl/GetCapabilities\"");
                _http.Write(reqMessageBinary);

                String paramStr = _http.Read();
                parentForm.settbLogBox("\r\nFunction OK : " + System.Reflection.MethodBase.GetCurrentMethod().Name + "\n");

                /* xml parsing */
                XDocument xdoc = XDocument.Parse(paramStr);

                var elements = xdoc.Descendants("Device").Elements("XAddr");

                var value = from element in elements
                                       select element.Value;


            }
            catch(XmlException xe)
            {
                parentForm.settbLogBox("\r\nXml Exception : " + xe.Message + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            catch (WebException we)
            {
                parentForm.settbLogBox("\r\nWeb Exception : " + we.Message + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        #endregion
        /* ONVIF Control API */
    }
}
