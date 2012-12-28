using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAvail.SqlConverter.SqlServer
{
    public class SqlServerRelation : Relation
    {
        public SqlServerRelation(ITable Table, IDataReader DataReader)
            : base(Table, DataReader)
        { 
        
        }

        protected override string ReaderChildColumnField
        {
            get { return "ForeignColumnName"; }
        }

        protected override string ReaderChildTableField
        {
            get { return "ForeignTableName"; }
        }

        protected override string ReaderParentColumnField
        {
            get { return "ColumnName"; }
        }

        protected override string ReaderParentTableField
        {
            get { return "TableName"; }
        }
    }
}
