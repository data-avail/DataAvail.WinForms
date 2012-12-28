using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;

namespace DataAvail.Controllers.Binding
{
    /// <summary>
    /// Controller control default properties
    /// </summary>
    public class CCBindingProperties
    {
        internal CCBindingProperties()
        {
        }

        private XOFieldContext _fieldContext;

        internal void Reset(XOFieldContext FieldContext)
        {
            _fieldContext = FieldContext;
        }

        private XOFieldContext FieldContext
        {
            get { return _fieldContext; }
        }

        public bool ReadOnly
        {
            get { return !FieldContext.IsCanEdit; }
        }

        public System.Drawing.Color BackColor
        {
            get { return System.Drawing.Color.White; }
        }

        public string Mask
        {
            get { return FieldContext.Mask != null ? FieldContext.Mask.Format() : null; }
        }
    }
}
