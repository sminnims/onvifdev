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
        #endregion
    }
}
