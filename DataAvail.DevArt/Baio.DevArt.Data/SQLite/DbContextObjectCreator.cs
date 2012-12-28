using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devart.Data.SQLite;

namespace DataAvail.DevArt.Data.SQLite
{
    public class DbContextObjectCreator : DataAvail.Data.DbContext.IDbContextObjectCreator
    {
        private readonly System.Data.IDbConnection _connection = new SQLiteConnection();

        #region IDbContextObjectCreator Members

        public System.Data.IDbConnection Connection
        {
            get
            {
                return _connection;
            }
        }
        public System.Data.IDbConnection CreateConnection()
        {
            return new SQLiteConnection();
        }

        public System.Data.DataSet CreateDataSet()
        {
            return new SQLiteDataSet();
        }

        public System.Data.DataTable CreateDataTable(System.Data.IDbCommand DbCommand)
        {
            return new SQLiteDataTable() { SelectCommand = (Devart.Data.SQLite.SQLiteCommand)DbCommand };
        }

        public DataAvail.Data.Function.Function CreateCommand()
        {
            return new Function();
        }

        #endregion
    }
}
