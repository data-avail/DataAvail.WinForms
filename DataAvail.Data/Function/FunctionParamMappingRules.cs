using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Data.Function
{
    public abstract class FunctionParamMappingRules<T> : IFunctionParamMappingRules<T>
    {
        public FunctionParamMappingRules(IDictionary<string, string> FieldParamPredefined, IFunctionParamCreator ParamCreator)
        {
            _fieldParamPredef = FieldParamPredefined;

            _paramCreator = ParamCreator;

        }

        private readonly IDictionary<string, string> _fieldParamPredef;

        private readonly IFunctionParamCreator _paramCreator;
   
        #region IFunctionParamMappingRules<T> Members

        public System.Data.IDbDataParameter GetParam(T Field)
        {
            string fieldName = GetFieldName(Field);

            object val = GetParamValue(Field);

            System.Data.ParameterDirection paramDirect = GetParamDirection(Field);

            if (val == null && fieldName == null && paramDirect != System.Data.ParameterDirection.ReturnValue)
                throw new Exception("Couldn't determine param name based on these rules!");

            Type fieldType = GetFieldType(Field);

            if (fieldType == null)
                throw new Exception("Couldn't determine param name based on these rules!");

            string paramName = GetParamName(Field);

            if (paramName == null)
            {
                if (_fieldParamPredef != null)
                {
                    if (_fieldParamPredef.Keys.Contains(fieldName))
                    {
                        paramName = _fieldParamPredef.Where(p => p.Key == fieldName).Select(p => p.Value).Single();

                        if (paramName == null)
                            return null;
                    }
                }
            }

            if (paramName == null && paramDirect != System.Data.ParameterDirection.ReturnValue)
            {
                string paramPrefix = GetParamPrefix(fieldType);

                paramName = string.Format("{0}{1}", paramPrefix, fieldName);
            }

            System.Data.IDbDataParameter param = null;

            RuleDbType ruleDbType = this.GetRuleDbType(Field);

            switch(ruleDbType)
            { 
                case RuleDbType.Blob:
                    _paramCreator.CreateBlobParam();
                    break;
                case RuleDbType.Cursor:
                    _paramCreator.CreateCursorParam();
                    break;
                case RuleDbType.VarChar:
                    _paramCreator.CreateVarCharParam();
                    break;
                case RuleDbType.DBType:
                    param = _paramCreator.CreateParam();
                    param.DbType = GetParamtType(fieldType);
                    break;

            }

            param.ParameterName = paramName;

            param.SourceColumn = fieldName;

            param.Direction = paramDirect;

            param.Size = GetFieldSize(Field);
           
            param.Value = val;

            return param;
        }

        #endregion

    
        protected virtual string GetParamPrefix(Type FieldType)
        {
            return null;
        }

        protected virtual System.Data.DbType GetParamtType(Type FieldType)
        {
            System.Data.DbType dbType = System.Data.DbType.AnsiString;

            if (FieldType == typeof(System.Int32)
              || FieldType == typeof(System.Byte))
            {
                dbType = System.Data.DbType.Int32;
            }
            else if (FieldType == typeof(System.Int64))
            {
                dbType = System.Data.DbType.Int64;
            }
            else if (FieldType == typeof(System.Double))
            {
                dbType = System.Data.DbType.Double;
            }
            else if (FieldType == typeof(System.Decimal))
            {
                dbType = System.Data.DbType.Decimal;
            }
            else if (FieldType == typeof(System.DateTime))
            {
                dbType = System.Data.DbType.DateTime;
            }
            else if (FieldType == typeof(System.String))
            {
                dbType = System.Data.DbType.AnsiString;
            }
            else
            {
                throw new System.Exception("Couldn't determine param type based on these rules");
            }

            return dbType;
        }

        protected virtual string GetParamName(T Field)
        {
            return null;
        }

        protected virtual System.Data.ParameterDirection GetParamDirection(T Field)
        {
            return System.Data.ParameterDirection.Input;
        }

        protected virtual object GetParamValue(T Field)
        {
            return null;
        }


        protected abstract string GetFieldName(T Field);

        protected abstract Type GetFieldType(T Field);

        protected abstract int GetFieldSize(T Field);

        protected abstract RuleDbType GetRuleDbType(T Field);

        public enum RuleDbType
        { 
            DBType,

            Blob,

            Cursor,

            VarChar
        }
    }

    
}
