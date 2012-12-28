using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Data.DbContext
{
    partial class DbContextDataAdapter
    {
        private readonly IDataTableStubManager _stubManager;

        public IDataTableStubManager StubManager
        {
            get { return _stubManager; }
        }

        protected IDataTableStub GetDataTableStub(System.Data.DataTable DataTable)
        {
            return StubManager.GetDataTableStub(DataTable);
        }

        public interface IDataTableStub
        {
            System.Data.DataTable DataTable { get; }

            int Fill();

            void Update();

            System.Data.IDbCommand SelectCommand { get; set; }

            System.Data.IDbCommand UpdateCommand { get; set; }

            System.Data.IDbCommand DeleteCommand { get; set; }

            System.Data.IDbCommand InsertCommand { get; set; }

            IAsyncResult BeginFill(AsyncCallback callback, object stateObject);

            void SuspendFill(bool wait);

            void CancelFetch();

            void EndFill(IAsyncResult res);
        }

        public interface IDataTableStubManager
        {
            IDataTableStub GetDataTableStub(System.Data.DataTable DataTable);
        }
    }
}
