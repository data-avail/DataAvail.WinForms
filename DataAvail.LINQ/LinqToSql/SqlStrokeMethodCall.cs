using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Linq.LinqToSql
{
    ///For comparision expressions that are ipmlied from Methods (such as Contains)
    internal class SqlStrokeMethodCall : SqlStroke
    {
        internal SqlStrokeMethodCall(System.Linq.Expressions.MethodCallExpression Expression)
            : base(Expression, new SqlStrokePart(Expression.Object), new SqlStrokePart(Expression.Arguments[0]))
        {
            if (Expression.Method.ReflectedType == typeof(string))
            {
                switch (Expression.Method.Name)
                {
                    case "Contains":
                        condition = "LIKE";
                        strEmbelishParam = "%{0}%";
                        break;                }
            }

            switch (Expression.Method.Name)
            {
                case "Equals":
                    condition = "=";
                    break;
            }

            if (string.IsNullOrEmpty(condition))
                throw new SqlVisitorException("Not a valid Function");
        }

        internal readonly string strEmbelishParam;

        internal object TransformParam(object Param)
        {
            if (string.IsNullOrEmpty(strEmbelishParam))
            {
                return Param;
            }
            else
            {
                return string.Format(strEmbelishParam, Param);
            }
        }
    }

}
