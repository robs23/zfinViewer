using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zfinViewer.Models
{
    public class MachineKeeper
    {
        public List<Machine> Machines { get; set; }

        public MachineKeeper()
        {

        }

        public Machine GetMachineFromMes(string Name)
        {
            Machine mach = null;

            string ConStr = Static.Secrets.OracleConnectionString;
            var Con = new Oracle.ManagedDataAccess.Client.OracleConnection(ConStr);

            if (Con.State == System.Data.ConnectionState.Closed)
            {
                Con.Open();
            }

            string str = string.Format("SELECT * FROM QMES_FO_MACHINE WHERE MACHINE_NR='{0}'", Name);

            var Command = new Oracle.ManagedDataAccess.Client.OracleCommand(str, Con);

            var reader = Command.ExecuteReader();

            if (reader.HasRows)
            {
                mach = new Machine();
                while (reader.Read())
                {
                    mach.MesId = Convert.ToInt32(reader[reader.GetOrdinal("MACHINE_ID")]);
                    mach.MesNumber = Name;
                    mach.Name = reader[reader.GetOrdinal("MACHINE_NAME")].ToString();
                    mach.MesType = Convert.ToInt32(reader[reader.GetOrdinal("MACHINE_TYPE_ID")]);
                }

                
            }

            return mach;
        }

        public void LoadFromMes(int[] Types = null)
        {
            string ConStr = Static.Secrets.OracleConnectionString;
            var Con = new Oracle.ManagedDataAccess.Client.OracleConnection(ConStr);
            string str = "";
            Machines = new List<Machine>();

            if (Con.State == System.Data.ConnectionState.Closed)
            {
                Con.Open();
            }

            if(Types != null)
            {
                str = string.Format("SELECT * FROM QMES_FO_MACHINE WHERE MACHINE_TYPE_ID IN ({0})", string.Join(",",Types));
            }
            else
            {
                str = string.Format("SELECT * FROM QMES_FO_MACHINE");
            }
            

            var Command = new Oracle.ManagedDataAccess.Client.OracleCommand(str, Con);

            var reader = Command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Machine mach = new Machine();
                    mach.MesId = Convert.ToInt32(reader[reader.GetOrdinal("MACHINE_ID")]);
                    mach.Name = reader[reader.GetOrdinal("MACHINE_NAME")].ToString();
                    mach.MesNumber = reader[reader.GetOrdinal("MACHINE_NR")].ToString();
                    mach.MesType = Convert.ToInt32(reader[reader.GetOrdinal("MACHINE_TYPE_ID")]);
                    Machines.Add(mach);
                }


            }
        }
    }
}
