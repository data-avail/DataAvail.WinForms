using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.DevArt.Data
{
    internal class DevArtDataTableStub : DataAvail.Data.DbContext.DbContextDataAdapter.IDataTableStub
    {
        internal DevArtDataTableStub(Devart.Common.DbDataTable DbDataTable)
        {
            _dbDataTable = DbDataTable;
        }

        private readonly Devart.Common.DbDataTable _dbDataTable;

        #region IDataTableStub Members

        public System.Data.DataTable DataTable
        {
            get { return _dbDataTable; }
        }

        public int Fill()
        {
            return _dbDataTable.Fill();
        }

        public System.Data.IDbCommand SelectCommand
        {
            get { return _dbDataTable.SelectCommand; }

            set { _dbDataTable.SelectCommand = (System.Data.Common.DbCommand)value; }
        }

        public System.Data.IDbCommand UpdateCommand
        {
            get { return _dbDataTable.UpdateCommand; }

            set { _dbDataTable.UpdateCommand = (System.Data.Common.DbCommand)value; }
        }

        public System.Data.IDbCommand DeleteCommand
        {
            get { return _dbDataTable.DeleteCommand; }

            set { _dbDataTable.DeleteCommand = (System.Data.Common.DbCommand)value; }
        }

        public System.Data.IDbCommand InsertCommand
        {
            get { return _dbDataTable.InsertCommand; }

            set { _dbDataTable.InsertCommand = (System.Data.Common.DbCommand)value; }
        }

        public IAsyncResult BeginFill(AsyncCallback callback, object stateObject)
        {
            return _dbDataTable.BeginFill(callback, stateObject);
        }

        public void SuspendFill(bool wait)
        {
            _dbDataTable.SuspendFill(wait);
        }

        public void CancelFetch()
        {
            _dbDataTable.CancelFetch();
        }

        public void EndFill(IAsyncResult res)
        {
            _dbDataTable.EndFill(res);
        }

        public void Update()
        {
            System.Data.DataTable dataTable = _dbDataTable.GetChanges();

            if (dataTable != null)
                _dbDataTable.UpdateRows(dataTable.Rows.Cast<System.Data.DataRow>().ToArray());
        }

        #endregion
    }
}
