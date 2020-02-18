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
using System.Text.RegularExpressions;
using System.Globalization;

namespace zfinViewer
{
    public partial class frmZfinOverview : Form
    {
        string zfinSql = @"SELECT z.zfinId, z.zfinIndex, z.zfinName, z.zfinIndex, CAST(z1.zfinIndex as nvarchar) + ' ' + z1.zfinName as zfor, u.unitWeight, u.pcPerPallet, u.pcPerBox, u.pcLayer, z.zfinType, z.prodStatus, mt.materialTypeName as Category,
            CAST(p.palletLength as varchar(4)) + 'x' + CAST(p.palletWidth as varchar(4)) + CASE WHEN p.palletChep<>0 THEN ' CHEP' ELSE ' NIE CHEP' END as Pallet,
            zp.[beans?] as Ziarno, zp.[decafe?] as Bezkofeinowa, zp.[eco?] as Ekologiczna, zp.[utz?] as UTZ, zp.[single-origin?] as [Single-origin], zp.[aromatic?] as Aromatyzowana, cs.custString as Customer,
            'Utworzony w dniu ' + CONVERT(nchar(19),z.creationDate, 121) + ' przez ' + us.userName + ' ' + us.userSurname as creationDate,
            'Ostatnio modyfikowany w dniu ' + CONVERT(nchar(19),z.lastUpdate, 121) + ' przez ' + us1.userName + ' ' + us1.userSurname as updateDate,
            cost.cost, cost.CostLotSize, cost.CostLotSizeUnit
            FROM tbZfin z LEFT JOIN tbZfinZfor zz ON z.zfinId=zz.zfinId LEFT JOIN tbZfin z1 ON z1.zfinId = zz.zforId
            LEFT JOIN tbUom u ON z.zfinId = u.zfinId LEFT JOIN tbPallets p ON u.palletType = p.palletId
            LEFT JOIN tbZfinProperties zp ON z.zfinId= zp.zfinId LEFT JOIN  tbCustomerString cs ON cs.custStringId = z.custString
            LEFT JOIN tbUsers us ON us.UserId = z.createdBy LEFT JOIN tbUsers us1 ON us1.UserId=z.lastUpdateBy
            LEFT JOIN tbMaterialType mt ON mt.materialTypeId = z.materialType
            LEFT JOIN tbCosting cost ON cost.zfinId=z.zfinId LEFT JOIN tbCostingReconciliation cr ON cr.CostingReconciliationId=cost.reconciliationId
            WHERE z.zfinIndex= @index AND (cost.reconciliationId = (SELECT TOP(1) reconciliationId FROM tbCostingReconciliation ORDER BY dateAdded DESC) OR cost.reconciliationID IS NULL)";
        int zfinIndex;
        Product thisProduct = new Product();

        public frmZfinOverview(int zfinId, Form parent)
        {
            InitializeComponent();
            zfinIndex = zfinId;
            this.Owner = parent;
        }

        private void loadMe(object sender, EventArgs e)
        {
            this.Location = new Point(this.Owner.Location.X + 20, this.Owner.Location.Y + 20);
            dgProd.ReadOnly = true;
            dgBom.ReadOnly = true;
            dgWhereUsed.ReadOnly = true;
            SqlConnection conn = new SqlConnection(Variables.npdConnectionString);
            SqlCommand sqlComand = new SqlCommand(zfinSql, conn);
            //sqlComand.CommandType = CommandType.Text;
            //sqlComand.Parameters.AddWithValue("@index", Variables.index);
            sqlComand.Parameters.Add("@index", SqlDbType.Int);
            sqlComand.Parameters["@index"].Value = zfinIndex;
            try{
                conn.Open();
                using (SqlDataReader reader = sqlComand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //MessageBox.Show(reader["zfinName"].ToString());
                        txtZfinNumber.Text = reader["zfinIndex"].ToString();
                        thisProduct.Index = reader.GetInt32(reader.GetOrdinal("zfinIndex"));
                        txtZfinName.Text = reader["zfinName"].ToString();
                        txtType.Text = reader["zfinType"].ToString();
                        txtCategory.Text = reader["Category"].ToString();
                        txtStatus.Text = reader["prodStatus"].ToString();
                        if (reader["cost"].ToString().Length > 0)
                        {
                            txtCost.Text = reader.GetDouble(reader.GetOrdinal("cost")).ToString() + " / " + reader.GetDouble(reader.GetOrdinal("CostLotSize")).ToString() + " " + reader["CostLotSizeUnit"].ToString();
                        }
                        lblCreated.Text = reader["creationDate"].ToString(); //?? "Empty1";
                        lblUpdated.Text = reader["updateDate"].ToString(); //?? "Empty1";
                        if (reader["zfinType"].ToString().ToLower() == "zfin")
                        {
                            txtUnitWeight.Text = reader["unitWeight"].ToString();
                            txtPcPerPal.Text = reader["pcPerPallet"].ToString();
                            txtPcPerBox.Text = reader["pcPerBox"].ToString();
                            txtPcPerLayer.Text = reader["pcLayer"].ToString();
                            txtKgPerPal.Text = Math.Round(reader.GetInt32(reader.GetOrdinal("pcPerPallet")) * reader.GetDouble(reader.GetOrdinal("unitWeight")), 1, MidpointRounding.AwayFromZero).ToString();
                            txtLayPerPal.Text = (reader.GetInt32(reader.GetOrdinal("pcPerPallet")) / reader.GetInt32(reader.GetOrdinal("pcLayer"))).ToString();
                            txtPalletType.Text = reader["Pallet"].ToString();
                        }


                        //fill in this Product
                        thisProduct.ID = reader.GetInt32(reader.GetOrdinal("zfinid"));
                        thisProduct.Index = reader.GetInt32(reader.GetOrdinal("zfinIndex"));
                        thisProduct.Name = reader["zfinName"].ToString();
                        thisProduct.Type = reader["zfinType"].ToString();
                        thisProduct.Category = reader["Category"].ToString();
                        thisProduct.Status = reader["prodStatus"].ToString();
                        if (reader["cost"].ToString().Length > 0)
                        {
                            thisProduct.cost = reader.GetDouble(reader.GetOrdinal("cost"));
                            thisProduct.costLotSize = (float)reader.GetDouble(reader.GetOrdinal("CostLotSize"));
                            thisProduct.costLotSizeUnit = reader["CostLotSizeUnit"].ToString();
                        }
                        if (reader["zfinType"].ToString().ToLower() == "zfin")
                        {
                            thisProduct.unitWeight = reader.GetDouble(reader.GetOrdinal("unitWeight"));
                            thisProduct.palletCount = reader.GetInt32(reader.GetOrdinal("pcPerPallet"));
                            thisProduct.boxCount = reader.GetInt32(reader.GetOrdinal("pcPerBox"));
                        }

                        //fill in the properties
                        for (int i = 0; i < clbProperties.Items.Count; i++)
                        {
                            string propName = clbProperties.Items[i].ToString();
                            if (!reader.IsDBNull(reader.GetOrdinal(propName)))
                            {
                                clbProperties.SetItemCheckState(i, (Convert.ToBoolean(reader.GetInt32(reader.GetOrdinal(propName))) ? CheckState.Checked : CheckState.Unchecked));
                            }
                            else
                            {
                                clbProperties.SetItemCheckState(i, CheckState.Indeterminate);
                            }
                        }
                        hideTabs();
                    }
                }
            }
            catch
            {
                MessageBox.Show(String.Format("Nie udało się nawiązać połączenia z bazą danych", "Błąd połączenia z bazą danych"));
            }
            finally
            {
                this.Text = thisProduct.Index.ToString() + " || " + thisProduct.Name;
                conn.Close();
            }
        }

