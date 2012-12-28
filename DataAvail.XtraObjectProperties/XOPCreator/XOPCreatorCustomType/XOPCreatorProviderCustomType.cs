using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraObjectProperties.XOPCreator.XOPCreatorCustomType
{
    public class XOPCreatorProviderCustomType : DataAvail.XtraObjectProperties.XOPCreator.XOPCreatorProviderBase<Type, System.Reflection.PropertyInfo>
    {
        public XOPCreatorProviderCustomType(IEnumerable<Type> ItemTypes)
            : base(ItemTypes.AsEnumerable())
        { 
        }

        protected override IEnumerable<System.Reflection.PropertyInfo> GetFieldItems(Type ObjectItem)
        {
            return ObjectItem.GetProperties().AsEnumerable();
        }

        protected override XtraObjectProperties GetObjectProperties(Type ObjectItem)
        {
            return new XtraObjectProperties(ObjectItem.Name, ObjectItem);
        }

        protected override XtraFieldProperties GetFieldPropertiesCore(System.Reflection.PropertyInfo FieldItem)
        {
            return new XtraFieldProperties(CurrentXtraObject, FieldItem.Name, FieldItem.PropertyType);
        }

        protected override IFKFieldProperties GetFKFieldProperties(XtraFieldProperties XtraFieldProperties, System.Reflection.PropertyInfo Item)
        {
            return FKFieldProperties.GetFKFieldProperties(Item, XtraFieldProperties);
        }
    }
}
