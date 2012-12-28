using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraBindings
{
    public interface IXtraBindingItemAdapter
    {
        object GetUnderlyingDataSourceItem(object Item);

        object GetAdaptedDataSourceItem(object Item);

        Type AdaptedItemType { get; }

        IXtraBindingUpdateSetDataConverter DataConverter { get; }

        System.ComponentModel.IBindingListView GetBindingListView(object DataSource);

        object Clone(object DataSource);
    }
}
