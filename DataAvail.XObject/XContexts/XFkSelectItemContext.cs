using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XObject.XContexts
{
    public class XFkSelectItemContext : XContext
    {
        public XFkSelectItemContext(XOFieldContext ChildFieldContext)
        {
            this.childFieldContext = ChildFieldContext;
        }

        public readonly XOFieldContext childFieldContext;
    }
}
