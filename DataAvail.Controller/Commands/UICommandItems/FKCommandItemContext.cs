using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;

namespace DataAvail.Controllers.Commands.UICommandItems
{
    public class FKCommandItemContext
    {
        public FKCommandItemContext(XOFieldContext FieldContext, string Filter)
        {
            this.fieldContext = FieldContext;

            this.filter = Filter;
        }

        public readonly XOFieldContext fieldContext;

        public readonly string filter;
    }
}
