using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baio.XtraBinding
{
    public abstract class XBBatchItemsAdapterDataTable : IXtraBindingBatchItemsAdapter
    {
        #region IXtraBindingBatchItemsAdapter Members

        public IEnumerable<object> GetModifyed(object DataSource)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IXtraBindingItemAdapter Members

        public object GetUnderlyingDataSourceItem(object Item)
        {
            throw new NotImplementedException();
        }

        public object GetAdaptedDataSourceItem(object Item)
        {
            throw new NotImplementedException();
        }

        public Type AdaptedItemType
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
