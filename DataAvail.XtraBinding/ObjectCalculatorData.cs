using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baio.XtraBinding
{
    public class ObjectCalculatorData
    {
        private readonly Dictionary<string, Dictionary<string, object>> _propFieldDictionary = new Dictionary<string, Dictionary<string, object>>();

        public const string VALUE_PROP_NAME = "value";

        public const string ENABLED_PROP_NAME = "enabled";

        public object this[string FieldName, string PropertyName]
        {
            get
            {
                if (_propFieldDictionary.ContainsKey(PropertyName))
                {
                    if (_propFieldDictionary[PropertyName].ContainsKey(FieldName))
                        return _propFieldDictionary[PropertyName][FieldName];
                }

                return null;
            }

            set
            {
                if (!_propFieldDictionary.ContainsKey(PropertyName))
                {
                    _propFieldDictionary.Add(PropertyName, new Dictionary<string, object>());
                }

                if (_propFieldDictionary[PropertyName].ContainsKey(FieldName))
                {
                    _propFieldDictionary[PropertyName][FieldName] = value;
                }
                else
                {
                    _propFieldDictionary[PropertyName].Add(FieldName, value);
                }
            }
        }
    }
}
