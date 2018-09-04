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
    public partial class frmFilterColumn : Form
    {
        public FilterColumn Column { get; set; }
        public DataTable DataTable { get; set; }

        public frmFilterColumn(Form parent, FilterColumn column)
        {
            InitializeComponent();
            Column = column;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void frmFilterColumn_Load(object sender, EventArgs e)
        {
            DataTable = new DataTable();
            DataColumn col = new DataColumn("Value");
            col.DataType = Column.Type;
            DataTable.Columns.Add(col);
            if (Column.LimitTo.Any())
            {
                //it's been already used and values are to limit main list
                foreach(object i in Column.LimitTo)
                {
                    DataRow row = DataTable.NewRow();
                    row[0] = i;
                    DataTable.Rows.Add(row);
                }
            }else if (Column.Exclude.Any())
            {
                //it's been already used and values are to exclude items from main list
                foreach (object i in Column.Exclude)
                {
                    DataRow row = DataTable.NewRow();
                    row[0] = i;
                    DataTable.Rows.Add(row);
                }
            }
            else
            {
                //it hasn't been used. Show all values
                foreach (object i in Column.Items)
                {
                    DataRow row = DataTable.NewRow();
                    row[0] = i;
                    DataTable.Rows.Add(row);
                }
            }

            dgvValues.DataSource = DataTable;
        }
    }
}
