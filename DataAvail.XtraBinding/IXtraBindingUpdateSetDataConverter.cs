using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraBindings
{
    public interface IXtraBindingUpdateSetDataConverter
    {
        string GetPrimaryKeyFieldName(object DataSource);

        IEnumerable<string> GetFieldsNames(object DataSource);

        IEnumerable<object> GetUnderlyingObjects(object DataSource, IEnumerable<XtraBindingStoredItem> Items, bool AppendToDataSource);

        object GetValue(object UnderlyingItem, string FieldName);

        void SetValue(object UnderlyingItem, string FieldName, object Value);
    }
}
