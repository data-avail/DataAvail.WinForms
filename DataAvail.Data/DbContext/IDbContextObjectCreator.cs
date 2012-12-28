using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Data.DbContext
{
    public interface IDbContextObjectCreator
    {
        System.Data.IDbConnection Connection { get; }

        System.Data.DataSet CreateDataSet();

        System.Data.DataTable CreateDataTable(System.Data.IDbCommand SelectCommand);

        Function.Function CreateCommand();
    }
}
