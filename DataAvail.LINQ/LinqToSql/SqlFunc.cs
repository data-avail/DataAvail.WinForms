using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Linq.LinqToSql
{
    internal class SqlFunc
    {
        internal SqlFunc(string Pattern, string ParamEmbelish)
        {
            pattern = Pattern;
        }

        internal readonly string pattern;

        internal string ToString(string Param)
        {
            return string.Format(string.Format(pattern, Param));
        }
    }
}
