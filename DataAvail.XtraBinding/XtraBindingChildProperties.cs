using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraBindings
{
    public class XtraBindingChildProperties
    {
        public XtraBindingChildProperties(XtraBinding ParentBinding, string ParentObjectName, string ParentFieldName, string ChildObjectName, string FkFieldName)
        {
            parentBinding = ParentBinding;

            parentFieldName = ParentFieldName;

            fkFieldName = FkFieldName;

            childObjectName = ChildObjectName;

            parentObjectName = ParentObjectName;
        }

        public readonly XtraBinding parentBinding;

        public readonly string parentObjectName;

        public readonly string parentFieldName;

        public readonly string childObjectName;

        public readonly string fkFieldName;
    }
}
