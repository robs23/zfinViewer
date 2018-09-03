using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zfinViewer.Models;

namespace zfinViewer
{
    public class Product : ISearchable
    {
        public int ID { get; set; }
        public int Index { get; set; }
        public string SearchableString { get
            {
                return Name;
            }
        }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public double unitWeight { get; set; }
        public int palletCount { get; set; }
        public double m2pc { get; set; }
        public int boxCount { get; set; }
        public double cost { get; set; }
        public float costLotSize { get; set; }
        public string costLotSizeUnit { get; set; }
        public string UoM { get; set; }
        public Product Parent { get; set; }//if it's component
        public List<ProductionUsage> ProductionUsages { get; set; }

        public Product()
        {
            ProductionUsages = new List<ProductionUsage>();
        }

        public double convert(double input, string uFrom, string uTo)
        {
            uFrom = uFrom.ToLower();
            switch (uFrom)
            {
                case "pc":
                    if (uTo == "kg")
                    {
                        return input * unitWeight;
                    }
                    else if (uTo == "box")
                    {
                        return input / boxCount;
                    }
                    else if (uTo == "pal")
                    {
                        return input / palletCount;
                    }
                    else
                    {
                        return input;
                    }
                    break;
                case "kg":
                    if (uTo == "pc")
                    {
                        return input / unitWeight;
                    }
                    else if (uTo == "box")
                    {
                        return (input / unitWeight) / boxCount;
                    }
                    else if (uTo == "pal")
                    {
                        return (input / unitWeight) / palletCount;
                    }
                    else
                    {
                        return input;
                    }
                    break;
                case "box":
                    if (uTo == "pc")
                    {
                        return input * boxCount;
                    }
                    else if (uTo == "kg")
                    {
                        return input * boxCount*unitWeight;
                    }
                    else if (uTo == "pal")
                    {
                        return (input * boxCount) / palletCount;
                    }
                    else
                    {
                        return input;
                    }
                    break;
                case "pal":
                    if (uTo == "pc")
                    {
                        return input * palletCount;
                    }
                    else if (uTo == "kg")
                    {
                        return input * palletCount * unitWeight;
                    }
                    else if (uTo == "box")
                    {
                        return (input * palletCount) / boxCount;
                    }
                    else
                    {
                        return input;
                    }
                    break;
                default:
                    return 0;
                    break;
            }

        }

        public string M2PC(DateTime date, SqlConnection conn)
        {
            string res = "NULL";
            double x = 0;

            string sql = @"DECLARE @fDate datetime ='" + date + @"'
                        DECLARE @zfin bigint = " + this.Index + @"
                        SELECT CASE WHEN EXISTS
                        (SELECT bomy.amount, bomy.unit FROM tbBom bomy RIGHT JOIN
                        (SELECT oBom.zfinId,  MAX(oBom.bomRecId) as bomRecId, MAX(oBom.dateAdded) as dateAdded FROM
                        (SELECT iBom.bomRecId, zfinId, br.dateAdded FROM tbBomReconciliation br JOIN (
                        SELECT bomRecId, bom.zfinId
                        FROM tbBom bom
                        GROUP BY bomRecId, bom.zfinId) iBom ON iBom.bomRecId=br.bomRecId) oBom
                        WHERE oBom.dateAdded <=@fDate
                        GROUP BY oBom.zfinId) freshBom ON freshBom.zfinId=bomy.zfinId AND freshBom.bomRecId=bomy.bomRecId 
                        LEFT JOIN tbZfin zfin ON zfin.zfinId=bomy.zfinId LEFT JOIN tbZfin mat ON mat.zfinId=bomy.materialId 
                        WHERE mat.materialType=2 AND zfin.zfinIndex=@zfin)
                        THEN 
                        (SELECT bomy.amount FROM tbBom bomy RIGHT JOIN
                        (SELECT oBom.zfinId,  MAX(oBom.bomRecId) as bomRecId, MAX(oBom.dateAdded) as dateAdded FROM
                        (SELECT iBom.bomRecId, zfinId, br.dateAdded FROM tbBomReconciliation br JOIN (
                        SELECT bomRecId, bom.zfinId
                        FROM tbBom bom
                        GROUP BY bomRecId, bom.zfinId) iBom ON iBom.bomRecId=br.bomRecId) oBom
                        WHERE oBom.dateAdded <=@fDate
                        GROUP BY oBom.zfinId) freshBom ON freshBom.zfinId=bomy.zfinId AND freshBom.bomRecId=bomy.bomRecId 
                        LEFT JOIN tbZfin zfin ON zfin.zfinId=bomy.zfinId LEFT JOIN tbZfin mat ON mat.zfinId=bomy.materialId 
                        WHERE mat.materialType=2 AND zfin.zfinIndex=@zfin)
                        ELSE
                        (SELECT bomy.amount FROM tbBom bomy RIGHT JOIN
                        (SELECT oBom.zfinId,  MAX(oBom.bomRecId) as bomRecId, MAX(oBom.dateAdded) as dateAdded FROM
                        (SELECT iBom.bomRecId, zfinId, br.dateAdded FROM tbBomReconciliation br JOIN (
                        SELECT bomRecId, bom.zfinId
                        FROM tbBom bom
                        GROUP BY bomRecId, bom.zfinId) iBom ON iBom.bomRecId=br.bomRecId) oBom
                        WHERE oBom.dateAdded <=GETDATE()
                        GROUP BY oBom.zfinId) freshBom ON freshBom.zfinId=bomy.zfinId AND freshBom.bomRecId=bomy.bomRecId 
                        LEFT JOIN tbZfin zfin ON zfin.zfinId=bomy.zfinId LEFT JOIN tbZfin mat ON mat.zfinId=bomy.materialId 
                        WHERE mat.materialType=2 AND zfin.zfinIndex=@zfin)
                        END As Result";

            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            SqlCommand command = new SqlCommand(sql, conn);

            using (SqlDataReader nReader = command.ExecuteReader())
            {
                while (nReader.Read())
                {
                    if(double.TryParse(nReader[0].ToString(), out x))
                    {
                        if (this.palletCount > 0)
                        {
                            res = (this.palletCount / x).ToString();
                        }
                    }
                }
            }

            return res;
        }

    }
}
