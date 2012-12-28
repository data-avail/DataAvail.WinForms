using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.DevArt.Data.SQLite
{
    public class FunctionParamCreator : DataAvail.Data.Function.IFunctionParamCreator
    {
        public System.Data.IDbDataParameter CreateParam()
        {
            return new Devart.Data.SQLite.SQLiteParameter();
        }

        public System.Data.IDbDataParameter CreateCursorParam()
        {
            throw new System.NotImplementedException();
        }

        public System.Data.IDbDataParameter CreateBlobParam()
        {
            return new Devart.Data.SqlServer.SqlParameter(null, Devart.Data.SQLite.SQLiteType.Blob);
        }

        public System.Data.IDbDataParameter CreateVarCharParam()
        {
            return new Devart.Data.SqlServer.SqlParameter(null, Devart.Data.SQLite.SQLiteType.Text);
        }

        public DataAvail.Data.Function.FunctionArrayParam CreateArrayParam(string TypeName, System.Data.ParameterDirection ParameterDirection)
        {
            throw new NotImplementedException();
        }
    }
}
