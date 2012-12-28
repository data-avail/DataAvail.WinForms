using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;
using DataAvail.XObject.XContexts;
using DataAvail.XtraBindings;

namespace DataAvail.Controllers
{
    internal class ItemSelectorController : Controller
    {
        internal ItemSelectorController(XtraBinding XtraBinding, IControllerDynamicContext ControllerDynamicContext) :
            base(XtraBinding, ControllerDynamicContext)
        {
            XORelation rel = this.TableContext.GetChildRelation(this.ChildFieldContext.XOField);

            _valueFieldName = rel.ParentField.Name;
        }

        private readonly string _valueFieldName;

        private object _result;

        private XFkSelectItemContext ItemSelectorContext { get { return (XFkSelectItemContext)TableContext.Context; } }

        private XOFieldContext ChildFieldContext { get { return ItemSelectorContext.childFieldContext; } }

        private bool IsSelectItemContext
        {
            get { return TableContext.Context.GetType() == typeof(XFkSelectItemContext); }
        }
             
        internal object ShowAsItemSelector(object Value)
        {
            _result = Value;

            this.Commands.Execute(DataAvail.Controllers.Commands.ControllerCommandTypes.ListShow);

            return _result;
        }

        internal object ShowAsAddItem(object Value)
        {
            _result = Value;

            this.Commands.Execute(DataAvail.Controllers.Commands.ControllerCommandTypes.Add);

            return _result;
        }

        protected override bool CurrentAcceptChanges()
        {
            if (!IsSelectItemContext)
            {
                if (base.CurrentAcceptChanges())
                {
                    _result = this.XtraBinding.GetCurrentItemValue(_valueFieldName);

                    return this.Commands.Execute(DataAvail.Controllers.Commands.ControllerCommandTypes.ItemClose);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return base.CurrentAcceptChanges();
            }
        }

        protected override bool ItemSelect(DataAvail.Controllers.Commands.IControllerCommandItem CommandItem)
        {
            _result = this.XtraBinding.GetCurrentItemValue(_valueFieldName);

            return this.Commands.Execute(DataAvail.Controllers.Commands.ControllerCommandTypes.ListClose);
        }

        protected override void OnListUIDataBound()
        {
            base.OnListUIDataBound();

            if (_result != System.DBNull.Value)
                this.XtraBinding.SetItemByValue(_valueFieldName, _result);

        }


    }
}
