using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Linq.LinqToSql
{
    public class SqlVisitorException : System.Exception
    {
        internal SqlVisitorException(string Message)
            : base(Message)
        { 
        
        }
    }
}
