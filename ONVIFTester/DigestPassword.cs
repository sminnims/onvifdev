using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ONVIFTester
{
    class DigestPassword
    {
        private static String validChars = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static Random randomNum = new Random();

        public static String getNonce(int length)
        {
            var nonceString = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                nonceString.Append(validChars[randomNum.Next(0, validChars.Length - 1)]);
            }

            return nonceString.ToString();
        }

        public static DateTime getCurrentTime(DateTime startDate, DateTime baseDateTime)
        {
            /* get synced-time */
            try
            { 
                DateTime currentDate = DateTime.Now;
                long elapsedTicks = currentDate.Ticks - startDate.Ticks;

                TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);

                baseDateTime = baseDateTime.AddTicks(elapsedTicks);
            }
            catch(ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException: {0}", ane.Message);
            }
            return baseDateTime;
        }
        public static String getCreatedTimeString(DateTime baseDateTime)
        {
            var timeString = new StringBuilder();

            DateTime baseTime = baseDateTime;

            timeString.Append(baseTime.Year.ToString() + "-");
            timeString.Append(baseTime.Month.ToString("D2") + "-");
            timeString.Append(baseTime.Day.ToString("D2") + "T");
            timeString.Append(baseTime.Hour.ToString("D2") + ":");
            timeString.Append(baseTime.Minute.ToString("D2") + ":");
            timeString.Append(baseTime.Second.ToString("D2") + ".");
            timeString.Append(baseTime.Millisecond.ToString("D3") + "Z");

            return timeString.ToString();
        }

        public static String getPasswordDigest(String nonceBase64, String createTime, String password)
        {
            byte[] nonceBinary = EncodingHelper.Base64Decode(nonceBase64);
            byte[] utctimeBinary = EncodingHelper.String2Byte(createTime);
            byte[] pwdBinary = EncodingHelper.String2Byte(password);

            byte[] combined = new byte[nonceBinary.Length + utctimeBinary.Length + pwdBinary.Length];

            System.Buffer.BlockCopy(nonceBinary, 0, combined, 0, nonceBinary.Length);
            System.Buffer.BlockCopy(utctimeBinary, 0, combined, nonceBinary.Length, utctimeBinary.Length);
            System.Buffer.BlockCopy(pwdBinary, 0, combined, nonceBinary.Length + utctimeBinary.Length, pwdBinary.Length);

            SHA1Managed sha1 = new SHA1Managed();

            byte[] passwordHash = sha1.ComputeHash(combined);

            String hashValue = System.Convert.ToBase64String(passwordHash);

            return hashValue;
        }
    }
}
