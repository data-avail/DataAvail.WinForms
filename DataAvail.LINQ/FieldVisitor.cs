using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data;

namespace DataAvail.Linq
{
    public class FieldVisitor : ExpressionVisitor
    {
        private FieldVisitor(ParameterExpression pe, string FieldName)
        {
            _pe = pe;

            _fieldName = FieldName;
        }

        private readonly ParameterExpression _pe;

        private readonly string _fieldName;

        public static Expression Eval(ParameterExpression pe, string FieldName, Expression Expression)
        {
            FieldVisitor visitor = new FieldVisitor(pe, FieldName);

            return visitor.Visit(Expression);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            if (_pe.Type != typeof(System.Data.DataRow))
                return Expression.PropertyOrField(_pe, _fieldName);
            else
            {
                return VisitDataRowParameterExpression(p.Type);
            }
        }

        private Expression VisitDataRowParameterExpression(Type ParamType)
        {
            System.Reflection.MethodInfo mi = typeof(System.Data.DataRowExtensions).GetMethod("Field", new Type[] { typeof(System.Data.DataRow), typeof(string) }).MakeGenericMethod(ParamType);

            System.Linq.Expressions.Expression paramRow = Expression.Parameter(typeof(System.Data.DataRow), null);

            System.Linq.Expressions.Expression paramField = Expression.Parameter(typeof(string), _fieldName);

            return System.Linq.Expressions.MethodCallExpression.Call(_pe, mi, paramRow, paramField);
        }
    }
}
