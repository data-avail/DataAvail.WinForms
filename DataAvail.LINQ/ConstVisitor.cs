using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Linq
{
    /// <summary>
    /// Retrieve from all localy accessed members their current values
    /// </summary>
    /// Const : 5
    /// Convert : int x = 5
    /// MemberCall : object x = 5

    public class ConstVisitor : ExpressionVisitor
    {
        public static System.Linq.Expressions.Expression Eval(System.Linq.Expressions.Expression Expression)
        {
            ConstVisitor constVisitor = new ConstVisitor();

            return constVisitor.Visit(Expression);
        }



        /// <summary>
        /// Local members access work in the last to first direction. It is needed to remember preceeded and when first member access
        /// is achived execute all sequence.
        /// </summary>
        Stack<System.Linq.Expressions.Expression> _preecededMemberAccesses = new Stack<System.Linq.Expressions.Expression>();

        protected override System.Linq.Expressions.Expression VisitMemberAccess(System.Linq.Expressions.MemberExpression m)
        {
            if (m.Expression != null && m.Expression.NodeType == System.Linq.Expressions.ExpressionType.Parameter)
            {
                return base.VisitMemberAccess(m);
            }
            else
            {
                System.Linq.Expressions.ConstantExpression constExpr = TryInvokeValue(new MemberAccessibleValueExpression(m));

                if (constExpr != null)
                {
                    return constExpr;
                }
                else
                {
                    System.Linq.Expressions.Expression expr = base.VisitMemberAccess(m);

                    if (expr != m)
                    {  
                        if (expr is System.Linq.Expressions.MemberExpression)
                        {
                            return InvokeValue(new MemberAccessibleValueExpression((System.Linq.Expressions.MemberExpression)expr));
                        }
                        else
                        {
                            throw new LinqToSql.SqlVisitorException("Visiting expression's type is not allowed to change");
                        }
                    }
                    else
                    {
                        return base.VisitMemberAccess(m);    
                    }
                }
            }
        }

        protected override System.Linq.Expressions.Expression VisitMethodCall(System.Linq.Expressions.MethodCallExpression m)
        {
            if (m.Object.NodeType == System.Linq.Expressions.ExpressionType.Parameter)
            {
                return base.VisitMethodCall(m);
            }
            else
            {
                System.Linq.Expressions.ConstantExpression constExpr = TryInvokeValue(new MethodCallAccessibleValueExpression(m));

                if (constExpr != null)
                {
                    return constExpr;
                }
                else
                {
                    System.Linq.Expressions.Expression expr = base.VisitMethodCall(m);

                    if (expr != m)
                    {
                        if (expr is System.Linq.Expressions.MethodCallExpression)
                        {
                            expr = TryInvokeValue(new MethodCallAccessibleValueExpression((System.Linq.Expressions.MethodCallExpression)expr));

                            if (expr != null)
                            {
                                return expr;
                            }
                        }
                        else
                        {
                            throw new LinqToSql.SqlVisitorException("Visiting expression's type is not allowed to change");
                        }
                    }
                }
            }

            return base.VisitMethodCall(m);
        }


        private System.Linq.Expressions.ConstantExpression InvokeValue()
        {
            System.Linq.Expressions.ConstantExpression constExpr = null;

            IAccessibleValueExpression accessVal = null;

            foreach (System.Linq.Expressions.Expression expression in _preecededMemberAccesses)
            {
                if (expression is System.Linq.Expressions.MethodCallExpression)
                {
                    System.Linq.Expressions.MethodCallExpression me = (System.Linq.Expressions.MethodCallExpression)expression;

                    if (constExpr != null)
                        me = System.Linq.Expressions.Expression.Call(constExpr, me.Method, me.Arguments);

                    accessVal = new MethodCallAccessibleValueExpression(me);
                }
                else if (expression is System.Linq.Expressions.MemberExpression)
                {
                    System.Linq.Expressions.MemberExpression me = (System.Linq.Expressions.MemberExpression)expression;

                    if (constExpr != null)
                        me = System.Linq.Expressions.Expression.MakeMemberAccess(constExpr, me.Member);

                    accessVal = new MemberAccessibleValueExpression(me);
                }
                else
                {
                    throw new System.Exception("Value could be accessed only for MemberExpression and MethodCallExpression");
                }


                constExpr = InvokeValue(accessVal);

            }

            return constExpr;
        }

        private static System.Linq.Expressions.ConstantExpression TryInvokeValue(IAccessibleValueExpression AccessibleValueExpression)
        {
            try
            {
                return InvokeValue(AccessibleValueExpression);
            }
            catch (System.Exception)
            {
                return null;
            }

        }

        private static System.Linq.Expressions.ConstantExpression InvokeValue(IAccessibleValueExpression AccessibleValueExpression)
        {
            System.Reflection.MemberInfo mi = AccessibleValueExpression.MemberInfo;

            System.Reflection.BindingFlags bindingFlag = GetMemberBindingFlagByMemberInfoType(mi.MemberType);

            object obj = null;

            if (AccessibleValueExpression.Expression != null)
            {
                System.Linq.Expressions.ConstantExpression ce = (System.Linq.Expressions.ConstantExpression)AccessibleValueExpression.Expression;

                obj = ce.Value.GetType().InvokeMember(mi.Name, bindingFlag, null, ce.Value, AccessibleValueExpression.Args);
            }
            else
            {
                //static member
                obj = mi.ReflectedType.InvokeMember(mi.Name, bindingFlag, null, null, AccessibleValueExpression.Args);
            }

            Type objType = obj.GetType();

            //Expression.MethodCall won't accept parameter if it diiffrent from its parameter type
            if (mi.ToString().Contains("System.Object"))
            {
                objType = typeof(object);
            }

            return System.Linq.Expressions.ConstantExpression.Constant(obj, objType); 
        }

        private static System.Reflection.BindingFlags GetMemberBindingFlagByMemberInfoType(System.Reflection.MemberTypes MemberType)
        {
            switch (MemberType)
            { 
                case System.Reflection.MemberTypes.Field:
                    return System.Reflection.BindingFlags.GetField;
                case System.Reflection.MemberTypes.Method:
                    return System.Reflection.BindingFlags.InvokeMethod;
                case System.Reflection.MemberTypes.Property:
                    return System.Reflection.BindingFlags.GetProperty;
                default:
                    throw new System.ArgumentOutOfRangeException("Couldn't recognize MemberType");
            }
        }


        protected override System.Linq.Expressions.Expression VisitUnary(System.Linq.Expressions.UnaryExpression u)
        {
            return base.VisitUnary(u);
        }


        private interface IAccessibleValueExpression
        {
            System.Linq.Expressions.Expression Expression { get; }

            System.Reflection.MemberInfo MemberInfo { get; }

            object[] Args { get; }
        }

        private class MemberAccessibleValueExpression : IAccessibleValueExpression
        {
            internal MemberAccessibleValueExpression(System.Linq.Expressions.MemberExpression MemberExpression)
            {
                _memberExpression = MemberExpression;
            }

            private readonly System.Linq.Expressions.MemberExpression _memberExpression;

            #region IAccessibleValueExpression Members

            public System.Linq.Expressions.Expression Expression
            {
                get { return _memberExpression.Expression; }
            }

            public System.Reflection.MemberInfo MemberInfo
            {
                get { return _memberExpression.Member; }
            }

            public object[] Args
            {
                get { return null; }
            }

            #endregion
        }

        private class MethodCallAccessibleValueExpression : IAccessibleValueExpression
        {
            internal MethodCallAccessibleValueExpression(System.Linq.Expressions.MethodCallExpression MethodCallExpression)
            {
                _methodCallExpression = MethodCallExpression;
            }

            private readonly System.Linq.Expressions.MethodCallExpression _methodCallExpression;

            #region IAccessibleValueExpression Members

            public System.Linq.Expressions.Expression Expression
            {
                get { return _methodCallExpression.Object; }
            }

            public System.Reflection.MemberInfo MemberInfo
            {
                get { return _methodCallExpression.Method; }
            }

            public object[] Args
            {
                get 
                {

                    return _methodCallExpression.Arguments.Select(p => ((System.Linq.Expressions.ConstantExpression)p).Value).ToArray();
                
                }
            }

            #endregion
        }


    }
}
