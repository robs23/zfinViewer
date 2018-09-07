using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using zfinViewer.Models;

namespace zfinViewer
{
    public partial class frmMassBalance : Form
    {
        Orders Orders;
        SqlConnection conn = new SqlConnection(Variables.npdConnectionString);
        frmLooper Looper;
        Filter Filter;
        DataGridViewColumn SortColumn;
        System.Windows.Forms.SortOrder SortOrder;

        public frmMassBalance()
        {
            InitializeComponent();
            Orders = new Orders();
        }

        private void frmMassBalance_Load(object sender, EventArgs e)
        {
            btnFilter.Enabled = false;
            Looper = new frmLooper(this);
            GetMaterialTypes();
        }

        private async void tbnUpdate_Click(object sender, EventArgs e)
        {
            Looper.Show(this);
            dgvData.DataSource = null;
            DateTime dFrom = txtDfrom.Value;
            DateTime dTo = txtDto.Value;
            int SelectedValue = (int)cmbCategory.SelectedValue;
            int SelectedType = cmbType.SelectedIndex;
            //await Task.Run( async () => { await UpdateTable(SelectedValue, SelectedType); });
            DataTable dt = await UpdateTable(SelectedValue, SelectedType, dFrom, dTo);
            dgvData.DataSource = dt;
            FillTable();
            Filter = new Filter();
            btnFilter.Image = zfinViewer.Properties.Resources.icon_filter_off;
            Looper.Hide();

            if (Orders.IsMissing)
            {
                MessageBox.Show(Orders.MissingData);
            }
        }

        private void FillTable()
        {
            DataTable dt = (DataTable)dgvData.DataSource;
            foreach (DataGridViewColumn col in dgvData.Columns)
            {
                col.HeaderText = dt.Columns[col.HeaderText].Caption;
            }
            dgvData.Columns[2].Width = 250;
            if (Orders.Items.Count > 0) { btnFilter.Enabled = true; }
            lblStatus.Text = "Liczba pozycji: " + Orders.Items.Count.ToString();
            double totLoss = 0;
            double bomLoss = 0;
            int iTot = 0;
            int iBom = 0;
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                double val;
                if (double.TryParse(row.Cells[7].Value.ToString(), out val))
                {
                    totLoss += val;
                    iTot++;
                }
                if (double.TryParse(row.Cells[8].Value.ToString(), out val))
                {
                    bomLoss += val;
                    iBom++;
                }
            }
            txtTotalLoss.Text = String.Format("{0:0.00}", (totLoss / iTot));
            txtBomLoss.Text = String.Format("{0:0.00}", (-1 * bomLoss / iBom));
        }
        
        private async Task<DataTable> UpdateTable(int SelectedValue, int SelectedType, DateTime dFrom, DateTime dTo)
        {
            DataTable dt =
            await Task.Run(async () =>
            {
               await Orders.Reload(dFrom, dTo, SelectedValue);
               dgvData.DataSource = null;
               

               if (SelectedType == 0)
               {
                   dt = await Orders.GetDataTable(MassBalanceType.byOrders);
               }
               else
               {
                   dt = await Orders.GetDataTable(MassBalanceType.byCategory);
               }
               return dt;
            });
            return dt;
        }

        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = ((DataGridView)sender).Columns[e.ColumnIndex].Name;

