namespace zfinViewer
{
    partial class frmCompany
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCompany));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.tabCompany = new System.Windows.Forms.TabControl();
            this.pgGeneral = new System.Windows.Forms.TabPage();
            this.tlpGeneral = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.txtType = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtZip = new System.Windows.Forms.TextBox();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.txtCountry = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtVat = new System.Windows.Forms.TextBox();
            this.pgContacts = new System.Windows.Forms.TabPage();
            this.dgvConctacts = new System.Windows.Forms.DataGridView();
            this.pgConnected = new System.Windows.Forms.TabPage();
            this.dgvConnected = new System.Windows.Forms.DataGridView();
            this.pgHistory = new System.Windows.Forms.TabPage();
            this.tlpHistory = new System.Windows.Forms.TableLayoutPanel();
            this.dgvHistory = new System.Windows.Forms.DataGridView();
            this.tlpHistoryIn = new System.Windows.Forms.TableLayoutPanel();
            this.cmbHistoryType = new System.Windows.Forms.ComboBox();
            this.cmbSummaryType = new System.Windows.Forms.ComboBox();
            this.Status = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.ddSummary = new System.Windows.Forms.ToolStripDropDownButton();
            this.sumaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.liczbaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sredniaStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tlpMain.SuspendLayout();
            this.tabCompany.SuspendLayout();
            this.pgGeneral.SuspendLayout();
            this.tlpGeneral.SuspendLayout();
            this.pgContacts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConctacts)).BeginInit();
            this.pgConnected.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConnected)).BeginInit();
            this.pgHistory.SuspendLayout();
            this.tlpHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).BeginInit();
            this.tlpHistoryIn.SuspendLayout();
            this.Status.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.tabCompany, 0, 0);
            this.tlpMain.Controls.Add(this.Status, 0, 1);
            this.tlpMain.Location = new System.Drawing.Point(-2, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 2;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tlpMain.Size = new System.Drawing.Size(505, 431);
            this.tlpMain.TabIndex = 0;
            // 
            // tabCompany
            // 
            this.tabCompany.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCompany.Controls.Add(this.pgGeneral);
            this.tabCompany.Controls.Add(this.pgContacts);
            this.tabCompany.Controls.Add(this.pgConnected);
            this.tabCompany.Controls.Add(this.pgHistory);
            this.tabCompany.Location = new System.Drawing.Point(3, 3);
            this.tabCompany.Multiline = true;
            this.tabCompany.Name = "tabCompany";
            this.tabCompany.SelectedIndex = 0;
            this.tabCompany.Size = new System.Drawing.Size(499, 398);
            this.tabCompany.TabIndex = 0;
            // 
            // pgGeneral
            // 
            this.pgGeneral.Controls.Add(this.tlpGeneral);
            this.pgGeneral.Location = new System.Drawing.Point(4, 22);
            this.pgGeneral.Name = "pgGeneral";
            this.pgGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.pgGeneral.Size = new System.Drawing.Size(491, 372);
            this.pgGeneral.TabIndex = 0;
            this.pgGeneral.Text = "Ogólne";
            this.pgGeneral.UseVisualStyleBackColor = true;
            // 
            // tlpGeneral
            // 
            this.tlpGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpGeneral.ColumnCount = 2;
            this.tlpGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpGeneral.Controls.Add(this.label6, 0, 5);
            this.tlpGeneral.Controls.Add(this.txtType, 1, 5);
            this.tlpGeneral.Controls.Add(this.label1, 0, 0);
            this.tlpGeneral.Controls.Add(this.label2, 0, 1);
            this.tlpGeneral.Controls.Add(this.label3, 0, 2);
            this.tlpGeneral.Controls.Add(this.label4, 0, 3);
            this.tlpGeneral.Controls.Add(this.label5, 0, 4);
            this.tlpGeneral.Controls.Add(this.txtName, 1, 0);
            this.tlpGeneral.Controls.Add(this.txtAddress, 1, 1);
            this.tlpGeneral.Controls.Add(this.txtZip, 1, 2);
            this.tlpGeneral.Controls.Add(this.txtCity, 1, 3);
            this.tlpGeneral.Controls.Add(this.txtCountry, 1, 4);
            this.tlpGeneral.Controls.Add(this.label7, 0, 6);
            this.tlpGeneral.Controls.Add(this.txtVat, 1, 6);
            this.tlpGeneral.Location = new System.Drawing.Point(6, 6);
            this.tlpGeneral.Name = "tlpGeneral";
            this.tlpGeneral.RowCount = 8;
            this.tlpGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpGeneral.Size = new System.Drawing.Size(482, 292);
            this.tlpGeneral.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 158);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(114, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Typ";
            // 
            // txtType
            // 
            this.txtType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtType.Location = new System.Drawing.Point(123, 155);
            this.txtType.Name = "txtType";
            this.txtType.ReadOnly = true;
            this.txtType.Size = new System.Drawing.Size(356, 20);
            this.txtType.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nazwa";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Adres";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Kod";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Miasto";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 128);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Kraj";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(123, 5);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(356, 20);
            this.txtName.TabIndex = 5;
            // 
            // txtAddress
            // 
            this.txtAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddress.Location = new System.Drawing.Point(123, 35);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.ReadOnly = true;
            this.txtAddress.Size = new System.Drawing.Size(356, 20);
            this.txtAddress.TabIndex = 6;
            // 
            // txtZip
            // 
            this.txtZip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtZip.Location = new System.Drawing.Point(123, 65);
            this.txtZip.Name = "txtZip";
            this.txtZip.ReadOnly = true;
            this.txtZip.Size = new System.Drawing.Size(356, 20);
            this.txtZip.TabIndex = 7;
            // 
            // txtCity
            // 
            this.txtCity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCity.Location = new System.Drawing.Point(123, 95);
            this.txtCity.Name = "txtCity";
            this.txtCity.ReadOnly = true;
            this.txtCity.Size = new System.Drawing.Size(356, 20);
            this.txtCity.TabIndex = 8;
            // 
            // txtCountry
            // 
            this.txtCountry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCountry.Location = new System.Drawing.Point(123, 125);
            this.txtCountry.Name = "txtCountry";
            this.txtCountry.ReadOnly = true;
            this.txtCountry.Size = new System.Drawing.Size(356, 20);
            this.txtCountry.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 188);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(114, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Numer VAT";
            // 
            // txtVat
            // 
            this.txtVat.AllowDrop = true;
            this.txtVat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVat.Location = new System.Drawing.Point(123, 185);
            this.txtVat.Name = "txtVat";
            this.txtVat.ReadOnly = true;
            this.txtVat.Size = new System.Drawing.Size(356, 20);
            this.txtVat.TabIndex = 15;
            // 
            // pgContacts
            // 
            this.pgContacts.Controls.Add(this.dgvConctacts);
            this.pgContacts.Location = new System.Drawing.Point(4, 22);
            this.pgContacts.Name = "pgContacts";
            this.pgContacts.Padding = new System.Windows.Forms.Padding(3);
            this.pgContacts.Size = new System.Drawing.Size(491, 372);
            this.pgContacts.TabIndex = 1;
            this.pgContacts.Text = "Kontakty";
            this.pgContacts.UseVisualStyleBackColor = true;
            // 
            // dgvConctacts
            // 
            this.dgvConctacts.AllowUserToAddRows = false;
            this.dgvConctacts.AllowUserToDeleteRows = false;
            this.dgvConctacts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvConctacts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvConctacts.Location = new System.Drawing.Point(3, 3);
            this.dgvConctacts.Name = "dgvConctacts";
            this.dgvConctacts.ReadOnly = true;
            this.dgvConctacts.Size = new System.Drawing.Size(485, 366);
            this.dgvConctacts.TabIndex = 0;
            this.dgvConctacts.SelectionChanged += new System.EventHandler(this.dgContSelectionChanged);
            this.dgvConctacts.DoubleClick += new System.EventHandler(this.loadContact);
            // 
            // pgConnected
            // 
            this.pgConnected.Controls.Add(this.dgvConnected);
            this.pgConnected.Location = new System.Drawing.Point(4, 22);
            this.pgConnected.Name = "pgConnected";
            this.pgConnected.Padding = new System.Windows.Forms.Padding(3);
            this.pgConnected.Size = new System.Drawing.Size(491, 372);
            this.pgConnected.TabIndex = 2;
            this.pgConnected.Text = "Powiązane firmy";
            this.pgConnected.UseVisualStyleBackColor = true;
            // 
            // dgvConnected
            // 
            this.dgvConnected.AllowUserToAddRows = false;
            this.dgvConnected.AllowUserToDeleteRows = false;
            this.dgvConnected.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvConnected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvConnected.Location = new System.Drawing.Point(3, 3);
            this.dgvConnected.Name = "dgvConnected";
            this.dgvConnected.ReadOnly = true;
            this.dgvConnected.Size = new System.Drawing.Size(485, 366);
            this.dgvConnected.TabIndex = 0;
            this.dgvConnected.SelectionChanged += new System.EventHandler(this.dgConnSelectionChanged);
            // 
            // pgHistory
            // 
            this.pgHistory.Controls.Add(this.tlpHistory);
            this.pgHistory.Location = new System.Drawing.Point(4, 22);
            this.pgHistory.Name = "pgHistory";
            this.pgHistory.Padding = new System.Windows.Forms.Padding(3);
            this.pgHistory.Size = new System.Drawing.Size(491, 372);
            this.pgHistory.TabIndex = 3;
            this.pgHistory.Text = "Historia";
            this.pgHistory.UseVisualStyleBackColor = true;
            // 
            // tlpHistory
            // 
            this.tlpHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpHistory.ColumnCount = 1;
            this.tlpHistory.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpHistory.Controls.Add(this.dgvHistory, 0, 1);
            this.tlpHistory.Controls.Add(this.tlpHistoryIn, 0, 0);
            this.tlpHistory.Location = new System.Drawing.Point(3, 3);
            this.tlpHistory.Name = "tlpHistory";
            this.tlpHistory.RowCount = 2;
            this.tlpHistory.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpHistory.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpHistory.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpHistory.Size = new System.Drawing.Size(485, 362);
            this.tlpHistory.TabIndex = 0;
            // 
            // dgvHistory
            // 
            this.dgvHistory.AllowUserToAddRows = false;
            this.dgvHistory.AllowUserToDeleteRows = false;
            this.dgvHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistory.Location = new System.Drawing.Point(3, 33);
            this.dgvHistory.Name = "dgvHistory";
            this.dgvHistory.ReadOnly = true;
            this.dgvHistory.Size = new System.Drawing.Size(479, 326);
            this.dgvHistory.TabIndex = 1;
            this.dgvHistory.SelectionChanged += new System.EventHandler(this.dgSelectionChanged);
            // 
            // tlpHistoryIn
            // 
            this.tlpHistoryIn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpHistoryIn.ColumnCount = 2;
            this.tlpHistoryIn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpHistoryIn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpHistoryIn.Controls.Add(this.cmbHistoryType, 0, 0);
            this.tlpHistoryIn.Controls.Add(this.cmbSummaryType, 1, 0);
            this.tlpHistoryIn.Location = new System.Drawing.Point(3, 3);
            this.tlpHistoryIn.Name = "tlpHistoryIn";
            this.tlpHistoryIn.RowCount = 1;
            this.tlpHistoryIn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpHistoryIn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tlpHistoryIn.Size = new System.Drawing.Size(479, 24);
            this.tlpHistoryIn.TabIndex = 0;
            // 
            // cmbHistoryType
            // 
            this.cmbHistoryType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbHistoryType.FormattingEnabled = true;
            this.cmbHistoryType.Location = new System.Drawing.Point(3, 3);
            this.cmbHistoryType.Name = "cmbHistoryType";
            this.cmbHistoryType.Size = new System.Drawing.Size(233, 21);
            this.cmbHistoryType.TabIndex = 0;
            this.cmbHistoryType.SelectedIndexChanged += new System.EventHandler(this.historyTypeChanged);
            // 
            // cmbSummaryType
            // 
            this.cmbSummaryType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSummaryType.FormattingEnabled = true;
            this.cmbSummaryType.Location = new System.Drawing.Point(242, 3);
            this.cmbSummaryType.Name = "cmbSummaryType";
            this.cmbSummaryType.Size = new System.Drawing.Size(234, 21);
            this.cmbSummaryType.TabIndex = 1;
            this.cmbSummaryType.SelectedIndexChanged += new System.EventHandler(this.summaryTypeChanged);
            // 
            // Status
            // 
            this.Status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.ddSummary});
            this.Status.Location = new System.Drawing.Point(0, 409);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(505, 22);
            this.Status.TabIndex = 1;
            this.Status.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(48, 17);
            this.lblStatus.Text = "Gotowy";
            // 
            // ddSummary
            // 
            this.ddSummary.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ddSummary.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sumaToolStripMenuItem,
            this.liczbaToolStripMenuItem,
            this.sredniaStripMenuItem1});
            this.ddSummary.Image = ((System.Drawing.Image)(resources.GetObject("ddSummary.Image")));
            this.ddSummary.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ddSummary.Name = "ddSummary";
            this.ddSummary.Size = new System.Drawing.Size(102, 20);
            this.ddSummary.Text = "Podsumowanie";
            this.ddSummary.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.summaryChanged);
            // 
            // sumaToolStripMenuItem
            // 
            this.sumaToolStripMenuItem.Name = "sumaToolStripMenuItem";
            this.sumaToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.sumaToolStripMenuItem.Text = "Suma";
            // 
            // liczbaToolStripMenuItem
            // 
            this.liczbaToolStripMenuItem.Name = "liczbaToolStripMenuItem";
            this.liczbaToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.liczbaToolStripMenuItem.Text = "Liczba";
            // 
            // sredniaStripMenuItem1
            // 
            this.sredniaStripMenuItem1.Name = "sredniaStripMenuItem1";
            this.sredniaStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.sredniaStripMenuItem1.Text = "Średnia";
            // 
            // frmCompany
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 426);
            this.Controls.Add(this.tlpMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCompany";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Dane firmy";
            this.Load += new System.EventHandler(this.loadMe);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.tabCompany.ResumeLayout(false);
            this.pgGeneral.ResumeLayout(false);
            this.tlpGeneral.ResumeLayout(false);
            this.tlpGeneral.PerformLayout();
            this.pgContacts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvConctacts)).EndInit();
            this.pgConnected.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvConnected)).EndInit();
            this.pgHistory.ResumeLayout(false);
            this.tlpHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).EndInit();
            this.tlpHistoryIn.ResumeLayout(false);
            this.Status.ResumeLayout(false);
            this.Status.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.TabControl tabCompany;
        private System.Windows.Forms.TabPage pgGeneral;
        private System.Windows.Forms.TabPage pgContacts;
        private System.Windows.Forms.TabPage pgConnected;
        private System.Windows.Forms.TabPage pgHistory;
        private System.Windows.Forms.TableLayoutPanel tlpGeneral;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.TextBox txtZip;
        private System.Windows.Forms.TextBox txtCity;
        private System.Windows.Forms.TextBox txtCountry;
        private System.Windows.Forms.DataGridView dgvConctacts;
        private System.Windows.Forms.DataGridView dgvConnected;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtVat;
        private System.Windows.Forms.TableLayoutPanel tlpHistory;
        private System.Windows.Forms.TableLayoutPanel tlpHistoryIn;
        private System.Windows.Forms.DataGridView dgvHistory;
        private System.Windows.Forms.ComboBox cmbHistoryType;
        private System.Windows.Forms.ComboBox cmbSummaryType;
        private System.Windows.Forms.StatusStrip Status;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripDropDownButton ddSummary;
        private System.Windows.Forms.ToolStripMenuItem sumaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem liczbaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sredniaStripMenuItem1;
    }
}