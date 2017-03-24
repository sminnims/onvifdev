using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ONVIFTester
{
    public partial class SystemLog : Form
    {
        public SystemLog(String log)
        {
            InitializeComponent();
            tbLogBox.Text = log;
        }
    }
}
