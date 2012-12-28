using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Data.DataAdapter
{
    public interface IDataAdapter
    {
        void Fill(object DataSource, string Filter);

        void Fill(IEnumerable<object> Items);

        void Update(object DataSource, IEnumerable<object> Items);

        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();

        DataAdapterSuppotedOperations SupportedOperations { get; }

        void Clear(object DataSource);
    }

    public class DataAdapterException : System.Exception
    {
        public DataAdapterException(string Message)
            : base(Message)
        { }
    }

    [Flags]
    public enum DataAdapterSuppotedOperations
    { 
        None = 0x00,
        Fill = 0x01,
        Create = 0x02,
        Update = 0x04,
        Delete = 0x08
    }
}
