namespace zfinViewer
{
    partial class frmZfinSearch
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.dgItems = new System.Windows.Forms.DataGridView();
            this.lblHitsCount = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.bilansMasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zFORToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zPKGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.planProdukcjiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ograniczeniaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgItems)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.txtSearch, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.dgItems, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblHitsCount, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.menuStrip1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(373, 386);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(3, 30);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(367, 20);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.updateSearch);
            this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SearchKeyUp);
            // 
            // dgItems
            // 
            this.dgItems.AllowUserToAddRows = false;
            this.dgItems.AllowUserToDeleteRows = false;
            this.dgItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgItems.Location = new System.Drawing.Point(3, 63);
            this.dgItems.Name = "dgItems";
            this.dgItems.ReadOnly = true;
            this.dgItems.Size = new System.Drawing.Size(367, 290);
            this.dgItems.TabIndex = 1;
            this.dgItems.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.displayProduct);
            // 
            // lblHitsCount
            // 
            this.lblHitsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHitsCount.AutoSize = true;
            this.lblHitsCount.Location = new System.Drawing.Point(3, 364);
            this.lblHitsCount.Name = "lblHitsCount";
            this.lblHitsCount.Size = new System.Drawing.Size(367, 13);
            this.lblHitsCount.TabIndex = 2;
            this.lblHitsCount.Text = "Hit count";
            this.lblHitsCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bilansMasToolStripMenuItem,
            this.planProdukcjiToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(373, 20);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // bilansMasToolStripMenuItem
            // 
            this.bilansMasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zFORToolStripMenuItem,
            this.zPKGToolStripMenuItem});
            this.bilansMasToolStripMenuItem.Name = "bilansMasToolStripMenuItem";
            this.bilansMasToolStripMenuItem.Size = new System.Drawing.Size(75, 16);
            this.bilansMasToolStripMenuItem.Text = "Bilans mas";
            // 
            // zFORToolStripMenuItem
            // 
            this.zFORToolStripMenuItem.Name = "zFORToolStripMenuItem";
            this.zFORToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.zFORToolStripMenuItem.Text = "ZFOR";
            // 
            // zPKGToolStripMenuItem
            // 
            this.zPKGToolStripMenuItem.Name = "zPKGToolStripMenuItem";
            this.zPKGToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.zPKGToolStripMenuItem.Text = "ZPKG";
            this.zPKGToolStripMenuItem.Click += new System.EventHandler(this.zPKGToolStripMenuItem_Click);
            // 
            // planProdukcjiToolStripMenuItem
            // 
            this.planProdukcjiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ograniczeniaToolStripMenuItem});
            this.planProdukcjiToolStripMenuItem.Name = "planProdukcjiToolStripMenuItem";
            this.planProdukcjiToolStripMenuItem.Size = new System.Drawing.Size(95, 16);
            this.planProdukcjiToolStripMenuItem.Text = "Plan produkcji";
            // 
            // ograniczeniaToolStripMenuItem
            // 
            this.ograniczeniaToolStripMenuItem.Name = "ograniczeniaToolStripMenuItem";
            this.ograniczeniaToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ograniczeniaToolStripMenuItem.Text = "Ograniczenia";
            this.ograniczeniaToolStripMenuItem.Click += new System.EventHandler(this.ograniczeniaToolStripMenuItem_Click);
            // 
            // frmZfinSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 386);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "frmZfinSearch";
            this.Text = "Wpisz numer produktu";
            this.Load += new System.EventHandler(this.loadMe);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgItems)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dgItems;
        private System.Windows.Forms.Label lblHitsCount;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem bilansMasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zFORToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zPKGToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem planProdukcjiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ograniczeniaToolStripMenuItem;
    }
}