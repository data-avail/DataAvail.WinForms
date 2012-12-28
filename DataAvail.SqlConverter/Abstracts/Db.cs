using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;


namespace DataAvail.SqlConverter
{
    public abstract class Db : IDb
    {
        internal Db(string ConnectionString)
        {
            using (IDbConnection connection = CreateConnection(ConnectionString))
            {
                connection.Open();

                IDbCommand cmd = CreateSelectTablesCommand(connection);

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    _tables = GetTables(reader).ToArray();
                }

                foreach(Table tab in Tables)
                {
                    tab.EndInit(connection);
                }


                connection.Close();
            }
        }

        private readonly ITable [] _tables;

        private IEnumerable<ITable> GetTables(IDataReader DataReader)
        {
            while (DataReader.Read())
            {
                yield return CrateTable(DataReader);
            }             
        }

        #region IDb Members

        public ITable [] Tables
        {
            get { return _tables; }
        }

        #endregion

        #region Abstracts

        protected abstract IDbConnection CreateConnection(string ConnectionString);

        protected abstract IDbCommand CreateSelectTablesCommand(IDbConnection DbConnection);

        protected abstract ITable CrateTable(IDataReader DataReader);

        #endregion
    }
}
