using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

        public frmMassBalance()
        {
            InitializeComponent();
            Orders = new Orders();
        }

        private void frmMassBalance_Load(object sender, EventArgs e)
        {
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
            foreach (DataGridViewColumn col in dgvData.Columns)
            {
                col.HeaderText = dt.Columns[col.HeaderText].Caption;
            }
            dgvData.Columns[2].Width = 250;
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
            Looper.Hide();

            if (Orders.IsMissing)
            {
                MessageBox.Show(Orders.MissingData);
            }
        }

        //private async Task<DataTable> UpdateTableAsync(int SelectedValue, int SelectedType)
        //{

        //}

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
    }
}
