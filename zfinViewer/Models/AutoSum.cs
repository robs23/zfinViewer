using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zfinViewer.Models
{
    public class AutoSum
    {
        ToolStripLabel Label;
        StatusStrip Status;
        DataGridView Dgv;

        public AutoSum(StatusStrip status, DataGridView dgv)
        {
            Status = status;
            Dgv = dgv;
        }

        public void Initilize()
        {
            Label = new ToolStripLabel("Gotowy");
            Dgv.SelectionChanged += selectionChanged;
            Status.Items.Add(Label);
        }

        private void selectionChanged(object sender, EventArgs e)
        {
            Update();
        }

        private void summaryChanged(object sender, ToolStripItemClickedEventArgs e)
        {
            Update();
        }

        public void Update()
        {
            double? SumResult = null;
            double CountResult = 0;
            double n;
            bool greenlight = true;

            if (greenlight)
            {
                foreach (DataGridViewCell cell in Dgv.SelectedCells)
                {
                    if (Double.TryParse(cell.Value.ToString(), out n))
                    {
                        if (SumResult == null)
                            SumResult = 0;
                        SumResult += n;
                    }
                    CountResult++;
                }

                if (CountResult < 2 )
                {
                    Label.Text = "Gotowy";
                }else if(SumResult == null)
                {
                    Label.Text = $"Liczba: {CountResult}";
                }
                else
                {
                    Label.Text = $"Suma: {SumResult}    Liczba: {CountResult}   Średnia: {Math.Round((double)SumResult / CountResult, 2)}";
                }
            }
        }

    }
}
