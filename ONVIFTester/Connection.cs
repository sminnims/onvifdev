using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ONVIFTester
{
    public partial class Connection : Form
    {
        private ONVIFControl _onvif;
        
        public Connection()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            tbLog.Clear();
            if(_onvif != null)
            {
                _onvif.Dispose();
                GC.SuppressFinalize(_onvif);
            }
            try
            {
                String deviceUrl;
                String addr = tbAddr.Text;
                /* check ip address */
                IPAddress ip = IPAddress.Parse(addr);
                _onvif = new ONVIFControl(this);
                deviceUrl = "http://" + tbAddr.Text;
                _onvif.setDeviceUrl(deviceUrl);
                _onvif.setUsername(tbID.Text);
                _onvif.setPassword(tbPass.Text);                
                _onvif.ONVIF_GetSystemDateAndTime();
                _onvif.ONVIF_GetCapabilities();
                _onvif.ONVIF_GetProfiles();
            }
            catch(ArgumentNullException ane)
            {
                settbLogBox("\r\nNull 예외 오류 : " + ane.Message);
            }
            catch(FormatException fe)
            {
                settbLogBox("\r\n주소 입력 오류 : " + fe.Message + "\n IP : " + tbAddr.Text);
            }
        }

        public void settbLogBox(String str)
        {
            tbLog.Text += str;
        }

        private void btnGetLog_Click(object sender, EventArgs e)
        {
            StringBuilder logStr = new StringBuilder();
            logStr.Append(_onvif.ONVIF_GetSystemLog());

            SystemLog logView = new SystemLog(logStr.ToString());
            logView.ShowDialog();
        }
    }
}