        private void hideTabs()
        {
            if (thisProduct.Type.ToLower() != "zfin" && thisProduct.Type.ToLower() != "zfor")
            {
                tabAll.TabPages.Remove(pgBom);
                tabAll.TabPages.Remove(pgProperties);
            }
            if (thisProduct.Type.ToLower() != "zfin")
            {
                tabAll.TabPages.Remove(pgCalc);
                tabAll.TabPages.Remove(pgUom);
            }
            if (thisProduct.Type.ToLower() == "zfin")
            {
                tabAll.TabPages.Remove(pgWhereUsed);
            }
        }

        private void updateProdHistory(string sqlStr)
        {
            DateTime startDate = DateTime.Now.AddDays(-7).StartOfWeek(DayOfWeek.Monday); //new DateTime(2019, 9, 22,22,0,0);
            dgProd.DataSource = sqlStr;
            SqlConnection conn = new SqlConnection(Variables.npdConnectionString);
            SqlCommand sqlComand = new SqlCommand(sqlStr, conn);
            sqlComand.Parameters.Add("@index", SqlDbType.Int);
            sqlComand.Parameters["@index"].Value = zfinIndex;
            sqlComand.Parameters.Add("@startDate", SqlDbType.DateTime);
            sqlComand.Parameters["@startDate"].Value = startDate;
            SqlDataAdapter sqlDataAdap = new SqlDataAdapter(sqlComand);
            DataTable dtProd = new DataTable();
            sqlDataAdap.Fill(dtProd);
            if(cmbDataType.Text== "Przepływ")
            {
                double prevValue = 0;
                DataColumn col = new DataColumn("Stan po");
                dtProd.Columns.Add(col);
                col.SetOrdinal(3);
                double currValue = 0;

                for (int i = 0; i < dtProd.Rows.Count; i++)
                {
                    
                    if ((string)dtProd.Rows[i][1]!="Zapas")
                    {
                        currValue = prevValue + Convert.ToDouble(dtProd.Rows[i][2]);
                        dtProd.Rows[i][3] = currValue;
                        prevValue = Convert.ToDouble(dtProd.Rows[i][3]);
                    }
                    else
                    {
                        dtProd.Rows[i][3] = currValue;
                        prevValue = Convert.ToDouble(dtProd.Rows[i][2]);
                    }
                    
                }
            }
            
            dgProd.DataSource = dtProd;
        }

        private void updateLosses(string sqlStr, DateTime dFrom, DateTime dTo)
        {
            dgLosses.DataSource = sqlStr;
            SqlConnection conn = new SqlConnection(Variables.npdConnectionString);
            SqlCommand sqlComand = new SqlCommand(sqlStr, conn);
            sqlComand.Parameters.Add("@index", SqlDbType.Int);
            sqlComand.Parameters["@index"].Value = zfinIndex;
            sqlComand.Parameters.Add("@dFrom", SqlDbType.DateTime);
            sqlComand.Parameters["@dFrom"].Value = dFrom;
            sqlComand.Parameters.Add("@dTo", SqlDbType.DateTime);
            sqlComand.Parameters["@dTo"].Value = dTo;
            SqlDataAdapter sqlDataAdap = new SqlDataAdapter(sqlComand);
            DataTable dtProd = new DataTable();
            sqlDataAdap.Fill(dtProd);
            dgLosses.DataSource = dtProd;
        }

        private void historyTypeChanged(object sender, EventArgs e)
        {
            if (dgProd.DataSource != null)
            {
                historyUpdate();
            }
            
        }

        private void historySummaryChanged(object sender, EventArgs e)
        {
            if (dgProd.DataSource != null)
            {
                historyUpdate();
            }
        }

