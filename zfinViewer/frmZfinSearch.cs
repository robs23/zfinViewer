using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;

namespace zfinViewer
{
    public partial class frmZfinSearch : Form
    {
        SqlConnection conn = new SqlConnection(Variables.npdConnectionString);
        List<ISearchable> items = new List<ISearchable>();
        List<ISearchable> allItems = new List<ISearchable>(); //items matching checked criteria e.g. IsActive
        IEnumerable<ISearchable> filteredItems;
        bool ActiveOnly = true; 

        public frmZfinSearch()  
        {
            InitializeComponent();
        }

        private void loadMe(object sender, EventArgs e)
        {
            loadData();
        }

        private void updateSearch(object sender, EventArgs e)
        {
            UpdateResults();
        }

        private void UpdateResults()
        {
            string str = txtSearch.Text;
            if (str.Length > 0)
            {
                if (ActiveOnly)
                {
                    filteredItems = from pr in items
                                    where (pr.SearchableString.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0 || pr.Index.ToString().IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0) && pr.IsActive == true
                                    select pr;
                }
                else
                {
                    filteredItems = from pr in items
                                    where pr.SearchableString.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0 || pr.Index.ToString().IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0
                                    select pr;
                }

                dgItems.DataSource = filteredItems.ToList();
                lblHitsCount.Text = "Liczba pasujących pozycji: " + filteredItems.ToList().Count.ToString();
            }
            else
            {
                dgItems.DataSource = items;
                lblHitsCount.Text = "Liczba pozycji: " + items.Count.ToString();
            }
        }

        private void displayProduct(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int i = dgItems.CurrentCell.RowIndex;
                action(i);
                
            }
            catch
            {
                MessageBox.Show("Nic nie zostało zaznaczone.","Brak zaznaczenia");
            }
        }

        private void action(int rowIndex)
        {
            int i = rowIndex;
            string type = dgItems.Rows[i].Cells[3].Value.ToString();
            if (string.Equals(type, "zfin", StringComparison.OrdinalIgnoreCase) || string.Equals(type, "zfor", StringComparison.OrdinalIgnoreCase) || string.Equals(type, "zcom", StringComparison.OrdinalIgnoreCase) || string.Equals(type, "zpkg", StringComparison.OrdinalIgnoreCase))
            {
                int zfinIndex = Convert.ToInt32(dgItems.Rows[i].Cells[1].Value);
                frmZfinOverview zo = new frmZfinOverview(zfinIndex, this);
                zo.Show();
            }
            else if (string.Equals(type, "Kontakt", StringComparison.OrdinalIgnoreCase)) {
                int contactId = Convert.ToInt32(dgItems.Rows[i].Cells[1].Value);
                frmContact co = new frmContact(contactId,this);
                co.Show();
            }else if(string.Equals(type, "Sold-to",StringComparison.OrdinalIgnoreCase) || string.Equals(type, "Ship-to", StringComparison.OrdinalIgnoreCase) || string.Equals(type, "Carrier", StringComparison.OrdinalIgnoreCase))
            {
                int companyId = Convert.ToInt32(dgItems.Rows[i].Cells[1].Value);
                frmCompany cp = new frmCompany(companyId,this);
                cp.Show();
            }
            else
            {
                MessageBox.Show("Funkcja nie jest jeszcze dostępna");
            }
        }

        private void loadData()
        {
            
            try
            {
                conn.Open();
                LoadSettings();
                loadProducts();
                loadContacts();
                loadComapanies();
                items = allItems.Where(i => i.IsActive == true).ToList();
            }
            catch
            {
                MessageBox.Show(String.Format("Nie udało się nawiązać połączenia z bazą danych", "Błąd połączenia z bazą danych"));
            }
            finally
            {
                if (items.Count > 0)
                {
                    //make sure there are any items in the list
                    filteredItems = items;
                    dgItems.DataSource = items;
                    dgItems.Columns[0].Visible = false;
                    dgItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    lblHitsCount.Text = "Liczba pozycji: " + items.Count.ToString();
                }
                conn.Close();
            }
        }
        private void loadProducts()
        {
            string sql = "SELECT zfinId, zfinIndex, zfinName,zfinType, CASE WHEN prodStatus='PR' THEN 1 ELSE 0 END as IsActive FROM tbZfin ";
           
            SqlCommand sqlComand;
            sqlComand = new SqlCommand(sql, conn);
            using (SqlDataReader reader = sqlComand.ExecuteReader())
            {
                while (reader.Read())
                {
                    Product pr = new Product { Index = reader.GetInt32(reader.GetOrdinal("zfinIndex")), Name = reader["zfinName"].ToString().Trim(), Type = reader["zfinType"].ToString(), ID = reader.GetInt32(reader.GetOrdinal("zfinId")), IsActive = Convert.ToBoolean(reader.GetInt32(reader.GetOrdinal("IsActive"))) };
                    items.Add(pr);
                    allItems.Add(pr);
                }
            }
        }

