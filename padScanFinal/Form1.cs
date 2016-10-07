using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace padScanFinal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] hallo = { 0x48, 0x83, 0xec, 0x00, 0xe8 };//this to get
            ///\x48\x83\xec\x00\xe8 //the code to take in
            string hoek = "\x48\x83\xec\x00\xe8";
            string bal = hoek.Replace(@"\x",string.Empty);

            //byte[] lees = Encoding.Default.GetBytes(hoek);//this shut get hallo
            byte[] lees = Encoding.Default.GetBytes(bal);
            string getstring = BitConverter.ToString(lees,0);///to see huh 3F not 83

            label1.Text = getstring;

        }
    }
}