        private void historyUpdate()
        {
            string prodHistStr;
            switch (cmbDataType.Text)
            {
                case "Produkcja":
                    switch (cmbStatType.Text)
                    {
                        case "Bez podsumowania":
                            prodHistStr = @"SELECT ord.sapId as [Zlecenie], od.plMoment as Czas, m.machineName as Maszyna, od.plAmount as [PC], od.plAmount* u.unitWeight as [KG], od.plAmount/u.pcPerBox  as [BOX], od.plAmount/u.pcPerPallet as [PAL]
                                FROM tbOperations o LEFT JOIN tbOperationData od ON o.operationId = od.operationId LEFT JOIN tbZfin z ON o.zfinId = z.zfinId LEFT JOIN tbMachine m ON od.plMach = m.machineId LEFT JOIN tbUom u ON z.zfinId = u.zfinId LEFT JOIN tbOrders ord ON ord.orderId=o.orderId
                                WHERE z.zfinIndex = @index ORDER BY od.plMoment DESC";
                            updateProdHistory(prodHistStr);
                            break;
                        case "Miesiąc":
                            prodHistStr = @"SELECT CASE WHEN MONTH(od.plMoment) < 10 THEN 
                                CAST(YEAR(od.plMoment) as varchar(4)) + '_0' + CAST(MONTH(od.plMoment) as varchar(1))
                                ELSE CAST(YEAR(od.plMoment) as varchar(4)) + '_' + CAST(MONTH(od.plMoment) as varchar(2)) END  As Okres,
                                SUM(od.plAmount * u.unitWeight) as [Ilość kg]
                                FROM tbOperations o LEFT JOIN tbOperationData od ON o.operationId = od.operationId LEFT JOIN tbZfin z ON z.zfinId=o.zfinId LEFT JOIN tbMachine m ON od.exMach = m.machineId LEFT JOIN tbUom u ON z.zfinId = u.zfinId
                                WHERE z.zfinIndex = @index
                                GROUP BY CASE WHEN MONTH(od.plMoment) < 10 THEN CAST(YEAR(od.plMoment) as varchar(4)) +'_0' + CAST(MONTH(od.plMoment) as varchar(1))
                                ELSE CAST(YEAR(od.plMoment) as varchar(4)) +'_' + CAST(MONTH(od.plMoment) as varchar(2)) END
                               HAVING SUM(od.plAmount * u.unitWeight) IS NOT NULL
                               ORDER BY Okres DESC";
                            updateProdHistory(prodHistStr);
                            break;
                        case "Rok":
                            prodHistStr = @"SELECT YEAR(od.plMoment)  As Okres, SUM(od.plAmount * u.unitWeight) as [Ilość kg]
                                FROM tbOperations o LEFT JOIN tbOperationData od ON o.operationId=od.operationId LEFT JOIN tbZfin z ON z.zfinId=o.zfinId LEFT JOIN tbMachine m ON od.plMach = m.machineId LEFT JOIN tbUom u ON o.zfinId = u.zfinId
                                WHERE z.zfinIndex = @index GROUP BY YEAR(od.plMoment)
                                HAVING SUM(od.plAmount * u.unitWeight) IS NOT NULL
                                ORDER BY Okres DESC";
                            updateProdHistory(prodHistStr);
                            break;
                        case "Tydzień":
                            prodHistStr = @"SELECT CASE WHEN DATEPART(ISO_WEEK,od.plMoment) < 10 THEN 
                                CAST(YEAR(od.plMoment) as varchar(4)) + '_0' + CAST(DATEPART(ISO_WEEK,od.plMoment) as varchar(1))
                                ELSE CAST(YEAR(od.plMoment) as varchar(4)) + '_' + CAST(DATEPART(ISO_WEEK,od.plMoment) as varchar(2)) END  As Okres,
                                SUM(od.plAmount * u.unitWeight) as [Ilość kg]
                                FROM tbOperations o LEFT JOIN tbOperationData od ON o.operationId = od.operationId LEFT JOIN tbZfin z ON z.zfinId = o.zfinId LEFT JOIN tbMachine m ON od.plMach = m.machineId LEFT JOIN tbUom u ON o.zfinId = u.zfinId 
                                WHERE z.zfinIndex = @index 
                                GROUP BY CASE WHEN DATEPART(ISO_WEEK, od.plMoment) < 10 THEN
                                CAST(YEAR(od.plMoment) as varchar(4)) + '_0' + CAST(DATEPART(ISO_WEEK, od.plMoment) as varchar(1))
                                ELSE CAST(YEAR(od.plMoment) as varchar(4)) +'_' + CAST(DATEPART(ISO_WEEK, od.plMoment) as varchar(2)) END
                                HAVING SUM(od.plAmount * u.unitWeight) IS NOT NULL
                                ORDER BY Okres DESC";
                            updateProdHistory(prodHistStr);
                            break;
                    }
                    break;
                case "Wysyłki":
                    switch (cmbStatType.Text)
                    {
                        case "Bez podsumowania":
                            prodHistStr = @"SELECT CONVERT(date,qr.qDate) as [Data],
                            qr.delNumber as [Delivery Note],
                            SUM(qd.batchSize*u.unitWeight) as [Ilość kg],
                            (SELECT TOP(1) sht.shipToString + ' ' + cd.companyName + ', ' + cd.companyCountry FROM tbDeliveryDetail dd LEFT JOIN tbShipTo sht ON sht.shipToId=dd.shipToId LEFT JOIN tbCompanyDetails cd ON cd.companyId=sht.companyId WHERE CHARINDEX(CONVERT(nvarchar,qr.delNumber),dd.deliveryNote)>0) as [Miejsce dostawy]
                            FROM tbQdocReconciliation qr LEFT JOIN tbQdocData qd ON  qr.qReconciliationId=qd.qReconciliationId LEFT JOIN tbBatch b ON b.batchId=qd.batchId LEFT JOIN tbZfin z ON z.zfinId=b.zfinId LEFT JOIN tbUom u ON u.zfinId=z.zfinId 
                            WHERE z.zfinIndex=@index and qr.qType='WHD_WZ'
                            GROUP BY qr.qDate, qr.delNumber
                            ORDER BY [Data] DESC;";
                            updateProdHistory(prodHistStr);
                            break;
                        case "Miesiąc":
                            prodHistStr = @"SELECT CASE WHEN MONTH(qr.qDate) < 10 THEN 
                            CAST(YEAR(qr.qDate) as varchar(4)) + '_0' + CAST(MONTH(qr.qDate) as varchar(1))
                            ELSE CAST(YEAR(qr.qDate) as varchar(4)) + '_' + CAST(MONTH(qr.qDate) as varchar(2)) END  As Okres,
                            SUM(qd.batchSize*u.unitWeight) as [Ilość kg]
                            FROM tbQdocReconciliation qr LEFT JOIN tbQdocData qd ON  qr.qReconciliationId=qd.qReconciliationId LEFT JOIN tbBatch b ON b.batchId=qd.batchId LEFT JOIN tbZfin z ON z.zfinId=b.zfinId LEFT JOIN tbUom u ON u.zfinId=z.zfinId 
                            WHERE z.zfinIndex=@index and qr.qType='WHD_WZ'
                            GROUP BY CASE WHEN MONTH(qr.qDate) < 10 THEN 
                            CAST(YEAR(qr.qDate) as varchar(4)) + '_0' + CAST(MONTH(qr.qDate) as varchar(1))
                            ELSE CAST(YEAR(qr.qDate) as varchar(4)) + '_' + CAST(MONTH(qr.qDate) as varchar(2)) END
                            ORDER BY [Okres] DESC";
                            updateProdHistory(prodHistStr);
                            break;
                        case "Tydzień":
                            prodHistStr = @"SELECT CASE WHEN DATEPART(ISO_WEEK,qr.qDate) < 10 THEN 
                            CAST(YEAR(qr.qDate) as varchar(4)) + '_0' + CAST(DATEPART(ISO_WEEK,qr.qDate) as varchar(1))
                            ELSE CAST(YEAR(qr.qDate) as varchar(4)) + '_' + CAST(DATEPART(ISO_WEEK,qr.qDate) as varchar(2)) END  As Okres,
                            SUM(qd.batchSize*u.unitWeight) as [Ilość kg]
                            FROM tbQdocReconciliation qr LEFT JOIN tbQdocData qd ON  qr.qReconciliationId=qd.qReconciliationId LEFT JOIN tbBatch b ON b.batchId=qd.batchId LEFT JOIN tbZfin z ON z.zfinId=b.zfinId LEFT JOIN tbUom u ON u.zfinId=z.zfinId 
                            WHERE z.zfinIndex=@index and qr.qType='WHD_WZ'
                            GROUP BY CASE WHEN DATEPART(ISO_WEEK,qr.qDate) < 10 THEN 
                            CAST(YEAR(qr.qDate) as varchar(4)) + '_0' + CAST(DATEPART(ISO_WEEK,qr.qDate) as varchar(1))
                            ELSE CAST(YEAR(qr.qDate) as varchar(4)) + '_' + CAST(DATEPART(ISO_WEEK,qr.qDate) as varchar(2)) END
                            ORDER BY [Okres] DESC";
                            updateProdHistory(prodHistStr);
                            break;
                        case "Rok":
                            prodHistStr = @"SELECT YEAR(qr.qDate)  As Okres,
                            SUM(qd.batchSize*u.unitWeight) as [Ilość kg]
                            FROM tbQdocReconciliation qr LEFT JOIN tbQdocData qd ON  qr.qReconciliationId=qd.qReconciliationId LEFT JOIN tbBatch b ON b.batchId=qd.batchId LEFT JOIN tbZfin z ON z.zfinId=b.zfinId LEFT JOIN tbUom u ON u.zfinId=z.zfinId 
                            WHERE z.zfinIndex=@index and qr.qType='WHD_WZ'
                            GROUP BY YEAR(qr.qDate)
                            ORDER BY [Okres] DESC";
                            updateProdHistory(prodHistStr);
                            break;
                    }
                    break;

                    case "Zapotrzebowanie":
                    switch (cmbStatType.Text)
                    {
                        case "Bez podsumowania":
                            prodHistStr = @"SELECT CONVERT(date,r.deliveryDate) as [Data], r.amount*u.unitWeight as [Ilość kg], cs.location as [Miejsce dostawy]
                            FROM tbZfin z LEFT JOIN tbReqs r ON z.zfinId=r.zfinId LEFT JOIN tbUom u ON u.zfinId = z.zfinId LEFT JOIN tbCustomerString cs ON cs.custStringId=r.target
                            WHERE z.zfinIndex = @index
                            ORDER BY [Data] DESC";
                            updateProdHistory(prodHistStr);
                            break;
                        case "Miesiąc":
                            prodHistStr = @"SELECT CASE WHEN MONTH(r.deliveryDate) < 10 THEN 
                            CAST(YEAR(r.deliveryDate) as varchar(4)) + '_0' + CAST(MONTH(r.deliveryDate) as varchar(1))
                            ELSE CAST(YEAR(r.deliveryDate) as varchar(4)) + '_' + CAST(MONTH(r.deliveryDate) as varchar(2)) END as [Okres], SUM(r.amount*u.unitWeight) as [Ilość kg]
                            FROM tbZfin z LEFT JOIN tbReqs r ON z.zfinId=r.zfinId LEFT JOIN tbUom u ON u.zfinId = z.zfinId LEFT JOIN tbCustomerString cs ON cs.custStringId=r.target
                            WHERE z.zfinIndex = @index
                            GROUP BY CASE WHEN MONTH(r.deliveryDate) < 10 THEN 
                            CAST(YEAR(r.deliveryDate) as varchar(4)) + '_0' + CAST(MONTH(r.deliveryDate) as varchar(1))
                            ELSE CAST(YEAR(r.deliveryDate) as varchar(4)) + '_' + CAST(MONTH(r.deliveryDate) as varchar(2)) END
                            ORDER BY [Okres] DESC";
                            updateProdHistory(prodHistStr);
                            break;
                        case "Tydzień":
                            prodHistStr = @"SELECT CASE WHEN DATEPART(ISO_WEEK,r.deliveryDate) < 10 THEN 
                            CAST(YEAR(r.deliveryDate) as varchar(4)) + '_0' + CAST(DATEPART(ISO_WEEK,r.deliveryDate) as varchar(1))
                            ELSE CAST(YEAR(r.deliveryDate) as varchar(4)) + '_' + CAST(DATEPART(ISO_WEEK,r.deliveryDate) as varchar(2)) END as [Okres], SUM(r.amount*u.unitWeight) as [Ilość kg]
                            FROM tbZfin z LEFT JOIN tbReqs r ON z.zfinId=r.zfinId LEFT JOIN tbUom u ON u.zfinId = z.zfinId LEFT JOIN tbCustomerString cs ON cs.custStringId=r.target
                            WHERE z.zfinIndex = @index
                            GROUP BY CASE WHEN DATEPART(ISO_WEEK,r.deliveryDate) < 10 THEN 
                            CAST(YEAR(r.deliveryDate) as varchar(4)) + '_0' + CAST(DATEPART(ISO_WEEK,r.deliveryDate) as varchar(1))
                            ELSE CAST(YEAR(r.deliveryDate) as varchar(4)) + '_' + CAST(DATEPART(ISO_WEEK,r.deliveryDate) as varchar(2)) END
                            ORDER BY [Okres] DESC";
                            updateProdHistory(prodHistStr);
                            break;
                        case "Rok":
                            prodHistStr = @"SELECT YEAR(r.deliveryDate) as [Okres], SUM(r.amount*u.unitWeight) as [Ilość kg]
                            FROM tbZfin z LEFT JOIN tbReqs r ON z.zfinId=r.zfinId LEFT JOIN tbUom u ON u.zfinId = z.zfinId LEFT JOIN tbCustomerString cs ON cs.custStringId=r.target
                            WHERE z.zfinIndex = @index
                            GROUP BY YEAR(r.deliveryDate)
                            ORDER BY [Okres] DESC";
                            updateProdHistory(prodHistStr);
                            break;
                    }
                    break;

                case "Planowane wysyłki":
                    switch (cmbStatType.Text)
                    {
                        case "Bez podsumowania":
                            prodHistStr = @"SELECT CONVERT(date,sh.PlannedDate) as [Plan], po.LPlant  as [L-Plant],
                            (SELECT TOP(1) CONVERT(date,t.transportDate) FROM tbDeliveryDetail dd LEFT JOIN tbCmr c ON c.detailId=dd.cmrDetailId LEFT JOIN tbTransport t ON t.transportId=c.transportId WHERE CHARINDEX(CONVERT(nvarchar,LEFT(sh.DeliveryNotes,10)),dd.deliveryNote)>0) as [Wysłano],
                            (SELECT TOP(1) sht.shipToString + ' ' + cd.companyName + ', ' + cd.companyCountry FROM tbDeliveryDetail dd LEFT JOIN tbShipTo sht ON sht.shipToId=dd.shipToId LEFT JOIN tbCompanyDetails cd ON cd.companyId=sht.companyId WHERE CHARINDEX(CONVERT(nvarchar,LEFT(sh.DeliveryNotes,10)),dd.deliveryNote)>0) as [Miejsce dostawy],
                            (SELECT TOP(1) CASE WHEN t.transportStatus = 2 THEN 'Wysłano' ELSE 'Oczekuje' END FROM tbDeliveryDetail dd LEFT JOIN tbCmr c ON c.detailId=dd.cmrDetailId LEFT JOIN tbTransport t ON t.transportId=c.transportId WHERE CHARINDEX(CONVERT(nvarchar,LEFT(sh.DeliveryNotes,10)),dd.deliveryNote)>0) as Status,
                            sh.DeliveryNotes as [Delivery Note], poi.Amount as [PC],
                            poi.Amount*u.unitWeight as [KG],
                            poi.Amount/u.pcPerPallet as [PAL]
                            FROM tbPlannedShipments sh
                            LEFT JOIN tbPo po ON po.shipmentId=sh.PlannedShipmentId
                            LEFT JOIN tbPoItem poi ON poi.PoId=po.PoId
                            LEFT JOIN tbZfin z ON z.zfinId=poi.ProductId
                            LEFT JOIN tbUom u ON u.zfinId=z.zfinId
                            WHERE z.zfinIndex=@index
                            ORDER BY PlannedDate";
                            updateProdHistory(prodHistStr);
                            break;
                    }
                    break;

                case "Podział produkcji":
                    switch (cmbStatType.Text)
                    {
                        case "Bez podsumowania":
                            prodHistStr = @"
                                        DECLARE @weekFrom int = DATEPART(ISO_WEEK,@startDate),
		                                @yearFrom int = YEAR(@startDate)
                                        SELECT CONVERT(nvarchar, pp.Year) + '-' + CONVERT(nvarchar, pp.Week) as [Tydzień], 
                                        pp.Lplant as [Alokacja],
                                        (SELECT TOP(1) ps.Amount FROM tbPlannedStock ps WHERE ps.Lplant=pp.Lplant AND ps.ProductId=pp.ProductId AND CONVERT(nvarchar,YEAR(ps.PlannedDate)) + '-' + CONVERT(nvarchar,DATEPART(ISO_WEEK,ps.PlannedDate)) = CONVERT(nvarchar, pp.Year) + '-' + CONVERT(nvarchar, pp.Week)) as [Zapas],
                                        pp.Amount as [Plan],
                                        (
	                                        SELECT SUM(t.PC) as [Wysłano] FROM
	                                        (SELECT CONVERT(nvarchar,YEAR(CONVERT(date,sh.PlannedDate)))+'-'+ CONVERT(nvarchar,DATEPART(iso_week,CONVERT(date,sh.PlannedDate))) as [Plan], 
	                                        (SELECT TOP(1) sht.shipToString + ' ' + cd.companyName + ', ' + cd.companyCountry FROM tbDeliveryDetail dd LEFT JOIN tbShipTo sht ON sht.shipToId=dd.shipToId LEFT JOIN tbCompanyDetails cd ON cd.companyId=sht.companyId WHERE CHARINDEX(CONVERT(nvarchar,LEFT(sh.DeliveryNotes,10)),dd.deliveryNote)>0) as [Miejsce dostawy],
	                                        (SELECT TOP(1) CASE WHEN t.transportStatus = 2 THEN 'Wysłano' ELSE 'Oczekuje' END FROM tbDeliveryDetail dd LEFT JOIN tbCmr c ON c.detailId=dd.cmrDetailId LEFT JOIN tbTransport t ON t.transportId=c.transportId WHERE CHARINDEX(CONVERT(nvarchar,LEFT(sh.DeliveryNotes,10)),dd.deliveryNote)>0) as Status,
	                                        poi.Amount as [PC]
	                                        FROM tbPlannedShipments sh
	                                        LEFT JOIN tbPo po ON po.shipmentId=sh.PlannedShipmentId
	                                        LEFT JOIN tbPoItem poi ON poi.PoId=po.PoId
	                                        LEFT JOIN tbZfin z ON z.zfinId=poi.ProductId
	                                        LEFT JOIN tbUom u ON u.zfinId=z.zfinId
	                                        WHERE z.zfinIndex=@index AND pp.Year>=@yearFrom AND pp.Week>=@weekFrom) t
	                                        WHERE t.Status='Wysłano' AND CHARINDEX(pp.Lplant,t.[Miejsce dostawy])>0 AND t.[Plan]=CONVERT(nvarchar, pp.Year) + '-' + CONVERT(nvarchar, pp.Week)
                                        ) as [Wysłano]
                                        FROM tbPlannedProduction pp
                                        LEFT JOIN tbZfin z ON pp.ProductId=z.zfinId
                                        LEFT JOIN tbUom u ON u.zfinId=z.zfinId
                                        WHERE z.zfinIndex=@index
                                        ORDER BY pp.Year, pp.Week";
                            updateProdHistory(prodHistStr);
                            break;
                    }
                    break;

                case "Zapasy":
                    switch (cmbStatType.Text)
                    {
                        case "Bez podsumowania":
                            prodHistStr = @"SELECT CONVERT(date,ps.PlannedDate) as [Data],
                            ps.Lplant,ps.Amount as [PC], 
                            ps.Amount*u.unitWeight as [KG], 
                            ps.Amount/u.pcPerPallet as [PAL]
                            FROM tbPlannedStock ps
                            LEFT JOIN tbZfin z ON ps.ProductId=z.zfinId
                            LEFT JOIN tbUom u ON u.zfinId=z.zfinId
                            WHERE z.zfinIndex=@index
                            ORDER BY ps.PlannedDate";
                            updateProdHistory(prodHistStr);
                            break;
                    }
                    break;

                case "Przepływ":
                    switch (cmbStatType.Text)
                    {
                        case "Bez podsumowania":
                            prodHistStr = @"SELECT CONVERT(date,ps.PlannedDate) as [Data],
                            'Zapas' as [Typ],
                            SUM(ps.Amount) as [PC], 
                            SUM(ps.Amount*u.unitWeight) as [KG], 
                            SUM(ps.Amount/u.pcPerPallet) as [PAL],
                            '' as [Delivery Note],
                            '' as [Dostawa],
                            '' as [Status]
                            FROM tbPlannedStock ps
                            LEFT JOIN tbZfin z ON ps.ProductId=z.zfinId
                            LEFT JOIN tbUom u ON u.zfinId=z.zfinId
                            WHERE z.zfinIndex=@index
                            GROUP BY CONVERT(date,ps.PlannedDate)
                            UNION ALL
                            SELECT od.plMoment as [Data],
                            'Produkcja' as [Typ],
                            SUM(od.plAmount) as PC,
                            SUM(ROUND(od.plAmount*u.unitWeight,1)) as KG,
                            SUM(ROUND(od.plAmount/u.pcPerPallet,1)) AS PAL,
                            '' as [Delivery Note],
                            '' as [Dostawa],
                            '' as [Status]
                            FROM tbOperations o LEFT JOIN tbOperationData od ON od.operationId=o.operationId LEFT JOIN tbZfin z ON z.zfinId=o.zfinId LEFT JOIN tbMachine m ON m.machineId=od.plMach LEFT JOIN tbZfinProperties zp ON zp.zfinId=z.zfinId LEFT JOIN tbUom u ON u.zfinId=z.zfinId LEFT JOIN tbCustomerString cs ON cs.custStringId = z.custString LEFT JOIN tbPallets p ON p.palletId=u.palletType
                            WHERE od.plMoment >= @startDate AND o.type = 'p' AND z.zfinIndex=@index
                            GROUP BY od.plMoment
                            UNION ALL
                            SELECT CONVERT(date,sh.PlannedDate) as [Data], 
                            'Wysyłka' as [Typ],
                            poi.Amount * -1 as [PC],
                            poi.Amount*u.unitWeight *-1 as [KG],
                            poi.Amount/u.pcPerPallet *-1 as [PAL],
                            sh.DeliveryNotes as [Delivery Note],
                            po.Lplant + CASE WHEN (SELECT TOP(1) cd.companyCountry FROM tbDeliveryDetail dd LEFT JOIN tbShipTo sht ON sht.shipToId=dd.shipToId LEFT JOIN tbCompanyDetails cd ON cd.companyId=sht.companyId WHERE CHARINDEX(CONVERT(nvarchar,LEFT(sh.DeliveryNotes,10)),dd.deliveryNote)>0) IS NOT NULL THEN ', ' + (SELECT TOP(1) cd.companyCountry FROM tbDeliveryDetail dd LEFT JOIN tbShipTo sht ON sht.shipToId=dd.shipToId LEFT JOIN tbCompanyDetails cd ON cd.companyId=sht.companyId WHERE CHARINDEX(CONVERT(nvarchar,LEFT(sh.DeliveryNotes,10)),dd.deliveryNote)>0) ELSE '' END as [Dostawa],
                            (SELECT TOP(1) CASE WHEN t.transportStatus = 2 THEN 'Wysłano' ELSE 'Oczekuje' END FROM tbDeliveryDetail dd LEFT JOIN tbCmr c ON c.detailId=dd.cmrDetailId LEFT JOIN tbTransport t ON t.transportId=c.transportId WHERE CHARINDEX(CONVERT(nvarchar,LEFT(sh.DeliveryNotes,10)),dd.deliveryNote)>0) as [Status]
                            FROM tbPlannedShipments sh
                            LEFT JOIN tbPo po ON po.shipmentId=sh.PlannedShipmentId
                            LEFT JOIN tbPoItem poi ON poi.PoId=po.PoId
                            LEFT JOIN tbZfin z ON z.zfinId=poi.ProductId
                            LEFT JOIN tbUom u ON u.zfinId=z.zfinId
                            WHERE z.zfinIndex=@index
                            ORDER BY [Data],[Typ] DESC";
                            updateProdHistory(prodHistStr);
                            break;
                    }
                    break;
            }
        }

