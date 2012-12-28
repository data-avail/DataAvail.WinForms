using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraObjectProperties
{
    public class XOFFunctionParamMappingRules : DataAvail.Data.Function.FunctionParamMappingRules<DataAvail.XtraObjectProperties.XtraObjectFunctionParameter>
    {
        public XOFFunctionParamMappingRules(IDictionary<string, string> NameParamMap, DataAvail.Data.Function.IFunctionParamCreator ParamCreator)
            : base(NameParamMap, ParamCreator)
        { }

        protected override string GetParamName(XtraObjectFunctionParameter Field)
        {
            if (Field.direction == System.Data.ParameterDirection.ReturnValue)
                return "RESULT";
            else
                return Field.name == null && Field.mappedField != null ? Field.mappedField.MappedParamName : Field.name;
        }

        protected override string GetFieldName(XtraObjectFunctionParameter Field)
        {
            return Field.mappedField != null ? Field.mappedField.FieldName : null;
        }

        protected override Type GetFieldType(XtraObjectFunctionParameter Field)
        {
            return Field.type ?? Field.mappedField.FieldType;
        }

        protected override int GetFieldSize(XtraObjectFunctionParameter Field)
        {
            int size = Field.size;

            if (size == 0)
            {
                if (Field.mappedField is DataAvail.XtraObjectProperties.XtraTextFieldProperties)
                {
                    size = ((DataAvail.XtraObjectProperties.XtraTextFieldProperties)Field.mappedField).MaxLength;
                }
            }

            if (size == 0)
                size = 1;

            return size;
        }

        protected override System.Data.ParameterDirection GetParamDirection(XtraObjectFunctionParameter Field)
        {
            return Field.direction;
        }

        protected override DataAvail.Data.Function.FunctionParamMappingRules<XtraObjectFunctionParameter>.RuleDbType GetRuleDbType(XtraObjectFunctionParameter Field)
        {
            return DataAvail.Data.Function.FunctionParamMappingRules<XtraObjectFunctionParameter>.RuleDbType.DBType;
        }

        protected override object GetParamValue(XtraObjectFunctionParameter Field)
        {
            return Field.val;
        }
    }
}
