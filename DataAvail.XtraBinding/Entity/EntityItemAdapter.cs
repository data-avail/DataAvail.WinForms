using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baio.XtraBinding.Entity
{
    public class EntityItemAdapter<T> : IXtraBindingItemAdapter where T : class, new()
    {
        #region IXtraBindingItemAdapter Members

        public object GetAdaptedDataSourceItem(object Item)
        {
            return new EditableObject<T>((T)Item);
        }

        public Type AdaptedItemType
        {
            get { return typeof(EditableObject<T>); }
        }

        public object GetUnderlyingDataSourceItem(object Item)
        {
            return Item;
        }


        public Baio.XtraBinding.IXtraBindingUpdateSetDataConverter DataConverter
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