        private void bomUpdate()
        {
            string sql = @"SELECT comp.zfinIndex as [Index], comp.zfinName as [Nazwa], bom.amount as [Ilość], bom.unit as [Jednostka], comp.zfinType as [Typ], mt.materialTypeName as [Kategoria] 
                FROM tbZfin zfin LEFT JOIN tbBom bom ON bom.zfinId=zfin.zfinId LEFT JOIN tbZfin comp ON comp.zfinId=bom.materialId LEFT JOIN tbMaterialType mt ON mt.materialTypeId=comp.materialType 
                WHERE zfin.zfinId=@index AND bom.bomRecId=(SELECT TOP(1) br.bomRecId FROM tbBomReconciliation br LEFT JOIN tbBom bm ON bm.bomRecId=br.bomRecId LEFT JOIN tbZfin z ON z.zfinId=bm.zfinId WHERE z.zfinId=@index ORDER BY br.dateAdded DESC)";
            dgBom.DataSource = sql;
            SqlConnection conn = new SqlConnection(Variables.npdConnectionString);
            SqlCommand sqlComand = new SqlCommand(sql, conn);
            sqlComand.Parameters.Add("@index", SqlDbType.Int);
            sqlComand.Parameters["@index"].Value = thisProduct.ID;
            SqlDataAdapter sqlDataAdap = new SqlDataAdapter(sqlComand);
            DataTable dtBom = new DataTable();
            sqlDataAdap.Fill(dtBom);
            dgBom.DataSource = dtBom;

        }

