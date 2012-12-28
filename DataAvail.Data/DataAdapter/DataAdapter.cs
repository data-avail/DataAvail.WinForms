using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using DataAvail.Data.DbContext;

namespace DataAvail.Data.DataAdapter
{
    public class DataAdapter : DataAdapterTable
    {

        public DataAdapter(System.Data.IDbConnection DbConnection, 
            DataAvail.Data.Function.Function Delete,
            DataAvail.Data.Function.Function Update,
            DataAvail.Data.Function.Function Insert)
            : base(DbConnection, Delete, Update, Insert)
        {

        }

        public override void Clear(object DataSource)
        {
            ((System.Data.DataTable)DataSource).Clear();
        }

        public override void Fill(object DataSource, string Filter)
        {
            ((System.Data.DataTable)DataSource).Fill(Filter);
        }

        public override void Fill(IEnumerable<object> Items)
        {
            if (Items != null)
            {
                Items.Cast<System.Data.DataRow>().Fill();
            }   
        }

        public override DataAvail.Data.DataAdapter.DataAdapterSuppotedOperations SupportedOperations
        {
            get
            {
                return base.SupportedOperations | DataAvail.Data.DataAdapter.DataAdapterSuppotedOperations.Fill;
            }
        }
    }
}
