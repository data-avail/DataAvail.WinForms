using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Data.Function
{
    public class DCFunctionParamMappingRules : FunctionParamMappingRules<System.Data.DataColumn>
    {
        public DCFunctionParamMappingRules(IDictionary<string, string> FieldParamPredef, IFunctionParamCreator ParamCreator)
            : base(FieldParamPredef, ParamCreator)
        { 
        }

        protected override string GetFieldName(System.Data.DataColumn Field)
        {
            return Field.ColumnName;
        }

        protected override Type GetFieldType(System.Data.DataColumn Field)
        {
            return Field.DataType;
        }

        protected override int GetFieldSize(System.Data.DataColumn Field)
        {
            return Field.MaxLength != 0 ? Field.MaxLength : 1;
        }

        protected override FunctionParamMappingRules<System.Data.DataColumn>.RuleDbType GetRuleDbType(System.Data.DataColumn Field)
        {
            return RuleDbType.DBType;
        }
    }
}
