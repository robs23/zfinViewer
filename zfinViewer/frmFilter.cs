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
            dgvData.Columns[2].ReadOnly = true;
            UpdateFilterInfo();
        }

        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                if(dgvData[1, e.RowIndex].Value.ToString().Length == 0)
                {
                    MessageBox.Show("Najpierw wybierz typ z listy rozwijanej!");
                }
                else
                {
                    if(dgvData[1, e.RowIndex].Value.ToString() == "wyklucz")
                    {
                        Filter.Columns[e.RowIndex].FilterType = FilterType.Exclude;
                    }
                    else
                    {
                        Filter.Columns[e.RowIndex].FilterType = FilterType.LimitTo;
                    }
                    
                    frmFilterColumn FrmFilterColumn = new frmFilterColumn(this, Filter.Columns[e.RowIndex]);
                    var res = FrmFilterColumn.ShowDialog();
                    if(res == DialogResult.OK)
                    {
                        dgvData.Columns[2].ReadOnly = false;
                        if (Filter.Columns[e.RowIndex].LimitTo.Count > 0)
                        {
                            if(Filter.Columns[e.RowIndex].LimitTo.Count > 0)
                            {
                                dgvData[2, e.RowIndex].Value = Filter.Columns[e.RowIndex].LimitTo.Count + " pozycji";
                                dgvData.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.AliceBlue;
                            }
                            else
                            {
                                dgvData[2, e.RowIndex].Value = null;
                                dgvData.Rows[e.RowIndex].DefaultCellStyle.BackColor = dgvData.DefaultCellStyle.BackColor;
                            }
                            
                        }
                        else if(Filter.Columns[e.RowIndex].Exclude.Count > 0)
                        {
                            if (Filter.Columns[e.RowIndex].Exclude.Count > 0)
                            {
                                dgvData[2, e.RowIndex].Value = Filter.Columns[e.RowIndex].Exclude.Count + " pozycji";
                                dgvData.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.AliceBlue;
                            }
                            else
                            {
                                dgvData[2, e.RowIndex].Value = null;
                                dgvData.Rows[e.RowIndex].DefaultCellStyle.BackColor = dgvData.DefaultCellStyle.BackColor;
                            }
                            
                        }

                        dgvData[1, e.RowIndex].ReadOnly = true;
                        dgvData.Columns[2].ReadOnly = true;
                    }
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if(dgvData.SelectedRows.Count > 0)
            {
                List<string> Cols = new List<string>();
                foreach(DataGridViewRow row in dgvData.SelectedRows)
                {
                    Cols.Add(row.Cells[0].Value.ToString());
                }
                Filter.Clear(Cols);
                UpdateFilterInfo();
            }
            else
            {
                Filter.Clear();
                this.Close();
            }
            
        }

        private void UpdateFilterInfo()
        {
            foreach (FilterColumn c in Filter.Columns)
            {
                if(c.Exclude.Count == 0 & c.LimitTo.Count == 0)
                {
                    dgvData[2, c.ID].Value = null;
                    dgvData.Rows[c.ID].DefaultCellStyle.BackColor = dgvData.DefaultCellStyle.BackColor;
                    dgvData.Rows[c.ID].Cells[1].ReadOnly = false;
                }
                else
                {
                    if(c.Exclude.Count > 0)
                    {
                        dgvData[1, c.ID].Value = "wyklucz";
                        dgvData[2, c.ID].Value = c.Exclude.Count + " pozycji";
                    }
                    else
                    {
                        dgvData[1, c.ID].Value = "ogranicz do";
                        dgvData[2, c.ID].Value = c.LimitTo.Count + " pozycji";
                    }
                    
                    dgvData.Rows[c.ID].DefaultCellStyle.BackColor = Color.AliceBlue;
                }
            }
        }

        private void frmFilter_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
