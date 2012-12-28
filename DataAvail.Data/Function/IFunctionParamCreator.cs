using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Data.Function
{
    public interface IFunctionParamCreator
    {
        System.Data.IDbDataParameter CreateParam();

        System.Data.IDbDataParameter CreateBlobParam();

        System.Data.IDbDataParameter CreateCursorParam();

        System.Data.IDbDataParameter CreateVarCharParam();

        FunctionArrayParam CreateArrayParam(string ElementType, System.Data.ParameterDirection ParameterDirection);
    }
}
