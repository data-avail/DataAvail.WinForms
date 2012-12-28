using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using DataAvail.Utils.Linq;
 

namespace DataAvail.XtraObjectProperties.XOPCreator.XOPCreatorCustomType
{
    public class FKFieldProperties : DataAvail.XtraObjectProperties.XOPCreator.FKFieldPropertiesBase
    {
        public FKFieldProperties (object PropertyObject, PropertyInfo PropertyInfo, XtraFieldProperties XtraFieldProperties)
            : base(XtraFieldProperties)
        {
            object dataSource = PropertyInfo.GetValue(PropertyObject, BindingFlags.GetProperty, null, null, null);

            this.ValueMember = GetValueMember(PropertyInfo, XtraFieldProperties.FieldType);

            this.DisplayMember = GetDisplayMember(PropertyInfo);

            if (this.DisplayMember == null)
                this.DisplayMember = this.ValueMember;

            SetDataSource(dataSource, null);
        }

        protected virtual string GetValueMember(PropertyInfo PropertyInfo, Type PropertyType)
        {
            Type itemType = DataAvail.Utils.Reflection.GetItemType(PropertyInfo.PropertyType);

            return DataAvail.Utils.Reflection.GetGetters(itemType, BindingFlags.Instance).Where(p => p.PropertyType == PropertyType).Select(p => p.Name).FirstOrDefault();        
        }

        protected virtual string GetDisplayMember(PropertyInfo PropertyInfo)
        {
            Type itemType = DataAvail.Utils.Reflection.GetItemType(PropertyInfo.PropertyType);

            return DataAvail.Utils.Reflection.GetGetters(itemType, BindingFlags.Instance).Where(p => p.PropertyType == typeof(string)).Select(p => p.Name).FirstOrDefault();
        }

        public static FKFieldProperties GetFKFieldProperties(System.Reflection.PropertyInfo AssociatedPropertyInfo, XtraFieldProperties XtraFieldProperties)
        {
            System.Reflection.PropertyInfo pi = DataAvail.Utils.Reflection.GetGetters(AssociatedPropertyInfo.ReflectedType, BindingFlags.Static).WhereCustomAttributes<FKDataSourceAttribute>(p => p.ThisField == AssociatedPropertyInfo.Name).FirstOrDefault();

            if (pi != null)
                return new FKFieldProperties(null, pi, XtraFieldProperties);
            else
                return null;
        }

    }
}
