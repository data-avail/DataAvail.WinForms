using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.DevArt.Data.SQLite
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
            return new Devart.Data.SQLite.SQLiteCommand();
        }

        protected override DataAvail.Data.Function.IFunctionParamCreator OnCreateParamCreator()
        {
            return new FunctionParamCreator();
        }

        internal System.Data.IDataParameter AddParam(string ParamName, Devart.Data.SQLite.SQLiteType SqlType)
        {
            System.Data.IDbDataParameter param = new Devart.Data.SQLite.SQLiteParameter(ParamName, SqlType);

            this.AddParam(param);

            return param;
        }

    }
}
