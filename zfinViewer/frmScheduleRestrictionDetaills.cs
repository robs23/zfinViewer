using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using zfinViewer.Models;

namespace zfinViewer
{
    public partial class frmScheduleRestrictionDetaills : Form
    {
        public List<Machine> Machines { get; set; }

        public frmScheduleRestrictionDetaills(Form parent)
        {
            InitializeComponent();
            this.Owner = parent;
            Machines = new List<Machine>();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmScheduleRestrictionDetaills_Load(object sender, EventArgs e)
        {
            this.Location = new Point(this.Owner.Location.X + 20, this.Owner.Location.Y + 20);
            txtStartDate.Value = DateTime.Now;
            LoadLines();
            cmbType.DataSource = Enum.GetValues(typeof(MesOrderType));
        }

        private void LoadLines()
        {
            string sql = "SELECT machineName, machineNumber, machineType FROM tbMachine ORDER BY machineType, machineNumber";

            SqlConnection conn = new SqlConnection(Variables.npdConnectionString);
            SqlCommand sqlComand = new SqlCommand(sql, conn);

            try
            {
                conn.Open();
                using (SqlDataReader reader = sqlComand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Machine m = new Machine
                        {
                            Name = reader[0].ToString(),
                            Number = Convert.ToInt32(reader[1].ToString()),
                            Type = reader[2].ToString()
                        };
                    Machines.Add(m);
                    }
                }
                dgvRestrictions.DataSource = Machines;
                dgvRestrictions.Columns[1].Visible = false;
                dgvRestrictions.Columns[2].Visible = false;
                dgvRestrictions.Columns[3].Visible = false;
                DataGridViewCheckBoxColumn IsSelected = new DataGridViewCheckBoxColumn();
                IsSelected.HeaderText = "Utworzyć dla maszyny?";
                IsSelected.FalseValue = 0;
                IsSelected.TrueValue = 1;
                dgvRestrictions.Columns.Insert(1, IsSelected);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSelectAllNone_Click(object sender, EventArgs e)
        {
            bool Selecting = false;
            if (btnSelectAllNone.Text=="Zaznacz wszystkie")
            {
                //all is deselected
                Selecting = true;
            }
            else
            {
                Selecting = false;
            }

            foreach(DataGridViewRow row in dgvRestrictions.Rows)
            {
                if (row.Cells[1].Value == null)
                {
                    row.Cells[1].Value = Selecting;
                }
                else
                {
                    row.Cells[1].Value = Selecting;
                }
            }
            if (Selecting)
            {
                btnSelectAllNone.Text = "Odznacz wszystkie";
            }
            else
            {
                btnSelectAllNone.Text = "Zaznacz wszystkie";
            }
        }

        private void txtPrefix_TextChanged(object sender, EventArgs e)
        {
            ChangePrefix();
        }

        private void ChangePrefix()
        {
            if (!string.IsNullOrEmpty(txtSuffix.Text))
            {
                int week = Utility.GetIsoWeekNumber(txtStartDate.Value);
                lblExample.Text = $"T{week}{txtStartDate.Value.Year}+Numer maszyny+{txtSuffix.Text}";
            }
        }

        private void txtStartDate_ValueChanged(object sender, EventArgs e)
        {
            ChangePrefix();
            RecalculateEndDate();
        }

        private void txtLength_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtLength.Text))
            {
                if (txtLength.Text.Substring(txtLength.Text.Length - 1, 1) == "h" || txtLength.Text.Substring(txtLength.Text.Length - 1, 1) == "H")
                {
                    //hours are given
                    int min = Convert.ToInt32(txtLength.Text.Substring(0, txtLength.Text.Length - 1));
                    txtLength.Text = (min * 60).ToString();
                }
                RecalculateEndDate();
            }
        }

        private void RecalculateEndDate()
        {
            if(txtStartDate.Value != null && !string.IsNullOrEmpty(txtLength.Text))
            {
                int min = Convert.ToInt32(txtLength.Text);
                
                lblEndDate.Text = txtStartDate.Value.AddMinutes(min).ToString();
            }
            else
            {
                lblEndDate.Text = "";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool IsSelected = false;

            foreach(Machine m in Machines)
            {
                IsSelected = false;
                foreach (DataGridViewRow row in dgvRestrictions.Rows)
                {
                    if(row.Cells[0].Value == m.Name)
                    {
                        if (row.Cells[1].Value != null)
                        {
                            if ((Convert.ToBoolean(row.Cells[1].Value)) == true)
                            {
                                IsSelected = true;
                            }
                        }
                    }
                }
                m.IsSelected = IsSelected;
            }
            int total = Machines.Where(m => m.IsSelected == true).Count();
            if (total > 0)
            {
                DialogResult result = MessageBox.Show($"Czy na pewno chcesz dodać zlecenie dla zaznaczonych {total} maszyn?", "Potwierdź", MessageBoxButtons.YesNo);
                if(result == DialogResult.Yes)
                {
                    //Dodajemy
                    AddOrders(Machines);
                }
            }
            
        }

        private string CreatePrefix()
        {
            int week = Utility.GetIsoWeekNumber(txtStartDate.Value);
            string _Result = week.ToString() + txtStartDate.Value.Year.ToString();
            return _Result;
        }

        private string GetUniqueNumber(string number)
        {
            MesOrderKeeper keeper = new MesOrderKeeper();
            bool found = false;
            int i = 1;
            do
            {
                if (!keeper.OrderExists(number + i.ToString()))
                {
                    found = true;
                }
                i += i;
            } while (found == false);
            return number + (i - 1).ToString();
        }

        private void AddOrders(List<Machine> Machines)
        {
            MachineKeeper MachineKeeper = new MachineKeeper();
            MachineKeeper.LoadFromMes(new int[] { 1, 2, 3 });

            foreach (Machine mach in Machines.Where(m => m.IsSelected == true))
            {
                MesOrder mesOrder = new MesOrder();
                if (mach.Type == "g" || mach.Type == "p")
                {
                    mesOrder.Number = GetUniqueNumber(CreatePrefix() + mach.Name.Substring(0,1) + mach.Number.ToString() + txtSuffix.Text);
                }
                else
                {
                    mesOrder.Number = GetUniqueNumber(CreatePrefix() + mach.Name.Substring(0, 2) + mach.Number.ToString() + txtSuffix.Text);
                }
                
                mesOrder.Name = txtDescription.Text;
                mesOrder.Description = txtDescription.Text;
                mesOrder.ScheduledStartDate = txtStartDate.Value;
                mesOrder.ScheduledFinishDate = txtStartDate.Value.AddMinutes(Convert.ToInt32(txtLength.Text));
                mesOrder.Machine = MachineKeeper.Machines.Where(m => m.MesNumber == mach.Name.Trim()).FirstOrDefault();
                mesOrder.Type = (MesOrderType)cmbType.SelectedValue;
            }
        }
    }
}
