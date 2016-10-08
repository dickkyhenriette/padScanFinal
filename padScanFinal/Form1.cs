using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
            byte[] getup = sigscan.ToByteArray(hoek);
            string getstring = BitConverter.ToString(getup,0);
            string getstring2 = BitConverter.ToString(hallo, 0);
            label1.Text = getstring;
            label2.Text = getstring2;


        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process myProcess;
            myProcess = Process.Start("NotePad");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //leave de .exe from the name as for 64 bit programs.
            var proc = Process.GetProcessesByName("notepad");
            if (proc.Length != 0)
            {
                proc[0].Kill();
            }
            //foreach (var process in proc)
            //{
            //    process.Kill();
            //}
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            sigscan.Writedumpfile("notepad");
            
        }
    }
}
