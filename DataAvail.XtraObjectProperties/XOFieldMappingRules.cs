using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XOP
{
    public class XOFieldMappingRules : DataAvail.Data.Function.FunctionParamMappingRules<DataAvail.XtraObjectProperties.XtraFieldProperties> 
    {
        public XOFieldMappingRules(IDictionary<string, string> NameParamMap, DataAvail.Data.Function.IFunctionParamCreator ParamCreator)
            : base(NameParamMap, ParamCreator)
        { }

        protected override string GetFieldName(DataAvail.XtraObjectProperties.XtraFieldProperties Field)
        {
            return Field.FieldName;
        }

        protected override Type GetFieldType(DataAvail.XtraObjectProperties.XtraFieldProperties Field)
        {
            return Field.FieldType;
        }

        protected override int GetFieldSize(DataAvail.XtraObjectProperties.XtraFieldProperties Field)
        {
            int size = 0;

            if (size == 0)
            {
                if (Field is DataAvail.XtraObjectProperties.XtraTextFieldProperties)
                {
                    size = ((DataAvail.XtraObjectProperties.XtraTextFieldProperties)Field).MaxLength;
                }
            }

            if (size == 0)
                size = 1;

            return size;
        }

        protected override DataAvail.Data.Function.FunctionParamMappingRules<DataAvail.XtraObjectProperties.XtraFieldProperties>.RuleDbType GetRuleDbType(DataAvail.XtraObjectProperties.XtraFieldProperties Field)
        {
            return DataAvail.Data.Function.FunctionParamMappingRules<DataAvail.XtraObjectProperties.XtraFieldProperties>.RuleDbType.DBType;
        }
    }
}
