using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject.XOP;
using DataAvail.XObject.XWP;
using DataAvail.XObject.XSP;


namespace DataAvail.XObject
{
    public class XOFunction
    {
        public XOFunction(XOTable XOTable, XOPFunction XOPFunction)
        {
            _xOTable = XOTable;

            _xOPFunction = XOPFunction;

            _params = XOPFunction.Params.Select(p => new XOFunctionParam(this, p)).ToArray();
        }

        private readonly XOTable _xOTable;

        private readonly XOPFunction _xOPFunction;

        private readonly XOFunctionParam[] _params;

        public XOTable XOTable
        {
            get { return _xOTable; }
        } 

        public XOPFunction XOPFunction
        {
            get { return _xOPFunction; }
        } 

        public XOFunctionParam[] Params
        {
            get { return _params; }
        }

        public string Name
        {
            get { return XOPFunction.Name; }
        }

    }
}
