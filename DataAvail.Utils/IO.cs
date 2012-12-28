using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Utils.IO
{
    public static class Directory
    {
        public static IEnumerable<string> GetFilesEx(string DirectorName, string FilePattern)
        {
            return FilePattern.Split('|').SelectMany(p => System.IO.Directory.GetFiles(DirectorName, p)).ToArray();
        }

    }
}
