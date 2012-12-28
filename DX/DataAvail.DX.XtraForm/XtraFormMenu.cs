using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.Linq;

namespace DataAvail.DX.XtraForm
{
    public class XtraFormMenu : DataAvail.XtraMenu.IXtraMenu
    {
        public XtraFormMenu(DevExpress.XtraBars.BarButtonItem [] BarButtonItems, DataAvail.XtraMenu.XtraMenuButtonType [] Types)
        {
            _buttons = BarButtonItems.Zip(Types, (p, s) => new XtraFormMenuButton(p, s));
        }

        private readonly IEnumerable<XtraFormMenuButton> _buttons;

        #region IXtraMenu Members

        public DataAvail.XtraMenu.IXtraMenuButton this[DataAvail.XtraMenu.XtraMenuButtonType ButtonType]
        {
            get 
            {
                return _buttons.SingleOrDefault(p => p.ButtonType == ButtonType);
            }
        }

        #endregion

        #region IEnumerable<IXtraMenuButton> Members

        public IEnumerator<DataAvail.XtraMenu.IXtraMenuButton> GetEnumerator()
        {
            return _buttons.Cast < DataAvail.XtraMenu.IXtraMenuButton>().GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _buttons.GetEnumerator();
        }

        #endregion
    }
}
