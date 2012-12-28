using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;
using DataAvail.XObject.XContexts;

namespace DataAvail.Controllers
{
    partial class Controller
    {
        public class ItemSelector
        {
            internal ItemSelector(Controller Controller)
            {
                _controller = Controller;
            }

            private readonly Controller _controller;

            private ControllerDynamicContext GetDynamicContextByItem(XOTableContext XOTableContext)
            {
                return null;
            }

            public object AddItem(DataAvail.Controllers.Commands.IControllerCommandItem CommandItem)
            {
                return SelectItem(CommandItem, false);
            }

            public object SelectItem(DataAvail.Controllers.Commands.IControllerCommandItem CommandItem)
            {
                return SelectItem(CommandItem, true);
            }

            private object SelectItem(DataAvail.Controllers.Commands.IControllerCommandItem CommandItem, bool ListSelector)
            {
                DataAvail.Controllers.Commands.UICommandItems.FKCommandItemContext commandContext = (DataAvail.Controllers.Commands.UICommandItems.FKCommandItemContext)CommandItem.Context;

                return FkItemAction(commandContext.fieldContext, CommandItem.Argument, ListSelector, commandContext.filter);
            }

            public object FkItemAction(XOFieldContext ChildFieldContext, object Value, bool ListSelector, string Filter)
            {

                XContext context = ListSelector ?
                    XContext.GetFkItemSelectContext(ChildFieldContext) :
                    XContext.GetFkAddItemContext(ChildFieldContext);


                //Select all Controllers, which parents relation's SeraializationName are the same.

                ItemSelectorController controller = Controller.controllerContext.Controllers.OfType<ItemSelectorController>()
                    .FirstOrDefault(p =>
                    p.TableContext.Context.GetType() == context.GetType() &&
                    ((XFkSelectItemContext)p.TableContext.Context).childFieldContext.ParentRelationTable ==
                    ChildFieldContext.ParentRelationTable &&
                    ((XFkSelectItemContext)p.TableContext.Context).childFieldContext.ParentRelation.SerializationName ==
                    ChildFieldContext.ParentRelation.SerializationName);

                if (controller == null)
                {
                    XOTableContext parTableContext = ChildFieldContext.ParentRelationTable.CreateTableContext(context);

                    ControllerDynamicContext dynamicContext = new ControllerDynamicContext(parTableContext);

                    controller = (ItemSelectorController)Controller.controllerContext.
                        AddController(parTableContext, (p, x) => new ItemSelectorController(p, dynamicContext));
                }
                else
                {
                    //Just switch coontext for the same table
                    ControllerDynamicContext dynamicContext = (ControllerDynamicContext)controller.DynamicContext;

                    XOTableContext appItemContext  = dynamicContext.GetAppItemContext(context);

                    if (appItemContext == null)
                    {
                        appItemContext = controller.TableContext.XOTable.CreateTableContext(context);

                        dynamicContext.AddAppItemContext(appItemContext, true);
                    }
                    else
                    {
                        dynamicContext.SetCurrentAppItemContext(appItemContext);
                    }
                }

                string totFilter = ChildFieldContext.ParentRelation.Filter;

                if (!string.IsNullOrEmpty(totFilter))
                {
                    if (!string.IsNullOrEmpty(Filter) && totFilter != Filter)
                        totFilter = string.Format("{0} AND {1}", totFilter, Filter);
                }
                else
                {
                    totFilter = Filter;
                }

                controller.SetDataSourceFilter(totFilter);

                if (ListSelector)
                    return controller.ShowAsItemSelector(Value);
                else
                    return controller.ShowAsAddItem(Value);
            }
        }
                
    }
}