            switch (colName)
            {
                case "Order":
                    string oNumber = ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value.ToString();
                    frmOrder Order = new frmOrder(int.Parse(oNumber), this);
                    Order.Show(this);
                    break;
                case "ZPKG":
                case "ZPKGName":
                    string comNumber = ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Value.ToString();
                    bool isInt = int.TryParse(comNumber, out int com);
                    if (isInt)
                    {
                        frmZfinOverview FrmZfinOverview = new frmZfinOverview(com, this);
                        FrmZfinOverview.Show(this);
                    }
                    else
                    {
                        string[] ops = Regex.Split(comNumber, Environment.NewLine);
                        List<string> Options = new List<string>(ops);
                        frmOptionPicker op = new frmOptionPicker("Wybierz który element chcesz otworzyć:", "Wiele elementów", Options, this);
                        var res = op.ShowDialog();
                        if (res == DialogResult.OK)
                        {
                            isInt = int.TryParse(op.ReturnValue, out com);
                            if (isInt)
                            {
                                frmZfinOverview FrmZfinOverview = new frmZfinOverview(com, this);
                                FrmZfinOverview.Show(this);
                            }
                            else
                            {
                                MessageBox.Show("Niezidentyfikowany błąd");
                            }
                            
                        }

                    }
                    
                    break;
                case "ZFIN":
                case "ZFINName":
                    string zNumber = ((DataGridView)sender).Rows[e.RowIndex].Cells[3].Value.ToString();
                    frmZfinOverview FrmProductOverview = new frmZfinOverview(int.Parse(zNumber), this);
                    FrmProductOverview.Show(this);
                    break;
            }
        }


        private void GetMaterialTypes()
        {
            string sql = "SELECT mt.materialTypeId, mt.materialTypeName FROM tbMaterialType mt";

            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataAdapter sqlDataAdap = new SqlDataAdapter(command);
            DataTable dtMaterialTypes = new DataTable();
            sqlDataAdap.Fill(dtMaterialTypes);
            cmbCategory.DataSource = dtMaterialTypes;
            cmbCategory.ValueMember = "materialTypeId";
            cmbCategory.DisplayMember = "materialTypeName";
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (!Filter.IsInitialized)
            {
                Filter.Init((DataTable)dgvData.DataSource);
            }
            frmFilter FrmFilter = new frmFilter(this, Filter);
            DialogResult result = FrmFilter.ShowDialog(this);
            if(result == DialogResult.OK)
            {
                if (Filter.IsOn)
                {
                    btnFilter.Image = zfinViewer.Properties.Resources.icon_filter_on;
                    Filter.Apply();
                    SaveSort();

                    dgvData.DataSource = null;
                    dgvData.DataSource = Filter.FilteredTable;
                    FillTable();
                    RestoreSort();
                }
                else
                {
                    btnFilter.Image = zfinViewer.Properties.Resources.icon_filter_off;
                    SaveSort();

                    dgvData.DataSource = null;
                    dgvData.DataSource = Filter.DataTable;
                    FillTable();
                    RestoreSort();

                }
            }
        }


        private void dgvData_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                if(dgvData.SelectedRows.Count > 0)
                {
                    ContextMenu m = new ContextMenu();
                    m.MenuItems.Add(new MenuItem("Ogranicz do zaznaczonych..", new EventHandler(LimitToRows)));
                    m.MenuItems.Add(new MenuItem("Wyklucz zaznaczone..", new EventHandler(ExcludeRows)));
                    m.Show(dgvData, new Point(e.X, e.Y));
                }
            }
        }


        private void SaveSort()
        {
            if (dgvData.SortedColumn != null)
            {
                SortColumn = dgvData.SortedColumn;
                SortOrder = dgvData.SortOrder;
            }
        }

        private void RestoreSort()
        {
            if (SortColumn != null)
            {
                if (SortOrder == System.Windows.Forms.SortOrder.Ascending)
                {
                    dgvData.Sort(dgvData.Columns[SortColumn.Name], ListSortDirection.Ascending);
                }
                else
                {
                    dgvData.Sort(dgvData.Columns[SortColumn.Name], ListSortDirection.Descending);
                }
            }
            
        }

        private void ExcludeRows(object sender, EventArgs e)
        {
            if (!Filter.IsInitialized)
            {
                Filter.Init((DataTable)dgvData.DataSource);
            }
            foreach (DataGridViewRow row in dgvData.SelectedRows)
            {
                Filter.Columns[0].Exclude.Add(row.Cells[0].Value);
            }
            Filter.Apply();
            SaveSort();

            dgvData.DataSource = null;
            dgvData.DataSource = Filter.FilteredTable;
            FillTable();
            RestoreSort();
            btnFilter.Image = zfinViewer.Properties.Resources.icon_filter_on;
        }

        private void LimitToRows(object sender, EventArgs e)
        {
            if (!Filter.IsInitialized)
            {
                Filter.Init((DataTable)dgvData.DataSource);
            }
            foreach (DataGridViewRow row in dgvData.SelectedRows)
            {
                Filter.Columns[0].LimitTo.Add(row.Cells[0].Value);
            }
            Filter.Apply();
            SaveSort();
            
            dgvData.DataSource = null;
            dgvData.DataSource = Filter.FilteredTable;
            FillTable();
            RestoreSort();
            btnFilter.Image = zfinViewer.Properties.Resources.icon_filter_on;

        }
    }
}
