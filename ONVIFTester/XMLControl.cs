using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace ONVIFTester
{
   
    class XMLControl
    {
        /* Class Method */
        #region CLASS_METHOD
        /* XML Control API */
        public static XmlNamespaceManager getAllNamespaces(XmlDocument doc)
        {
            XmlNamespaceManager result = new XmlNamespaceManager(doc.NameTable);

            IDictionary<string, string> localNamespaces = null;
            XPathNavigator xnav = doc.CreateNavigator();
            while (xnav.MoveToFollowing(XPathNodeType.Element))
            {
                localNamespaces = xnav.GetNamespacesInScope(XmlNamespaceScope.Local);
                foreach (var localNamespace in localNamespaces)
                {
                    string prefix = localNamespace.Key;
                    if (string.IsNullOrEmpty(prefix))
                    { 
                        prefix = "DEFAULT";
                    }
                    result.AddNamespace(prefix, localNamespace.Value);
                }
            }
            return result;
        }

        public static ONVIFDevice findAllXAddr(XmlDocument doc, ONVIFDevice _onvifdev)
        {
            ONVIFDevice onvifdev = _onvifdev;
            XmlNodeList xnode_list = doc.GetElementsByTagName(onvifdev.devicePrefix + ":Capabilities");

            XmlNode xnode = xnode_list[0]; /* capabilities elements */           

            foreach(XmlElement xelement in xnode)
            {
                String[] attribute = xelement.Name.Split(':');
                String tag = attribute[attribute.Length - 1];

                switch(tag)
                {
                    case "Analytics":
                        onvifdev.analysticxaddr = xelement.FirstChild.FirstChild.Value;
                        break;
                    case "Device":
                        onvifdev.devicexaddr = xelement.FirstChild.FirstChild.Value;
                        break;
                    case "Events":
                        onvifdev.eventxaddr = xelement.FirstChild.FirstChild.Value;
                        break;
                    case "Imaging":
                        onvifdev.imagexaddr = xelement.FirstChild.FirstChild.Value;
                        break;
                    case "Media":
                        onvifdev.mediaxaddr = xelement.FirstChild.FirstChild.Value;
                        break;                    
                    case "DeviceIO":
                        onvifdev.devioxaddr = xelement.FirstChild.FirstChild.Value;
                        break;
                    case "Recording":
                        onvifdev.recordxaddr = xelement.FirstChild.FirstChild.Value;
                        break;
                    case "Search":
                        onvifdev.searchxaddr = xelement.FirstChild.FirstChild.Value;
                        break;
                    case "Replay":
                        onvifdev.replayxaddr = xelement.FirstChild.FirstChild.Value;
                        break;
                    default:
                        break;
                }
                
            }
            return onvifdev;
        }
        #endregion
    }
}
