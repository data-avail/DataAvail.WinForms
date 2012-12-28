namespace DataAvail.DX.XtraReportForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XtraReportForm));
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.saveChangesBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.rejectChangesBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.endEditBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.cancelEditBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.movePervBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.moveNextBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.cloneBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
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
            this.saveChangesBarButtonItem,
            this.rejectChangesBarButtonItem,
            this.endEditBarButtonItem,
            this.cancelEditBarButtonItem,
            this.movePervBarButtonItem,
            this.moveNextBarButtonItem,
            this.cloneBarButtonItem});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 7;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.saveChangesBarButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.rejectChangesBarButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.endEditBarButtonItem, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cancelEditBarButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.movePervBarButtonItem, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.moveNextBarButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.cloneBarButtonItem, true)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // saveChangesBarButtonItem
            // 
            this.saveChangesBarButtonItem.Caption = "Save";
            this.saveChangesBarButtonItem.Glyph = global::DataAvail.DX.XtraReportForm.Properties.Resources.filesave;
            this.saveChangesBarButtonItem.Id = 0;
            this.saveChangesBarButtonItem.Name = "saveChangesBarButtonItem";
            // 
            // rejectChangesBarButtonItem
            // 
            this.rejectChangesBarButtonItem.Caption = "Reject";
            this.rejectChangesBarButtonItem.Glyph = global::DataAvail.DX.XtraReportForm.Properties.Resources.reject;
            this.rejectChangesBarButtonItem.Id = 1;
            this.rejectChangesBarButtonItem.Name = "rejectChangesBarButtonItem";
            // 
            // endEditBarButtonItem
            // 
            this.endEditBarButtonItem.Caption = "End edit";
            this.endEditBarButtonItem.Glyph = global::DataAvail.DX.XtraReportForm.Properties.Resources.apply;
            this.endEditBarButtonItem.Id = 2;
            this.endEditBarButtonItem.Name = "endEditBarButtonItem";
            // 
            // cancelEditBarButtonItem
            // 
            this.cancelEditBarButtonItem.Caption = "Cancel edit";
            this.cancelEditBarButtonItem.Glyph = global::DataAvail.DX.XtraReportForm.Properties.Resources.cancel;
            this.cancelEditBarButtonItem.Id = 3;
            this.cancelEditBarButtonItem.Name = "cancelEditBarButtonItem";
            // 
            // movePervBarButtonItem
            // 
            this.movePervBarButtonItem.Caption = "movePervBarButtonItem";
            this.movePervBarButtonItem.Glyph = ((System.Drawing.Image)(resources.GetObject("movePervBarButtonItem.Glyph")));
            this.movePervBarButtonItem.Id = 4;
            this.movePervBarButtonItem.Name = "movePervBarButtonItem";
            // 
            // moveNextBarButtonItem
            // 
            this.moveNextBarButtonItem.Caption = "moveNextBarButtonItem";
            this.moveNextBarButtonItem.Glyph = ((System.Drawing.Image)(resources.GetObject("moveNextBarButtonItem.Glyph")));
            this.moveNextBarButtonItem.Id = 5;
            this.moveNextBarButtonItem.Name = "moveNextBarButtonItem";
            // 
            // cloneBarButtonItem
            // 
            this.cloneBarButtonItem.Caption = "cloneBarButtonItem";
            this.cloneBarButtonItem.Glyph = ((System.Drawing.Image)(resources.GetObject("cloneBarButtonItem.Glyph")));
            this.cloneBarButtonItem.Id = 6;
            this.cloneBarButtonItem.Name = "cloneBarButtonItem";
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(746, 34);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 566);
            this.barDockControlBottom.Size = new System.Drawing.Size(746, 22);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 34);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 532);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(746, 34);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 532);
            // 
            // XtraReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 588);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "XtraReportForm";
            this.Text = "XtraReportForm";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
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
        private DevExpress.XtraBars.BarButtonItem saveChangesBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem rejectChangesBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem endEditBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem cancelEditBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem movePervBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem moveNextBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem cloneBarButtonItem;
    }
}