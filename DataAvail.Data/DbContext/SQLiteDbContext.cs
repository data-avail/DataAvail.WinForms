using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Data.DbContext
{
    public class SQLiteDbContext : IDbContext
    {
        private static IDbContextWhereFormatter _whereFormatter = new DbContextWhereFormatter("yyyy-MM-dd HH:mm:ss", "Datetime('{0}')");

        #region IDbContext Members

        public bool IsPkIncludedIntoUpdate
        {
            get { return true; }
        }

        public string GetIdentityCommandText(string TableName)
        {
            return "SELECT last_insert_rowid()";
        }

        public IDbContextWhereFormatter WhereFormatter
        {
            get { return _whereFormatter; }
        }

        public string ParameterValuePrefix { get { return ":"; } }

        public virtual IDbContextObjectCreator ObjectCreator
        {
            get { return null; }
        }

        public virtual IDbContextDataAdapter DataAdapter
        {
            get { return null; }
        }

        public string WrapTop(string Query, int Top)
        {
            return string.Format("{0} LIMIT {1}", Query, Top);
        }

        #endregion
    }
}
