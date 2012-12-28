using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.DXEditors
{
    partial class LikeLookUpEdit
    {
        #region InitializeComponents

        private void InitializeComponent()
        {
            this.popupContainerControl1 = new DevExpress.XtraEditors.PopupContainerControl();
            this.listBoxControl1 = new DataAvail.DXEditors.DX.ListBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).BeginInit();
            this.popupContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // popupContainerControl1
            // 
            this.popupContainerControl1.Controls.Add(this.listBoxControl1);
            this.popupContainerControl1.Location = new System.Drawing.Point(15, 26);
            this.popupContainerControl1.Name = "popupContainerControl1";
            this.popupContainerControl1.Size = new System.Drawing.Size(383, 121);
            this.popupContainerControl1.TabStop = false;
            // 
            // listBoxControl1
            // 
            this.listBoxControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxControl1.HighlightedItemStyle = DevExpress.XtraEditors.HighlightStyle.Skinned;
            this.listBoxControl1.Location = new System.Drawing.Point(0, 0);
            this.listBoxControl1.Name = "listBoxControl1";
            this.listBoxControl1.Size = new System.Drawing.Size(383, 121);
            this.listBoxControl1.TabStop = false;
            // 
            // popupContainerEdit1
            //
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "popupContainerEdit1";
            this.Properties.ShowPopupCloseButton = false;
            this.Properties.ShowPopupShadow = false;
            this.Properties.PopupResizeMode = DevExpress.XtraEditors.Controls.ResizeMode.LiveResize;
            this.Properties.PopupSizeable = false;
            this.Properties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.NoBorder;
            this.Size = new System.Drawing.Size(180, 20);
            this.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.Properties.PopupControl = popupContainerControl1;
            // 
            // GoogleLikeCombo
            // 
            this.Controls.Add(this.popupContainerControl1);
            this.Name = "GoogleLikeCombo";
            this.Size = new System.Drawing.Size(180, 19);
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).EndInit();
            this.popupContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        private DevExpress.XtraEditors.PopupContainerControl popupContainerControl1;
        private DX.ListBoxControl listBoxControl1;

        #endregion
    }
}
