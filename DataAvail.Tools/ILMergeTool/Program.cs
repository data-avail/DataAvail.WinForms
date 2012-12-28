using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DataAvail.Linq;
using System.Diagnostics;

namespace DataAvail.ILMergeTool
{
    class Program
    {
        private const string ILMergePath = @"C:\ILMerge\ILMerge.exe";

        private const string TargetFolder = @"C:\DataAvail\DataAvail.AppShell\bin\Release";

        private const string OutputFile = @"C:\DataAvail.Projects\DataAvail.AppShell\AppShell.exe";

        static void Main(string[] args)
        {
            string prms = string.Format("/lib:{0} /ndebug /target:winexe /out:{1} {2}",
                TargetFolder, OutputFile, GetInputFilesString(TargetFolder));

            Process.Start(ILMergePath, prms);
        }

        private static string GetInputFilesString(string FolderPath)
        {
            return GetInputFiles(FolderPath).Where(p => !p.Contains(".vshost.exe") && p.Contains("DataAvail.")).ToString(" ");
        }

        private static IEnumerable<string> GetInputFiles(string FolderPath)
        {
            return DataAvail.Utils.IO.Directory.GetFilesEx(FolderPath, "*.exe|*.dll").Select(p => Path.GetFileName(p));
        }
    }
}
