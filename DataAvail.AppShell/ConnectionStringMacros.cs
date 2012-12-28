using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.Utils;


namespace DataAvail.AppShell
{
    internal class ConnectionStringMacros
    {
        internal static string Parse(string ModelFilePath, string RawConnectionString)
        {
            StringBuilder sb = new StringBuilder(RawConnectionString);

            foreach(var macro in Macros.Where(p=>RawConnectionString.Contains(p.Key)))
            {
                
                string str = sb.ToString();

                int fm = str.IndexOf(macro.Key);

                int f = fm + macro.Key.Length + 1;

                int cnt = str.IndexOf(')', f) - f;

                string macroVal = str.Substring(f, cnt);

                object macroObjVal = macroVal;

                switch (macro.Key)
                { 
                    case "$path":
                        macroObjVal = new string[] { ModelFilePath, macroVal };
                        break;
                }

                string val = macro.Value(macroObjVal);

                sb.Remove(fm, cnt + macro.Key.Length + 2);

                sb.Insert(fm, val);
            }

            return sb.ToString();
        }

        private static IDictionary<string, Func<object, string>> Macros
        {
            get 
            {
                Dictionary<string, Func<object, string>> dict = new Dictionary<string, Func<object, string>>();

                dict.Add("$path", PathMacro);

                return dict;
            }
        }

        private object GetPathValue(string ModelFilePath, string MacroVal)
        {
            return new string[] { ModelFilePath, MacroVal };
        }

        private static string PathMacro(object MacroValues)
        {
            string[] s = (string[])MacroValues;

            return Path.GetFullPath(s[0], s[1]);
        }

        private enum MacroType
        { 
            Path
        }
    }
}
