using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace DataAvail.DXEditors
{
    public partial class XtraEditContainer : 
        DevExpress.XtraEditors.XtraUserControl,
        DevExpress.Utils.Controls.IXtraResizableControl, 
        DevExpress.XtraEditors.ISupportStyleController
    {
        public XtraEditContainer()
        {
            InitializeComponent();            
        }

        private DevExpress.XtraEditors.BaseEdit _baseEdit;

        private IStyleController _styleController;

        public DevExpress.XtraEditors.BaseEdit BaseEdit
        {
            get { return _baseEdit; }

            set
            {
                if (_baseEdit != value)
                {
                    OnBaseEditChanging();

                    _baseEdit = value;

                    OnBaseEditChanged();
                }
            }
        }

        protected virtual void OnBaseEditChanging()
        {
            if (BaseEdit != null)
            {
                BaseEdit.Resize -= new EventHandler(BaseEdit_Resize);

                this.Resize -= new EventHandler(XtraEditContainer_Resize);
            }
        }



        protected virtual void OnBaseEditChanged()
        {
            if (BaseEdit != null)
            {
                this.Controls.Add(BaseEdit);
         
                BaseEdit.Dock = DockStyle.Fill;

                BaseEdit.StyleController = StyleController;

                BaseEdit.Resize += new EventHandler(BaseEdit_Resize);

                this.Resize += new EventHandler(XtraEditContainer_Resize);
            }
        }


        void BaseEdit_Resize(object sender, EventArgs e)
        {
            DoResize();
        }

        void XtraEditContainer_Resize(object sender, EventArgs e)
        {
            DoResize();
        }

        private void DoResize()
        {

            if (this.Size.Height != this.BaseEdit.Size.Height)
            {
                this.Height = BaseEdit.Height;

                if (Changed != null)
                    Changed(this, EventArgs.Empty);

                this.ResumeLayout();
            }
        }

        public event EventHandler Changed;

        public bool IsCaptionVisible
        {
            get { return true; }
        }

        public Size MaxSize
        {
            get { return ((DevExpress.Utils.Controls.IXtraResizableControl)this.BaseEdit).MaxSize; }
        }

        public Size MinSize
        {
            get { return ((DevExpress.Utils.Controls.IXtraResizableControl)this.BaseEdit).MinSize; }
        }

        public DevExpress.XtraEditors.IStyleController StyleController
        {
            get
            {
                return _styleController;
            }
            set
            {
                if (_styleController != value)
                {
                    _styleController = value;

                    OnStyleControllerChanged();
                }
            }
        }

        protected virtual void OnStyleControllerChanged()
        {
            if (BaseEdit != null)
            {
                BaseEdit.StyleController = StyleController;
            }
        }
    }
}
