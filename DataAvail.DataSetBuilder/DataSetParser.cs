using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DataAvail.DataSetParser
{
    public static class DataSetParser
    {
        public static Dictionary<string, string> TypesConverters = null;

        public static KeyValuePair<Type, int> GetFieldTypeSize(string Attr)
        {
            string[] strs = Attr.Split(',');

            string typeStr = strs[0];

            if (TypesConverters != null && TypesConverters.Keys.Contains(typeStr))
            {
                typeStr = TypesConverters[typeStr];
            }

            Type type = GetFieldType(typeStr);

            int size = 1;

            if (strs.Length == 2)
                size = int.Parse(strs[1]);

            return new KeyValuePair<Type, int>(type, size);

        }

        public static Type GetFieldType(string FieldName)
        {
            switch (FieldName)
            {
                case "int":
                    return typeof(int);
                case "long":
                    return typeof(long);
                case "double":
                    return typeof(double);
                case "date":
                    return typeof(System.DateTime);
                case "string":
                    return typeof(string);
            }

            throw new Exception("Can't parse column type");
        }

        public static object GetDefaultValue(string DefValue, Type Type)
        {
            return Type.GetMethod("Parse", new Type[] { typeof(string) }).Invoke(null, new object[] { DefValue });
        }
    }
}
