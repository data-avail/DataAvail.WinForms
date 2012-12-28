using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraBindings
{
    public class DataSourceQueriable<T> : DataAvail.Data.IDataSourceQueriable
    {
        public DataSourceQueriable(System.Linq.IQueryable<T> Queriable)
        {
            _queriable = Queriable;
        }

        private readonly System.Linq.IQueryable<T> _queriable;

        private readonly List<T> _list = new List<T>();

        #region IDataSourceQueriable Members

        public object DataSource
        {
            get { return _list; }
        }

        public event System.EventHandler DataSourceChanged;

        public void Refresh(System.Linq.Expressions.Expression Expression)
        {
            _list.Clear();

            _list.AddRange(_queriable.Where((System.Linq.Expressions.Expression<Func<T, bool>>)Expression));

            OnDataSourceChanged();
        }

        #endregion

        private void OnDataSourceChanged()
        {
            if (DataSourceChanged != null)
                DataSourceChanged(this, EventArgs.Empty);
        }
    }
}
