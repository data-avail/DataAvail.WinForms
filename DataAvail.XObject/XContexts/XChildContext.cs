using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XObject.XContexts
{
    public class XChildContext : XContext
    {
        public XChildContext(XOFieldContext ParentFieldContext)
        {
            _parentFieldContext = ParentFieldContext;
        }

        private XOFieldContext _parentFieldContext;

        public XOFieldContext ParentFieldContext
        {
            get { return _parentFieldContext; }

            set { _parentFieldContext = value; }
        }
    }
}
