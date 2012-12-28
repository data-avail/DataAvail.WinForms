using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Data.DbContext
{
    public interface IDbContextDataAdapter
    {
        void Fill(IEnumerable<System.Data.DataRow> Rows);

        void Fill(System.Data.DataTable DataTable, string Where);

        void Update(System.Data.DataTable DataTable);

        void InitializeCommands(
            System.Data.DataTable DataTable,
            System.Data.IDbCommand InsertCommand,
            System.Data.IDbCommand UpdateCommand,
            System.Data.IDbCommand DeleteCommand,
            System.Data.IDbCommand SelectCommand);

    }
}
