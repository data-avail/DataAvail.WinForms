using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraObjectProperties.XOPCreator.XOPCreatorDataSet
{
    public class XOPCreatorProviderDataSet : DataAvail.XtraObjectProperties.XOPCreator.XOPCreatorProviderBase<System.Data.DataTable, System.Data.DataColumn>
    {
        public XOPCreatorProviderDataSet(System.Data.DataSet DataSet)
            : base(DataSet.Tables.Cast<System.Data.DataTable>())
        {
        }

        protected override IEnumerable<System.Data.DataColumn> GetFieldItems(System.Data.DataTable ObjectItem)
        {
            return ObjectItem.Columns.Cast<System.Data.DataColumn>();
        }

        protected override XtraObjectProperties GetObjectProperties(System.Data.DataTable ObjectItem)
        {
            return new DataAvail.XtraObjectProperties.XtraObjectProperties(ObjectItem.TableName, ObjectItem.NewRow().GetType());
        }

        protected override XtraFieldProperties GetFieldPropertiesCore(System.Data.DataColumn DataColumn)
        {
            return new DataAvail.XtraObjectProperties.XtraFieldProperties(CurrentXtraObject, DataColumn.ColumnName, DataColumn.DataType)
            {
                AllowUserNull = DataColumn.AllowDBNull,
                AllowUserEdit = !DataColumn.ReadOnly && !DataColumn.Table.PrimaryKey.Contains(DataColumn),
                Caption = DataColumn.Caption
            };
        }

        protected override XtraTextFieldProperties GetTextFieldProperties(XtraFieldProperties XtraFieldProperties, System.Data.DataColumn DataColumn)
        {
            if (XtraFieldProperties.FieldType == typeof(string))
            {
                return new DataAvail.XtraObjectProperties.XtraTextFieldProperties(XtraFieldProperties)
                {
                    MaxLength = DataColumn.MaxLength
                };
            }
            else
            {
                return null;
            }
        }

        protected override IFKFieldProperties GetFKFieldProperties(XtraFieldProperties XtraFieldProperties, System.Data.DataColumn DataColumn)
        {
            System.Data.DataRelation dataRel = DataColumn.Table.ParentRelations.Cast<System.Data.DataRelation>().FirstOrDefault(p => p.ChildTable == DataColumn.Table && p.ChildColumns.Length == 1 && p.ChildColumns[0] == DataColumn);

            if (dataRel != null)
            {
                return new FKFieldProperties(XtraFieldProperties, dataRel);
            }
            else
            {
                return null;
            }
        }

        public override XtraFieldProperties CreateFkField(XtraFieldProperties XtraFieldProperties, FKFieldDescriptor FKFieldDescriptor)
        {
            if (FKFieldDescriptor.GetType() == typeof(FKFieldDescriptorTable))
            {
                FKFieldDescriptorTable descr = (FKFieldDescriptorTable)FKFieldDescriptor;

                System.Data.DataSet dataSet = ObjectItems.First().DataSet;

                System.Data.DataColumn parCol = dataSet.Tables[descr.parentTable].Columns[descr.parentColumn];

                System.Data.DataColumn childCol = dataSet.Tables[descr.childTable].Columns[descr.childColumn];

                System.Data.DataRelation dRel = DataAvail.Data.DataSet.GetRelation(parCol, childCol);

                if (dRel != null)
                {
                    dataSet.Relations.Remove(dRel);
                }

                return new FKFieldProperties(XtraFieldProperties, DataAvail.Data.DataSet.AddRelation(parCol, childCol, FKFieldDescriptor.Filter));
            }
            else
            {
                return base.CreateFkField(XtraFieldProperties, FKFieldDescriptor);
            }
        }

        public override void FinalizeCreation(IEnumerable<XtraObjectProperties> XtraObjects)
        {
            foreach (var i in from t in ObjectItems
                              join o in XtraObjects on t.TableName equals o.ObjectName
                              select new { dt = t, obj = o })
            {
                foreach (var k in from c in i.dt.Columns.Cast<System.Data.DataColumn>()
                                  join f in i.obj.Fields.Where(p=>p.DefaultValue != null) on c.ColumnName equals f.FieldName
                                  select new { dc = c, fld = f })
                {
                    k.dc.DefaultValue = k.fld.DefaultValue;
                }
            }
        }
    }
}
