using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraBindings
{
    public class XtraBindingStoredItem
    {
        private IDictionary<string, object> _fieldsValues;

        private XtraBindingStoredItemState _state;

        private object _dataSourceItem;

        public void Update(IDictionary<string, object> FieldsValues, XtraBindingStoredItemState State)
        {
            _fieldsValues = FieldsValues;

            _state = State;
        }

        public IDictionary<string, object> FieldsValues
        {
            get
            {
                return _fieldsValues;
            }
        }

        public XtraBindingStoredItemState State
        {
            get
            {
                return _state;
            }
        }

        public object DataSourceItem
        {
            get
            {
                return _dataSourceItem;
            }
        }

        internal void UpdateDataSourceItem(object DataSourceItem)
        {
            _dataSourceItem = DataSourceItem;
        }

    }

    public enum XtraBindingStoredItemState
    { 
        Modifyed,

        Added,

        Removed
    }
}
