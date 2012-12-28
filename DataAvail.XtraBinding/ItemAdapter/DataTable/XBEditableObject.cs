using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraBindings.ItemAdapter.DataTable
{
    internal class XBEditableObject : System.ComponentModel.IEditableObject, DataAvail.XtraBindings.IObjectStateProvider, DataAvail.XtraBindings.IModifyableObjectProvider
    {
        internal XBEditableObject(System.Data.DataRowView DbDataRowView)
        {
            _dataRowView = DbDataRowView;

            _modifyableObject = new XbModifyableObject(DbDataRowView.Row);
        }

        private readonly System.Data.DataRowView _dataRowView;

        private readonly DataAvail.XtraBindings.IModifyableObject _modifyableObject;

        #region IEditableObject Members

        public void BeginEdit()
        {
            _dataRowView.BeginEdit();
        }

        public void CancelEdit()
        {
            _dataRowView.CancelEdit();
        }

        public void EndEdit()
        {
            _dataRowView.EndEdit();
        }

        #endregion

        #region IObjectStateProvider Members

        public bool IsEdit
        {
            get { return _dataRowView.IsNew || (_dataRowView.IsEdit && IsRealEdit); }
        }

        public bool IsNew
        {
            get { return _dataRowView.IsNew; }
        }

        #endregion

        #region IModifyableObjectProvider Members

        public DataAvail.XtraBindings.IModifyableObject ModifyableObject
        {
            get { return _modifyableObject; }
        }

        #endregion

        private bool IsRealEdit
        {
            get
            {
                System.Data.DataRowVersion rowVersion = !_dataRowView.Row.HasVersion(System.Data.DataRowVersion.Current) ? System.Data.DataRowVersion.Original : System.Data.DataRowVersion.Current;

                foreach (System.Data.DataColumn dataColumn in _dataRowView.Row.Table.Columns)
                {
                    if (!_dataRowView.Row[dataColumn, rowVersion].Equals(_dataRowView[dataColumn.ColumnName])) return true;
                }

                return false;
            }
        }
    }
}
