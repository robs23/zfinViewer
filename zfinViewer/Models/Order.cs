using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using zfinViewer;

namespace zfinViewer.Models
{
    public class Order
    {
        public int Number { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public float? ProdSap { get; set; }
        public int? ProdMes { get; set; }
        public int? ProdScada { get; set; }
        public string ProdDates { get; set; }
        public int? SessionId { get; set; }
        public string Type { get; set; }
        public List<Operation> Operations { get; set; }
        public List<ProductionUsage> ProductUsages { get; set; }

        public Order(int OrderNumber)
        {
            Number = OrderNumber;
            ProductUsages = new List<ProductionUsage>();
        }

        public Order(int OrderNumber, float? PW)
        {
            Number = OrderNumber;
            ProdSap = PW == null ? 0 : PW;
            ProductUsages = new List<ProductionUsage>();
        }


        public void Load()
        {
            string sql = @"SELECT z.zfinIndex, z.zfinName, z.zfinType, o.executedSap, o.executedMes
                        FROM tbOrders o LEFT JOIN tbZfin z ON z.zfinId = o.zfinId
                        WHERE o.sapId = @index";
            SqlConnection conn = new SqlConnection(Variables.npdConnectionString);
            SqlCommand sqlComand = new SqlCommand(sql, conn);
            sqlComand.Parameters.Add("@index", SqlDbType.Int);
            sqlComand.Parameters["@index"].Value = Number;
            try
            {
                conn.Open();
                using (SqlDataReader reader = sqlComand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Index = reader.GetInt32(reader.GetOrdinal("zfinIndex"));
                        Name = reader["zfinName"].ToString();
                        Type = reader["zfinType"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("executedSap")))
                        {
                            ProdSap = Convert.ToInt32(reader["executedSap"].ToString());
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("executedMes")))
                        {
                            ProdMes = Convert.ToInt32(reader["executedMes"].ToString());
                        }
                    }
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void GetConsumption()
        {
            string sql = @"SELECT comp.zfinIndex as compIndex, comp.zfinName as compName, comp.basicUom as compUoM, pc.actualConsumption, pc.bomScrap, pc.actualScrap,pc.targetConsumption,pc.targetScrap, pc.createdOn
                        FROM tbOrders o LEFT JOIN tbProductionConsumption pc ON o.orderId=pc.orderId LEFT JOIN tbZfin comp ON comp.zfinId=pc.componentId
                        WHERE o.sapId=@order";

            SqlConnection conn = new SqlConnection(Variables.npdConnectionString);
            SqlCommand sqlComand = new SqlCommand(sql, conn);
            sqlComand.Parameters.Add("@order", SqlDbType.Int);
            sqlComand.Parameters["@order"].Value = Number;
            try
            {
                conn.Open();
                using (SqlDataReader reader = sqlComand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ProductionUsage pu = new ProductionUsage
                        {
                            Component = new Product { Index = int.Parse(reader[0].ToString()), Name = reader[1].ToString(), UoM = reader[2].ToString() },
                            ActualConsumption = float.Parse(reader[3].ToString()),
                            BomScrap = float.Parse(reader[4].ToString()),
                            ActualScrap = float.Parse(reader[5].ToString()),
                            TargetConsumption = float.Parse(reader[6].ToString()),
                            TargetScrap = float.Parse(reader[7].ToString())
                        };
                        ProductUsages.Add(pu);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void GetOperations()
        {
            string sql = @"SELECT op.*, od.plMoment, m.machineName, od.plAmount
                        FROM tbOrders o LEFT JOIN tbOperations op ON op.orderId = o.orderId LEFT JOIN tbOperationData od ON od.operationId = op.operationId LEFT JOIN tbMachine m ON m.machineId = od.plMach
                        WHERE o.sapId = @index";

            SqlConnection conn = new SqlConnection(Variables.npdConnectionString);
            SqlCommand sqlComand = new SqlCommand(sql, conn);
            sqlComand.Parameters.Add("@index", SqlDbType.Int);
            sqlComand.Parameters["@index"].Value = Number;
            try
            {
                conn.Open();
                using (SqlDataReader reader = sqlComand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int opId = Convert.ToInt32(reader["mesId"].ToString());
                        if (Operations.Where(o => o.MesId == opId).Any()){
                            //operation already eists. Add just operationData to it
                            Operation op = Operations.Where(o => o.MesId == opId).FirstOrDefault();
                            OperationData od = new OperationData();
                            od.MachineName = reader["machineName"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
