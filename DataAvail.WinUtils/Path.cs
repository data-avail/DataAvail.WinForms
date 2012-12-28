using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.WinUtils
{
    public static class Path
    {
        public static string GetFilePath(string FilePath, string FileName)
        {
            return string.Format("{0}\\{1}\\{2}", System.AppDomain.CurrentDomain.BaseDirectory, FilePath, FileName);
        }


        public static string GetFilePath(string FileName)
        {
            return string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, FileName);
        }
    }
}
