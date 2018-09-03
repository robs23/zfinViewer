namespace zfinViewer
{
    partial class frmToast
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
            this.components = new System.ComponentModel.Container();
            this.lblToast = new System.Windows.Forms.Label();
            this.toastTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // lblToast
            // 
            this.lblToast.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblToast.AutoSize = true;
            this.lblToast.Location = new System.Drawing.Point(12, 10);
            this.lblToast.Name = "lblToast";
            this.lblToast.Size = new System.Drawing.Size(35, 13);
            this.lblToast.TabIndex = 0;
            this.lblToast.Text = "label1";
            this.lblToast.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // toastTimer
            // 
            this.toastTimer.Enabled = true;
            this.toastTimer.Interval = 3000;
            this.toastTimer.Tick += new System.EventHandler(this.closeMe);
            // 
            // frmToast
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.ClientSize = new System.Drawing.Size(62, 32);
            this.ControlBox = false;
            this.Controls.Add(this.lblToast);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmToast";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "frmToast";
            this.Load += new System.EventHandler(this.loadMe);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblToast;
        private System.Windows.Forms.Timer toastTimer;
    }
}