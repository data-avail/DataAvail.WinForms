namespace DataAvail.XtraReportForm
{
    partial class XtraReportForm
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
            this.xtraContainer1 = CreateXtraContainer(); 
            this.SuspendLayout();
            // 
            // xtraContainer1
            // 
            this.xtraContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraContainer1.Location = new System.Drawing.Point(0, 24);
            this.xtraContainer1.Name = "xtraContainer1";
            this.xtraContainer1.Size = new System.Drawing.Size(497, 349);
            this.xtraContainer1.TabIndex = 0;
            // 
            // XtraForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 373);
            this.Controls.Add(this.xtraContainer1);
            this.Name = "XtraForm";
            this.Text = "XtraForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataAvail.XtraContainer.XtraContainer xtraContainer1;
    }
}