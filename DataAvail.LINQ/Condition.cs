using System.ComponentModel;
using System.Reflection;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;
using System.Data;

//The class is simple translation from .VB code on http:\\www...
//Translation was made exclusively for learning puprpose.



namespace DataAvail.Linq
{
    public abstract class Condition
    {
        //Used to ensure we get the same instance of a particular ParameterExpression
        //across multiple queries
        private static Dictionary<string, ParameterExpression> _paramTable = new Dictionary<string, ParameterExpression>();

        //The expression tree which will be passed to the LINQ to SQL runtime
        private LambdaExpression _lambdaExpr;

        public enum Compare
        {
            Or = ExpressionType.Or,
            And = ExpressionType.And,
            Xor = ExpressionType.ExclusiveOr,
            Not = ExpressionType.Not,
            Equal = ExpressionType.Equal,
            Like = ExpressionType.TypeIs + 1,
            Upper = Like + 1,
            Lower = Like + 2,
            NotEqual = ExpressionType.NotEqual,
            OrElse = ExpressionType.OrElse,
            AndAlso = ExpressionType.AndAlso,
            GreaterThan = ExpressionType.GreaterThan,
            LessThan = ExpressionType.LessThan,
            LessThanOrEqual = ExpressionType.LessThanOrEqual,
            GreaterThanOrEqual = ExpressionType.GreaterThanOrEqual,
        }


        internal LambdaExpression LambdaExpression
        {
            get { return _lambdaExpr; }

            set { _lambdaExpr = value; }
        }

        public Expression Expression
        {
            get { return LambdaExpression.Body; }
        }

        public static Expression PropertyExpression(Expression Expression, string FieldName)
        {
            return Expression.Property(Expression, FieldName);
        }

        public static Expression CombineExpression(Expression Expression, string FieldName, Compare CondType, Expression Right)
        {
            return CombineExpression(PropertyExpression(Expression, FieldName), CondType, Right);
        }

        public static Expression CombineExpression<T>(Expression Expression, string FieldName, Compare CondType, T Value)
        {
            return CombineExpression(Expression, FieldName, CondType, Value, typeof(T));
        }

        public static Expression CombineExpression(Expression Expression, string FieldName, Compare CondType, object Value, Type Type)
        {
            return CombineExpression(PropertyExpression(Expression, FieldName), CondType, Expression.Constant(Value, Type));
        }

        public static bool IsBinaryEquality(Expression Expression)
        {
            switch (Expression.NodeType)
            {
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                return true;
            }

            return false;
        }

        public static bool IsBinaryJoiner(Expression Expression)
        {
            switch (Expression.NodeType)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return true;
            }

            return false;
        }



        #region Protected Methods

