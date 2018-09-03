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
    public partial class frmLooper : Form
    {
        public frmLooper(Form parent)
        {
            InitializeComponent();
            this.Owner = parent;
            //Point p = new Point(this.Owner.Width / 2 - this.Width / 2, this.Owner.Height / 2 - this.Height / 2);
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(parent.Location.X + (parent.Width - this.Width) / 2, parent.Location.Y + (parent.Height - this.Height) / 2);
        }

        private void frmLooper_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.Owner.Enabled = false;
            }
            else
            {
                this.Owner.Enabled = true;
            }
        }
    }
}
