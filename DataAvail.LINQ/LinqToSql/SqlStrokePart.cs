using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Linq.LinqToSql
{
    internal class SqlStrokePart
    {
        internal SqlStrokePart(System.Linq.Expressions.Expression Expression)
        {
            expression = Expression;
        }

        internal readonly System.Linq.Expressions.Expression expression;

        internal string param;

        internal List<SqlFunc> funcs = new List<SqlFunc>();

        public override string ToString()
        {
            string str = param;

            for (int i = funcs.Count - 1; i >= 0; i--)
            {
                str = funcs[i].ToString(str);
            }

            return str;
        }

    }
}
