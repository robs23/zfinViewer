using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zfinViewer.Models
{
    public class MesOrders
    {
        public MesOrders()
        {

        }

        public bool OrderExists(string id)
        {
            string ConStr = Static.Secrets.OracleConnectionString;
            var Con = new Oracle.ManagedDataAccess.Client.OracleConnection(ConStr);
            bool Existent = false;

            if (Con.State == System.Data.ConnectionState.Closed)
            {
                Con.Open();
            }

            string str = string.Format("SELECT * FROM QMES_WIP_ORDER WHERE ORDER_NR='{0}'", id);

            var Command = new Oracle.ManagedDataAccess.Client.OracleCommand(str, Con);

            var reader = Command.ExecuteReader();

            if (reader.HasRows)
            {
                Existent = true;
            }

            return Existent;
        }
    }
}
