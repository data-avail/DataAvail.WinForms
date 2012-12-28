using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraObjectProperties
{
    public class XtraFieldPropertiesCollection : IEnumerable<XtraFieldProperties>
    {
        internal XtraFieldPropertiesCollection()
        { }

        private readonly Dictionary<string, XtraFieldProperties> _fieldProperties = new Dictionary<string, XtraFieldProperties>();

        internal void AddFieldProperties(XtraFieldProperties XtraFieldProperties)
        {
            _fieldProperties.Add(XtraFieldProperties.FieldName, XtraFieldProperties);
        }

        public XtraFieldProperties this[string FieldName]
        {
            get
            {
                return _fieldProperties[FieldName];
            }
        }

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            return _fieldProperties.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable<XtraFieldProperties> Members

        IEnumerator<XtraFieldProperties> IEnumerable<XtraFieldProperties>.GetEnumerator()
        {
            return _fieldProperties.Values.GetEnumerator();
        }

        #endregion


        public void Swap(XtraFieldProperties Old, XtraFieldProperties New)
        {
            if (Old.FieldName != New.FieldName)
            {
                throw new Exception("Then field names of old and new fieldProperties must be the same");
            }

            _fieldProperties[Old.FieldName] = New;
        }

    }
}
