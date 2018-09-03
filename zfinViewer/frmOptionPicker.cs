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
    public partial class frmOptionPicker : Form
    {
        public string ReturnValue { get; set; }

        public frmOptionPicker(string text, string title, List<string>Options, Form parent)
        {
            InitializeComponent();
            cmbOptions.DataSource = Options;
            this.Text = title;
            lblText.Text = text;
            this.Owner = parent;
        }

        private void frmOptionPicker_Load(object sender, EventArgs e)
        {
            this.Location = new Point(this.Owner.Location.X + (this.Owner.Width/2-this.Width/2), this.Owner.Location.Y + (this.Owner.Height/2-this.Height/2));
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.ReturnValue = (string)cmbOptions.SelectedValue;
            this.DialogResult = DialogResult.OK;
        }
    }
}
