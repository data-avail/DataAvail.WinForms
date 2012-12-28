using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.Utils.XmlLinq;

namespace DataAvail.XOP
{
    public class XOPFieldValueCollection : System.Collections.IEnumerable
    {
        internal XOPFieldValueCollection(XtraObjectProperties.XtraObjectProperties XtraObjectProperties, string Pairs)
        {

            var r = (from p in Pairs.Split(',')
                     let pair = p.Split('=').ToArray()
                     select new { key = pair[0].Trim(), value = pair[1].Trim() }).Select(
                    p => new { k = p.key, v = DataAvail.Utils.Reflection.Parse(XtraObjectProperties.Fields[p.key].FieldType, p.value) });
            
            
            AddRange(r.ToDictionary(k => k.k, v =>  v.v));
         
        }

        private readonly Dictionary<string, object> _fieldValues = new Dictionary<string, object>();

        internal static XOPFieldValueCollection Parse(XtraObjectProperties.XtraObjectProperties XtraObjectProperties, System.Xml.Linq.XElement RelationElement)
        {
            string pairs = XmlLinq.GetAttribute(RelationElement, "defaultValues");

            if (!string.IsNullOrEmpty(pairs))
            {
                return new XOPFieldValueCollection(XtraObjectProperties, pairs);
            }
            else
            {
                return null;
            }
        }

        internal void AddRange(IDictionary<string, object> Pairs)
        {
            foreach (KeyValuePair<string, object> kvp in Pairs)
                Add(kvp.Key, kvp.Value);
        }

        internal void Add(string Field, object Value)
        {
            _fieldValues.Add(Field, Value);
        }

        internal object this[string Field]
        {
            get { return _fieldValues[Field]; }
        }


        public System.Collections.IEnumerator GetEnumerator()
        {
            return _fieldValues.GetEnumerator();
        }

    }
}
