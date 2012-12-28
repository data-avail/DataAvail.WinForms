using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraContainerBuilder
{
    internal class XtraContainerDataTableParser : XtraContainerBaseParser<System.Data.DataColumn> 
    {
        internal XtraContainerDataTableParser(System.Data.DataTable DataTable, DataAvail.XtraObjectProperties.IXtraObjectPropertiesProvider XtraObjectPropertiesProvider)
            : base(XtraObjectPropertiesProvider, DataTable.Columns.GetEnumerator())
        {
        }

        protected override XtraContainerFieldProperties GetFieldProperties(System.Data.DataColumn Item)
        {
            return new XtraContainerFieldProperties(Item.ColumnName, Item.DataType, XtraObjectPropertiesProvider != null ? XtraObjectPropertiesProvider.GetFieldProperties(Item.ColumnName) : null);
        }

    }
}
