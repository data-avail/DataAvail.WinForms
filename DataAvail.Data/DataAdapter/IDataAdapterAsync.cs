using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Data.DataAdapter
{
    public interface IDataAdapterAsync 
    {
        void BeginFill(object DataSource, string Filter);

        event DataSyncAdapterEndFillHandler EndFill;

        void CancelFill();
    }

    public class DataAdapterAsyncEndFillEventArgs : System.EventArgs
    {
        public DataAdapterAsyncEndFillEventArgs(object DataSource, bool Canceled, System.Exception Exception)
        {
            dataSource = DataSource;

            canceled = Canceled;

            exception = Exception;
        }

        public readonly object dataSource;

        public readonly bool canceled;

        public readonly System.Exception exception;
    }

    public delegate void DataSyncAdapterEndFillHandler(object sender, DataAdapterAsyncEndFillEventArgs args);
}
