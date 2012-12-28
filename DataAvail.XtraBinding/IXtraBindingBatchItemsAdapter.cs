using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraBindings
{
    public interface IXtraBindingBatchItemsAdapter : IXtraBindingItemAdapter
    {
        IEnumerable<object> GetModifyed(object DataSource);

        void AcceptChanges(object DataSource);

        void RejectChanges(object DataSource);
    }
}
