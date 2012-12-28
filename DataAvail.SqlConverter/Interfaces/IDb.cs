using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.SqlConverter
{
    public interface IDb
    {
        ITable [] Tables { get; }
    }
}
