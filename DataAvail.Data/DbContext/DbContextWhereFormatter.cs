using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Data.DbContext
{
    public class DbContextWhereFormatter : DataAvail.Data.DbContext.IDbContextWhereFormatter
    {
        public DbContextWhereFormatter(string FormatString, string ValueFormatter)
        {
            _formatString = FormatString;

            _valueFormatter = ValueFormatter;
        }

        private readonly string _formatString;

        private readonly string _valueFormatter;

        #region IDbContextWhereFormatter Members

        public string Format(DateTime InvariantDateTime)
        {
            string val = InvariantDateTime.ToString(_formatString);

            if (!string.IsNullOrEmpty(_valueFormatter))
                val = string.Format(_valueFormatter, val);

            return val;
        }

        #endregion

        public static string FormatDateTime(DataAvail.Data.DbContext.IDbContextWhereFormatter WhereFormatter, string Where, string[] DateTimeColumns)
        {
            System.Text.StringBuilder sb = new StringBuilder(Where);

            int i = 0;

            foreach (string dtField in DateTimeColumns)
            {
                int fIndex = sb.ToString().IndexOf(i == 0 ? dtField : " " + dtField, i);

                while (fIndex != -1)
                {
                    string str = sb.ToString();

                    fIndex += dtField.Length;

                    //Find first alphabetic character after field's name 
                    char ch = str.ToCharArray().Skip(fIndex).FirstOrDefault(p => char.IsNumber(p));

                    if (ch != char.MinValue)
                    {
                        int dtValF = str.IndexOf(ch, fIndex);

                        if (dtValF != -1)
                        {
                            int dtValE = sb.Length;

                            //Find first not null & not numeric
                            ch = str.ToCharArray().Skip(dtValF).FirstOrDefault(p => char.IsLetter(p));

                            if (ch != char.MinValue)
                                dtValE = str.IndexOf(ch, dtValF);

                            //Substitute invariant value by formatted one

                            string dtVal = str.Substring(dtValF, dtValE - dtValF);

                            System.DateTime dt;

                            if (System.DateTime.TryParse(dtVal, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                            {
                                sb.Remove(dtValF, dtValE - dtValF);

                                sb.Insert(dtValF, WhereFormatter.Format(dt) + " ");

                                i = dtValF;

                                fIndex = sb.ToString().IndexOf(dtField, i);

                                continue;
                            }
                        }
                    }

                    fIndex = -1;
                }
            }

            return sb.ToString();
        }
    }
}
