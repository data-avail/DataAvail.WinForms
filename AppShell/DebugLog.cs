using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.AppShell
{
    public class DebugLog : System.IO.TextWriter
    {
        public override void WriteLine(string value)
        {
            System.Diagnostics.Debug.WriteLine(value); 
        }

        public override void Write(string value)
        {
            System.Diagnostics.Debug.Write(value); 
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
