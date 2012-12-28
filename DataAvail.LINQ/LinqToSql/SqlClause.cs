using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Linq.LinqToSql
{
    internal class SqlClause
    {
        internal SqlClause(System.Linq.Expressions.Expression Expression)
            : this(null, Expression)
        {
        }

        internal SqlClause(SqlClause Parent, System.Linq.Expressions.Expression Expression)
        {
            parent = Parent;

            expression = Expression;

            if (DataAvail.Linq.Condition.IsBinaryJoiner(expression))
            {
                sqlLeft = new SqlClause(this, ((System.Linq.Expressions.BinaryExpression)Expression).Left);

                sqlRight = new SqlClause(this, ((System.Linq.Expressions.BinaryExpression)Expression).Right);

                sqlJoiner = GetStorkeOperand(expression.NodeType);
            }
            else
            {
                if (Expression is System.Linq.Expressions.BinaryExpression)
                {
                    sqlStroke = new SqlStroke((System.Linq.Expressions.BinaryExpression)Expression);
                }
                else if (Expression is System.Linq.Expressions.MethodCallExpression && ((System.Linq.Expressions.MethodCallExpression)Expression).Arguments.Count == 1)
                {
                    sqlStroke = new SqlStrokeMethodCall((System.Linq.Expressions.MethodCallExpression)Expression);
                }
                else
                {
                    throw new SqlVisitorException("Sql stroke supports only Binary and MethodCall expressions which contains single argument");
                }
            }
        }

        internal static string GetStorkeOperand(System.Linq.Expressions.ExpressionType ExpressionType)
        {
            switch (ExpressionType)
            {
                case System.Linq.Expressions.ExpressionType.Equal:
                    return "=";
                case System.Linq.Expressions.ExpressionType.GreaterThan:
                    return ">";
                case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case System.Linq.Expressions.ExpressionType.LessThan:
                    return "<";
                case System.Linq.Expressions.ExpressionType.LessThanOrEqual:
                    return "<=";
                case System.Linq.Expressions.ExpressionType.NotEqual:
                    return "<>";
                case System.Linq.Expressions.ExpressionType.And:
                case System.Linq.Expressions.ExpressionType.AndAlso:
                    return "AND";
                case System.Linq.Expressions.ExpressionType.Or:
                case System.Linq.Expressions.ExpressionType.OrElse:
                    return "OR";
            }

            throw new System.ArgumentException("Not a valid ExpressionType Type");
        }

        internal static SqlFunc GetFunc(System.Reflection.MethodInfo MethodInfo)
        {
            if (MethodInfo.ReflectedType == typeof(System.Data.DataRowExtensions))
            {
                switch (MethodInfo.Name)
                {
                    case "Field":
                        return new SqlFunc("{0}", null);
                }            
            }

            if (MethodInfo.ReflectedType == typeof(string))
            {
                switch (MethodInfo.Name)
                {
                    case "ToUpper":
                        return new SqlFunc("UPPER({0})", null);
                    case "ToLower":
                        return new SqlFunc("LOWER({0})", null);
                }
            }

            throw new SqlVisitorException("Not a valid Function");
        }

        public readonly SqlClause parent;

        public readonly System.Linq.Expressions.Expression expression;

        public SqlClause sqlLeft;

        public SqlClause sqlRight;

        public SqlStroke sqlStroke;

        public string sqlJoiner;

        public bool IsStroke { get { return sqlStroke != null; } }

        public override string ToString()
        {
            if (IsStroke)
            {
                return sqlStroke.ToString();
            }
            else
            {
                return string.Format("({0} {1} {2})", sqlLeft, sqlJoiner, sqlRight);
            }
        }

        internal SqlClause GetSiblingClause(System.Linq.Expressions.Expression Expression)
        {
            if (parent == null)
            {
                throw new SqlVisitorException("Couldn't get sibling clause from clause which doesn't have parent clause");
            }

            if (parent.sqlLeft.expression != null && parent.sqlLeft.expression == Expression)
            {
                return parent.sqlLeft;
            }

            if (parent.sqlRight.expression != null && parent.sqlRight.expression == Expression)
            {
                return parent.sqlRight;
            }

            throw new SqlVisitorException("Couldn't get sibling clause");
        }

        internal SqlClause GetChildClause(System.Linq.Expressions.Expression Expression)
        {
            if (sqlLeft.expression != null && sqlLeft.expression == Expression)
            {
                return sqlLeft;
            }

            if (sqlRight.expression != null && sqlRight.expression == Expression)
            {
                return sqlRight;
            }

            throw new SqlVisitorException("Couldn't get child clause");
        }
    }
}
