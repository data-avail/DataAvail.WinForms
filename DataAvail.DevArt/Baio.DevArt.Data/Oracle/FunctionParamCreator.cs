using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.DevArt.Data.Oracle
{
    public class FunctionParamCreator : DataAvail.Data.Function.IFunctionParamCreator
    {
        public System.Data.IDbDataParameter CreateParam()
        {
            return new Devart.Data.Oracle.OracleParameter();
        }

        public System.Data.IDbDataParameter CreateCursorParam()
        {
            return new Devart.Data.Oracle.OracleParameter(null, Devart.Data.Oracle.OracleDbType.Cursor);
        }

        public System.Data.IDbDataParameter CreateBlobParam()
        {
            return new Devart.Data.Oracle.OracleParameter(null, Devart.Data.Oracle.OracleDbType.Blob);
        }

        public System.Data.IDbDataParameter CreateVarCharParam()
        {
            return new Devart.Data.Oracle.OracleParameter(null, Devart.Data.Oracle.OracleDbType.VarChar);
        }

        public DataAvail.Data.Function.FunctionArrayParam CreateArrayParam(string TypeName, System.Data.ParameterDirection ParameterDirection)
        {
            FunctionArrayParam arrParam = new FunctionArrayParam(new Devart.Data.Oracle.OracleParameter(null, Devart.Data.Oracle.OracleDbType.Table)
            {
                ObjectTypeName = TypeName,
                Direction = ParameterDirection,
                ParameterName = ParameterDirection == System.Data.ParameterDirection.ReturnValue ? "RETURN" : null
            });

            if (ParameterDirection != System.Data.ParameterDirection.ReturnValue)
            {
                arrParam.DataParameter.Value = new Devart.Data.Oracle.OracleArray(TypeName, null);
            }

            return arrParam;
        }
    }
}
