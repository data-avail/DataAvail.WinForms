using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Data.DbContext
{
    public static class DbContext
    {
        public static IDbContext CurrentContext;

        /// <summary>
        /// Return value of first selected row, first selected column
        /// </summary>
        public static object GetScalar(string SQLSatFrmt, params object[] prms)
        {
            System.Data.DataTable dt = GetData(string.Format(SQLSatFrmt, prms));

            if (dt.Rows.Count != 0)
                return dt.Rows[0][0];
            else
                return null;
        }

        public static int GetScalarInt(string SQLSatFrmt, params object[] prms)
        {
            return Convert.ToInt32(GetScalar(SQLSatFrmt, prms));
        }

        public static System.Data.DataTable GetDataTop(string SQLSatFrmt, int Top, params object[] prms)
        {
            return GetData(DbContext.CurrentContext.WrapTop(string.Format(SQLSatFrmt, prms), Top));
        }

        public static System.Data.DataTable GetData(string SQLSatFrmt, params object [] prms)
        {
            return GetData(string.Format(SQLSatFrmt, prms));
        }

        public static System.Data.DataTable GetData(string SQLSatement)
        {
            DataAvail.Data.Function.Function command = DataAvail.Data.DbContext.DbContext.CurrentContext.ObjectCreator.CreateCommand();

            command.Initialize(DataAvail.Data.DbContext.DbContext.CurrentContext.ObjectCreator.Connection, SQLSatement, System.Data.CommandType.Text);

            System.Data.DataTable dataTable = DataAvail.Data.DbContext.DbContext.CurrentContext.ObjectCreator.CreateDataTable(command.Command);

            dataTable.Fill(null);

            return dataTable;

        }
    }
}
