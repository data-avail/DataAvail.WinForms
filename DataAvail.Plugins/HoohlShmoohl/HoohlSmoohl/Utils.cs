using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HoohlShmoohl
{
    public static class Utils
    {
        public static bool IsLat(string Str, bool DigitAsLat)
        { 
            Encoding iso = Encoding.GetEncoding("ISO8859-1");

            byte[] incl = iso.GetBytes(LatCharactersIncludes);

            //32 - space, 44 - comma
            byte b = iso.GetBytes(Str).FirstOrDefault(p => (p < 64 || p > 128) && !incl.Contains(p) && (!DigitAsLat || !char.IsDigit((char)p)));

            return b == default(byte);
        }

        public static char[] LatCharactersIncludes { get { return new char[] { ' ', ',' }; } }

        public static string ToString(params object[] Objs)
        {
            return Objs.Aggregate((x, p)=> (x != null ? x.ToString() : "") + (p != null && !string.IsNullOrEmpty(p.ToString()) ? string.Format(", {0}", p) : ""))
                .ToString().Trim(',',' ');
        }
    }
}
