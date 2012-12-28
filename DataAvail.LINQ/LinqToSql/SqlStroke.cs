using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Linq.LinqToSql
{
    /// For comparision expressions that ipmlied from operators (such as ==, !=)
    internal class SqlStroke
    {
        protected SqlStroke(System.Linq.Expressions.Expression Expression, SqlStrokePart Left, SqlStrokePart Right)
        {
            expression = Expression;

            left = Left;

            right = Right;
        }

        internal SqlStroke(System.Linq.Expressions.BinaryExpression Expression)
            : this(Expression, new SqlStrokePart(Expression.Left), new SqlStrokePart(Expression.Right))
        {
        }

        internal SqlStrokePart GetPart(System.Linq.Expressions.Expression Expression)
        {
            if (Expression == left.expression) return left;

            if (Expression == right.expression) return right;

            return null;
        }

        internal readonly System.Linq.Expressions.Expression expression;

        internal readonly SqlStrokePart left;

        internal readonly SqlStrokePart right;

        internal string condition;

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", left, condition, right);
        }

        
    }

}
