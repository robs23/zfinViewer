using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using zfinViewer.Models;

namespace zfinViewer
{
    public partial class frmFilter : Form
    {
        Filter Filter;

        public frmFilter(Form parent, Filter f)
        {
            InitializeComponent();
            Filter = f;
        }

        private void frmFilter_Load(object sender, EventArgs e)
        {
            DataTable Dt = new DataTable();
            DataColumn col = new DataColumn("Kolumna");
            Dt.Columns.Add(col);
            col = new DataColumn("Typ");
            Dt.Columns.Add(col);
            col = new DataColumn("Wartości");
            Dt.Columns.Add(col);
            Dt.Columns[0].ReadOnly = true;
            Dt.Columns[2].ReadOnly = true;


            foreach (FilterColumn c in Filter.Columns)
            {
                DataRow row = Dt.NewRow();
                row["Kolumna"] = c.Name;
                Dt.Rows.Add(row);
            }
            dgvData.DataSource = Dt;

            List<string> Types = new List<string>();
            Types.Add("ogranicz do");
            Types.Add("wyklucz");
            
            foreach(DataGridViewRow r in dgvData.Rows)
            {
                r.Cells[1] = new DataGridViewComboBoxCell();
                ((DataGridViewComboBoxCell)r.Cells[1]).DataSource = Types;
            }

        }

        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                frmFilterColumn FrmFilterColumn = new frmFilterColumn(this, Filter.Columns[e.RowIndex]);
                FrmFilterColumn.Show(this);
            }
        }
    }
}
