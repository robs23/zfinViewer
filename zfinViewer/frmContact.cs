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
    public partial class frmContact : Form
    {
        string contactSql = @"SELECT contactName, contactLastname, contactMail1, contactMail2, contactPhone,
            contactMobile, CASE WHEN s.soldToString IS NOT NULL THEN s.soldToString + ' ' ELSE CASE WHEN sh.shipToString IS NOT NULL THEN sh.shipToString + ' ' ELSE '' END END + cd.companyName + ', ' + cd.companyCity + ', ' + cd.companyCountry as company
            FROM tbContacts c LEFT JOIN tbCompanyDetails cd ON cd.companyId = c.contactCompany LEFT JOIN tbSoldTo s ON s.companyId= cd.companyId LEFT JOIN tbShipTo sh ON sh.companyId = cd.companyId
            WHERE c.contactId =@contactId;";
 
        int contactId;
        Contact thisContact = new Contact();
        public frmContact(int contId, frmZfinSearch parent)
        {
            InitializeComponent();
            contactId = contId;
            this.Owner = parent;
        }

        public frmContact(int contId, frmCompany parent)
        {
            InitializeComponent();
            contactId = contId;
            this.Owner = parent;
        }

        private void tb_Click(object sender, System.EventArgs e)
        {
            if (((TextBox)sender).Text.Length > 0)
            {
                Clipboard.SetText(((TextBox)sender).Text);
                frmToast toast = new frmToast("Skopiowano", Cursor.Position.Y, Cursor.Position.X);
                toast.Show();
            }
        }   

        private void loadMe(object sender, EventArgs e)
        {
            this.Location = new Point(this.Owner.Location.X + 20, this.Owner.Location.Y + 20);
            SqlConnection conn = new SqlConnection(Variables.npdConnectionString);
            SqlCommand sqlComand = new SqlCommand(contactSql, conn);
            sqlComand.Parameters.Add("@contactId", SqlDbType.Int);
            sqlComand.Parameters["@contactId"].Value = contactId;
            try
            {
                conn.Open();
                using (SqlDataReader reader = sqlComand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //MessageBox.Show(reader["zfinName"].ToString());
                        txtName.Text = reader["contactName"].ToString();
                        txtLastname.Text = reader["contactLastname"].ToString();
                        txtMail.Text = reader["contactMail1"].ToString();
                        txtMail2.Text = reader["contactMail2"].ToString();
                        txtPhone.Text = reader["contactPhone"].ToString();
                        txtPhone2.Text = reader["contactMobile"].ToString();
                        txtCompany.Text = reader["company"].ToString();
                    }
                }
            }
            catch
            {
                MessageBox.Show(String.Format("Nie udało się nawiązać połączenia z bazą danych", "Błąd połączenia z bazą danych"));
            }
            finally
            {
                conn.Close();
                //AddEvent(this);
                txtMail.DoubleClick += new EventHandler(mailTo);
                txtMail2.DoubleClick += new EventHandler(mailTo);
                txtPhone.DoubleClick += new EventHandler(callTo);
                txtPhone2.DoubleClick += new EventHandler(callTo);
            }
        }

        //private void AddEvent(Control parentCtrl)
        //{
        //    foreach (Control tb in parentCtrl.Controls)
        //    {
        //        if(tb.GetType() == typeof(TextBox)){
        //            if(tb != txtMail)
        //            {
        //                tb.Click += new EventHandler(tb_Click);
        //            }
        //        }
        //        AddEvent(tb);
        //    }
        //}

        private void mailTo(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text.Length > 0)
            {
                frmToast toast = new frmToast("Tworzę wiadomość", Cursor.Position.Y, Cursor.Position.X);
                toast.Show();
                string command = "mailto:" + ((TextBox)sender).Text;
                Process.Start(command);
            }
        }

        private void callTo(object sender, EventArgs e)
        {
            string who = ((TextBox)sender).Text;
            if (who.Length > 0)
            {
                frmToast toast = new frmToast("Przechodzę do Skype", Cursor.Position.Y, Cursor.Position.X);
                toast.Show();
                string command = "tel:" + who;
                Process.Start(command);
            }
        }

        private void DigFromCompany(object sender, EventArgs e)
        {
            string[] result;
            result = txtCompany.Text.Split(new string[] { "," }, StringSplitOptions.None);
            if(result[0].Length > 0)
            {
                MessageBox.Show(result[0]);
            }
        }
    }
}
