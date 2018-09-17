using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GS_CJDA
{
    public partial class frmMessageShow : Form
    {
        private int _status;

        public frmMessageShow(string title, string message, int status)
        {
            InitializeComponent();
            setTitle(title);
            setMessage(message);
            setStatus(status);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        public void setTitle(string title)
        {
            this.Text = title;
        }

        public void setMessage(string message)
        {
            this.labMessage.Text = message;
            toolTip1.SetToolTip(this.labMessage, this.labMessage.Text);
        }

        public void setStatus(int status)
        {
            _status = status;
            if (status == 0)
            {
                this.btnOK.Enabled = true;
                button1.Visible = false;
            }
            else if (status == 1)
            {
                this.btnOK.Enabled = false;
                button1.Visible = true;
            }
        }

        public void setInfo(string message, int status)
        {
            if (message != null && message != "")
            {
                setMessage(message);
                setStatus(status);
            }
        }

        private void frmMessageShow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this._status == 1)
            {
                e.Cancel = true;
            }
        }

        private void frmMessageShow_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process[] pro = Process.GetProcesses();//获取已开启的所有进程
            for (int i = 0; i < pro.Length; i++)
            {
                if (pro[i].ProcessName.ToString().Contains("GS_CJDA"))
                {
                    pro[i].Kill();//结束进程
                }
                if (pro[i].ProcessName.ToString().Contains("gobackhome1513"))
                {
                    pro[i].Kill();//结束进程
                }
            }
            //Application.Exit();
            System.Environment.Exit(0);
        }

        private void frmMessageShow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                button1_Click(this, EventArgs.Empty);
            }
          
        }



    }
}
