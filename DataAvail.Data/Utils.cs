using System;
using System.Collections.Generic;
using System.Linq;
using DataAvail.Linq;

namespace DataAvail.Data
{
    internal class Utils
    {
        internal static string GetCommandText(System.Data.IDbCommand DbCommand)
        {
            if (DbCommand.CommandType == System.Data.CommandType.StoredProcedure)
            {
                return string.Format("{0}(\r\n{1}\r\n)\r\n", DbCommand.CommandText,
                    DbCommand.Parameters.Cast<System.Data.IDbDataParameter>().ToString(p => string.Format("{0} => {1}", p.ParameterName, p.Value == System.DBNull.Value ? "NULL" : p.Value), ",\r\n"));
            }
            else
            {
                return string.Format("{0}\r\n", DbCommand.CommandText);
            }
        }
    }
}
