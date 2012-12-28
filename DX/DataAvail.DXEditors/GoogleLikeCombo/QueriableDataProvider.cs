using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.DXEditors
{
    public interface IQueriableDataProvider<T>
    {
        IQueryable<T> GetTopTextData(string TopText);

        IQueryable<T> GetExpressionData(string Expression, bool StartsWith);
    }

    public class QueriableDataProvider<T>
    {
        public QueriableDataProvider(IQueriableDataProvider<T> QueriableDataProvider, bool StartsWith, bool SlowButComprehensive)
        {
            _startsWith = StartsWith;

            _queriableDataProvider = QueriableDataProvider;

            _slowButComprehensive = SlowButComprehensive;
        }

        private readonly IQueriableDataProvider<T> _queriableDataProvider;

        private readonly bool _startsWith;

        private readonly bool _slowButComprehensive;

        private bool IsStartsWithProvider { get { return _startsWith; } }

        public IEnumerable<T> GetData(string Expression, int TopCount)
        {
            if (_slowButComprehensive)
            {
                var firstOnes = from p in _queriableDataProvider.GetExpressionData(Expression, true).Take(TopCount) select p;

                var likeOnes = from p in _queriableDataProvider.GetExpressionData(Expression, false).Except(firstOnes).Take(TopCount) select p;

                var nextOnes = from p in _queriableDataProvider.GetTopTextData(Expression).Except(firstOnes.Union(likeOnes)).Take(TopCount) select p;

                var otherOnes = from p in _queriableDataProvider.GetTopTextData("").Except(firstOnes.Union(likeOnes).Union(nextOnes)).Take(TopCount) select p;

                T[] r = firstOnes.ToArray();

                if (r.Length < TopCount)
                {
                    r = r.Union(firstOnes.ToArray()).ToArray();
                }

                if (r.Length < TopCount)
                {
                    r = r.Union(likeOnes.ToArray()).ToArray();
                }

                if (r.Length < TopCount)
                {
                    r = r.Union(nextOnes.ToArray()).ToArray();
                }

                if (r.Length < TopCount)
                {
                    r = r.Union(otherOnes.ToArray()).ToArray();
                }

                return r.Take(TopCount);
            }
            else
            {
                var exprOnes = from p in _queriableDataProvider.GetExpressionData(Expression, _startsWith).Take(TopCount) select p;

                var otherOnes = from p in _queriableDataProvider.GetTopTextData("").Except(exprOnes).Take(TopCount) select p;

                T[] r = exprOnes.ToArray();

                if (r.Length < TopCount)
                {
                    r = r.Union(exprOnes.ToArray()).ToArray();
                }

                if (r.Length < TopCount)
                {
                    r = r.Union(otherOnes.ToArray()).ToArray();
                }

                return r;
            }
        }

        public int GetHighligh(string Text, string Expression)
        {
            int startIndex = -1;

            if (IsStartsWithProvider)
                startIndex = Text.ToUpper().StartsWith(Expression.ToUpper()) ? 0 : -1;
            else
                startIndex = Text.ToUpper().IndexOf(Expression.ToUpper());

            return startIndex;
        }


        public static IEnumerable<T> GetData(IQueriableDataProvider<T> QueriableDataProvider, bool StartsWith, string TopText, string Expression, int TopCount)
        {
            QueriableDataProvider<T> prr = new QueriableDataProvider<T>(QueriableDataProvider, StartsWith, false);

            return prr.GetData(Expression, TopCount);
        }
    }
}
