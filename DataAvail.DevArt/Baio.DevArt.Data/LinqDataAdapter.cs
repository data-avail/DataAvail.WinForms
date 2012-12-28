using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.DevArt.Data
{
    public class LinqDataAdapter : DataAvail.Data.DataAdapter.IDataAdapter
    {
        #region IDataAdapter Members

        public void Fill(object DataSource, string Filter)
        {
            //((DataAvail.Data.IDataSourceQueriable)DataSource).Refresh(SelectExpression);
        }

        public void Clear(object DataSource)
        { }

        public void Fill(IEnumerable<object> Items)
        {
            throw new NotImplementedException();
        }

        
        public void Update(object DataSource, IEnumerable<object> Items)
        {
            throw new NotImplementedException();
        }

        public System.Data.IDbConnection Connection
        {
            get { throw new NotImplementedException(); }
        }

        public void BeginTransaction()
        {
        }

        public void CommitTransaction()
        {
        }

        public void RollbackTransaction()
        {
        }

        public DataAvail.Data.DataAdapter.DataAdapterSuppotedOperations SupportedOperations
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
