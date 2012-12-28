using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XObject.XContexts
{
    public class XFkAddItemContext : XFkSelectItemContext
    {
        public XFkAddItemContext(XOFieldContext ChildFieldContext)
            : base(ChildFieldContext)
        {
        }
    }
}
