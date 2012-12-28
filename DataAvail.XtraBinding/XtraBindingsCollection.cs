using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraBindings
{
    public class XtraBindingsCollection : IEnumerable<XtraBindingChild>
    {
        private readonly List<XtraBindingChild> _list = new List<XtraBindingChild>();

        public void Add(XtraBindingChild XtraBinding)
        {
            _list.Add(XtraBinding);

            OnItemAdded(new XtraBindingsCollectionItemAddedEventArgs(XtraBinding));
        }

        #region IEnumerable<XtraBinding> Members

        public IEnumerator<XtraBindingChild> GetEnumerator()
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

        public event XtraBindingsCollectionItemHandler ItemAdded;

        private void OnItemAdded(XtraBindingsCollectionItemAddedEventArgs args)
        {
            if (ItemAdded != null)
                ItemAdded(this, args);
        }
    }

    public delegate void XtraBindingsCollectionItemHandler(object sender, XtraBindingsCollectionItemAddedEventArgs args);

    public class XtraBindingsCollectionItemAddedEventArgs : System.EventArgs
    {
        public XtraBindingsCollectionItemAddedEventArgs(XtraBindingChild AddedItem)
        {
            addedItem = AddedItem;
        }

        public readonly XtraBindingChild addedItem;
    }

}
