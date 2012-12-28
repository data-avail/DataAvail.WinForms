using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.Linq;
using System.Data;
using DataAvail.Data.DbContext;

namespace DataAvail.Data.DbContext
{
    public static class DataSetExtensions
    {
        public static void Fill(this IEnumerable<System.Data.DataRow> Rows)
        {
            DbContext.CurrentContext.DataAdapter.Fill(Rows);
        }

        public static void Fill(this System.Data.DataTable DataTable, string Filer)
        {
            DbContext.CurrentContext.DataAdapter.Fill(DataTable, Filer);
        }

        public static void Update(this System.Data.DataTable DataTable)
        {
            DbContext.CurrentContext.DataAdapter.Update(DataTable);
        }

        #region Async Fill

        private static IDbContextDataAdapterAsync DataAdapterAsync 
        {
            get { return DbContext.CurrentContext.DataAdapter as IDbContextDataAdapterAsync; }
        }

        public static void FillAsync(this System.Data.DataTable DataTable, AsyncCallback callback, string Filter)
        {
            DataAdapterAsync.FillAsync(DataTable, callback, Filter);
        }

        public static void CancelFetch(this System.Data.DataTable DataTable)
        {
            DataAdapterAsync.CancelFetch(DataTable);
        }

        public static void EndFill(this System.Data.DataTable DataTable, IAsyncResult ar)
        {
            DataAdapterAsync.EndFill(DataTable, ar);
        }

        public static void SuspendFill(this System.Data.DataTable DataTable, bool wait)
        {
            DataAdapterAsync.SuspendFill(DataTable, wait);
        }

        #endregion
    }
}
