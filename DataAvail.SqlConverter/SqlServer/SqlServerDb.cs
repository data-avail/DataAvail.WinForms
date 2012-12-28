using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DataAvail.SqlConverter.SqlServer
{
    public class SqlServerDb : Db
    {
        public SqlServerDb(string ConnectionString)
            : base(ConnectionString)
        { }

        protected override IDbConnection CreateConnection(string ConnectionString)
        {
            return new SqlConnection(ConnectionString);
        }

        protected override IDbCommand CreateSelectTablesCommand(IDbConnection Connection)
        {
            return new SqlCommand(@"select * from INFORMATION_SCHEMA.TABLES  where TABLE_TYPE = 'BASE TABLE'", (SqlConnection)Connection);
        }

        protected override ITable CrateTable(IDataReader DataReader)
        {
            return new SqlServerTable(this, DataReader);
        }
    }
}
