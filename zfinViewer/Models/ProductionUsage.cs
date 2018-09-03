using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zfinViewer.Models
{
    public class ProductionUsage
    {
        public Product Component { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }
        public float ActualConsumption { get; set; }
        public float? BomScrap { get; set; }
        public float? ActualScrap { get; set; }
        public float? TargetConsumption { get; set; }
        public float? TargetScrap { get; set; }
    }
}
