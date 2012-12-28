using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.DevArt.Data
{
    public abstract class Function : DataAvail.Data.Function.Function
    {    
        public Function()
            : base()
        {
        }

        public Function(string CommandText, System.Data.CommandType CommandType)
            : base(CommandText, CommandType)
        {
        }

        protected override string ReturnParameterName
        {
            get { return "RESULT"; }
        }

        protected static int GetParameterSize(System.Data.DataColumn DataColumn)
        {
            return DataColumn.MaxLength;
        }
    }
}
