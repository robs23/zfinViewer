using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zfinViewer.Models
{
    public class Operation
    {
        public int OperationId { get; set; }
        public int? MesId { get; set; }
        public string Type { get; set; }
        public string MesString { get; set; }
        public int? SessionId { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<OperationData> OperationDatas { get; set; }
    }
}
