using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.DevArt.Data
{
    public class FunctionArrayParam : DataAvail.Data.Function.FunctionArrayParam
    {
        public FunctionArrayParam(System.Data.IDataParameter DataParameter)
            : base(DataParameter)
        { 
        }

        public override int Count
        {
            get { return ((Devart.Data.Oracle.OracleArray)DataParameter.Value).Count; }
        }

        public override object this[int Index]
        {
            get { return ((Devart.Data.Oracle.OracleArray)DataParameter.Value)[Index]; }
        }

        public override void Add(object Value)
        {
           ((Devart.Data.Oracle.OracleArray)DataParameter.Value).Add(Value);
        }
    }
}
