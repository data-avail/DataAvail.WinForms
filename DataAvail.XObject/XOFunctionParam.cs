using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject.XOP;
using DataAvail.XObject.XWP;
using DataAvail.XObject.XSP;
using System.Data;


namespace DataAvail.XObject
{
    public class XOFunctionParam
    {
        public XOFunctionParam(XOFunction XOFunction, XOPFunctionParam XOPFunctionParam)
        {
            _xOFunction = XOFunction;

            _xOPFunctionParam = XOPFunctionParam;
        }

        private readonly XOFunction _xOFunction;

        private readonly XOPFunctionParam _xOPFunctionParam;

        public string Name
        {
            get { return null; }
        }

        public Type Type
        {
            get { return null; }
        }

        public int Size
        {
            get { return 1; }
        }

        public ParameterDirection Direction
        {
            get { return ParameterDirection.Input; }
        }

        public object Value
        {
            get { return 1; }
        }


    }
}
