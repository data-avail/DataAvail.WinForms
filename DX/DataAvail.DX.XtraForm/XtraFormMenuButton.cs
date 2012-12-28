using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.DX.XtraForm
{
    public class XtraFormMenuButton : DataAvail.XtraMenu.IXtraMenuButton
    {
        public XtraFormMenuButton(DevExpress.XtraBars.BarButtonItem BarButtonItem, DataAvail.XtraMenu.XtraMenuButtonType ButtonType)
        {
            _barButtonItem = BarButtonItem;

            _buttonType = ButtonType;

            BarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(BarButtonItem_ItemClick);
        }

        private readonly DevExpress.XtraBars.BarButtonItem _barButtonItem;

        private readonly DataAvail.XtraMenu.XtraMenuButtonType _buttonType;

        void BarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (ButtonClick != null)
                ButtonClick(this, EventArgs.Empty);
        }

        #region IXtraFormMenuButton Members

        public DataAvail.XtraMenu.XtraMenuButtonType ButtonType
        {
            get { return _buttonType; }
        }

        public event EventHandler ButtonClick;

        public bool Visible
        {
            get
            {
                return _barButtonItem.Visibility == DevExpress.XtraBars.BarItemVisibility.Always;
            }

            set
            {
                _barButtonItem.Visibility = value ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
            }
        }

        public bool Enabled
        {
            get
            {
                return _barButtonItem.Enabled;
            }

            set
            {
                _barButtonItem.Enabled = value;
            }
        }

        #endregion
    }
}
