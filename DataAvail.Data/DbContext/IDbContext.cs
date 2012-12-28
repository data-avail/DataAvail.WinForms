using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Data.DbContext
{
    public interface IDbContext
    {
        IDbContextObjectCreator ObjectCreator { get; }

        IDbContextDataAdapter DataAdapter { get; }

        bool IsPkIncludedIntoUpdate { get; }

        string GetIdentityCommandText(string TableName);

        IDbContextWhereFormatter WhereFormatter { get; }

        string ParameterValuePrefix { get; }

        string WrapTop(string Query, int Top);
    }
}
