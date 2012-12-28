using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.WinUtils.Log
{
    public class Logger : System.IO.TextWriter
    {
        public override void Write(string value)
        {
            System.Diagnostics.Debug.WriteLine(value);
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.Default; }
        }
    }
}
