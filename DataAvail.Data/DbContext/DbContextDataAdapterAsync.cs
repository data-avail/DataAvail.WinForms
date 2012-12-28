using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Data.DbContext
{
    public class DbContextDataAdapterAsync : DbContextDataAdapter, IDbContextDataAdapterAsync
    {
        public DbContextDataAdapterAsync(IDataTableStubManager Stubmanager)
            : base(Stubmanager)
        {
        }

        #region IDbContextDataAdapterAsync Members

        public void FillAsync(System.Data.DataTable DataTable, AsyncCallback callback, string Filter)
        {
            IDataTableStub dataTableStub = GetDataTableStub(DataTable);

            SelectCommandManager scm = SelectCommandManager.SubstituteCommand(dataTableStub.DataTable, dataTableStub.SelectCommand, Filter);

            dataTableStub.DataTable.Clear();

            try
            {
                dataTableStub.BeginFill(EndFill, new AsyncData(callback, scm));
            }
            catch
            {
                scm.RestoreCommand();

                throw;
            }
        }

        public void CancelFetch(System.Data.DataTable DataTable)
        {
            GetDataTableStub(DataTable).CancelFetch();
        }

        public void EndFill(System.Data.DataTable DataTable, IAsyncResult ar)
        {
            GetDataTableStub(DataTable).EndFill(ar);
        }

        public void SuspendFill(System.Data.DataTable DataTable, bool wait)
        {
            GetDataTableStub(DataTable).SuspendFill(wait);
        }

        #endregion

        private static void EndFill(IAsyncResult ar)
        {
            ((AsyncData)ar.AsyncState).selectCommandManager.RestoreCommand();

            ((AsyncData)ar.AsyncState).callback.Invoke(ar);
        }

        private class AsyncData
        {
            internal AsyncData(AsyncCallback Callback, SelectCommandManager SelectCommandManager)
            {
                this.callback = Callback;

                this.selectCommandManager = SelectCommandManager;
            }

            internal readonly AsyncCallback callback;

            internal readonly SelectCommandManager selectCommandManager;
        }
    }
}
