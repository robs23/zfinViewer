using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zfinViewer.Models
{
    public class MesOrder
    {
        public int OrderId { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public MesOrderType Type { get; set; }
        public DateTime ScheduledStartDate { get; set; }
        public DateTime ScheduledFinishDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public Machine Machine { get; set; }

        public string Add()
        {
            string _Result = "OK";

            string ConStr = Static.Secrets.OracleConnectionString;
            var Con = new Oracle.ManagedDataAccess.Client.OracleConnection(ConStr);

            if (Con.State == System.Data.ConnectionState.Closed)
            {
                Con.Open();
            }


            string iStr = @"INSERT INTO QMES_WIP_ORDER (ORDER_NR, NAME, DESCRIPTION_LONG, ORDER_TYPE_ID, SCHEDULED_START_DATE, SCHEDULED_END_DATE, MACHINE_ID, STATUS) 
                            VALUES (:TheNumber, :TheName, :Description, :TypeId, :ScheduledStart, :ScheduledFinish, :MachineId, :Status)";
            try
            {
                var Command = new Oracle.ManagedDataAccess.Client.OracleCommand(iStr, Con);

                OracleParameter[] parameters = new OracleParameter[]
                {
                new OracleParameter("TheNumber", this.Number),
                new OracleParameter("TheName", this.Name),
                new OracleParameter("Description", this.Description),
                new OracleParameter("TypeId", (int)this.Type),
                new OracleParameter("ScheduledStart", this.ScheduledStartDate),
                new OracleParameter("ScheduledFinish", this.ScheduledFinishDate),
                new OracleParameter("MachineId", this.Machine.MesId),
                new OracleParameter("Status", "OK")
                };
                Command.Parameters.AddRange(parameters);
                Command.ExecuteNonQuery();
            }catch(Exception ex)
            {
                _Result = $"Error: {ex.Message}";
            }

            return _Result;
        }
    }
}
