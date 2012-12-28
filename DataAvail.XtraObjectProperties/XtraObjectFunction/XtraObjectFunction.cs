using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraObjectProperties
{
    public class XtraObjectFunction
    {
        public XtraObjectFunction(string FuncName)
        {
            funcName = FuncName;
        }

        public readonly string funcName;

        public readonly XtraObjectFunctionParameters funcParams = new XtraObjectFunctionParameters();
    }
}
