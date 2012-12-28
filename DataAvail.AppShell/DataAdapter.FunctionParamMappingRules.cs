using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.Data.Function;
using DataAvail.XObject;
using System.Data;

namespace DataAvail.AppShell
{
    partial class DataAdapter
    {
        private class FieldMappingRules : FunctionParamMappingRules<XOField>
        {
            public FieldMappingRules(IDictionary<string, string> NameParamMap, IFunctionParamCreator ParamCreator)
                : base(NameParamMap, ParamCreator)
            { }

            protected override string GetFieldName(XOField Field)
            {
                return Field.Name;
            }

            protected override Type GetFieldType(XOField Field)
            {
                return Field.FieldType;
            }

            protected override int GetFieldSize(XOField Field)
            {
                return Field.FieldSize;
            }

            protected override FunctionParamMappingRules<XOField>.RuleDbType GetRuleDbType(XOField Field)
            {
                return FunctionParamMappingRules<XOField>.RuleDbType.DBType;
            }
        }

        private class FunctionParamMappingRules : FunctionParamMappingRules<XOFunctionParam>
        {
            public FunctionParamMappingRules(IDictionary<string, string> NameParamMap, IFunctionParamCreator ParamCreator)
                : base(NameParamMap, ParamCreator)
            { }

            protected override string GetFieldName(XOFunctionParam Param)
            {
                return Param.Name;
            }

            protected override Type GetFieldType(XOFunctionParam Param)
            {
                return Param.Type;
            }

            protected override int GetFieldSize(XOFunctionParam Param)
            {
                return Param.Size;
            }

            protected override ParameterDirection GetParamDirection(XOFunctionParam Param)
            {
                return Param.Direction;
            }

            protected override object GetParamValue(XOFunctionParam Param)
            {
                return Param.Value;
            }

            protected override FunctionParamMappingRules<XOFunctionParam>.RuleDbType GetRuleDbType(XOFunctionParam Field)
            {
                return FunctionParamMappingRules<XOFunctionParam>.RuleDbType.DBType;
            }
        }
    }
}
