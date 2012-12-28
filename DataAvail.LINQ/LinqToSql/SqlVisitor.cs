using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Linq.LinqToSql
{

    public class SqlVisitor : ExpressionVisitor
    {
        SqlClause _sqlClause = null;

        SqlClause _currentSqlClause = null;

        SqlStrokePart _currentSqlStrokePart = null;

        Dictionary<System.Linq.Expressions.Expression, SqlClause> _exprClause = new Dictionary<System.Linq.Expressions.Expression, SqlClause>();

        List<SqlParam> _params = new List<SqlParam>();

        public class SqlVisitorExpression
        {
            internal SqlVisitorExpression(string Command, SqlParam[] SqlParams)
            {
                command = Command;

                sqlParams = SqlParams;
            }

            public readonly string command;

            public readonly SqlParam[] sqlParams;
        }

        public static SqlVisitorExpression Eval(System.Linq.Expressions.Expression Expression)
        {
            SqlVisitor visitor = new SqlVisitor();

            visitor.Visit(DataAvail.Linq.ConstVisitor.Eval(Expression));

            return new SqlVisitorExpression(visitor._sqlClause.ToString(), visitor._params.ToArray());
        }

        
        /// <summary>
        /// Clause : COL1 = 10 AND COL2.Contains('A') 
        /// COL1 = 10 - Left clause(stroke)
        /// COL2.Contains('A') - Right clause (stroke)
        /// -------------------------------------------
        /// Clause : (COL1 = 10 OR COL2 > 100) AND COL3.Contains('A') 
        /// (COL1 = 10 OR COL2 > 100) - Left clause
        /// COL3.Contains('A') - Right clause (stroke)
        /// </summary>
        /// <param name="Expression"></param>
        private void SetCurrentSqlClause(System.Linq.Expressions.Expression Expression) 
        {
            if (_sqlClause == null)
            {
                _currentSqlClause = _sqlClause = new SqlClause(Expression);
            }

            if (_currentSqlClause.expression != Expression)
            {
                if (_exprClause.ContainsKey(Expression))
                {
                    _currentSqlClause = _exprClause[Expression];
                }
                else
                {
                    if (!_currentSqlClause.IsStroke)
                    {
                        _exprClause.Add(_currentSqlClause.sqlLeft.expression, _currentSqlClause.sqlLeft);

                        _exprClause.Add(_currentSqlClause.sqlRight.expression, _currentSqlClause.sqlRight);

                        _currentSqlClause = _exprClause[Expression];
                    }
                }
            }
        }

        private void SetCurrentSqlStrokePart(System.Linq.Expressions.Expression Expression)
        {
            if (CurrentSqlClause == null)
            {
                throw new SqlVisitorException("Stroke part can be retrieved only after parent sql clause is iterated");
            }

            if (!CurrentSqlClause.IsStroke)
            {
                throw new SqlVisitorException("Couldn't set current stroke part for clause that is not stroke");
            }

            SqlStrokePart sqlStrokePart = CurrentSqlClause.sqlStroke.GetPart(Expression);

            if (sqlStrokePart != null)
            {
                _currentSqlStrokePart = sqlStrokePart;
            }

            /*
            if (CurrentSqlStrokePart == null)
            {
                throw new SqlVisitorException("Couldn't set current stroke part");
            }
             */

        }

        private SqlClause CurrentSqlClause
        {
            get
            {
                return _currentSqlClause;
            }
        }

        private SqlStrokePart CurrentSqlStrokePart
        {
            get
            {
                return _currentSqlStrokePart;
            }
        }


        protected override System.Linq.Expressions.BinaryExpression VisitBinaryJoiner(System.Linq.Expressions.BinaryExpression b)
        {
            SetCurrentSqlClause(b);

            if (CurrentSqlClause.IsStroke)
            {
                throw new SqlVisitorException("Couldn't visit bainary joiner in clause which is stroke");
            }

            return base.VisitBinaryJoiner(b);
        }

        protected override System.Linq.Expressions.BinaryExpression VisitBinaryEquality(System.Linq.Expressions.BinaryExpression b)
        {
            SetCurrentSqlClause(b);

            if (!CurrentSqlClause.IsStroke)
            {
                throw new SqlVisitorException("Couldn't visit bainary equality in clause which is not stroke");
            }

            CurrentSqlClause.sqlStroke.condition = SqlClause.GetStorkeOperand(b.NodeType);

            return base.VisitBinaryEquality(b);
        }

        protected override System.Linq.Expressions.Expression VisitMethodCall(System.Linq.Expressions.MethodCallExpression m)
        {
            SetCurrentSqlClause(m);

            if (!CurrentSqlClause.IsStroke)
            {
                throw new SqlVisitorException("Couldn't visit method call in clause which is not stroke");
            }

            SetCurrentSqlStrokePart(m);

            if (CurrentSqlStrokePart != null && CurrentSqlStrokePart.expression == m)
            {
                SqlFunc func = SqlClause.GetFunc(m.Method);

                //func = null, means no function translation to SQL, just decoration such as Field<>()
                CurrentSqlStrokePart.funcs.Add(func);
            }

            return base.VisitMethodCall(m);
        }

        protected override System.Linq.Expressions.Expression VisitParameter(System.Linq.Expressions.ParameterExpression p)
        {
            //SqlFunc sqlFunc = this.CurrentSqlStrokePart.funcs.LastOrDefault();
            if (p.Name != null)
                CurrentSqlStrokePart.param = p.Name;

            return base.VisitParameter(p);
        }
         

        protected override System.Linq.Expressions.Expression VisitConstant(System.Linq.Expressions.ConstantExpression c)
        {
            if (!CurrentSqlClause.IsStroke)
            {
                throw new SqlVisitorException("Couldn't visit constant in clause which is not stroke");
            }

            SetCurrentSqlStrokePart(c);

            string valName = string.Format("P{0}", _params.Count + 1);

            CurrentSqlStrokePart.param = string.Format("@{0}", valName);

            object obj = c.Value;

            if (CurrentSqlClause.sqlStroke.GetType() == typeof(SqlStrokeMethodCall))
            {
                obj = ((SqlStrokeMethodCall)CurrentSqlClause.sqlStroke).TransformParam(obj);
            }

            _params.Add(new SqlParam(valName, obj, c.Type));

            return base.VisitConstant(c);
        }

        protected override System.Linq.Expressions.Expression VisitMemberAccess(System.Linq.Expressions.MemberExpression m)
        {
            if (!CurrentSqlClause.IsStroke)
            {
                throw new SqlVisitorException("Couldn't visit MemberAccess in clause which is not stroke");
            }

            SetCurrentSqlStrokePart(m);

            CurrentSqlStrokePart.param = m.Member.Name;

            return base.VisitMemberAccess(m);
        }

        protected override System.Linq.Expressions.Expression VisitUnary(System.Linq.Expressions.UnaryExpression u)
        {
            if (!CurrentSqlClause.IsStroke)
            {
                throw new SqlVisitorException("Couldn't visit unary in clause which is not stroke");
            }

            SetCurrentSqlStrokePart(u);

            return base.VisitUnary(u);

        }
    }
}
