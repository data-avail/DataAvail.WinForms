using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baio.DevArt.XtraBinding
{
    public class XtraBindingItemAdapter : Baio.XtraBinding.IXtraBindingBatchItemsAdapter
    {
        private readonly Baio.XtraBinding.XtraBindingUpdateSetDataConverter _dataConverter = new Baio.XtraBinding.XtraBindingUpdateSetDataConverter();

        #region IXtraBindingItemAdapter Members

        public object GetAdaptedDataSourceItem(object Item)
        {
            return new XtraBindingEditableObject((System.Data.DataRowView)Item);
        }

        public object GetUnderlyingDataSourceItem(object Item)
        {
            return ((System.Data.DataRowView)Item).Row;
        }

        public Type AdaptedItemType { get { return typeof(XtraBindingEditableObject); } }

        public System.ComponentModel.IBindingListView GetBindingListView(object DataSource)
        {
            return new System.Data.DataView(((System.Data.DataTable)DataSource));
        }

        public object Clone(object DataSource)
        {
            return ((System.Data.DataTable)DataSource).Clone();
        }


        #endregion

        #region IXtraBindingBatchItemsAdapter Members

        public IEnumerable<object> GetModifyed(object DataSource)
        {
            return ((System.Data.DataTable)DataSource).Select(null, null, System.Data.DataViewRowState.Added | System.Data.DataViewRowState.Deleted | System.Data.DataViewRowState.ModifiedCurrent);
        }


        public void AcceptChanges(object DataSource)
        {
            ((System.Data.DataTable)DataSource).AcceptChanges();
        }

        public void RejectChanges(object DataSource)
        {
            ((System.Data.DataTable)DataSource).RejectChanges();
        }


        public Baio.XtraBinding.IXtraBindingUpdateSetDataConverter DataConverter
        {
            get { return _dataConverter; }
        }

        #endregion

    }
}
