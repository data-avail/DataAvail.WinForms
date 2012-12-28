using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Data.Function
{
    public abstract class FunctionArrayParam
    {
        public FunctionArrayParam(System.Data.IDataParameter DataParameter)
        {
            _dataParameter = DataParameter;
        }

        private readonly System.Data.IDataParameter _dataParameter;

        public System.Data.IDataParameter DataParameter { get { return _dataParameter; } }

        public abstract int Count { get; }

        public abstract object this[int Index] { get; }

        public abstract void Add(object Value);

        public void AddRange(IEnumerable<object> Values)
        {
            foreach (object val in Values) Add(val);
        }

    }
}
