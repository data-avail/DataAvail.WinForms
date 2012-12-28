using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XObject.XContexts
{
    public class XContext
    {
        public static XDefaultContext GetDefaultContext()
        {
            return new XDefaultContext();
        }

        public static XChildContext GetChildContext(XOFieldContext ParentFieldContext)
        {
            return new XChildContext(ParentFieldContext);
        }

        public static XFkSelectItemContext GetFkItemSelectContext(XOFieldContext XOFieldContext)
        {
            return new XFkSelectItemContext(XOFieldContext);
        }

        public static XFkAddItemContext GetFkAddItemContext(XOFieldContext XOFieldContext)
        {
            return new XFkAddItemContext(XOFieldContext);
        }

    }
}
