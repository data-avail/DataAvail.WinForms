using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace DataAvail.Linq
{
    public static class Expressions
    {
        public static Expression Expr<T>(Expression<Func<T, bool>> Expr)
        {
            return Expr.Body;
        }

        public static Expression<Func<T, bool>> Predicate<T>(Expression<Func<T, bool>> Expr)
        {
            return Expr;
        }

        public static Expression DefaultExpression(object Object, string FieldName, ParameterExpression ParameterExpression)
        {
            return DefaultExpression(Object, typeof(Object), FieldName, ParameterExpression);
        }

        public static Expression DefaultExpression(object Object, Type Type, string FieldName, ParameterExpression ParameterExpression)
        {
            if (Type == typeof(string))
            {
                return UpperLike((string)Object, FieldName, ParameterExpression);
            }
            else if (Type == typeof(System.DateTime))
            {
                return DateRange((System.DateTime)Object, (System.DateTime)Object, FieldName, ParameterExpression, false);
            }
            else if (Type.IsValueType)
            {
                return Equality(Object, Type, FieldName, ParameterExpression);
            }

            return null;
        }

        public static Expression Equality(object Object, Type Type, string FieldName, ParameterExpression ParameterExpression)
        {
            return FieldVisitor.Eval(ParameterExpression, FieldName, Expr<object>(p => p.Equals(Object)));
        }

        public static Expression Like(string Object, string FieldName, ParameterExpression ParameterExpression)
        {
            return DataAvail.Linq.Condition.CombineExpression(ParameterExpression, FieldName, DataAvail.Linq.Condition.Compare.Like, Object);
        }

        public static Expression UpperLike(string Object, string FieldName, ParameterExpression ParameterExpression)
        {
            return FieldVisitor.Eval(ParameterExpression, FieldName, Expr<string>(p => p.ToUpper().Contains(Object)));
        }

        public static Expression DateRange(System.DateTime DateFrom, System.DateTime DateTo, string FieldName, ParameterExpression ParameterExpression, bool IsNullable)
        {
            if (IsNullable)
            {
                return DateRangeNullable(DateFrom, DateTo, FieldName, ParameterExpression);
            }
            else
            {
                return DateRangeNotNullable(DateFrom, DateTo, FieldName, ParameterExpression);
            }
        }

        public static Expression DateRangeNotNullable(System.DateTime DateFrom, System.DateTime DateTo, string FieldName, ParameterExpression ParameterExpression) 
        {
            if (DateFrom != System.DateTime.MinValue && DateTo != System.DateTime.MinValue)
            {
                return FieldVisitor.Eval(ParameterExpression, FieldName, Expr<System.DateTime>(p => p >= DateFrom.Date && p < DateTo.Date.AddDays(1)));
            }
            else if (DateFrom != System.DateTime.MinValue)
            {
                return FieldVisitor.Eval(ParameterExpression, FieldName, Expr<System.DateTime>(p => p >= DateFrom.Date));
            }
            else if (DateTo != System.DateTime.MinValue)
            {
                return FieldVisitor.Eval(ParameterExpression, FieldName, Expr<System.DateTime>(p => p < DateTo.Date.AddDays(1)));
            }
            else
            {
                return null;
            }
        }

        public static Expression DateRangeNullable(System.DateTime DateFrom, System.DateTime DateTo, string FieldName, ParameterExpression ParameterExpression) 
        {
            if (DateFrom != System.DateTime.MinValue && DateTo != System.DateTime.MinValue)
            {
                return FieldVisitor.Eval(ParameterExpression, FieldName, Expr<System.Nullable<System.DateTime>>(p => p >= DateFrom.Date && p < DateTo.Date.AddDays(1)));
            }
            else if (DateFrom != System.DateTime.MinValue)
            {
                return FieldVisitor.Eval(ParameterExpression, FieldName, Expr<System.Nullable<System.DateTime>>(p => p >= DateFrom.Date));
            }
            else if (DateTo != System.DateTime.MinValue)
            {
                return FieldVisitor.Eval(ParameterExpression, FieldName, Expr<System.Nullable<System.DateTime>>(p => p < DateTo.Date.AddDays(1)));
            }
            else
            {
                return null;
            }
        }

    }
}
