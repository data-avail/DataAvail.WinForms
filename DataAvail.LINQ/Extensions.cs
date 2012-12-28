using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Linq
{
    public static class Extensions
    {

        public static Dictionary<T1, T2> AsDictionary<T1, T2>(this IEnumerable<T1> source1, IEnumerable<T2> source2)
        {
            return source1.Zip(source2, (p1, p2) => new KeyValuePair<T1, T2>(p1, p2)).ToDictionary(p => p.Key, s => s.Value);
        }

        //Code is taken fom : http://weblogs.thinktecture.com/cnagel/2010/02/linq-with-net-4-zip.html 
        public static IEnumerable<TResult> Zip<T1, T2, TResult>(this IEnumerable<T1> source1, IEnumerable<T2> source2, Func<T1, T2, TResult> func)
        {
            using (var iter1 = source1.GetEnumerator())
            using (var iter2 = source2 != null ? source2.GetEnumerator() : null)
            {
                while (iter1.MoveNext())
                {
                    yield return func(iter1.Current, iter2 != null && iter2.MoveNext() ? iter2.Current : (T2)(object)null);
                }
            }
        }

        public static string ToString<T1, T2>(this IEnumerable<T1> source1, IEnumerable<T2> source2, IEnumerable<Func<T1, string>> Formatters, string NameValueSepar, string PairSepar)
        {
            string nameValFormatter = string.Format("{{0}}{0}", NameValueSepar);

            IEnumerable<string> items = items = source1.Zip(Formatters, (p, s) => p == null || (object)p == System.DBNull.Value ? null : (s != null ? s.Invoke(p) : p.ToString()));

            IEnumerable<string> r = items.Zip(source2,
                (i, c) => i != null && (object)i != System.DBNull.Value ?
                    string.Format("{0}{1}{2}", c != null ? string.Format(nameValFormatter, c) : null, i, i != items.Last() ? PairSepar : null) : null);


            return new string(r.SelectMany(p => p != null ? p : "").ToArray());
        }

        public static string ToString<T1, T2>(this IEnumerable<T1> source1, IEnumerable<T2> source2, IEnumerable<Func<T1, string>> Formatters)
        {
            return ToString(source1, source2, Formatters, " = ", ", ");
        }

        /*
        public static string ToSting<T1>(this IEnumerable<T1> source1, IEnumerable<Func<T1, string>> Formatters, params string[] Params)
        {
            return ToSting(source1, Params.AsEnumerable(), Formatters);
        }
         */

        public static string ToString<T1>(this IEnumerable<T1> source1, bool ThisIsParams, params string[] Params)
        {
            return ToString(source1, Params.AsEnumerable(), null);
        }

        public static string ToString<T1>(this IEnumerable<T1> source1, string Separ)
        {
            return ToString(source1, (Func<T1, string>)null, Separ);
        }


        public static string ToString<T1>(this IEnumerable<T1> source1, Func<T1, string> Formatter, string PairSepar)
        {
            return ToString(source1, (IEnumerable<object>)null, new ConstEnumerable<Func<T1, string>>(Formatter), (string)null, (string)PairSepar);
        }

        public static string ToString(this System.Data.DataRow DataRow, IEnumerable<Func<object, string>> Formatters, params string[] Columns)
        {
            return ToString(Columns.Select(p => DataRow[p]), DataRow.Table.Columns.Cast<System.Data.DataColumn>().Where(p => Columns.Contains(p.ColumnName)).Select(p => !string.IsNullOrEmpty(p.Caption) ? p.Caption : p.ColumnName), Formatters);
        }


        public static string ToString(this System.Data.DataRow DataRow, params string[] Columns)
        {
            return ToString(Columns.Select(p => DataRow[p]), DataRow.Table.Columns.Cast<System.Data.DataColumn>().Where(p => Columns.Contains(p.ColumnName)).Select(p => !string.IsNullOrEmpty(p.Caption) ? p.Caption : p.ColumnName), null);
        }

        public static int GetFirstMissed(this IEnumerable<int> Seq)
        {
            IEnumerable<int> seq = Seq.OrderBy(p => p);

            //http://stackoverflow.com/questions/1098601/efficient-way-to-get-first-missing-element-in-ordered-sequence
            int res = seq.TakeWhile((x, i) => x == i).LastOrDefault();

            if (res == 0 && (seq.Count() == 0 || seq.FirstOrDefault() != 0))
            {
                res = -1;
            }

            return res + 1;
        }

        public static string TrimToLength(this string Str, int Length)
        {
            if (Str.Length <= Length)
            {
                return Str;
            }
            else
            {
                return Str.Remove(Length - 3).PadRight(Length, '.');
            }
        }

        public static bool AtLeastOneStraying<T, TResult>(this IEnumerable<T> Seq, Func<T, TResult> Selector)
        {
            return AtLeastOneStraying(Seq.Select(Selector));
        }

        public static bool AtLeastOneStraying<T>(this IEnumerable<T> Seq)
        {
            return Seq.Distinct().Count() > 1;
        }

        internal class ConstEnumerable<T> : IEnumerable<T>
        {
            public ConstEnumerable(T Object)
            {
                _object = Object;
            }

            private readonly T _object;

            #region IEnumerable<T> Members

            public IEnumerator<T> GetEnumerator()
            {
                return new ConstEnumerator<T>(_object);
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return new ConstEnumerator<T>(_object);
            }

            #endregion

            private class ConstEnumerator<Y> : IEnumerator<Y>
            {
                internal ConstEnumerator(Y Object)
                {
                    _object = Object;
                }

                private readonly Y _object;

                #region IEnumerator<Y> Members

                public Y Current
                {
                    get { return _object; }
                }

                #endregion

                #region IDisposable Members

                public void Dispose()
                {
                }

                #endregion

                #region IEnumerator Members

                object System.Collections.IEnumerator.Current
                {
                    get { return _object; }
                }

                public bool MoveNext()
                {
                    return true;
                }

                public void Reset()
                {

                }

                #endregion
            }
        }

        public static void AddRange<T>(this IList<T> List, IEnumerable<T> Seq)
        {
            foreach (T item in Seq)
                List.Add(item);
        }

        public static void AddRange(this System.Collections.IList List, System.Collections.IEnumerable Seq)
        {
            foreach (object item in Seq)
                List.Add(item);
        }

        public static string ToWhere(this IEnumerable<System.Data.DataRow> DataRows)
        {
            System.Data.DataRow firstRow = DataRows.FirstOrDefault();

            if (firstRow != null)
            {
                string pkName = firstRow.Table.PrimaryKey[0].ColumnName;

                return string.Format("{0} IN ({1})", pkName, string.Format(DataRows.ToString(p => p[pkName, p.RowState == System.Data.DataRowState.Deleted ? System.Data.DataRowVersion.Original : System.Data.DataRowVersion.Current].ToString(), ",")));
            }
            else
            {
                return null;
            }
        }

        /*
        public static string EnumToString<T>(this IEnumerable<T> Enum, string Separ)
        {
            var r = Enum.Select(p=>p.ToString()).Aggregate((p, s) => string.Format("{0}{1}{2}", p, Separ, s));

            return r;
        }
         */


    }
}
