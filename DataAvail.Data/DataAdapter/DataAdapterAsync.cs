using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.Data.DbContext;

namespace DataAvail.Data.DataAdapter
{
    public class DataAdapterAsync : DataAdapter, IDataAdapterAsync
    {
        public DataAdapterAsync(System.Data.IDbConnection DbConnection, 
            DataAvail.Data.Function.Function Delete,
            DataAvail.Data.Function.Function Update,
            DataAvail.Data.Function.Function Insert)
            : base(DbConnection, Delete, Update, Insert)
        {
        }

        private bool _isFillCanceled;

        private System.Data.DataTable _dataTable;

        #region IDataAdapterAsync Members

        public void BeginFill(object DataSource, string Filter)
        {
            _isFillCanceled = false;

            try
            {
                _dataTable = (System.Data.DataTable)DataSource;

                _dataTable.FillAsync(new AsyncCallback(FillComplete), Filter);
            }
            catch (System.Exception e)
            {
                OnEndFill(false, e);
            }
        }

        public void CancelFill()
        {
            _isFillCanceled = true;

            _dataTable.SuspendFill(false);
        }

        public event DataSyncAdapterEndFillHandler EndFill;

        #endregion

        void FillComplete(IAsyncResult ar)
        {
            _dataTable.CancelFetch();

            _dataTable.EndFill(ar);

            OnEndFill(_isFillCanceled, null);
        }

        protected virtual void OnEndFill(bool Canceled, System.Exception Exception)
        {
            if (EndFill != null)
            {
                EndFill(this, new DataAdapterAsyncEndFillEventArgs(_dataTable, Canceled, Exception));
            }

        }
    }
}
