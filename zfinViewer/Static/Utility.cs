using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Util-1 More namespaces.  
using System.Configuration;
using System.Globalization;

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

        // This presumes that weeks start with Monday.
        // Week 1 is the 1st week of the year with a Thursday in it.
        public static int GetIsoWeekNumber(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }
}