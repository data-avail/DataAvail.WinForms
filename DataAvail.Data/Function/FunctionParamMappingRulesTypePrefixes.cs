using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Data.Function
{
    public abstract class FunctionParamMappingRulesTypePrefixes<T> : FunctionParamMappingRules<T>
    {
        public FunctionParamMappingRulesTypePrefixes(IDictionary<string, string> FieldParamPredefined, IFunctionParamCreator ParamCreator)
            : base(FieldParamPredefined, ParamCreator)
        {
        }

        public static string NumericTypePrefix = "ni";

        public static string DateTypePrefix = "dti";

        public static string StringTypePrefix = "si";

        protected override string GetParamPrefix(Type FieldType)
        {
            string paramPrefix = null;

            if (FieldType == typeof(System.Int32)
                || FieldType == typeof(System.Byte)
                || FieldType == typeof(System.Double)
                || FieldType == typeof(System.Decimal))
            {
                paramPrefix = NumericTypePrefix;
            }
            else if (FieldType == typeof(System.DateTime))
            {
                paramPrefix = DateTypePrefix;
            }
            else if (FieldType == typeof(System.String))
            {
                paramPrefix = StringTypePrefix;
            }

            return paramPrefix;
        }

    }
}
