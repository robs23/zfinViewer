using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zfinViewer.Models
{
    public class Filter
    {
        public List<FilterColumn> Columns { get; set; }
        public bool IsOn { get; set; }
        public DataTable DataTable { get; set; }
        public DataTable FilteredTable { get; set; }
        public bool IsInitialized { get; set; }

        public Filter()
        {
            Columns = new List<FilterColumn>();
        }

        public void Init(DataTable dt)
        {
            if (!IsInitialized)
            {
                DataTable = dt;//Keep oryginal data at all times
                foreach (DataColumn col in DataTable.Columns)
                {
                    FilterColumn nCol = new FilterColumn() { ID = col.Ordinal, Name = col.Caption };
                    List<object> items = new List<object>();
                    foreach(DataRow row in DataTable.Rows)
                    {
                        items.Add(row[col.Ordinal]);
                    }
                    nCol.Items = items;
                    nCol.LimitTo = new List<object>();
                    nCol.Exclude = new List<object>();
                    nCol.Type = col.DataType;
                    Columns.Add(nCol);
                }
                IsInitialized = true;
            }
        }
    }
}
