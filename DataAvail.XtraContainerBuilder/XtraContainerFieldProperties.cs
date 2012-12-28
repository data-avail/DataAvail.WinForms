using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraContainerBuilder
{
    public class XtraContainerFieldProperties 
    {
        public XtraContainerFieldProperties(string FieldName, System.Type Type, DataAvail.XtraObjectProperties.XtraFieldProperties FieldProperties)
        {
            this.type = Type;

            this.fieldProperties = FieldProperties;

            this.fieldName = FieldName;
        }

        public readonly string fieldName;

        public readonly System.Type type;

        public readonly DataAvail.XtraObjectProperties.XtraFieldProperties fieldProperties;

        public string FieldCaption { get { return fieldProperties != null && fieldProperties.Caption != null ? fieldProperties.Caption : fieldName; } }
    }
}
