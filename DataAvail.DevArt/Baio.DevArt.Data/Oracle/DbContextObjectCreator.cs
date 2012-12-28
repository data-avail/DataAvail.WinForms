using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.DevArt.Data.Oracle
{
    public class DbContextObjectCreator : DataAvail.Data.DbContext.IDbContextObjectCreator
    {
        #region IDbContextObjectCreator Members

        public System.Data.IDbConnection Connection
        {
            get
            {
                throw new NotImplementedException();
            }
        }


        public System.Data.IDbConnection CreateConnection()
        {
            throw new NotImplementedException();
        }

        public System.Data.DataSet CreateDataSet()
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable CreateDataTable(System.Data.IDbCommand DbCommand)
        {
            throw new NotImplementedException();
        }

        public DataAvail.Data.Function.Function CreateCommand()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
