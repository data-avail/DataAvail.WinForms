using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baio.DevArt.XtraBinding
{
    internal class XtraBindingModifyableObject : Baio.XtraBinding.IModifyableObject, Baio.XtraBinding.IObjectStateProvider
    {
        internal XtraBindingModifyableObject(System.Data.DataRow DataRow)
        {
            _dataRow = DataRow;
        }

        private readonly System.Data.DataRow _dataRow;

        #region IModifyableObject Members

        public void AcceptChanges()
        {
            if (_dataRow.RowState != System.Data.DataRowState.Detached)
                _dataRow.AcceptChanges();
        }

        public void RejectChanges()
        {
            if (_dataRow.RowState != System.Data.DataRowState.Detached)
                _dataRow.RejectChanges();
        }

        #endregion

        #region IObjectStateProvider Members

        public bool IsEdit
        {
            get { return _dataRow.RowState != System.Data.DataRowState.Unchanged; }
        }

        public bool IsNew
        {
            get { return _dataRow.RowState == System.Data.DataRowState.Added; }
        }

        #endregion
    }
}
