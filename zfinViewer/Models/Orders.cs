using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zfinViewer.Models
{
    public class Orders
    {
        public List<Order> Items { get; set; }
        SqlConnection conn = new SqlConnection(Variables.npdConnectionString);
        public DateTime StartDate;
        public DateTime FinishDate;
        public string MissingData = "Nie znaleziono BOMu lub UoM dla poniższych produktów:" + Environment.NewLine;
        public bool IsMissing = false;

        public Orders()
        {
            Items = new List<Order>();
        }

        public void Reload(DateTime dFrom, DateTime dTo, int materiaTypeId)
        {
            Items.Clear();

            StartDate = dFrom;
            FinishDate = dTo;

            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            
            string sql = @"SELECT DISTINCT a.[Order], a.executedSap
                        FROM(
                        SELECT comp.zfinIndex as [ZPKG], comp.zfinName as [Nazwa], comp.basicUom as [UoM], o.sapId as [Order], pc.actualScrap as [Rzeczywista strata w %], pc.targetScrap as [Oczekiwana strata w %], pc.targetConsumption - pc.actualConsumption as [Strata / Zysk(vs BOM)], pc.targetConsumption, pc.actualConsumption, o.executedSap,
                        (SELECT TOP(1) CONVERT(nvarchar(4), YEAR(od.plMoment)) + '_' + CASE WHEN(CONVERT(nvarchar(2), MONTH(od.plMoment)) < 10) THEN '0' + CONVERT(nvarchar(2), MONTH(od.plMoment)) ELSE CONVERT(nvarchar(2), MONTH(od.plMoment)) END as p
                        FROM tbOperations op LEFT JOIN(SELECT * FROM tbOperationData WHERE plMoment >= @dFrom AND plMoment <= @dTo) od ON od.operationId = op.operationId
                        WHERE op.orderId = o.orderId
                        ORDER BY p DESC) as [Miesiąc]
                        FROM tbProductionConsumption pc LEFT JOIN tbZfin comp ON comp.zfinId = pc.componentId LEFT JOIN tbOrders o ON o.orderId = pc.orderId
                        LEFT JOIN tbZfin z ON z.zfinId = o.zfinId
                        WHERE pc.actualConsumption > 0 AND comp.materialType = @materialType) a
                        WHERE a.[Miesiąc] IS NOT NULL
                        ORDER BY a.[Order]";
            SqlCommand sqlComand;
            sqlComand = new SqlCommand(sql, conn);
            sqlComand.Parameters.Add("@dFrom", SqlDbType.DateTime);
            sqlComand.Parameters["@dFrom"].Value = dFrom;
            sqlComand.Parameters.Add("@dTo", SqlDbType.DateTime);
            sqlComand.Parameters["@dTo"].Value = dTo;
            sqlComand.Parameters.Add("@materialType", SqlDbType.Int);
            sqlComand.Parameters["@materialType"].Value = materiaTypeId;
            using (SqlDataReader reader = sqlComand.ExecuteReader())
            {
                while (reader.Read())
                {
                    Order Order = new Order(Int32.Parse(reader[0].ToString()), float.Parse(reader[1].ToString()));
                    Items.Add(Order);
                }
            }

            GetProductUsage(materiaTypeId);
        }

        public void GetProductUsage(int materialTypeId)
        {
            string str = "";
            foreach(Order o in Items)
            {
                str += o.Number + ",";
            }
            
            
            str = str.Length > 0 ? str.Substring(0, str.Length - 1) : str;

            string sql = @"SELECT o.sapId, comp.zfinIndex as component, comp.zfinName as comp_name, comp.basicUom, pc.actualConsumption,pc.actualScrap,pc.targetScrap, z.zfinIndex, z.zfinName, o.executedSap, u.pcPerBox, u.pcPerPallet
                        FROM tbProductionConsumption pc LEFT JOIN tbZfin comp ON comp.zfinId=pc.componentId LEFT JOIN tbOrders o ON o.orderId=pc.orderId
                        LEFT JOIN tbZfin z ON z.zfinId=o.zfinId LEFT JOIN tbUom u ON u.zfinId=z.zfinId
                        WHERE pc.actualConsumption IS NOT NULL AND pc.actualConsumption > 0 AND o.sapId IN (" + str+ ") AND comp.materialType=" + materialTypeId;

            if (str.Length > 0)
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                SqlCommand command = new SqlCommand(sql, conn);

                try
                {
                    using (SqlDataReader nReader = command.ExecuteReader())
                    {
                        while (nReader.Read())
                        {
                            try
                            {
                                ProductionUsage Pu = new ProductionUsage
                                {
                                    Component = new Product
                                    {
                                        Index = Int32.Parse(nReader[1].ToString()),
                                        Name = nReader[2].ToString(),
                                        UoM = nReader[3].ToString(),
                                        CategoryId = materialTypeId,
                                    },
                                    Product = new Product
                                    {
                                        Index = Int32.Parse(nReader[7].ToString()),
                                        Name = nReader[8].ToString(),
                                        boxCount = int.Parse(nReader[10].ToString()),
                                        palletCount = int.Parse(nReader[11].ToString())
                                    },
                                    Order = new Order(Int32.Parse(nReader[0].ToString()), float.Parse(nReader[9].ToString())),
                                    ActualConsumption = float.Parse(nReader[4].ToString(), System.Globalization.NumberStyles.Number),
                                    ActualScrap = float.Parse(nReader[5].ToString(), System.Globalization.NumberStyles.Number),
                                    TargetScrap = float.Parse(nReader[6].ToString(), System.Globalization.NumberStyles.Number)
                                };
                                Items.Where(i => i.Number == Int32.Parse(nReader[0].ToString())).FirstOrDefault().ProductUsages.Add(Pu);
                            }
                            catch (Exception ex)
                            {

                            }

                        }
                    }
                }catch(Exception ex)
                {

                }
                
            }
        }

        public DataTable GetDataTable(MassBalanceType reportType)
        {
            DataTable dt = new DataTable("MassBalance");
            DataColumn nCol;

            if (reportType == MassBalanceType.byOrders)
            {
                nCol = new DataColumn("Order");
                nCol.DataType = Type.GetType("System.Int32");
                nCol.Caption = "Zlecenie";
                dt.Columns.Add(nCol);
                nCol = new DataColumn("ZPKG");
                nCol.DataType = Type.GetType("System.String");
                nCol.Caption = "Komponent";
                dt.Columns.Add(nCol);
                nCol = new DataColumn("ZPKGName");
                nCol.DataType = Type.GetType("System.String");
                nCol.Caption = "Nazwa komponentu";
                dt.Columns.Add(nCol);
                nCol = new DataColumn("ZFIN");
                nCol.DataType = Type.GetType("System.String");
                nCol.Caption = "Produkt";
                dt.Columns.Add(nCol);
                nCol = new DataColumn("ZFINName");
                nCol.DataType = Type.GetType("System.String");
                nCol.Caption = "Nazwa produktu";
                dt.Columns.Add(nCol);
            }
            else
            {
                nCol = new DataColumn("ZPKG");
                nCol.DataType = Type.GetType("System.Int32");
                nCol.Caption = "Komponent";
                dt.Columns.Add(nCol);
                nCol = new DataColumn("ZPKGName");
                nCol.DataType = Type.GetType("System.String");
                nCol.Caption = "Nazwa komponentu";
                dt.Columns.Add(nCol);
                
            }

            nCol = new DataColumn("ZeroConsumpton");
            nCol.DataType = Type.GetType("System.Double");
            nCol.Caption = "Zerowe zużycie";
            dt.Columns.Add(nCol);
            nCol = new DataColumn("ActConsumption");
            nCol.DataType = Type.GetType("System.Double");
            nCol.Caption = "Rzeczywiste zużycie";
            dt.Columns.Add(nCol);
            nCol = new DataColumn("ActScrap");
            nCol.DataType = Type.GetType("System.Double");
            nCol.Caption = "Rzeczywista strata";
            dt.Columns.Add(nCol);
            nCol = new DataColumn("TargetScrap");
            nCol.DataType = Type.GetType("System.Double");
            nCol.Caption = "Założona strata";
            dt.Columns.Add(nCol);


            if (Items.Any())
            {
                foreach (Order o in Items)
                {
                    if (o.ProductUsages != null && o.ProductUsages.Any())
                    {
                        DataRow nRow = dt.NewRow();
                        nRow["Order"] = o.Number;
                        string compInd = "";
                        string compName = "";
                        float actCons = 0;
                        float tScrap = 0;
                        int zfin = 0;
                        string zfinName = "";
                        string pcInM = "";
                        int divider = 1;
                        int multiplier=0;

                        foreach (ProductionUsage pu in o.ProductUsages)
                        {
                            compInd += pu.Component.Index.ToString() + Environment.NewLine;
                            compName += pu.Component.Name + Environment.NewLine;
                            if(pu.TargetScrap > 0)
                            {
                                tScrap = (float)pu.TargetScrap;
                            }
                            if (zfin == 0)
                            {
                                zfin = pu.Product.Index;
                                zfinName = pu.Product.Name;
                            }
                            if (pu.Component.CategoryId == 4)
                            {
                                //box
                                divider = pu.Component.boxCount;
                                if(pu.Component.Name.Substring(0,2)=="LD" || pu.Component.Name.Substring(0, 2) == "TR")
                                {
                                    multiplier++;
                                }

                            }
                            else if (pu.Component.CategoryId == 5)
                            {
                                //pallet
                                divider = pu.Component.palletCount;
                            }
                            if (pu.Component.UoM == "M")
                            {
                                pcInM = pu.Product.M2PC(StartDate, conn);
                                if (pcInM == "NULL")
                                {
                                    IsMissing = true;
                                    MissingData += pu.Product.Index + Environment.NewLine;
                                    actCons += pu.ActualConsumption;
                                }
                                else
                                {
                                    actCons += (pu.ActualConsumption * float.Parse(pcInM));
                                }
                                
                            }
                            else
                            {
                                actCons += pu.ActualConsumption;
                            }
                            
                            //nRow["ActConsumption"] = pu.ActualConsumption;
                            //nRow["ActScrap"] = pu.ActualScrap;
                            //nRow["TargetScrap"] = pu.TargetScrap;
                            
                        }
                        multiplier = multiplier == 0 ? multiplier+1 : multiplier;
                        nRow["ZPKG"] = compInd;
                        nRow["ZPKGName"] = compName;
                        nRow["ActConsumption"] =  actCons;
                        nRow["TargetScrap"] = tScrap;
                        nRow["ZeroConsumpton"] = (o.ProdSap / divider)*multiplier;
                        nRow["ActScrap"] = (100*(((o.ProdSap/divider)*multiplier) - actCons) / actCons);
                        nRow["ZFIN"] = zfin;
                        nRow["ZFINName"] = zfinName;
                        dt.Rows.Add(nRow);
                    }
                }
            }

            return dt;
        }
    }

    public enum MassBalanceType
    {
        byOrders,
        byCategory
    }

}
