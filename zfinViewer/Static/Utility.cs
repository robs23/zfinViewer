using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Util-1 More namespaces.  
using System.Configuration;

namespace zfinViewer
{
    internal class Utility
    {

        //Get the connection string from App config file.  
        internal static string GetConnectionString()
        {
            ////Util-2 Assume failure.  
            //string returnValue = null;

            ////Util-3 Look for the name in the connectionStrings section.  
            //ConnectionStringSettings settings =
            //ConfigurationManager.ConnectionStrings["zfinViewer.Properties.Settings.npdConnectionString"];

            ////If found, return the connection string.  
            //if (settings != null)
            //    returnValue = settings.ConnectionString;

            string returnValue = Static.RuntimeSettings.ConnectionString;

            return returnValue;
        }
    }
}