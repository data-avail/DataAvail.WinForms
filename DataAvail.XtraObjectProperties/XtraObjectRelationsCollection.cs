using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraObjectProperties
{
    public class XtraObjectRelationsCollection : IEnumerable<XtraObjectRelation>
    {
        private readonly List<XtraObjectRelation> _list = new List<XtraObjectRelation>();

        #region IEnumerable<XtraObjectRelation> Members

        public IEnumerator<XtraObjectRelation> GetEnumerator()
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

        internal void Add(XtraObjectRelation XtraObjectRelation)
        {
            _list.Add(XtraObjectRelation);
        }
    }
}
