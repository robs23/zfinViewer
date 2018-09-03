using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using zfinViewer.Models;

namespace zfinViewer
{
    public partial class frmMassBalance : Form
    {
        Orders Orders;
        SqlConnection conn = new SqlConnection(Variables.npdConnectionString);

        public frmMassBalance()
        {
            InitializeComponent();
            Orders = new Orders();
        }

        private void frmMassBalance_Load(object sender, EventArgs e)
        {
            GetMaterialTypes();
        }

        private void tbnUpdate_Click(object sender, EventArgs e)
        {
            Orders.Reload(txtDfrom.Value, txtDto.Value, (int)cmbCategory.SelectedValue);
            dgvData.DataSource = null;
            DataTable dt;

            if(this.cmbType.SelectedIndex == 0)
            {
                dt = Orders.GetDataTable(MassBalanceType.byOrders);
            }
            else
            {
                dt = Orders.GetDataTable(MassBalanceType.byCategory);
            }
            
            dgvData.DataSource = dt;
            foreach(DataGridViewColumn col in dgvData.Columns)
            {
                col.HeaderText = dt.Columns[col.HeaderText].Caption;
            }
            dgvData.Columns[2].Width = 250;
            lblStatus.Text = "Liczba pozycji: " + Orders.Items.Count.ToString();

            if (Orders.IsMissing)
            {
                MessageBox.Show(Orders.MissingData);
            }
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
