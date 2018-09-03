using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zfinViewer
{
    public partial class frmToast : Form
    {
        int top;
        int left;
        string mess;
        Timer opaTimer = new Timer();
        public frmToast(string message, int Top, int Left)
        {
            InitializeComponent();
            top = Top;
            left = Left;
            mess = message;
            opaTimer.Interval = 30;
            opaTimer.Tick += new EventHandler(opacityTimer_Tick);
            opaTimer.Start();
        }

        private void loadMe(object sender, EventArgs e)
        {
            lblToast.Text = mess;
            this.Top = top;
            this.Left = left;
            //this.Width = lblToast.Width + 30;
        }

        private void opacityTimer_Tick(object sender, EventArgs e)
        {
            if(this.Opacity > 0.01)
            {
                this.Opacity = this.Opacity - 0.01;
            }
        }

        private void closeMe(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }
    }
}
