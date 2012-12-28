using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraContainerBuilder
{
    internal abstract class XtraContainerBaseParser<T> : IXtraContinerObjectParser
    {
        internal XtraContainerBaseParser(DataAvail.XtraObjectProperties.IXtraObjectPropertiesProvider XtraObjectPropertiesProvider, System.Collections.IEnumerator Items)
        {
            _xtraObjectPropertiesProvider = XtraObjectPropertiesProvider;

            _items = Items;
        }

        private readonly DataAvail.XtraObjectProperties.IXtraObjectPropertiesProvider _xtraObjectPropertiesProvider;

        private readonly System.Collections.IEnumerator _items;

        protected DataAvail.XtraObjectProperties.IXtraObjectPropertiesProvider XtraObjectPropertiesProvider { get { return _xtraObjectPropertiesProvider; } }

        #region IXtraContinerObjectParser Members

        public XtraContainerObjectProperties ObjectProperies
        {
            get 
            {
                return new XtraContainerObjectProperties(XtraObjectPropertiesProvider.GetObjectProperties());
            }
        }


        public XtraContainerFieldProperties GetNextFieldProperties()
        {
            if (_items.MoveNext())
            {
                return GetFieldProperties((T)_items.Current);
            }
            else
            {
                return null;
            }
        }

        #endregion

        protected abstract XtraContainerFieldProperties GetFieldProperties(T Item);

    }
}
