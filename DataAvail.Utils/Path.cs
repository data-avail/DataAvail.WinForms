using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DataAvail.Utils
{
    public static class Path
    {
        public static string BasePath = null;

        public static string GetFullPath(string FilePath)
        {
            return GetFullPath(BasePath, FilePath);
        }

        public static string GetFullPath(string BasePath, string FilePath)
        {
            if (!string.IsNullOrEmpty(BasePath))
                return System.IO.Path.GetFullPath(string.Format("{0}/{1}", BasePath, FilePath));
            else
                return System.IO.Path.GetFullPath(FilePath); 
        }

        public static string GetFullPath(params string[] Paths)
        {
            if (Paths.Length < 2)
                throw new ArgumentException("Paths should contain 2 elements at least.");

            string path = GetFullPath(Paths[0], Paths[1]);

            foreach (string str in Paths.Skip(2).Where(p=>!string.IsNullOrEmpty(p)))
            {
                path = GetFullPath(path, str);
            }

            return path;

        }

    }
}
