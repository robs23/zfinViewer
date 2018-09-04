using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zfinViewer.Models
{
    public class FilterColumn
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
        public List<object> Items { get; set; }
        public List<object> LimitTo { get; set; }
        public List<object> Exclude { get; set; }

        public FilterColumn()
        {

        }


    }
}