        //Combines two Expressions according to the specified operator (condType)
        public static Expression CombineExpression(Expression Left, Compare CondType, Expression Right)
        {
            switch (CondType)
            {
                case Compare.Or: return Expression.Or(Left, Right);
                case Compare.And: return Expression.And(Left, Right);
                case Compare.Xor: return Expression.ExclusiveOr(Left, Right);
                case Compare.Equal: return Expression.Equal(Left, Right);
                case Compare.OrElse: return Expression.OrElse(Left, Right);
                case Compare.AndAlso: return Expression.AndAlso(Left, Right);
                case Compare.NotEqual: return Expression.NotEqual(Left, Right);
                case Compare.LessThan: return Expression.LessThan(Left, Right);
                case Compare.GreaterThan: return Expression.GreaterThan(Left, Right);
                case Compare.LessThanOrEqual: return Expression.LessThanOrEqual(Left, Right);
                case Compare.GreaterThanOrEqual: return Expression.GreaterThanOrEqual(Left, Right);
                case Compare.Like:
                    return Expression.Call(Left, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), new Expression[] { Right });
                case Compare.Upper:
                    return Expression.Call(Left, typeof(string).GetMethods().First(p=>p.Name == "ToUpper" && p.GetParameters().Length == 0));
                case Compare.Lower:
                    return Expression.Call(Left, typeof(string).GetMethods().First(p => p.Name == "ToLower" && p.GetParameters().Length == 0));
                default:
                    throw new System.ArgumentException("Not a valid Condition Type", "condType");
            }
        }

        //Since both type parameters must be the same, we can turn what would normally
        //be a Func(Of T, T, Boolean) into a Func(Of T, Boolean)
        protected static Func<T, bool> CombineFunc<T>(Func<T, bool> d1, Compare condType, Func<T, bool> d2)
        {

            //Return a delegate which combines delegates d1 and d2
            switch (condType)
            {
                case Compare.Or: return x => d1(x) || d2(x);
                case Compare.And: return x => d1(x) && d2(x);
                case Compare.Xor: return x => d1(x) & d2(x);
                case Compare.Equal: return x => d1(x) == d2(x);
                case Compare.OrElse: return x => d1(x) || d2(x);
                case Compare.AndAlso: return x => d1(x) && d2(x);
                case Compare.NotEqual: return x => d1(x) != d2(x);
                case Compare.LessThan: return x => d1(x).CompareTo(d2(x)) < 0;
                case Compare.GreaterThan: return x => d1(x).CompareTo(d2(x)) > 0;
                case Compare.LessThanOrEqual: return x => d1(x).CompareTo(d2(x)) <= 0;
                case Compare.GreaterThanOrEqual: return x => d1(x).CompareTo(d2(x)) >= 0;
                default:
                    throw new System.ArgumentException("Not a valid Condition Type", "condType");
            }
        }

        #endregion

        protected static ParameterExpression GetParamInstance(Type Type)
        {
            if (!_paramTable.ContainsKey(Type.Name))
                _paramTable.Add(Type.Name, Expression.Parameter(Type, Type.Name));

            return _paramTable[Type.Name];
        }

        ///<summary>
        ///Creates a Condition which combines two other Conditions
        /// </summary>
        /// <typeparam name="T">The type the condition will execute against</typeparam>
        /// <param name="cond1">The first Condition</param>
        /// <param name="condType">The operator to use on the conditions</param>
        /// <param name="cond2">The second Condition</param>
        /// <returns>A new Condition which combines two Conditions into one according to the specified operator</returns>
        /// <remarks></remarks>
        public static Condition<T> Combine<T>(Condition<T> cond1, Compare condType, Condition<T> cond2)
        {
            return Condition<T>.Combine(cond1, condType, new Condition<T>[] { cond2 });
        }

        public static Condition<T> Combine<T>(Condition<T> cond1, Compare condType, params Condition<T>[] cond2)
        {
            return Condition<T>.Combine(cond1, condType, cond2);
        }


        internal static Condition<T, S> Create<T, S>(IEnumerable<T> dataSource, string propName, Compare condType, S Value)
        {
            return new Condition<T, S>(propName, condType, Value);
        }
    }

    public class Condition<T> : Condition
    {
        private Condition()
        { }

        public Condition(string propName, Compare condType, Object value, Type valueType)
        {
            string[] strs = propName.Split(new char[] { '.' });

            GetParamInstance(typeof(T));

            PropertyInfo pInfo = typeof(T).GetProperty(propName);

            ParameterExpression pe = GetParamInstance(typeof(T));

            MemberExpression me = Expression.MakeMemberAccess(pe, pInfo);

            //For each member specified, construct the additional MemberAccessExpression
            //For example, if the user says "myCustomer.Order.OrderID = 4" we need an
            //additional MemberAccessExpression for "Order.OrderID = 4"
            for (int i = 1; i < strs.Length; i++)
            {
                pInfo = pInfo.PropertyType.GetProperty(strs[i]);

                me = Expression.MakeMemberAccess(me, pInfo);
            }

            ConstantExpression constExpr = Expression.Constant(value, valueType);

            Expression expr = CombineExpression(me, condType, constExpr);

            LambdaExpression = Expression.Lambda(expr, new ParameterExpression[] { pe });

            //Compile the lambda expression into a delegate
            _del = (Func<T, bool>)LambdaExpression.Compile();
        }

        private Func<T, bool> _del;

        internal Func<T, bool> Delegator { get { return _del; } }

        private static Condition<T> Combine(Condition<T> cond1, Compare condType, Condition<T> cond2)
        {
            Condition<T> c = new Condition<T>();

            Expression expr = CombineExpression(cond1.LambdaExpression.Body, condType, cond2.LambdaExpression.Body);

            ParameterExpression pe = GetParamInstance(typeof(T));

            c.LambdaExpression = Expression.Lambda(expr, new ParameterExpression[] { pe });

            c._del = Condition.CombineFunc(cond1._del, condType, cond2._del);

            return c;
        }


        //Combines multiple conditions according to the specified operator
        private static Condition<T> Combine(Condition<T> cond1, Compare condType, params Condition<T>[] Conditions)
        {
            Condition<T> finalCond = cond1;

            foreach (Condition<T> c in Conditions)
            {
                finalCond = Condition.Combine(finalCond, condType, c);
            }

            return finalCond;
        }

        //Run query locally instead of remotely
        public bool Contains(T Item)
        {
            return _del(Item);
        }

        #region Overloaded Operators

        //Overloaded operators - allows syntax like "(condition1 Or condition2) And condition3"
        public static Condition<T> operator &(Condition<T> c1, Condition<T> c2)
        {
            return Condition<T>.Combine(c1, Compare.And, c2);
        }

        public static Condition<T> operator |(Condition<T> c1, Condition<T> c2)
        {
            return Condition<T>.Combine(c1, Compare.Or, c2);
        }

        #endregion

    }

    public class Condition<T, S> : Condition<T>
    {
        ///Represents a condition like "object.Property = value"
        ///In this case object is of type T, and value is of type S
        ///Even though most of the logic for this is already in the base class, 
        ///defining a second generic parameter means the user doesn't have to
        ///pass in a System.Type - it can just be inferred.
        public Condition(string propName, Compare condType, S value)
            : base(propName, condType, value, typeof(S))
        {
        }
    }


    public static class ConditionExtentions
    {
        /// Filters an IQueryable(Of T) according to the specified condition
        public static System.Linq.IQueryable<T> Where<T>(this System.Linq.IQueryable<T> source, Condition<T> condition)
        {
            MethodCallExpression mce = Expression.Call(typeof(System.Linq.Queryable), "Where", new Type[] { source.ElementType }, source.Expression, Expression.Quote(condition.LambdaExpression));

            return (System.Linq.IQueryable<T>)source.Provider.CreateQuery(mce);
        }
        public static IEnumerable<T> Where<T>(this IEnumerable<T> source, Condition<T> condition)
        {
            return source.Where(condition.Delegator);
        }

        ///Extension method that can be called off any type that implements IEnumerable(Of T), 
        ///which constructs a Condition with T as the element type and S as the value's type
        [EditorBrowsable]
        public static Condition<T, S> CreateCondition<T, S>(IEnumerable<T> dataSource, string propName, Condition.Compare condType, S Value)
        {
            return Condition.Create(dataSource, propName, condType, Value);
        }
    }
}