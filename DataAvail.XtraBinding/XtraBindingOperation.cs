using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraBindings
{
    public class XtraBindingOperation 
    {
        public XtraBindingOperation(object DataSource, DataAvail.Data.DataAdapter.IDataAdapter DataAdapter, Calculator.IObjectCalculator ObjectCalculator)
        {
            _dataSource = DataSource;

            dataAdapter = DataAdapter;

            _objectCalculator = ObjectCalculator;

            _isAsync = dataAdapter is DataAvail.Data.DataAdapter.IDataAdapterAsync;

            if (_isAsync)
            {
                ((DataAvail.Data.DataAdapter.IDataAdapterAsync)dataAdapter).EndFill += new DataAvail.Data.DataAdapter.DataSyncAdapterEndFillHandler(XtraBindingOperation_EndFill);
            }
        }

        protected object _dataSource;

        protected readonly DataAvail.Data.DataAdapter.IDataAdapter dataAdapter;

        private readonly Calculator.IObjectCalculator _objectCalculator;

        private readonly bool _isAsync = false;

        internal void Clear()
        {
            dataAdapter.Clear(_dataSource);
        }

        public void Fill(string Filter, bool ForcedSync)
        {
            if (dataAdapter != null)
            {
                if (!ForcedSync && _isAsync)
                {
                    FillAsync(Filter);
                }
                else
                {
                    FillSync(Filter);
                }
            }
        }

        public void Update(IEnumerable<object> Objects)
        {
            if (dataAdapter != null)
            {
                dataAdapter.Update(DataSource, Objects);
            }
        }

        public Calculator.IObjectCalculator ObjectCalculator
        {
            get { return _objectCalculator; }
        }

        public event XtraBindingOperationEndFillHandler EndFill;

        public void StopFill()
        {
            if (_isAsync)
            {
                ((DataAvail.Data.DataAdapter.IDataAdapterAsync)dataAdapter).CancelFill();
            }
        }

        public void Fill(IEnumerable<object> Items)
        {
            dataAdapter.Fill(Items);
        }

        public DataAvail.Data.DataAdapter.DataAdapterSuppotedOperations SuppotedOperations 
        {
            get 
            {
                return dataAdapter == null ? DataAvail.Data.DataAdapter.DataAdapterSuppotedOperations.None : dataAdapter.SupportedOperations;
            }
        }

        protected internal object DataSource
        {
            get { return _dataSource; }

            set { _dataSource = value; }
        }

        private void FillSync(string Filter)
        {
            try
            {
                //dataAdapter.Fill(DataSource, Expression);
                FillSync(DataSource, Filter);

                OnEndFill(false, null);
            }
            catch (System.Exception e)
            {
                OnEndFill(false, e);
            }
        }

        internal void FillSync(object DataSource, string Filter)
        {
            dataAdapter.Fill(DataSource, Filter);
        }

        private void FillAsync(string Filter)
        {
            ((DataAvail.Data.DataAdapter.IDataAdapterAsync)dataAdapter).BeginFill(DataSource, Filter);
        }

        void XtraBindingOperation_EndFill(object sender, DataAvail.Data.DataAdapter.DataAdapterAsyncEndFillEventArgs args)
        {
            OnEndFill(args.canceled, args.exception);
        }

        protected virtual void OnEndFill(bool Canceled, System.Exception Exception)
        {
            if (EndFill != null)
            {
                EndFill(this, new XtraBindingOperationEndFillEventArgs(Canceled, Exception));
            }
        }

        public void BeginTransaction()
        {
            dataAdapter.BeginTransaction();
        }

        public void CommitTransaction()
        {
            dataAdapter.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            dataAdapter.RollbackTransaction();
        }
    }
}
