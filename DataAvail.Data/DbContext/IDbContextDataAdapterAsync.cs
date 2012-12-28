using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Data.DbContext
{
    public interface IDbContextDataAdapterAsync
    {
        void FillAsync(System.Data.DataTable DataTable, AsyncCallback callback, string Filter);

        void CancelFetch(System.Data.DataTable DataTable);

        void EndFill(System.Data.DataTable DataTable, IAsyncResult ar);

        void SuspendFill(System.Data.DataTable DataTable, bool wait);
    }
}
