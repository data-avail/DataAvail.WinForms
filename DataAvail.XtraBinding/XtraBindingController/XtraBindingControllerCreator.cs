using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baio.XtraBinding.XtraBindingController
{
    public static class XtraBindingCreator
    {
        public static XtraBindingListController CreateListController(Baio.XtraObjectProperties.XtraObjectProperties XtraObjectProperties, IXtraBindingControllerProvider ControllerProvider)
        {
            Baio.XOP.AppContext.AppItemContext appItemContext = XtraObjectProperties.Container.AppContext.GetAppItemContext(XtraObjectProperties.ObjectName, new Baio.XOP.AppContext.DefaultContext());

            return new XtraBindingListController(CreateXtraBindingSource(XtraObjectProperties, ControllerProvider, null, appItemContext), appItemContext);
        }

        private static XtraBinding CreateXtraBindingSource(Baio.XtraObjectProperties.XtraObjectProperties XtraObjectProperties, IXtraBindingControllerProvider ControllerProvider, XtraBindingChildProperties ChildProperties, Baio.XOP.AppContext.AppItemContext AppItemContext)
        {
            object dataSource = ControllerProvider.GetDataSource(XtraObjectProperties);

            Baio.Data.IDataAdapter dataAdapter = ControllerProvider.GetDataAdapter(XtraObjectProperties);

            Baio.XtraBinding.XtraBindingOperationForm oper = new Baio.XtraBinding.XtraBindingOperationForm(dataSource, dataAdapter, ControllerProvider.GetObjectCalculator(XtraObjectProperties));

            XtraBinding xtraBinding = null;

            if (XtraObjectProperties.ShownChildrenRelations.Count() > 0)
            {
                xtraBinding = new Baio.XtraBinding.XtraBindingContainer(dataSource, oper);

                foreach (Baio.XtraObjectProperties.XtraObjectRelation rel in XtraObjectProperties.ShownChildrenRelations)
                {
                    Baio.XtraBinding.XtraBinding childBinding = CreateXtraBindingSource(rel.ChildObject, ControllerProvider, new XtraBindingChildProperties(
                        xtraBinding, rel.ParentObject.ObjectName, rel.ParentField.FieldName, rel.ChildObject.ObjectName, rel.ChildField.FieldName, rel.ChildObject.ItemType), AppItemContext.GetChild(rel.ChildObject.ObjectName));

                    ((XtraBindingContainer)xtraBinding).Children.Add((XtraBindingChild)childBinding);
                }
            }
            else if (ChildProperties != null)
            {
                xtraBinding = new Baio.XtraBinding.XtraBindingChild(dataSource, oper, ChildProperties);
            }
            else
            {
                xtraBinding = new XtraBinding(dataSource, oper);
            }

            XtraBindingItemController itemController = new XtraBindingItemController(xtraBinding, AppItemContext);

            ((XtraBindingOperationForm)xtraBinding.xtraBindingOperation).Form = ControllerProvider.CreateReportForm(itemController, AppItemContext);

            return xtraBinding;
        }
    }
}