        private void SetLosses()
        {
            lossesSourceLoad();
            List<string> rngOptions = new List<string>();
            rngOptions.Add("Bieżący miesiąc");
            rngOptions.Add("Poprzedni miesiąc");
            rngOptions.Add("Ostatni kwartał");
            rngOptions.Add("Ostatnie półrocze");
            rngOptions.Add("Ostatni rok");
            cmbLossesRange.DataSource = rngOptions;
            List<string> lossSummary = new List<string>();
            lossSummary.Add("Bez podsumowania");
            lossSummary.Add("Tydzień");
            lossSummary.Add("Miesiąc");
            lossSummary.Add("Rok");
            cmbLossesSummary.DataSource = lossSummary;
        }

        private void lossesSourceLoad()
        {
            string sql = "SELECT scrapReconciliationId, dateAdded FROM tbScrapReconciliation ORDER BY dateAdded DESC";
            using (SqlConnection conn = new SqlConnection(Variables.npdConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand sqlComand = new SqlCommand(sql, conn);
                    DataTable dt = new DataTable();
                    dt.Load(sqlComand.ExecuteReader());
                    cmbLossDataSource.DataSource = dt;
                    cmbLossDataSource.ValueMember = dt.Columns[0].ColumnName;
                    cmbLossDataSource.DisplayMember = dt.Columns[1].ColumnName;
                }catch(Exception ex)
                {

                }
                
            }
        }

