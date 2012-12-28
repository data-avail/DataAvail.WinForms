using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DataAvail.SqlConverter.SqlServer
{
    public class SqlServerTable : Table
    {
        public SqlServerTable(IDb Db, IDataReader DataReader)
            : base(Db, DataReader)
        { 
        }

        protected override string ReaderSchemeNameField
        {
            get { return "TABLE_SCHEMA"; }
        }

        protected override string ReaderNameFieldName
        {
            get { return "TABLE_NAME"; }
        }

        protected override IDbCommand CreateSelectColumnsCommand(IDbConnection DbConnection)
        {
            /*
            return new SqlCommand(
               string.Format(
               "SELECT C.COLUMN_NAME,COLUMN_DEFAULT,IS_NULLABLE,DATA_TYPE, (columnproperty(object_id(C.TABLE_NAME), C.COLUMN_NAME, 'IsIdentity')) AS [IDENT], "+ 
                "CHARACTER_MAXIMUM_LENGTH AS CSIZE,CASE KCU.COLUMN_NAME WHEN C.COLUMN_NAME THEN 1 ELSE 0 END AS IS_PK "+
                "FROM INFORMATION_SCHEMA.COLUMNS C "+
                "JOIN information_schema.key_column_usage KCU ON KCU.TABLE_NAME = C.TABLE_NAME "+
                "LEFT JOIN information_schema.table_constraints TC ON KCU.CONSTRAINT_NAME = TC.CONSTRAINT_NAME AND TC.CONSTRAINT_TYPE = 'PRIMARY KEY' "+
                "WHERE C.TABLE_NAME = '{0}' "+ 
                "ORDER BY C.ORDINAL_POSITION ASC", Name), (SqlConnection)DbConnection);
             */
            return new SqlCommand(string.Format("SELECT  " +
                "C.COLUMN_NAME, " +
                "C.COLUMN_DEFAULT, " +
                "C.IS_NULLABLE, " +
                "C.DATA_TYPE,  " +
                "(columnproperty(object_id(C.TABLE_NAME), C.COLUMN_NAME, 'IsIdentity')) AS [IDENT],  " +
                "C.CHARACTER_MAXIMUM_LENGTH AS CSIZE, " +
                "CASE C.COLUMN_NAME WHEN S.COLUMN_NAME THEN 1 ELSE 0 END AS IS_PK " +
                "FROM INFORMATION_SCHEMA.COLUMNS C, " +
                "(SELECT B.COLUMN_NAME " +
                "FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS A, INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE B " +
                "WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND A.CONSTRAINT_NAME = B.CONSTRAINT_NAME AND A.TABLE_NAME = '{0}') S " +
                "WHERE C.TABLE_NAME = '{0}' " +
                "ORDER BY C.ORDINAL_POSITION ASC", Name), (SqlConnection)DbConnection);

        }

        protected override IDbCommand CreateSelectFkRelationsCommand(IDbConnection DbConnection)
        {
            return new SqlCommand(string.Format(
               @"SELECT " +
               @"  ColumnName = CU.COLUMN_NAME, " +
               @"  ForeignTableName  = PK.TABLE_NAME, " +
               @"  ForeignColumnName = PT.COLUMN_NAME, " +
               @"  DeleteRule = C.DELETE_RULE, " +
               @"  IsNullable = COL.IS_NULLABLE " +
               @"FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS C " +
               @"INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS FK ON C.CONSTRAINT_NAME = FK.CONSTRAINT_NAME " +
               @"INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS PK ON C.UNIQUE_CONSTRAINT_NAME = PK.CONSTRAINT_NAME " +
               @"INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CU ON C.CONSTRAINT_NAME = CU.CONSTRAINT_NAME " +
               @"INNER JOIN " +
               @"  ( " +
               @"    SELECT i1.TABLE_NAME, i2.COLUMN_NAME " +
               @"    FROM  INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1 " +
               @"    INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME " +
               @"    WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY' " +
               @"  ) " +
               @"PT ON PT.TABLE_NAME = PK.TABLE_NAME " +
               @"INNER JOIN INFORMATION_SCHEMA.COLUMNS AS COL ON CU.COLUMN_NAME = COL.COLUMN_NAME AND FK.TABLE_NAME = COL.TABLE_NAME " +
               @"WHERE FK.Table_NAME='{0}'", Name), (SqlConnection)DbConnection);
        }

        protected override IColumn CreateColumn(IDataReader DataReader)
        {
            return new SqlServerColumn(this, DataReader);
        }

        protected override IRelation CreateRelation(IDataReader DataReader)
        {
            return new SqlServerRelation(this, DataReader);
        }

        public override bool IsSysTable
        {
            get { return Name == "sysdiagrams"; }
        }
    }
}
