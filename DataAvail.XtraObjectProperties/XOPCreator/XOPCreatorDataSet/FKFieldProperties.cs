using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraObjectProperties.XOPCreator.XOPCreatorDataSet
{
    public class FKFieldProperties : DataAvail.XtraObjectProperties.XOPCreator.FKFieldPropertiesBase
    {
        internal FKFieldProperties(XtraFieldProperties XtraFieldProperties, System.Data.DataRelation DataRelation):
            base(XtraFieldProperties)
        {
            base.ValueMember = DataRelation.ParentColumns[0].ColumnName;

            System.Data.DataColumn displayCol = DataRelation.ParentTable.Columns.Cast<System.Data.DataColumn>().FirstOrDefault(p => p.DataType == typeof(string));

            base.DisplayMember = displayCol != null ? displayCol.ColumnName : base.ValueMember;

            string filter = DataAvail.Data.DataRelation.GetFilter(DataRelation);

            this.SetDataSource(new System.Data.DataView(DataRelation.ParentTable,filter, this.DisplayMember, System.Data.DataViewRowState.CurrentRows), filter);
        }
    }
}
