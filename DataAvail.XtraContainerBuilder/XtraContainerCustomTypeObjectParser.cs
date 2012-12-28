using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraContainerBuilder
{
    internal class XtraContainerCustomTypeObjectParser : XtraContainerBaseParser<System.Reflection.PropertyInfo> 
    {
        internal XtraContainerCustomTypeObjectParser(Type CustomType, DataAvail.XtraObjectProperties.IXtraObjectPropertiesProvider XtraObjectPropertiesProvider)
            : base(XtraObjectPropertiesProvider, CustomType.GetProperties().Cast<System.Reflection.PropertyInfo>().GetEnumerator())
        {
        }

        protected override XtraContainerFieldProperties GetFieldProperties(System.Reflection.PropertyInfo Item)
        {
            return new XtraContainerFieldProperties(Item.Name, Item.PropertyType, XtraObjectPropertiesProvider == null ? null : XtraObjectPropertiesProvider.GetFieldProperties(Item.Name));
        }

    }
}
