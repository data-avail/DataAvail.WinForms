using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.AppShell
{
    public class AppShellInitializerException : System.Exception
    {
        public AppShellInitializerException(string Message)
            : base(Message)
        { }
    }
}
