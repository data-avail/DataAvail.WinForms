using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XOP.AppContext
{
    internal class AppItemContextsCollection : IEnumerable<AppItemContext>
    {
        internal AppItemContextsCollection(AppItem AppItem)
        {
            _appItem = AppItem;
        }

        private readonly AppItem _appItem;

        private readonly List<AppItemContext> _list = new List<AppItemContext>();

        #region IEnumerable<AppItemContext> Members

        public IEnumerator<AppItemContext> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion

        internal AppItemContext Add(ModelView.ModelViewAppItem ModelViewAppItem, Context Context)
        {
            if (this[Context] == null)
            {
                AppItemContext item = new AppItemContext(_appItem, ModelViewAppItem, _appItem.ModelSecObject, Context);

                _list.Add(item);

                return item;
            }
            else
            {
                throw new Exception("Only unique context is allowed within AppItem");
            }
        }
        /*
        internal AppItemContext Add(ModelView.ModelViewAppItem ModelViewAppItem, AppItemContext Parent)
        {
            return Add(ModelViewAppItem, Parent == null ? (Context)new DefaultContext() : (Context)new ChildContext(Parent));
        }
         */

        internal AppItemContext this[Context Context]
        {
            get
            {
                return _list.FirstOrDefault(p=>p.Context.Equals(Context));
            }
        }
    }
}
