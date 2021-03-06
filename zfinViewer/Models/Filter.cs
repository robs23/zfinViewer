﻿using System;
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
        public bool IsOn { get
            {
                if (Columns.Any())
                {
                    foreach(FilterColumn col in Columns)
                    {
                        if(col.Exclude.Any() || col.LimitTo.Any())
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }
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
                        if (!items.Where(i => i.Equals(row[col.Ordinal])).Any())
                        {
                            items.Add(row[col.Ordinal]);
                        }
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

        public void Apply()
        {

            foreach (FilterColumn col in Columns)
            {
                if (col.LimitTo.Any())
                {
                    var result = from DataRow row in DataTable.Rows
                                 where col.LimitTo.Contains(row[col.ID])
                                 select row;
                    FilteredTable = result.CopyToDataTable<DataRow>();
                }else if (col.Exclude.Any())
                {
                    var result = from DataRow row in DataTable.Rows
                                 where !col.Exclude.Contains(row[col.ID])
                                 select row;
                    FilteredTable = result.CopyToDataTable<DataRow>();
                }
            }
        }


        public void Clear(List<string>Cols = null)
        {
            if (Cols == null)
            {
                //remove whole filter
                foreach(FilterColumn col in Columns)
                {
                    col.Exclude.Clear();
                    col.LimitTo.Clear();
                }
            }
            else
            {
                foreach (FilterColumn col in Columns)
                {
                    if (Cols.Where(c => c.Equals(col.Name)).Any())
                    {
                        col.Exclude.Clear();
                        col.LimitTo.Clear();
                    }
                }
            }
        }
    }
}
