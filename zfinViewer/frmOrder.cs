using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using zfinViewer.Models;

namespace zfinViewer
{
    public partial class frmOrder : Form
    {
        Order _this;
        int OrderNumber;
        public frmOrder(int orderNumber, Form parent)
        {
            InitializeComponent();
            OrderNumber = orderNumber;
            this.Owner = parent;
            this.Location = new Point(this.Owner.Location.X + 20, this.Owner.Location.Y + 20);
            Reload();
            LoadConsumption();
        }

        public void Reload()
        {
            _this = new Order(OrderNumber);
            _this.Load();
            txtOrder.Text = _this.Number.ToString();
            txtIndex.Text = _this.Index.ToString();
            txtName.Text = _this.Name.ToString();
            txtProdSap.Text = _this.ProdSap.ToString();
            txtProdMes.Text = _this.ProdMes.ToString();
        }

        public void LoadConsumption()
        {
            _this.GetConsumption();
            var Usages = (from pu in _this.ProductUsages
                         select new
                         {
                             Komponent = pu.Component.Index,
                             Nazwa = pu.Component.Name,
                             UoM = pu.Component.UoM,
                             RzeczywistaKonsumpcja = pu.ActualConsumption,
                             OczekiwanaKonsumpcja = pu.TargetConsumption,
                             RzeczywistaStrata = pu.ActualScrap,
                             OczekiwanaStrata = pu.TargetScrap,
                             StrataBom = pu.BomScrap
                         });
            dgvConsumption.DataSource = Usages.ToList();
        }
    }
}
