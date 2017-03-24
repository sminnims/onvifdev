using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

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
        /* namespace prefix */
        public String schemaPrefix { get; set; }
        public String devicePrefix { get; set; }

        /* element */
        public XmlNode capabilitiesNode { get; set; }
        public XmlNode profilesNode { get; set; }
        public XmlNode systemlogNode { get; set; }
        /* node struct */
        //public ONVIFCapabilities capabilities { get; set; }

        /* command xaddr */
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

#if false // test 
    class ONVIFCapabilities
    {
        ONVIFCapaAnalystics Analytics;
        ONVIFCapaDevice Device;
        ONVIFCapaEvents Events;
        ONVIFCapaImaging Imaging;
        ONVIFCapaMedia Media;
        ONVIFCapaExtension Extension;
    }
    class ONVIFCapaSubElement
    {
        public String XAddr { get; set; }

    }
    class ONVIFCapaAnalystics : ONVIFCapaSubElement
    {
        bool RuleSupport { get; set; }
        bool AnalyticsModuleSupport { get; set; }
    }
    class ONVIFCapaDevice : ONVIFCapaSubElement
    {
        ONVIFCapaNetwork Network { get; set;}
        ONVIFCapaSystem System { get; set; }
        ONVIFCapaIO IO { get; set; }

    }
    class ONVIFCapaNetwork
    {
        bool IPFilter { get; set; }
        bool ZeroConfiguration { get; set; }
        bool IPVersion { get; set; }
        bool DynDNS { get; set; }
        /* extension */
        bool Dot11Configuration { get; set; }
    }
    class ONVIFCapaSystem
    {
        bool DiscoveryResolve { get; set; }
        bool DiscoveryBye { get; set; }
        bool RemoteDiscovery { get; set; }
        bool SystemBackup { get; set; }
        bool SystemLogging { get; set; }
        bool FirmwareUpgrade { get; set; }
        Int32 SupportedVersion { get; set; }
        /* extension */
        bool HttpFirmwareUpgrade { get; set; }
        bool HttpSystemBackup { get; set; }
        bool HttpSystemLogging { get; set; }
        bool HttpSupportInformation { get; set; }
    }
    class ONVIFCapaIO
    {
        Int32 InputConnectors { get; set; }
        Int32 RelayOutputs { get; set; }
        bool Auxiliary { get; set; }

    }
    class ONVIFCapaSecurity
    {
        bool TLS1Dot1 { get; set; }
        bool TLS1Dot2 { get; set; }
        bool OnboardKeyGeneration { get; set; }
        bool AccessPolicyConfig { get; set; }
        bool XDot509Token { get; set; }
        bool SAMLToken { get; set; }
        bool KerberosToken { get; set; }
        bool RELToken { get; set; }
        /* extension of Security */
        bool TLS1Dot0 { get; set; }
        /* extension of TLS1.0 */
        bool Dot1X { get; set; }
        List<Int32> SupportedEAPMethod { get; set; }
        bool RemoteUserHandling { get; set; }
    }
    class ONVIFCapaEvents : ONVIFCapaSubElement
    {
        bool WSSubscriptionPolicySupport { get; set; }
        bool WSPullPointSupport { get; set; }
        bool WSPausableSubscriptionManagerInterfaceSupport { get; set; }
    }
    class ONVIFCapaImaging : ONVIFCapaSubElement { }

    class ONVIFCapaMedia : ONVIFCapaSubElement
    {
        ONVIFCapaStreamingCpapa Streaming { get; set; }
        /* extension */
        ONVIFCapaProfileCapa Profile { get; set; }
    }
    class ONVIFCapaStreamingCpapa
    {
        bool RTPMulticast { get; set; }
        bool RTP_TCP { get; set; }
        bool RTP_RTSP_TCP { get; set; }
    }
    class ONVIFCapaProfileCapa
    {
        Int32 MaximumNumberOfProfiles { get; set; }
    }

    class ONVIFCapaExtension
    {
        ONVIFCapaDeviceIO DeviceIO { get; set; }
        ONVIFCapaRecording Recording { get; set; }
        ONVIFCapaSearch Search { get; set; }
        ONVIFCapaReplay Replay { get; set; }
    }

    class ONVIFCapaDeviceIO : ONVIFCapaSubElement
    {
        Int32 VideoSources { get; set; }
        Int32 VideoOutputs { get; set; }
        Int32 AudioSources { get; set; }
        Int32 AudioOutputs { get; set; }
        Int32 RelayOutputs { get; set; }
    }
    class ONVIFCapaRecording : ONVIFCapaSubElement
    {
        bool ReceiverSource { get; set; }
        bool MediaProfileSource { get; set; }
        bool DynamicRecordings { get; set; }
        bool DynamicTracks { get; set; }
        Int32 MaxStringLength { get; set; }
    }
    class ONVIFCapaSearch : ONVIFCapaSubElement
    {
        bool MetadataSearch { get; set; }        
    }
    class ONVIFCapaReplay : ONVIFCapaSubElement { }
