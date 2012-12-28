using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraObjectProperties
{
    public class XtraObjectRelation
    {
        internal XtraObjectRelation(XtraObjectProperties ParentObject, XtraFieldProperties ParentField,
            XtraObjectProperties ChildObject, XtraFieldProperties ChildField, bool IsShown,
            XOP.XOPFieldValueCollection DefaultValues, string SerializationName)
        {
            this.IsShown = IsShown;

            this.ParentObject = ParentObject;

            this.ParentField = ParentField;

            this.ChildObject = ChildObject;

            this.ChildField = ChildField;

            this.DefaultValues = DefaultValues;

            this.SerializationName = SerializationName;
        }

        public readonly XtraObjectProperties ParentObject;

        public readonly XtraFieldProperties ParentField;

        public readonly XtraObjectProperties ChildObject;

        public readonly XtraFieldProperties ChildField;

        public readonly bool IsShown;

        public readonly XOP.XOPFieldValueCollection DefaultValues;

        public readonly string SerializationName;

    }
}
