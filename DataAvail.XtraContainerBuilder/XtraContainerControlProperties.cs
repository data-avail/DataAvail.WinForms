using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;

namespace DataAvail.XtraContainerBuilder
{
    public class XtraContainerControlProperties
    {
        internal XtraContainerControlProperties(object Control, XtraContainerBuilderControlType ControlType, XOFieldContext FieldContext)
        {
            control = Control;

            controlType = ControlType;

            fieldContext = FieldContext;
        }

        public readonly object control;

        public readonly XtraContainerBuilderControlType controlType;

        public readonly XOFieldContext fieldContext;
    }
}
