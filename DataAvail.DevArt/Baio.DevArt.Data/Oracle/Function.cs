using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.DevArt.Data.Oracle
{
    public class Function : DataAvail.DevArt.Data.Function
    {
        public Function()
            : base()
        {
        }

        public Function(string CommandText, System.Data.CommandType CommandType)
            : base(CommandText, CommandType)
        {
        }


        protected override System.Data.IDbCommand OnCreateCommand()
        {
            return new Devart.Data.Oracle.OracleCommand() { PassParametersByName = true };
        }

        protected override DataAvail.Data.Function.IFunctionParamCreator OnCreateParamCreator()
        {
            return new FunctionParamCreator();
        }

        internal System.Data.IDataParameter AddParam(string ParamName, Devart.Data.Oracle.OracleDbType OracleDbType)
        {
            System.Data.IDbDataParameter param = new Devart.Data.Oracle.OracleParameter(ParamName, OracleDbType);

            this.AddParam(param);

            return param;
        }

    }
}
