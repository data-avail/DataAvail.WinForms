using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace DataAvail.Utils
{
    public static class Plugins
    {
        public static object [] LoadPlugins(string Dir, Type ClassAttrType)
        {
            List<object> list = new List<object>();

            if (Directory.Exists(Dir))
            {
                foreach (string fileName in Directory.GetFiles(Dir))
                {
                    list.Add(LoadPlugin(fileName, ClassAttrType, false));
                }
            }

            return list.ToArray();
        }

        public static object LoadPlugin(string AssemblyPath, Type ClassAttrType, bool CheckZeroFound)
        {
            Assembly a = Assembly.LoadFrom(AssemblyPath);

            Type [] types = GetAssemblyClassTypes(a, ClassAttrType);

            if (types.Length == 0 && CheckZeroFound)
                throw new Exception(string.Format("Can't find {0} class in assembly. Please confirm that in assembly exists class attributed with {0}.", ClassAttrType.Name));

            if (types.Length > 1)
                throw new Exception(string.Format("More than 1 {0} class has been found in assembly. Please confirm that in assembly exists only one class attributed with {0}.", ClassAttrType.Name));

            return Activator.CreateInstance(types[0]);
        }

        private static Type [] GetAssemblyClassTypes(Assembly Assembly, Type ClassAttrType)
        {
            return Assembly.GetTypes().Where(p => p.GetCustomAttributes(ClassAttrType, true).Count() != 0).ToArray();
        }
    }
}
