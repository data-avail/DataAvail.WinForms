using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.SqlConverter
{
    public interface IRelation
    {
        ITable Table { get; }

        string ChildColumn { get; }

        string ChildTable { get; }

        string ParentColumn { get; }

        string ParentTable { get; }
    }
}
