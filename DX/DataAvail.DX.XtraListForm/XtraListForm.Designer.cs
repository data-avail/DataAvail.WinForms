namespace DataAvail.DX.XtraListForm
{
    partial class XtraListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XtraListForm));
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.updateBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.rejectBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.refillBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.uploadToExcelBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barEditItem1 = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemMarqueeProgressBar1 = new DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.xtraGrid1 = CreateXtraGrid();
            this.xtraSearchContainer1 = CreateXtraSearchContainer(); 
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMarqueeProgressBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2,
            this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.updateBarButtonItem,
            this.rejectBarButtonItem,
            this.refillBarButtonItem,
            this.uploadToExcelBarButtonItem,
            this.barButtonItem1,
            this.barEditItem1});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 4;
            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMarqueeProgressBar1});
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.updateBarButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.rejectBarButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.refillBarButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.uploadToExcelBarButtonItem)
            });
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barEditItem1)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Stop fill";
            this.barButtonItem1.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem1.Glyph")));
            this.barButtonItem1.Id = 2;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.Hint = "Click button to stop data filling";
            this.barButtonItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barButtonItem1.Enabled = true;
            this.barButtonItem1.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            // 
            // barEditItem1
            // 
            this.barEditItem1.Caption = "fillMarqueeBarEditItem";
            this.barEditItem1.Edit = this.repositoryItemMarqueeProgressBar1;
            this.barEditItem1.Id = 3;
            this.barEditItem1.Name = "barEditItem1";
            this.barEditItem1.Width = 75;
            this.barEditItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // repositoryItemMarqueeProgressBar1
            // 
            this.repositoryItemMarqueeProgressBar1.Name = "repositoryItemMarqueeProgressBar1";
            this.repositoryItemMarqueeProgressBar1.StartColor = System.Drawing.Color.LightBlue;
            this.repositoryItemMarqueeProgressBar1.EndColor = System.Drawing.Color.DarkBlue;
            this.repositoryItemMarqueeProgressBar1.ProgressViewStyle = DevExpress.XtraEditors.Controls.ProgressViewStyle.Broken;
            this.repositoryItemMarqueeProgressBar1.ProgressAnimationMode = DevExpress.Utils.Drawing.ProgressAnimationMode.PingPong;

            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 26);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.xtraSearchContainer1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.xtraGrid1);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(649, 397);
            this.splitContainerControl1.SplitterPosition = 65;
            this.splitContainerControl1.TabIndex = 4;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // xtraSearchContainer1
            // 
            this.xtraSearchContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraSearchContainer1.Location = new System.Drawing.Point(0, 0);
            this.xtraSearchContainer1.Name = "xtraSearchContainer1";
            this.xtraSearchContainer1.Size = new System.Drawing.Size(649, 65);
            this.xtraSearchContainer1.TabIndex = 0;
            // 
            // xtraGrid1
            // 
            this.xtraGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraGrid1.Location = new System.Drawing.Point(0, 0);
            this.xtraGrid1.Name = "xtraGrid1";
            this.xtraGrid1.Size = new System.Drawing.Size(649, 326);
            this.xtraGrid1.TabIndex = 0;
            // 
            // updateBarButtonItem
            // 
            this.updateBarButtonItem.Caption = "Update";
            this.updateBarButtonItem.Glyph = ((System.Drawing.Image)(resources.GetObject("accept16")));
            this.updateBarButtonItem.Id = 0;
            this.updateBarButtonItem.Name = "updateBarButtonItem";
            // 
            // rejectBarButtonItem
            // 
            this.rejectBarButtonItem.Caption = "Reject";
            this.rejectBarButtonItem.Glyph = ((System.Drawing.Image)(resources.GetObject("rejectBarButtonItem.Glyph")));
            this.rejectBarButtonItem.Id = 1;
            this.rejectBarButtonItem.Name = "rejectBarButtonItem";
            // 
            // refillBarButtonItem
            // 
            this.refillBarButtonItem.Caption = "Refill";
            this.refillBarButtonItem.Glyph = global::DataAvail.DX.XtraListForm.Properties.Resources.refresh_16;
            this.refillBarButtonItem.Id = 2;
            this.refillBarButtonItem.Name = "refillBarButtonItem";
            // 
            // uploadToExcelBarButtonItem
            // 
            this.uploadToExcelBarButtonItem.Caption = "UploadToExcel";
            this.uploadToExcelBarButtonItem.Glyph = global::DataAvail.DX.XtraListForm.Properties.Resources.excel_16;
            this.uploadToExcelBarButtonItem.Id = 3;
            this.uploadToExcelBarButtonItem.Name = "uploadToExcelBarButtonItem";

            // 
            // XtraListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 449);
            this.Controls.Add(this.splitContainerControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "XtraListForm";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMarqueeProgressBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DataAvail.XtraContainer.XtraContainer xtraSearchContainer1;
        private DataAvail.XtraGrid.XtraGrid xtraGrid1;
        private DevExpress.XtraBars.BarButtonItem updateBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem rejectBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem refillBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem uploadToExcelBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarEditItem barEditItem1;
        private DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar repositoryItemMarqueeProgressBar1;
    }
}