        private void lossesBomLoad()
        {
            string sql = @"SELECT cs.scrap 
                        FROM tbComponentScrap cs
                        WHERE cs.zfinId = @zfinId AND cs.scrapReconciliationId = @reconId";
            using (SqlConnection conn = new SqlConnection(Variables.npdConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand sqlComand = new SqlCommand(sql, conn);
                    sqlComand.Parameters.Add("@zfinId", SqlDbType.Int);
                    sqlComand.Parameters["@zfinId"].Value = thisProduct.ID;
                    sqlComand.Parameters.Add("@reconId", SqlDbType.Int);
                    sqlComand.Parameters["@reconId"].Value = cmbLossDataSource.SelectedValue;
                    using (SqlDataReader reader = sqlComand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            txtBomScrap.Text = reader["scrap"].ToString() ;
                        }
                    }
                }
                catch (Exception ex)
                {

                }

            }
        }

        private void whereUsedUpdate()
        {
            string sql = @"SELECT z.zfinIndex as [Index], z.zfinName as [Nazwa], bom.amount as [Ilość], bom.unit as [Jednostka], MAX(br.dateAdded) as [Bom aktualizowany dnia]
                FROM tbBom bom LEFT JOIN tbZfin z ON z.zfinId=bom.zfinId LEFT JOIN tbBomReconciliation br ON br.bomRecId=bom.bomRecId LEFT JOIN tbZfin mat ON mat.zfinId = bom.materialId
                WHERE mat.zfinIndex=@index
                GROUP BY z.zfinIndex, z.zfinName, bom.amount, bom.unit";

            dgWhereUsed.DataSource = sql;
            SqlConnection conn = new SqlConnection(Variables.npdConnectionString);
            SqlCommand sqlComand = new SqlCommand(sql, conn);
            sqlComand.Parameters.Add("@index", SqlDbType.Int);
            sqlComand.Parameters["@index"].Value = zfinIndex;
            SqlDataAdapter sqlDataAdap = new SqlDataAdapter(sqlComand);
            DataTable dtWhereUsed = new DataTable();
            sqlDataAdap.Fill(dtWhereUsed);
            dgWhereUsed.DataSource = dtWhereUsed;
        }

        private void updateCalc(object sender, EventArgs e)
        {
            recalc();
        }

        private void recalc()
        {
            //check if the input is numeric type
            if(txtCalcIn.Text.ToLower().Contains("pc") || txtCalcIn.Text.ToLower().Contains("kg") || txtCalcIn.Text.ToLower().Contains("box") || txtCalcIn.Text.ToLower().Contains("pal"))
            {
                string inUnit;
                double inCalc;
                string inCalcStr;
                inCalcStr = "";
                if (txtCalcIn.Text.ToLower().Contains("pc"))
                {
                    inUnit = "pc";
                }else if (txtCalcIn.Text.ToLower().Contains("kg"))
                {
                    inUnit = "kg";
                }else if (txtCalcIn.Text.ToLower().Contains("box"))
                {
                    inUnit = "box";
                }else
                {
                    inUnit = "pal";
                }
                foreach(char s in txtCalcIn.Text)
                {
                    if(char.IsDigit(s) || s.Equals(Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)))
                    {
                        inCalcStr += s;
                    }
                }

                if (double.TryParse(inCalcStr, out inCalc))
                {
                    if (inCalc == 0)
                    {
                        txtCalcPc.Text = "0";
                        txtCalKg.Text = "0";
                        txtCalPal.Text = "0";
                        txtCalBox.Text = "0";
                    }
                    else
                    {

                        txtCalcPc.Text = thisProduct.convert(inCalc, inUnit, "pc").ToString();
                        txtCalBox.Text = thisProduct.convert(inCalc, inUnit, "box").ToString();
                        txtCalKg.Text = thisProduct.convert(inCalc, inUnit, "kg").ToString();
                        txtCalPal.Text = thisProduct.convert(inCalc, inUnit, "pal").ToString();
                    }
                }
                else
                {
                    txtCalcPc.Text = "";
                    txtCalKg.Text = "";
                    txtCalPal.Text = "";
                    txtCalBox.Text = "";
                }
            }else
            {
                txtCalcPc.Text = "";
                txtCalKg.Text = "";
                txtCalPal.Text = "";
                txtCalBox.Text = "";
            }  
        }

        private void unitChanged(object sender, EventArgs e)
        {
            recalc();
        }

        private void te_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabChanged(object sender, EventArgs e)
        {
            if (tabAll.SelectedTab.Text == "Gdzie używany" && dgWhereUsed.DataSource == null)
            {
                whereUsedUpdate();
            }
            if (tabAll.SelectedTab.Text == "BOM" && dgBom.DataSource == null)
            {
                bomUpdate();
            }
            if (tabAll.SelectedTab.Text == "Historia" && dgProd.DataSource == null)
            {
                if (thisProduct.Type.ToLower() == "zfin")
                {
                    var historyType = new[]
                    {
                        "Produkcja","Wysyłki","Zapotrzebowanie","Planowane wysyłki","Podział produkcji","Zapasy","Przepływ"//,"Wszystko"
                    };
                    cmbDataType.DataSource = historyType;
                }
                else
                {
                    var historyType = new[]
                    {
                        "Produkcja"
                    };
                    cmbDataType.DataSource = historyType;
                }

                var prodHistorySum = new[]
                {
                    "Bez podsumowania", "Tydzień", "Miesiąc", "Rok"
                };
                cmbStatType.DataSource = prodHistorySum;
                historyUpdate();
            }
            if (tabAll.SelectedTab.Text == "Straty" && cmbLossDataSource.DataSource == null)
            {
                SetLosses();
            }
            if(tabAll.SelectedTab.Text =="Statystyki" && dgvStats.DataSource == null)
            {
                SetStats();
                UpdateStats();
            }
        }

        private void UpdateStats()
        {
            string sql = "";
            string type = "";
            DateTime? dFrom = null;
            DateTime? dTo = null;
            switch (cmbStatsType.SelectedText)
            {
                case "Prażenie":
                    type = "r";
                    break;
                case "Mielenie":
                    type = "g";
                    break;
                case "Pakowanie":
                    type = "p";
                    break;
                default:
                    type = "";
                    break;
            }
            sql = @"SELECT m.machineName, (m.Amount/m.Total)*100 AS ProductionStats FROM 
                    (SELECT m.machineName, SUM(od.plAmount) AS Amount,
	                    (
	                    SELECT SUM(od.plAmount) 
	                    FROM tbOperationData od
	                    LEFT JOIN tbOperations op ON op.operationId=od.operationId
	                    LEFT JOIN tbZfin z ON z.zfinId=op.zfinId
	                    WHERE od.plMoment > @dateFrom  AND od.plMoment < @dateTo AND z.zfinIndex=34005471 AND type=@statType
	                    ) as Total
                    FROM
                    tbOperationData od
                    LEFT JOIN tbOperations op ON op.operationId=od.operationId
                    LEFT JOIN tbZfin z ON z.zfinId=op.zfinId
                    LEFT JOIN tbMachine m ON m.machineId=od.plMach
                    WHERE od.plMoment > @dateFrom  AND od.plMoment < @dateTo AND z.zfinIndex=34005471 AND type=@statType
                    GROUP BY m.machineName, type)m
                    ORDER BY m.Amount DESC";

        }

        private void SetStats()
        {
            DateTime PrevDate = DateTime.Now.AddDays(-180);
            txtStatsDateFrom.Value = PrevDate;
            txtStatsDateTo.Value = DateTime.Now;
            List<string> StatTypes = new List<string> { "Prażenie", "Mielenie", "Pakowanie", "Wysyłka" };
            cmbStatsType.DataSource = StatTypes;
        }

        private void cmbLossDataSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            lossesBomLoad();
        }

        private void DiggFromBom(object sender, EventArgs e)
        {
            int i = 0;
            try
            {
                i = dgBom.CurrentCell.RowIndex;
            }
            catch
            {
                MessageBox.Show("Nic nie zostało zaznaczone.", "Brak zaznaczenia");
            }
            finally
            {
                try
                {
                    int comp = (int)dgBom.Rows[i].Cells[0].Value;
                    frmZfinOverview CompForm = new frmZfinOverview(comp,this);
                    CompForm.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

        }

        private void DigFromWhereUsed(object sender, EventArgs e)
        {
            int i = 0;
            try
            {
                i = dgWhereUsed.CurrentCell.RowIndex;
            }
            catch
            {
                MessageBox.Show("Nic nie zostało zaznaczone.", "Brak zaznaczenia");
            }
            finally
            {
                try
                {
                    int zfinIndex = (int)dgWhereUsed.Rows[i].Cells[0].Value;
                    frmZfinOverview ZfinForm = new frmZfinOverview(zfinIndex, this);
                    ZfinForm.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void UpdateLosses()
        {
            string sql = "";
            DateTime? dFrom = null;
            DateTime? dTo = null;
            switch (cmbLossesSummary.Text){
                case "Bez podsumowania":
                    sql = @"SELECT *
                            FROM(
                                SELECT o.orderId as [Id], o.sapId as [Zlecenie], z.zfinIndex as [Nazwa], comp.basicUom as [UoM], pc.actualScrap * -1 as [Rzeczywista strata w %], pc.targetConsumption - pc.actualConsumption as [Strata / Zysk (vs BOM)],
                                (pc.targetConsumption - pc.actualConsumption) * (SELECT c.cost / c.CostLotSize
                                FROM tbCosting c
                                WHERE c.reconciliationId = (SELECT TOP(1) cr.CostingReconciliationId FROM tbCostingReconciliation cr ORDER BY cr.dateAdded DESC) AND c.zfinId = comp.zfinId) as [Strata / zysk w € (vs BOM)],
	                            (SELECT TOP(1) CONVERT(nvarchar(4), YEAR(od.plMoment)) + '_' + CASE WHEN(CONVERT(nvarchar(2),MONTH(od.plMoment))<10) THEN '0' + CONVERT(nvarchar(2),MONTH(od.plMoment)) ELSE CONVERT(nvarchar(2),MONTH(od.plMoment)) END as p
                                FROM tbOperations op LEFT JOIN(SELECT * FROM tbOperationData WHERE plMoment >= @dFrom AND plMoment <= @dTo) od ON od.operationId = op.operationId
                                WHERE op.orderId = o.orderId
                                ORDER BY p DESC) as [Miesiąc]
                                FROM tbProductionConsumption pc LEFT JOIN tbZfin comp ON comp.zfinId = pc.componentId LEFT JOIN tbOrders o ON o.orderId = pc.orderId
                                LEFT JOIN tbZfin z ON z.zfinId = o.zfinId
                                WHERE comp.zfinIndex = @index AND pc.actualConsumption > 0
                            ) a
                            WHERE a.[Miesiąc] IS NOT NULL
                            ORDER BY a.[Miesiąc]";
                    break;
            }
            if(cmbLossesRange.Text=="Bieżący miesiąc")
            {
                dTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                dFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }else if(cmbLossesRange.Text =="Poprzedni miesiąc")
            {
                DateTime p = DateTime.Today.AddMonths(-1);
                dFrom = new DateTime(p.Year, p.Month, 1);
                dTo = new DateTime(p.Year, p.Month, DateTime.DaysInMonth(p.Year, p.Month));
            }else if(cmbLossesRange.Text == "Ostatni kwartał")
            {
                DateTime p = DateTime.Today.AddMonths(-2);
                dFrom = new DateTime(p.Year, p.Month, 1);
                dTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            }
            else if (cmbLossesRange.Text == "Ostatnie półrocze")
            {
                DateTime p = DateTime.Today.AddMonths(-5);
                dFrom = new DateTime(p.Year, p.Month, 1);
                dTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            }
            else if (cmbLossesRange.Text == "Ostatni rok")
            {
                DateTime p = DateTime.Today.AddMonths(-11);
                dFrom = new DateTime(p.Year, p.Month, 1);
                dTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            }


            if (dFrom!=null && dTo != null)
            {
                updateLosses(sql, (DateTime)dFrom, (DateTime)dTo);
            }
        }

        private void lossesRangeChanged(object sender, EventArgs e)
        {
            if(cmbLossesRange.DataSource!=null && cmbLossesSummary.DataSource != null)
            {
                UpdateLosses();
            }
        }

        private void lossesSummaryChanged(object sender, EventArgs e)
        {
            if (cmbLossesRange.DataSource != null && cmbLossesSummary.DataSource != null)
            {
                UpdateLosses();
            }
        }

        private void digg(object sender, DataGridViewCellEventArgs e)
        {
            string colName = ((DataGridView)sender).Columns[e.ColumnIndex].Name;

            switch (colName)
            {
                case "Zlecenie":
                    string oNumber = ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    frmOrder FrmOrder = new frmOrder(Convert.ToInt32(oNumber),this);
                    FrmOrder.Show();
                    break;
            }
        }

        private void diggHistory(object sender, DataGridViewCellEventArgs e)
        {
            string colName = ((DataGridView)sender).Columns[e.ColumnIndex].Name;

            switch (colName)
            {
                case "Zlecenie":
                    string oNumber = ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    frmOrder FrmOrder = new frmOrder(Convert.ToInt32(oNumber), this);
                    FrmOrder.Show();
                    break;
            }
        }

        private void btnUpdateStats_Click(object sender, EventArgs e)
        {
            UpdateStats();
        }
    }


}
