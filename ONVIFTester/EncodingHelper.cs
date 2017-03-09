using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONVIFTester
{
    class EncodingHelper
    {
        public static String Byte2String(byte[] bStr, Encoding eType = null)
        {
            if (eType == null)
            {
                eType = Encoding.UTF8;
            }

            return eType.GetString(bStr);
        }

        public static byte[] String2Byte(String str, Encoding eType = null)
        {
            if (eType == null)
            {
                eType = Encoding.UTF8;
            }

            return eType.GetBytes(str);
        }
        public static String Base64Encode(String str, Encoding eType = null)
        {
            if (eType == null)
            {
                eType = Encoding.UTF8;
            }

            byte[] sByte = eType.GetBytes(str);

            return System.Convert.ToBase64String(sByte);
        }

        public static byte[] Base64Decode(String str)
        {
            return System.Convert.FromBase64String(str);
        }
    }
}
