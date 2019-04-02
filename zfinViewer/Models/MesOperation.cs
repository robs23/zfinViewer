using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zfinViewer.Models
{
    public class MesOperation
    {
        public int OperationId { get; set; }
        public int OrderId { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public MesOperationType Type { get; set; }
    }

    public enum MesOperationType
    {
        Przegląd = 9
    }
}
