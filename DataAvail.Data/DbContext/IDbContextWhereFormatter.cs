using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Data.DbContext
{
    public interface IDbContextWhereFormatter
    {
        string Format(System.DateTime InvariantDateTime);
    }
}