#endif
    class ONVIFNamespace
    {
        public static String schema = "http://www.onvif.org/ver10/schema";
        public static String device = "http://www.onvif.org/ver10/device/wsdl";        
    }
    class ONVIFControl : IDisposable
    {
        private ONVIFDevice onvifdev;
        private wsdUsernameToken _wsdUsernameToken;
        private String deviceURL;
        private String schemaPrefix;
        private Connection parentForm;

        private HttpControl _http;

        private DateTime startDate;
        private DateTime baseDateTime;

        private bool isDispose = false;

        public ONVIFControl(Connection _form)
        {
            onvifdev = new ONVIFDevice();
            _wsdUsernameToken = new wsdUsernameToken();
            _http = new HttpControl();
            parentForm = _form;
        }

        ~ONVIFControl()
        {
            if (!isDispose)
            {
                Dispose();
            }   
        }

        public void Dispose()
        {
            isDispose = true;
            GC.SuppressFinalize(this);
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

            XmlNamespaceManager xnamespace = XMLControl.getAllNamespaces(xdoc);
            onvifdev.schemaPrefix = xnamespace.LookupPrefix(ONVIFNamespace.schema);
            /* test */
#if false
            XmlElement xelements = xdoc.DocumentElement;
            XmlAttributeCollection xAttribute = xdoc.DocumentElement.Attributes;
            /* need to find prefix of namespace:/ver10/schema */
            foreach (XmlAttribute attr in xAttribute)
            {
                /* need to find  prefix schema of "http://www.onvif.org/ver10/schema" */
                bool result = attr.Value.Equals("http://www.onvif.org/ver10/schema", StringComparison.Ordinal);

                if (result == true)
                {
                    String[] split_word = attr.Name.Split(':');
                    schemaPrefix = split_word[(split_word.Length) - 1];
                    break;
                }
            }
#endif
            try
            {
                /* Onvif Application Programmers Guide - Created – The UtcTime when the request is made. */
                XmlNodeList xnode_list = xdoc.GetElementsByTagName(onvifdev.schemaPrefix + ":UTCDateTime");

                XmlNode time_node = xnode_list[0].FirstChild;
                XmlNode date_node = xnode_list[0].LastChild;

                String _hour = time_node[onvifdev.schemaPrefix + ":Hour"].InnerText;
                String _min = time_node[onvifdev.schemaPrefix + ":Minute"].InnerText;
                String _sec = time_node[onvifdev.schemaPrefix + ":Second"].InnerText;

                String _year = date_node[onvifdev.schemaPrefix + ":Year"].InnerText;
                String _month = date_node[onvifdev.schemaPrefix + ":Month"].InnerText;
                String _day = date_node[onvifdev.schemaPrefix + ":Day"].InnerText;

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
            setNonce();
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

                /* parser soap response */
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(paramStr.ToString());

                XmlNamespaceManager xnamespace = XMLControl.getAllNamespaces(xdoc);
                onvifdev.schemaPrefix = xnamespace.LookupPrefix(ONVIFNamespace.schema);
                onvifdev.devicePrefix = xnamespace.LookupPrefix(ONVIFNamespace.device);

                /* get function xaddr in document */
                onvifdev = XMLControl.findAllXAddr(xdoc, onvifdev);
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

        public void ONVIF_GetProfiles()
        {
            setNonce();
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
            reqMessage.Append("<GetProfiles xmlns=\"http://www.onvif.org/ver10/media/wsdl\"/>");
            reqMessage.Append("</s:Body></s:Envelope>");

            /* encoding */
            byte[] reqMessageBinary = EncodingHelper.String2Byte(reqMessage.ToString());

            /* Get Url */
            String Url = onvifdev.mediaxaddr;
            try
            {
                _http.Connect(Url);
                _http.setONVIFHeader("\"http://www.onvif.org/ver10/device/wsdl/GetProfiles\"");
                _http.Write(reqMessageBinary);

                String paramStr = _http.Read();
                parentForm.settbLogBox("\r\nFunction OK : " + System.Reflection.MethodBase.GetCurrentMethod().Name + "\n");

                /* parser soap response */
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(paramStr.ToString());

                XmlNamespaceManager xnamespace = XMLControl.getAllNamespaces(xdoc);
                onvifdev.schemaPrefix = xnamespace.LookupPrefix(ONVIFNamespace.schema);
                onvifdev.devicePrefix = xnamespace.LookupPrefix(ONVIFNamespace.device);
                /* get function xaddr in document */
            }
            catch (XmlException xe)
            {
                parentForm.settbLogBox("\r\nXml Exception : " + xe.Message + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            catch (WebException we)
            {
                parentForm.settbLogBox("\r\nWeb Exception : " + we.Message + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        public String ONVIF_GetSystemLog()
        {
            setNonce();
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
            reqMessage.Append("<GetSystemLog xmlns=\"http://www.onvif.org/ver10/device/wsdl\"><LogType>System</LogType></GetSystemLog></s:Body></s:Envelope>");

            /* encoding */
            byte[] reqMessageBinary = EncodingHelper.String2Byte(reqMessage.ToString());

            /* Get Url */
            String Url = onvifdev.devicexaddr;
            try
            {
                _http.Connect(Url);
                _http.setONVIFHeader("\"http://www.onvif.org/ver10/device/wsdl/GetSystemLog\"");
                _http.Write(reqMessageBinary);

                String paramStr = _http.Read();
                parentForm.settbLogBox("\r\nFunction OK : " + System.Reflection.MethodBase.GetCurrentMethod().Name + "\n");

                /* parser soap response */
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(paramStr.ToString());

                XmlNamespaceManager xnamespace = XMLControl.getAllNamespaces(xdoc);
                onvifdev.schemaPrefix = xnamespace.LookupPrefix(ONVIFNamespace.schema);
                onvifdev.devicePrefix = xnamespace.LookupPrefix(ONVIFNamespace.device);
                /* get function xaddr in document */
                /*
                 *   <SOAP-ENV:Header></SOAP-ENV:Header>
                      <SOAP-ENV:Body>
                        <tds:GetSystemLogResponse>
                          <tds:SystemLog>
                            <tds2:String>Mar 13 08:08:27 VB-
                */
                String logStr = XMLControl.getLogStr(xdoc, onvifdev);
                return logStr;

            }
            catch (XmlException xe)
            {
                parentForm.settbLogBox("\r\nXml Exception : " + xe.Message + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            catch (WebException we)
            {
                parentForm.settbLogBox("\r\nWeb Exception : " + we.Message + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            return "test";
        }
#endregion
        /* ONVIF Control API */
            }
}
