using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zfinViewer.Static
{
    public static class Secrets
    {
        public static string OracleConnectionString
        {
            get { return OracleTestConnectionString; }
        }

        public static string OracleTestConnectionString
        {
            get { return "DATA SOURCE=10.142.27.102/Test:1521/Test;PERSIST SECURITY INFO=True;USER ID=QGUARADM;PASSWORD=quantum"; }
        }

        public static string OracleProductionConnectionString
        {
            get { return "DATA SOURCE=10.142.27.102:1521/QGUAR;PERSIST SECURITY INFO=True;USER ID=QGUARADM;PASSWORD=Quantum88"; }
        }
    }
}
