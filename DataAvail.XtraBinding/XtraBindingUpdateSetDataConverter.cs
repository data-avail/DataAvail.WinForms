using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraBindings
{
    public class XtraBindingUpdateSetDataConverter : IXtraBindingUpdateSetDataConverter
    {
        #region IXtraBindingUpdateSetDataConverter Members

        public string GetPrimaryKeyFieldName(object DataSource)
        {
            return ((System.Data.DataTable)DataSource).PrimaryKey[0].ColumnName;
        }

        public IEnumerable<string> GetFieldsNames(object DataSource)
        {
            return ((System.Data.DataTable)DataSource).Columns.Cast<System.Data.DataColumn>().Select(p => p.ColumnName);
        }

        public IEnumerable<object> GetUnderlyingObjects(object DataSource, IEnumerable<XtraBindingStoredItem> Items, bool UsePassedDataSource)
        {
            System.Data.DataTable dataTable = UsePassedDataSource ? (System.Data.DataTable)DataSource : ((System.Data.DataTable)DataSource).Clone();

            if (Items != null)
            {
                foreach (XtraBindingStoredItem item in Items)
                {
                    System.Data.DataRow dataRow = dataTable.Rows.Add(item.FieldsValues.Values.ToArray());

                    dataRow.AcceptChanges();

                    if (item.State == XtraBindingStoredItemState.Added)
                    {
                        dataRow.SetAdded();
                    }
                    else if (item.State == XtraBindingStoredItemState.Modifyed)
                    {
                        dataRow.SetModified();
                    }
                    else if (item.State == XtraBindingStoredItemState.Removed)
                    {
                        dataRow.Delete();
                    }
                }
            }

            return dataTable.Rows.Cast<object>();
        }

        public object GetValue(object UnderlyingItem, string FieldName)
        {
            return ((System.Data.DataRow)UnderlyingItem)[FieldName];
        }

        public void SetValue(object UnderlyingItem, string FieldName, object Value)
        {
            ((System.Data.DataRow)UnderlyingItem)[FieldName] = Value;
        }

        #endregion
    }
}
