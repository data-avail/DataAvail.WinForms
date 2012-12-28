using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Data
{
    public interface IDataSourceQueriable
    {
        object DataSource { get; }

        event System.EventHandler DataSourceChanged;

        void Refresh(System.Linq.Expressions.Expression Expression);
    }
}
