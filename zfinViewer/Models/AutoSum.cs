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
        ToolStripDropDownButton Button;
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
            Button = new ToolStripDropDownButton();
            Label = new ToolStripLabel("Gotowy");
            Button.Text = "Podsumowanie";
            Button.DisplayStyle = ToolStripItemDisplayStyle.Text;
            Button.DropDownItems.Add("Suma");
            Button.DropDownItems.Add("Liczba");
            Button.DropDownItems.Add("Średnia");
            Button.DropDownItemClicked += summaryChanged;
            Dgv.SelectionChanged += selectionChanged;
            Status.Items.Add(Label);
            Status.Items.Add(Button);
        }

        private void selectionChanged(object sender, EventArgs e)
        {
            Update();
        }

        private void summaryChanged(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Suma")
            {

                Button.Text = "Suma";
            }
            else if (e.ClickedItem.Text == "Liczba")
            {
                Button.Text = "Liczba";
            }
            else
            {
                Button.Text = "Średnia";
            }
            Update();
        }

        public void Update()
        {
            int counter = 0;
            double Result = 0;
            double n;
            bool greenlight = true;

            if (greenlight)
            {
                if (Button.Text == "Suma")
                {
                    foreach (DataGridViewCell cell in Dgv.SelectedCells)
                    {
                        if (Double.TryParse(cell.Value.ToString(), out n))
                        {
                            Result += n;
                        }
                    }
                    if (Result > 0)
                    {
                        Label.Text = Result.ToString();
                    }
                }
                else if (Button.Text == "Liczba")
                {
                    foreach (DataGridViewCell cell in Dgv.SelectedCells)
                    {
                        Result++;
                    }
                    if (Result > 0)
                    {
                        Label.Text = Result.ToString();
                    }
                }
                else if (Button.Text == "Średnia")
                {
                    foreach (DataGridViewCell cell in Dgv.SelectedCells)
                    {
                        if (Double.TryParse(cell.Value.ToString(), out n))
                        {
                            Result += n;
                            counter++;
                        }
                    }
                    if (Result > 0)
                    {
                        Label.Text = (Result / counter).ToString();
                    }
                }
            }
        }

    }
}
