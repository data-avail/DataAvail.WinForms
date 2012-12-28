using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;

namespace DataAvail.XOP.ModelSecurity
{
    public class ModelSecField : ModelSecBase
    {
        internal ModelSecField(XElement XSec)
            : base(XSec)
        { 
        }
    }
}
