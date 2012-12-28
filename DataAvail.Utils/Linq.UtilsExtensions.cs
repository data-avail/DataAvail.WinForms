using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.Linq;
using DataAvail.Utils;

namespace DataAvail.Utils.Linq
{
    public static class UtilsExtensions
    {
        public static IEnumerable<System.Reflection.PropertyInfo> WhereCustomAttributes<T>(this IEnumerable<System.Reflection.PropertyInfo> PropertyInfos, Func<T, bool> Func) where T : System.Attribute
        {
            return from s in PropertyInfos
                   let x = Reflection.GetCustomAttribute<T>(s)
                   where x != null && Func(x) == true
                   select s;
        }



    }
}
