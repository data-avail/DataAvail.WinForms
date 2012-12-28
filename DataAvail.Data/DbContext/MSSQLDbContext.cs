using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Data.DbContext
{
    public class MSSQLDbContext : IDbContext
    {
        private static IDbContextWhereFormatter _whereFormatter = new DbContextWhereFormatter("MM/dd/yyyy HH:mm:ss", "'{0}'");

        #region IDbContext Members

        public bool IsPkIncludedIntoUpdate
        {
            get { return false; }
        }

        public string GetIdentityCommandText(string TableName)
        {
            return string.Format("SELECT IDENT_CURRENT('{0}')", TableName);
        }

        public IDbContextWhereFormatter WhereFormatter
        {
            get { return _whereFormatter; }
        }

        public string ParameterValuePrefix { get { return "@"; } }

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
            int i = Query.ToUpper().IndexOf("SELECT") + "SELECT".Length;

            return Query.Insert(i, string.Format(" TOP({0})", Top));
        }

        #endregion
    }
}
