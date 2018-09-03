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

namespace zfinViewer
{
    public partial class frmCompany : Form
    {
        string companySql = @"SELECT CASE WHEN s.companyId IS NOT NULL THEN s.soldToString ELSE CASE WHEN sh.companyId IS NOT NULL THEN sh.shipToString ELSE '' END END as companyString,
                    cd.companyId, cd.companyName, cd.companyAddress, cd.companyCode, cd.companyCity, cd.companyCountry,
                    CASE WHEN s.companyId IS NOT NULL THEN 'Sold-to' ELSE CASE WHEN sh.companyId IS NOT NULL THEN 'Ship-to' ELSE 'Carrier' END END as companyType, cd.companyVat
                    FROM tbCompanyDetails cd LEFT JOIN tbSoldTo s ON cd.companyId = s.companyId LEFT JOIN tbShipTo sh ON sh.companyId = cd.companyId LEFT JOIN tbCarriers c ON c.companyId = cd.companyId WHERE cd.companyId= @companyId;";

        int ID;
        Company thisCompany = new Company();
        public frmCompany(int companyId, frmZfinSearch parent)
        {
            InitializeComponent();
            ID = companyId;
            this.Owner = parent;
        }

        private void loadMe(object sender, EventArgs e)
        {
            this.Location = new Point(this.Owner.Location.X + 20, this.Owner.Location.Y + 20);
            SqlConnection conn = new SqlConnection(Variables.npdConnectionString);
            SqlCommand sqlComand = new SqlCommand(companySql, conn);
            sqlComand.Parameters.Add("@companyId", SqlDbType.Int);
            sqlComand.Parameters["@companyId"].Value = ID;
            try
            {
                conn.Open();
                using (SqlDataReader reader = sqlComand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //MessageBox.Show(reader["zfinName"].ToString());
                        thisCompany.ID = Convert.ToInt32(reader["companyId"].ToString());
                        thisCompany.Type = reader["companyType"].ToString();
                        txtName.Text = reader["companyName"].ToString().Trim();
                        txtAddress.Text = reader["companyAddress"].ToString().Trim();
                        txtZip.Text = reader["companyCode"].ToString().Trim();
                        txtCity.Text = reader["companyCity"].ToString().Trim();
                        txtCountry.Text = reader["companyCountry"].ToString().Trim();
                        txtType.Text = reader["companyType"].ToString();
                        txtVat.Text = reader["companyVat"].ToString();
                        displayContacts();
                        displayConnections();
                        loadHistory();

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
            }
        }

        private void displayContacts()
        {
            string sql = @"SELECT c.contactId as ID, c.contactName + ' ' + c.contactLastname as [Imię i nazwisko]
                    FROM tbCompanyDetails cd LEFT JOIN tbContacts c ON cd.companyId=c.contactCompany
                    WHERE cd.companyId=@companyId;";
            dgvConctacts.DataSource = sql;
            SqlConnection conn = new SqlConnection(Variables.npdConnectionString);
            SqlCommand sqlComand = new SqlCommand(sql, conn);
            sqlComand.Parameters.Add("@companyId", SqlDbType.Int);
            sqlComand.Parameters["@companyId"].Value = ID;
            SqlDataAdapter sqlDataAdap = new SqlDataAdapter(sqlComand);
            DataTable dtContacts = new DataTable();
            sqlDataAdap.Fill(dtContacts);
            dgvConctacts.DataSource = dtContacts;
            dgvConctacts.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvConctacts.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void displayConnections()
        {
            string sql;

            switch (thisCompany.Type)
            {
                case "Carrier":
                    sql = @"SELECT cdSh.companyId as ID, sh.shipToString + ' ' + cdSh.companyName + ', ' + cdSh.companyCity + ', ' + cdsh.companyCountry as Firma, CONVERT(varchar,COUNT(dd.shipToId)) + ' wysyłek, ostatnia ' + CONVERT(varchar,CONVERT(date,MAX(t.transportDate))) as Relacja
                        FROM tbCompanyDetails cd LEFT JOIN tbCarriers c ON c.companyId=cd.companyId LEFT JOIN tbTransport t ON t.carrierId=c.carrierId LEFT JOIN tbCmr cmr ON cmr.transportId=t.transportId LEFT JOIN tbDeliveryDetail dd ON cmr.detailId=dd.cmrDetailId
                        LEFT JOIN tbShipTo sh ON sh.shipToId=dd.shipToId LEFT JOIN tbCompanyDetails cdSh ON cdSh.companyId = sh.companyId 
                        WHERE cd.companyId=@id
                        GROUP BY cdSh.companyId, sh.shipToString + ' ' + cdSh.companyName + ', ' + cdSh.companyCity + ', ' + cdsh.companyCountry
                        ORDER BY COUNT(dd.shipToId) DESC";
                    break;
                case "Ship-to":
                    sql = @"SELECT cCd.companyId as ID, ccd.companyName + ', ' + ccd.companyAddress + ', ' + ccd.companyCity + ', ' + ccd.companyCountry as Firma, CONVERT(varchar,COUNT(dd.cmrDetailId)) + ' razy dostarczył usługę transportową, ostatni raz ' + CONVERT(varchar,CONVERT(date,MAX(t.transportDate))) as Relacja 
                        FROM tbCompanyDetails shCd LEFT JOIN tbShipTo sh ON sh.companyId=shCd.companyId LEFT JOIN tbDeliveryDetail dd ON dd.shipToId=sh.shipToId LEFT JOIN tbCmr cmr ON cmr.detailId=dd.cmrDetailId LEFT JOIN tbTransport t ON t.transportId=cmr.transportId LEFT JOIN tbCarriers c ON c.carrierId=t.carrierId LEFT JOIN tbCompanyDetails cCd ON cCd.companyId=c.companyId
                        WHERE shCd.companyId=@id
                        GROUP BY cCd.companyId, ccd.companyName + ', ' + ccd.companyAddress + ', ' + ccd.companyCity + ', ' + ccd.companyCountry
                        UNION
                        SELECT ccd. companyId as ID, ccd.companyName + ', ' + ccd.companyAddress + ', ' + ccd.companyCity + ', ' + ccd.companyCountry as Firma, 'Pomocniczy przewoźnik' as Relacja
                        FROM tbCompanyDetails shCd LEFT JOIN tbShipTo sh ON sh.companyId=shCd.companyId LEFT JOIN tbCarriers c ON c.carrierId=sh.supportiveCarrier LEFT JOIN tbCompanyDetails cCd ON cCd.companyId=c.companyId
                        WHERE shCd.companyId=@id AND ccd.companyId IS NOT NULL
                        UNION
                        SELECT cCd.companyId as ID, ccd.companyName + ', ' + ccd.companyAddress + ', ' + ccd.companyCity + ', ' + ccd.companyCountry as Firma, 'Podstawowy przewoźnik' as Relacja
                        FROM tbCompanyDetails shCd LEFT JOIN tbShipTo sh ON sh.companyId=shCd.companyId LEFT JOIN tbCarriers c ON c.carrierId=sh.primaryCarrier LEFT JOIN tbCompanyDetails cCd ON cCd.companyId=c.companyId
                        WHERE shCd.companyId=@id
                        UNION
                        SELECT sCd.companyId as ID, s.soldToString + ' ' + sCd.companyName + ', ' + sCd.companyCity + ', ' + sCd.companyCountry as Firma, 'Sold-to, do którego należy to ship-to' as Relacja
                        FROM tbCompanyDetails shCd LEFT JOIN tbShipTo sh ON sh.companyId=shCd.companyId LEFT JOIN tbSoldTo s ON s.soldToId=sh.soldTo LEFT JOIN tbCompanyDetails sCd ON sCd.companyId=s.companyId
                        WHERE shCd.companyId=@id";
                    break;
                case "Sold-to":
                    sql = @"SELECT shCd.companyId as ID, sh.shipToString + ' ' + shCd.companyName + ', ' + shCd.companyCity + ', ' + shCd.companyCountry as Firma, CONVERT(varchar,COUNT(dd.cmrDetailId)) + ' dostaw, w sumie ' + CONVERT(varchar,SUM(dd.weightNet)/1000) + ' ton netto' as Relacja
                        FROM tbCompanyDetails sCd LEFT JOIN tbSoldTo s ON s.companyId = sCd.companyid LEFT JOIN tbShipTo sh ON sh.soldTo = s.soldToId LEFT JOIN tbCompanyDetails shCd ON shCd.companyId = sh.companyId
                        LEFT JOIN tbDeliveryDetail dd ON dd.shipToId = sh.shipToId
                        WHERE sCd.companyId = @id
                        GROUP BY shCd.companyId, sh.shipToString + ' ' + shCd.companyName + ', ' + shCd.companyCity + ', ' + shCd.companyCountry
                        ORDER BY SUM(dd.weightNet) DESC";
                    break;
                default:
                    sql = @"SELECT cdSh.companyId as ID, sh.shipToString + ' ' + cdSh.companyName + ', ' + cdSh.companyCity + ', ' + cdsh.companyCountry as Firma, CONVERT(varchar,COUNT(dd.shipToId)) + ' wysyłek, ostatnia ' + CONVERT(varchar,CONVERT(date,MAX(t.transportDate))) as Relacja
                        FROM tbCompanyDetails cd LEFT JOIN tbCarriers c ON c.companyId=cd.companyId LEFT JOIN tbTransport t ON t.carrierId=c.carrierId LEFT JOIN tbCmr cmr ON cmr.transportId=t.transportId LEFT JOIN tbDeliveryDetail dd ON cmr.detailId=dd.cmrDetailId
                        LEFT JOIN tbShipTo sh ON sh.shipToId=dd.shipToId LEFT JOIN tbCompanyDetails cdSh ON cdSh.companyId = sh.companyId 
                        WHERE cd.companyId=@id
                        GROUP BY cdSh.companyId, sh.shipToString + ' ' + cdSh.companyName + ', ' + cdSh.companyCity + ', ' + cdsh.companyCountry
                        ORDER BY COUNT(dd.shipToId) DESC";
                    break;
            }

            dgvConnected.DataSource = sql;
            SqlConnection conn = new SqlConnection(Variables.npdConnectionString);
            SqlCommand sqlComand = new SqlCommand(sql, conn);
            sqlComand.Parameters.Add("@id", SqlDbType.Int);
            sqlComand.Parameters["@id"].Value = ID;
            SqlDataAdapter sqlDataAdap = new SqlDataAdapter(sqlComand);
            DataTable dtConnections = new DataTable();
            sqlDataAdap.Fill(dtConnections);
            dgvConnected.DataSource = dtConnections;
            dgvConnected.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvConnected.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvConnected.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void loadContact(object sender, EventArgs e)
        {
            int i = dgvConctacts.CurrentCell.RowIndex;
            int contactId = Convert.ToInt32(dgvConctacts.Rows[i].Cells[0].Value);
            frmContact contact = new frmContact(contactId,this);
            contact.Show();
        }

        private void loadHistory()
        {

            var summaryType = new[]
            {
                "Bez podsumowania","Tydzień","Miesiąc","Rok"
            };

            cmbSummaryType.DataSource = summaryType;

            if (thisCompany.Type == "Ship-to")
            {
                var historyType = new[]
                {
                   "Wysyłki","Zapotrzebowanie"
                };
                cmbHistoryType.DataSource = historyType;
            }
            else if (thisCompany.Type == "Carrier")
            {
                var historyType = new[]
                {
                   "Wysyłki"
                };
                cmbHistoryType.DataSource = historyType;
            }
            else if (thisCompany.Type == "Sold-to")
            {
                var historyType = new[]
               {
                   "Wysyłki","Zapotrzebowanie"
                };
                cmbHistoryType.DataSource = historyType;
            }

        }

        private void updateHistory(string sqlStr)
        {
            dgvHistory.DataSource = sqlStr;
            SqlConnection conn = new SqlConnection(Variables.npdConnectionString);
            SqlCommand sqlComand = new SqlCommand(sqlStr, conn);
            sqlComand.Parameters.Add("@id", SqlDbType.Int);
            sqlComand.Parameters["@id"].Value = ID;
            SqlDataAdapter sqlDataAdap = new SqlDataAdapter(sqlComand);
            DataTable dtHistory = new DataTable();
            sqlDataAdap.Fill(dtHistory);
            dgvHistory.DataSource = dtHistory;
        }


        private void historyUpdate()
        {
            string sql = "";

            switch (thisCompany.Type)
            {
                case "Ship-to":
                    switch (cmbHistoryType.Text)
                    {
                        case "Wysyłki":
                            switch (cmbSummaryType.Text)
                            {
                                case "Bez podsumowania":
                                    sql = @"SELECT CONVERT(date,qr.qDate) as [Data], z.zfinIndex as [Indeks], z.zfinName as [Nazwa], qr.delNumber as [Delivery Note], SUM(qd.batchSize*u.unitWeight) as [Ilosc kg]
                                    FROM tbQdocReconciliation qr LEFT JOIN tbQdocData qd ON qr.qReconciliationId = qd.qReconciliationId LEFT JOIN tbBatch b ON b.batchId = qd.batchId LEFT JOIN tbZfin z ON z.zfinId = b.zfinId LEFT JOIN tbUom u ON u.zfinId = z.zfinId
                                    WHERE CONVERT(nvarchar, qr.delNumber) IN(
                                    SELECT dd.deliveryNote
                                    FROM tbCompanyDetails shCd LEFT JOIN tbShipTo sh ON sh.companyId = shCd.companyId LEFT JOIN tbDeliveryDetail dd ON dd.shipToId = sh.shipToId
                                    WHERE shCd.companyId = @id)
                                    GROUP BY qr.qDate, z.zfinIndex, z.zfinName, qr.delNumber
                                    ORDER BY qr.qDate DESC, qr.delNumber";
                                    break;
                            }
                            break;
                        case "Zapotrzebowanie":
                            switch(cmbSummaryType.Text)
                            {
                                case "Bez podsumowania":
                                    sql = @"SELECT CONVERT(date,r.deliveryDate) as [Data], z.zfinIndex as [Index], z.zfinName as [Nazwa], r.amount as [Ilość szt.],
                                    ROUND(r.amount * u.unitWeight,1) as [Ilość kg], ROUND(r.amount/u.pcPerPallet,1) as [Ilość pal]
                                    FROM tbCompanyDetails cd LEFT JOIN tbShipTo sh ON sh.companyId=cd.companyId 
                                    LEFT JOIN tbCustomerString cs ON cs.companyId=cd.companyId LEFT JOIN tbReqs r ON r.target=cs.custStringId
                                    LEFT JOIN tbZfin z ON z.zfinId=r.zfinId LEFT JOIN tbUom u ON u.zfinId=z.zfinId
                                    WHERE cd.companyId=@id
                                    ORDER BY Data DESC";
                                    break;
                                case "Tydzień":
                                    sql = @"SELECT CONVERT(char(4),YEAR(r.deliveryDate)) + '_' + CASE WHEN DATEPART(ISO_WEEK,r.deliveryDate) < 10 THEN '0' + CONVERT(char(1),DATEPART(ISO_WEEK,r.deliveryDate)) ELSE CONVERT(char(2),DATEPART(ISO_WEEK,r.deliveryDate)) END as [Tydzień], 
                                    z.zfinIndex as [Index], z.zfinName as [Nazwa], SUM(r.amount) as [Ilość szt.],
                                    SUM(ROUND(r.amount * u.unitWeight,1)) as [Ilość kg], SUM(ROUND(r.amount/u.pcPerPallet,1)) as [Ilość pal]
                                    FROM tbCompanyDetails cd LEFT JOIN tbShipTo sh ON sh.companyId=cd.companyId 
                                    LEFT JOIN tbCustomerString cs ON cs.companyId=cd.companyId LEFT JOIN tbReqs r ON r.target=cs.custStringId
                                    LEFT JOIN tbZfin z ON z.zfinId=r.zfinId LEFT JOIN tbUom u ON u.zfinId=z.zfinId
                                    WHERE cd.companyId=@id
                                    GROUP BY CONVERT(char(4),YEAR(r.deliveryDate)) + '_' + CASE WHEN DATEPART(ISO_WEEK,r.deliveryDate) < 10 THEN '0' + CONVERT(char(1),DATEPART(ISO_WEEK,r.deliveryDate)) ELSE CONVERT(char(2),DATEPART(ISO_WEEK,r.deliveryDate)) END, z.zfinIndex, z.zfinName
                                    ORDER BY Tydzień DESC";
                                    break;
                                case "Miesiąc":
                                    sql = @"SELECT CONVERT(char(4),YEAR(r.deliveryDate)) + '_' + CASE WHEN MONTH(r.deliveryDate) < 10 THEN '0' + CONVERT(char(1),MONTH(r.deliveryDate)) ELSE CONVERT(char(2),MONTH(r.deliveryDate)) END as [Miesiąc], 
                                    z.zfinIndex as [Index], z.zfinName as [Nazwa], SUM(r.amount) as [Ilość szt.],
                                    SUM(ROUND(r.amount * u.unitWeight,1)) as [Ilość kg], SUM(ROUND(r.amount/u.pcPerPallet,1)) as [Ilość pal]
                                    FROM tbCompanyDetails cd LEFT JOIN tbShipTo sh ON sh.companyId=cd.companyId 
                                    LEFT JOIN tbCustomerString cs ON cs.companyId=cd.companyId LEFT JOIN tbReqs r ON r.target=cs.custStringId
                                    LEFT JOIN tbZfin z ON z.zfinId=r.zfinId LEFT JOIN tbUom u ON u.zfinId=z.zfinId
                                    WHERE cd.companyId=@id
                                    GROUP BY CONVERT(char(4),YEAR(r.deliveryDate)) + '_' + CASE WHEN MONTH(r.deliveryDate) < 10 THEN '0' + CONVERT(char(1),MONTH(r.deliveryDate)) ELSE CONVERT(char(2),MONTH(r.deliveryDate)) END, z.zfinIndex, z.zfinName
                                    ORDER BY Miesiąc DESC";
                                    break;
                                case "Rok":
                                    sql = @"SELECT YEAR(r.deliveryDate) as [Rok], 
                                    z.zfinIndex as [Index], z.zfinName as [Nazwa], SUM(r.amount) as [Ilość szt.],
                                    SUM(ROUND(r.amount * u.unitWeight,1)) as [Ilość kg], SUM(ROUND(r.amount/u.pcPerPallet,1)) as [Ilość pal]
                                    FROM tbCompanyDetails cd LEFT JOIN tbShipTo sh ON sh.companyId=cd.companyId 
                                    LEFT JOIN tbCustomerString cs ON cs.companyId=cd.companyId LEFT JOIN tbReqs r ON r.target=cs.custStringId
                                    LEFT JOIN tbZfin z ON z.zfinId=r.zfinId LEFT JOIN tbUom u ON u.zfinId=z.zfinId
                                    WHERE cd.companyId=@id
                                    GROUP BY YEAR(r.deliveryDate), z.zfinIndex, z.zfinName
                                    ORDER BY Rok DESC";
                                    break;
                            }
                            break;
                    }
                    break;
                case "Carrier":
                    switch (cmbHistoryType.Text)
                    {
                        case "Wysyłki":
                            switch (cmbSummaryType.Text)
                            {
                                case "Bez podsumowania":
                                    sql = @"SELECT CONVERT(date,t.transportDate) as [Data], t.transportNumber as [Numer transportu], sh.shipToString + ' ' + cdSh.companyName + ', ' + cdSh.companyCity + ', ' + cdSh.companyCountry as [Miejsce dostawy], dd.deliveryNote as [Delivery Note]  
                                        FROM tbCompanyDetails cd LEFT JOIN tbCarriers car ON car.companyId=cd.companyId LEFT JOIN tbTransport t ON t.carrierId=car.carrierId LEFT JOIN tbCmr cmr ON cmr.transportId=t.transportId LEFT JOIN tbDeliveryDetail dd ON dd.cmrDetailId=cmr.detailId 
                                        LEFT JOIN tbShipTo sh ON sh.shipToId=dd.shipToId LEFT JOIN tbCompanyDetails cdSh ON cdSh.companyId=sh.companyId
                                        WHERE cd.companyId=@id
                                        ORDER BY Data DESC";
                                    break;
                                case "Tydzień":
                                    sql = @"SELECT CONVERT(char(4),YEAR(t.transportDate)) + '_' + CASE WHEN DATEPART(ISO_WEEK,t.transportDate) >9 THEN CONVERT(varchar(2),DATEPART(ISO_WEEK,t.transportDate)) ELSE '0' + CONVERT(varchar(2),DATEPART(ISO_WEEK,t.transportDate)) END as [Tydzień],
                                         COUNT(DISTINCT t.transportNumber) as [Liczba transportów], sh.shipToString + ' ' + cdSh.companyCity + ', ' + cdSh.companyCountry as [Miejsce]
                                        FROM tbCompanyDetails cd LEFT JOIN tbCarriers car ON car.companyId=cd.companyId LEFT JOIN tbTransport t ON t.carrierId=car.carrierId LEFT JOIN tbCmr cmr ON cmr.transportId=t.transportId LEFT JOIN tbDeliveryDetail dd ON dd.cmrDetailId=cmr.detailId 
                                        LEFT JOIN tbShipTo sh ON sh.shipToId=dd.shipToId LEFT JOIN tbCompanyDetails cdSh ON cdSh.companyId=sh.companyId
                                        WHERE cd.companyId=@id
                                        GROUP BY CONVERT(char(4),YEAR(t.transportDate)) + '_' + CASE WHEN DATEPART(ISO_WEEK,t.transportDate) >9 THEN CONVERT(varchar(2),DATEPART(ISO_WEEK,t.transportDate)) ELSE '0' + CONVERT(varchar(2),DATEPART(ISO_WEEK,t.transportDate)) END,
                                        sh.shipToString + ' ' + cdSh.companyCity + ', ' + cdSh.companyCountry
                                        ORDER BY Tydzień DESC";
                                    break;
                                case "Miesiąc":
                                    sql = @"SELECT CONVERT(char(4),YEAR(t.transportDate)) + '_' + CASE WHEN MONTH(t.transportDate) >9 THEN CONVERT(varchar(2),MONTH(t.transportDate)) ELSE '0' + CONVERT(varchar(2),MONTH(t.transportDate)) END as [Miesiąc],
                                         COUNT(DISTINCT t.transportNumber) as [Liczba transportów], sh.shipToString + ' ' + cdSh.companyCity + ', ' + cdSh.companyCountry as [Miejsce]
                                        FROM tbCompanyDetails cd LEFT JOIN tbCarriers car ON car.companyId=cd.companyId LEFT JOIN tbTransport t ON t.carrierId=car.carrierId LEFT JOIN tbCmr cmr ON cmr.transportId=t.transportId LEFT JOIN tbDeliveryDetail dd ON dd.cmrDetailId=cmr.detailId 
                                        LEFT JOIN tbShipTo sh ON sh.shipToId=dd.shipToId LEFT JOIN tbCompanyDetails cdSh ON cdSh.companyId=sh.companyId
                                        WHERE cd.companyId=@id
                                        GROUP BY CONVERT(char(4),YEAR(t.transportDate)) + '_' + CASE WHEN MONTH(t.transportDate) >9 THEN CONVERT(varchar(2),MONTH(t.transportDate)) ELSE '0' + CONVERT(varchar(2),MONTH(t.transportDate)) END,
                                        sh.shipToString + ' ' + cdSh.companyCity + ', ' + cdSh.companyCountry
                                        ORDER BY Miesiąc DESC";
                                    break;
                                case "Rok":
                                    sql = @"SELECT YEAR(t.transportDate) as [Rok],
                                         COUNT(DISTINCT t.transportNumber) as [Liczba transportów], sh.shipToString + ' ' + cdSh.companyCity + ', ' + cdSh.companyCountry as [Miejsce]
                                        FROM tbCompanyDetails cd LEFT JOIN tbCarriers car ON car.companyId=cd.companyId LEFT JOIN tbTransport t ON t.carrierId=car.carrierId LEFT JOIN tbCmr cmr ON cmr.transportId=t.transportId LEFT JOIN tbDeliveryDetail dd ON dd.cmrDetailId=cmr.detailId 
                                        LEFT JOIN tbShipTo sh ON sh.shipToId=dd.shipToId LEFT JOIN tbCompanyDetails cdSh ON cdSh.companyId=sh.companyId
                                        WHERE cd.companyId=@id
                                        GROUP BY YEAR(t.transportDate),
                                        sh.shipToString + ' ' + cdSh.companyCity + ', ' + cdSh.companyCountry
                                        ORDER BY Rok DESC";
                                    break;
                            }
                            break;
                    }
                    break;
            }

            updateHistory(sql);
        }

        private void historyTypeChanged(object sender, EventArgs e)
        {
            if (cmbHistoryType.SelectedIndex > -1 && cmbSummaryType.SelectedIndex > -1)
            {
                historyUpdate();
            }
        }

        private void summaryTypeChanged(object sender, EventArgs e)
        {
            if (cmbHistoryType.SelectedIndex > -1 && cmbSummaryType.SelectedIndex > -1)
            {
                historyUpdate();
            }
        }

        private void summaryChanged(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Suma")
            {

                ddSummary.Text = "Suma";
            }else if (e.ClickedItem.Text == "Liczba")
            {
                ddSummary.Text = "Liczba";
            }
            else
            {
                ddSummary.Text = "Średnia";
            }
            updateSummary();
        }

        private void dgSelectionChanged(object sender, EventArgs e)
        {
            updateSummary();
        }

        private void updateSummary()
        {
            int counter = 0;
            double Result = 0;
            double n;
            bool greenlight = false;
            DataGridView dg = dgvHistory;

            if (tabCompany.SelectedTab.Name == "pgHistory")
            {
                greenlight = true;

            }else if(tabCompany.SelectedTab.Name == "pgConnected"){
                dg = dgvConnected;
                greenlight = true;
            }
            else if (tabCompany.SelectedTab.Name == "pgContacts")
            {
                dg = dgvConctacts;
                greenlight = true;
            }

            if (greenlight)
            {
                if (ddSummary.Text == "Suma")
                {
                    foreach (DataGridViewCell cell in dg.SelectedCells)
                    {
                        if (Double.TryParse(cell.Value.ToString(), out n))
                        {
                            Result += n;
                        }
                    }
                    if (Result > 0)
                    {
                        lblStatus.Text = Result.ToString();
                    }
                }
                else if (ddSummary.Text == "Liczba")
                {
                    foreach (DataGridViewCell cell in dg.SelectedCells)
                    {
                        Result++;
                    }
                    if (Result > 0)
                    {
                        lblStatus.Text = Result.ToString();
                    }
                }
                else if (ddSummary.Text == "Średnia")
                {
                    foreach (DataGridViewCell cell in dg.SelectedCells)
                    {
                        if (Double.TryParse(cell.Value.ToString(), out n))
                        {
                            Result += n;
                            counter++;
                        }
                    }
                    if (Result > 0)
                    {
                        lblStatus.Text = (Result / counter).ToString();
                    }
                }
            }
        }

        private void dgConnSelectionChanged(object sender, EventArgs e)
        {
            updateSummary();
        }

        private void dgContSelectionChanged(object sender, EventArgs e)
        {
            updateSummary();
        }
    }
}   