        private void LoadSettings()
        {
            opcjeToolStripMenuItem.DropDownItems.Add("Tylko aktywne");
            ((ToolStripMenuItem)opcjeToolStripMenuItem.DropDownItems[0]).Checked = true;
            ((ToolStripMenuItem)opcjeToolStripMenuItem.DropDownItems[0]).Click += ActiveOnly_Changed;
        }

        private void ActiveOnly_Changed(object sender, EventArgs e)
        {
            if (((ToolStripMenuItem)opcjeToolStripMenuItem.DropDownItems[0]).Checked)
            {
                //uncheck
                ((ToolStripMenuItem)opcjeToolStripMenuItem.DropDownItems[0]).Checked = false;
                items = allItems;
            }
            else
            {
                //check
                ((ToolStripMenuItem)opcjeToolStripMenuItem.DropDownItems[0]).Checked = true;
                items = allItems.Where(i => i.IsActive == true).ToList();
            }
            ActiveOnly = ((ToolStripMenuItem)opcjeToolStripMenuItem.DropDownItems[0]).Checked;
            UpdateResults();
        }

        private void loadContacts()
        {
            string sql = "SELECT contactId, contactName, contactLastname FROM tbContacts";

            SqlCommand sqlComand;
            sqlComand = new SqlCommand(sql, conn);
            using (SqlDataReader reader = sqlComand.ExecuteReader())
            {
                while (reader.Read())
                {
                    Contact ct = new Contact { Index = reader.GetInt32(reader.GetOrdinal("contactId")), FirstName = reader["contactName"].ToString().Trim(), LastName=reader["contactLastname"].ToString().Trim(), ID = reader.GetInt32(reader.GetOrdinal("contactId")), IsActive=true };
                    items.Add(ct);
                    allItems.Add(ct);
                }
            }
        }

        private void loadComapanies()
        {
            string sql = @"SELECT CASE WHEN s.companyId IS NOT NULL THEN s.soldToString ELSE CASE WHEN sh.companyId IS NOT NULL THEN sh.shipToString ELSE '' END END as companyString,
                    cd.companyId, cd.companyName, cd.companyAddress, cd.companyCode, cd.companyCity, cd.companyCountry,
                    CASE WHEN s.companyId IS NOT NULL THEN 'Sold-to' ELSE CASE WHEN sh.companyId IS NOT NULL THEN 'Ship-to' ELSE 'Carrier' END END as companyType, cd.isActive as IsActive
                    FROM tbCompanyDetails cd LEFT JOIN tbSoldTo s ON cd.companyId = s.companyId LEFT JOIN tbShipTo sh ON sh.companyId = cd.companyId LEFT JOIN tbCarriers c ON c.companyId = cd.companyId";


            SqlCommand sqlComand;
            sqlComand = new SqlCommand(sql, conn);
            using (SqlDataReader reader = sqlComand.ExecuteReader())
            {
                while (reader.Read())
                {
                    Company cp = new Company { Index = reader.GetInt32(reader.GetOrdinal("companyId")), Name=reader["companyName"].ToString().Trim(), Address = reader["companyAddress"].ToString().Trim(), Zip = reader["companyCode"].ToString().Trim(), ID = reader.GetInt32(reader.GetOrdinal("companyId")), City=reader["companyCity"].ToString().Trim(), CompanyString=reader["companyString"].ToString().Trim(), Country=reader["companyCountry"].ToString().Trim(), Type=reader["companyType"].ToString(), IsActive = Convert.ToBoolean(reader["IsActive"].ToString()) };
                    items.Add(cp);
                    allItems.Add(cp);
                }
            }
        }

        private void SearchKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dgItems.RowCount>0)
            {
                
                action(0);
                e.Handled = true;
            }
        }

        private void zPKGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMassBalance FrmMassBalance = new frmMassBalance();
            FrmMassBalance.Show();
        }

        private void ograniczeniaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmScheduleRestrictions FrmScheduleRestrictions = new frmScheduleRestrictions();
            FrmScheduleRestrictions.Show();
        }
    }
}
