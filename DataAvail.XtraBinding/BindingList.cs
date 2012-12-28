using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baio.XtraBinding
{
    public class BindingList<T> : System.ComponentModel.BindingList<T>
    {
        public BindingList(Baio.Data.IDataSourceQueriable DataSourceQueriable) 
            : this(new List<T>())
        {
            DataSourceQueriable.DataSourceChanged += new EventHandler(DataSourceQueriable_DataSourceChanged);
        }

        public BindingList(IEnumerable<T> Items)
            : base(new List<T>(Items))
        {
        }

        public BindingList(IList<T> Items)
            : base(Items) 
        { 
        }

        protected override void OnListChanged(System.ComponentModel.ListChangedEventArgs e)
        {
            base.OnListChanged(e);

            if (e.ListChangedType == System.ComponentModel.ListChangedType.ItemMoved)
            { 
                
            }
        }

        void DataSourceQueriable_DataSourceChanged(object sender, EventArgs e)
        {
            this.ClearItems();

            foreach (T item in (System.Collections.IEnumerable)((Baio.Data.IDataSourceQueriable)sender).DataSource)
            {
                this.Add(item);
            }

            this.ResetBindings();
        }
    }
}
