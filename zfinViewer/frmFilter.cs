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
    public partial class frmFilter : Form
    {
        public frmFilter()
        {
            InitializeComponent();
        }

        private void frmFilter_Load(object sender, EventArgs e)
        {
            List<string> Types = new List<string>();
            Types.Add("Ogranicz do");
            Types.Add("Wyklucz");
            cmbType.DataSource = Types;

            List<string> Columns = new List<string>();
            Columns.Add("Zlecenie");
            cmbColumn.DataSource = Columns;
        }
    }
}
