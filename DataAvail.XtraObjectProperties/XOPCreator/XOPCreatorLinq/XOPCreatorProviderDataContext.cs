using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.Utils.Linq;

namespace DataAvail.XtraObjectProperties.XOPCreator.XOPCreatorLinq
{
    public class XOPCreatorProviderDataContext : DataAvail.XtraObjectProperties.XOPCreator.XOPCreatorCustomType.XOPCreatorProviderCustomType
    {                
        public XOPCreatorProviderDataContext(object DataContext)
            : base(GetTablesGetters(DataContext).Select(p => p.PropertyType.GetGenericArguments()[0]))
        {
            _dataContext = DataContext;
        }
         
        private readonly object _dataContext;

        private IEnumerable<System.Reflection.PropertyInfo> _associatedProps;

        private static IEnumerable<System.Reflection.PropertyInfo> GetTablesGetters(object DataContext)
        {
            return from x in
                        (
                            from s in DataContext.GetType().GetProperties()
                            where s.PropertyType.GetInterface("ITable", false) != null
                            select s
                            )
                    let p = x.PropertyType.GetGenericArguments()
                    where p.Where(s => s.GetCustomAttributes(typeof(System.Data.Linq.Mapping.TableAttribute), false).Length != 0).Count() != 0
                    select x;
        }


        protected override IEnumerable<System.Reflection.PropertyInfo> GetFieldItems(Type ObjectItem)
        {
            _associatedProps = ObjectItem.GetProperties().WhereCustomAttributes<System.Data.Linq.Mapping.AssociationAttribute>(p=>p.IsForeignKey == true);
 
            return ObjectItem.GetProperties().Where(p => p.GetCustomAttributes(typeof(System.Data.Linq.Mapping.ColumnAttribute), false).Length != 0);
        }

        protected override IFKFieldProperties GetFKFieldProperties(XtraFieldProperties XtraFieldProperties, System.Reflection.PropertyInfo Item)
        {
            System.Reflection.PropertyInfo pi = _associatedProps.WhereCustomAttributes<System.Data.Linq.Mapping.AssociationAttribute>(p => p.ThisKey == XtraFieldProperties.FieldName).SingleOrDefault();

            if (pi != null)
            {
                return new FKFieldProperties(_dataContext, GetTablesGetters(_dataContext).Single(p => p.PropertyType.GetGenericArguments()[0] == pi.PropertyType), XtraFieldProperties);
            }
            else
            {
                return null;
            }
        }
    }

}
