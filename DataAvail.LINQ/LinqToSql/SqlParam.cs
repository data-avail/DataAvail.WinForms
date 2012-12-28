using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Linq.LinqToSql
{
    public class SqlParam
    {
        internal SqlParam(string Name, object Value, Type Type)
        {
            this.name = Name;

            this.value = Value;

            this.type = Type;
        }

        public readonly string name;

        public readonly object value;

        public readonly Type type;
    }
}
