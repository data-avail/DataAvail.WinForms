using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.SqlConverter
{
    public interface ITable
    {
        IDb Db { get; }

        bool IsSysTable { get; }

        string SchemeName { get; }

        string Name { get; }

        IColumn [] Columns { get; }

        IRelation [] FkRelations { get; }
    }
}
