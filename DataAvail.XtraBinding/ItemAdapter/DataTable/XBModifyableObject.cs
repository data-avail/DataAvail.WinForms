using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraBindings.ItemAdapter.DataTable
{
    internal class XbModifyableObject : DataAvail.XtraBindings.IModifyableObject, DataAvail.XtraBindings.IObjectStateProvider
    {
        internal XbModifyableObject(System.Data.DataRow DataRow)
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
