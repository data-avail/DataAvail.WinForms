using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.DevArt.Data.MSSQL
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
            return new Devart.Data.SqlServer.SqlCommand();
        }

        protected override DataAvail.Data.Function.IFunctionParamCreator OnCreateParamCreator()
        {
            return new FunctionParamCreator();
        }

        internal System.Data.IDataParameter AddParam(string ParamName, Devart.Data.SqlServer.SqlType SqlType)
        {
            System.Data.IDbDataParameter param = new Devart.Data.SqlServer.SqlParameter(ParamName, SqlType);

            this.AddParam(param);

            return param;
        }

    }
}
