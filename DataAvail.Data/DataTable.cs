using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.Linq;

namespace DataAvail.Data
{
    public static class DataTable
    {
        public static string GetSelectCommandText(string SchemeName, string SourceName, IEnumerable<string> Columns)
        {
            string sourceName = SourceName;

            string str = Columns.ToString(p => p, ",");

            return !string.IsNullOrEmpty(str) ? 
                string.Format("SELECT {0} FROM {1}{2}", str, !string.IsNullOrEmpty(SchemeName) ? SchemeName + "." : null, sourceName) :
                null;
        }
    }
}
