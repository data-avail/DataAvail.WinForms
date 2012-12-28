using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Data
{
    public class DataSet 
    {        
        #region static 

        public static System.IO.TextWriter Log;

        public static System.Data.DataRelation AddRelation(System.Data.DataColumn ParentColumn, System.Data.DataColumn ChildColumn, string Filter)
        {
            System.Data.DataRelation dataRel = new System.Data.DataRelation(string.Format("{0}_{1}_{2}_{3}", ParentColumn.ColumnName, ParentColumn.Table.TableName, ChildColumn.ColumnName, ChildColumn.Table.TableName),
                ParentColumn, ChildColumn, false);

            dataRel.ExtendedProperties.Add(DataRelation.RealtionFilterExtendedPropertyName, Filter);

            ParentColumn.Table.DataSet.Relations.Add(dataRel);

            return dataRel;
        }

        public static System.Data.DataRelation GetRelation(System.Data.DataColumn ParentColumn, System.Data.DataColumn ChildColumn)
        {
            return (from s in ParentColumn.Table.DataSet.Relations.Cast<System.Data.DataRelation>()
                    where s.ParentColumns[0] == ParentColumn && s.ChildColumns[0] == ChildColumn
                    select s).SingleOrDefault();

        }

        #endregion


    }
}
