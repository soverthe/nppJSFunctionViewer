using Kbg.NppPluginNET.PluginInfrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Kbg.NppPluginNET
{
    public partial class frmMyDlg : Form
    {
        public frmMyDlg()
        {
            InitializeComponent();
            ActiveControl = null;
        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmMyDlg_Load(object sender, EventArgs e)
        {
            this.ActiveControl = null;
            ActiveControl = null;
        }
        
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void GoToFunctionClick(object sender, EventArgs e)
        {
            isGoToFunctionClicked = true;
            Main.CheckGoToButtons();
        }

        private void GoUpClick(object sender, EventArgs e)
        {
            isGoUpClicked = true;
            Main.CheckGoToButtons();
        }
        private void GoDownClick(object sender, EventArgs e)
        {
            isGoDownClicked = true;
            Main.CheckGoToButtons();
        }
        private void GoToXClick(object sender, EventArgs e)
        {
            Main.HideDialog();
            Main.isFuncEnabled = false;
        }

        
        public bool CheckGoToFunction()
        {
            if (isGoToFunctionClicked)
            {
                isGoToFunctionClicked = false;
                return true;
            }
            return false;
        }
        public bool CheckGoUp()
        {
            if (isGoUpClicked)
            {
                isGoUpClicked = false;
                return true;
            }
            return false;
        }
        public bool CheckGoDown()
        {
            if (isGoDownClicked)
            {
                isGoDownClicked = false;
                return true;
            }
            return false;
        }

        public void ShowGoUpDownButtons()
        {
            GoUp.Visible = true;
            GoDown.Visible = true;
        }
        public void HideGoUpDownButtons()
        {
            GoUp.Visible = false;
            GoDown.Visible = false;
        }
    }
}
