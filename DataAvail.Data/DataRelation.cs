using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Data
{
    public static class DataRelation
    {
        public static System.Data.DataRelation GetDataRelation(System.Data.DataColumn FKColumn)
        {
            return FKColumn.Table.ParentRelations.Cast<System.Data.DataRelation>().FirstOrDefault(p => p.ChildColumns[0] == FKColumn);
        }

        public static string GetFilter(System.Data.DataRelation DataRelation)
        {
            return (string)DataRelation.ExtendedProperties[RealtionFilterExtendedPropertyName];
        }

        public const string RealtionFilterExtendedPropertyName = "Filter";
    }
}
