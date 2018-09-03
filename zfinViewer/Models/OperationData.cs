using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zfinViewer.Models
{
    public class OperationData
    {
        public DateTime Moment { get; set; }
        public string MachineName { get; set; }
        public int? Amount { get; set; }
    }
}
