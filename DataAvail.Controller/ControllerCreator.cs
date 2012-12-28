using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;
using DataAvail.XtraBindings;

namespace DataAvail.Controllers
{
    public static class XtraBindingCreator
    {
        public static Controller CreateController(XOTableContext TableContext, IControllerProvider ControllerProvider, Func<XtraBinding, XOTableContext, Controller> CreateControllerInstanceFunc)
        {
            return CreateControllerInstanceFunc.Invoke(CreateXtraBindingSource(TableContext, ControllerProvider, null), TableContext);
        }


        private static XtraBinding CreateXtraBindingSource(XOTableContext TableContext, IControllerProvider ControllerProvider, XtraBindingChildProperties ChildProperties)
        {
            object dataSource = ControllerProvider.GetDataSource(TableContext.XOTable);

            DataAvail.Data.DataAdapter.IDataAdapter dataAdapter = ControllerProvider.GetDataAdapter(TableContext.XOTable);

            DataAvail.XtraBindings.XtraBindingOperation oper = new DataAvail.XtraBindings.XtraBindingOperation(dataSource, dataAdapter, ControllerProvider.GetObjectCalculator(TableContext.XOTable));

            XtraBinding xtraBinding = null;

            if (TableContext.ShownChildrenRelations.Count() > 0)
            {
                xtraBinding = new DataAvail.XtraBindings.XtraBindingContainer(dataSource, oper);

                foreach (XORelation rel in TableContext.ShownChildrenRelations)
                {
                    DataAvail.XtraBindings.XtraBinding childBinding = CreateXtraBindingSource(rel.ChildTable.CreateTableChildContext(TableContext.Fields.First(p=>p.Name == rel.ParentField.Name)), ControllerProvider, new XtraBindingChildProperties(
                        xtraBinding, rel.ParentTable.Name, rel.ParentField.Name, rel.ChildTable.Name, rel.ChildField.Name));

                    ((XtraBindingContainer)xtraBinding).Children.Add((XtraBindingChild)childBinding);
                }
            }
            else if (ChildProperties != null)
            {
                xtraBinding = new DataAvail.XtraBindings.XtraBindingChild(dataSource, oper, ChildProperties);
            }
            else
            {
                xtraBinding = new XtraBinding(dataSource, oper);
            }

            return xtraBinding;
        }
    }
}
