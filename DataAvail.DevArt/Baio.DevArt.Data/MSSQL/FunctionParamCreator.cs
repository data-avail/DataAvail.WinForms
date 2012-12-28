using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.DevArt.Data.MSSQL
{
    public class FunctionParamCreator : DataAvail.Data.Function.IFunctionParamCreator
    {
        public System.Data.IDbDataParameter CreateParam()
        {
            return new Devart.Data.SqlServer.SqlParameter();
        }

        public System.Data.IDbDataParameter CreateCursorParam()
        {
            throw new NotImplementedException();
        }

        public System.Data.IDbDataParameter CreateBlobParam()
        {
            throw new NotImplementedException();
        }

        public System.Data.IDbDataParameter CreateVarCharParam()
        {
            throw new NotImplementedException();
        }

        public DataAvail.Data.Function.FunctionArrayParam CreateArrayParam(string TypeName, System.Data.ParameterDirection ParameterDirection)
        {
            throw new NotImplementedException();
        }
    }
}
