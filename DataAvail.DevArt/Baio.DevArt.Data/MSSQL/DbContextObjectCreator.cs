using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devart.Data.SqlServer;

namespace DataAvail.DevArt.Data.MSSQL
{
    public class DbContextObjectCreator : DataAvail.Data.DbContext.IDbContextObjectCreator
    {
        private readonly System.Data.IDbConnection _connection = new SqlConnection();

        #region IDbContextObjectCreator Members

        public System.Data.IDbConnection Connection
        {
            get
            {
                return _connection;
            }
        }

        public System.Data.DataSet CreateDataSet()
        {
            return new SqlDataSet();
        }

        public System.Data.DataTable CreateDataTable(System.Data.IDbCommand DbCommand)
        {
            return new SqlDataTable() { SelectCommand = (Devart.Data.SqlServer.SqlCommand)DbCommand };
        }

        public DataAvail.Data.Function.Function CreateCommand()
        {
            return new Function();
        }

        #endregion
    }
}
