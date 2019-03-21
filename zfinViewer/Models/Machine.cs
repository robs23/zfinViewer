using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zfinViewer.Models
{
    public class Machine
    {
        [Browsable(false)]
        public int? MesId { get; set; }
        [Browsable(false)]
        public string MesNumber { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public string Type { get; set; }
        public int MesType { get; set; }
        [Browsable(false)]
        public bool IsSelected { get; set; }
    }
}
