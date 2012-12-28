using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;

namespace DataAvail.Controllers.Binding
{
    internal class ControllerDataSourcesCollection
    {
        internal ControllerDataSourcesCollection(Controller Controller)
        {
            _controller = Controller;
        }

        private readonly Controller _controller;

        private readonly Dictionary<XOFieldContext, ControllerDataSource> _items = new Dictionary<XOFieldContext, ControllerDataSource>();

        public Controller Controller
        {
            get { return _controller; }
        } 

        private Dictionary<XOFieldContext, ControllerDataSource> Items
        {
            get { return _items; }
        }

        private ControllerDataSource this[XOFieldContext FieldContext]
        {
            get
            {
                return Items.Keys.Contains(FieldContext) ? Items[FieldContext] : null;
            }
        }

        internal ControllerDataSource GetDataSourceProperty(XOFieldContext FieldContext)
        {
            ControllerDataSource ds = this[FieldContext];

            if (ds == null)
            {
                ds = new ControllerDataSource(Controller, FieldContext);

                Items.Add(FieldContext, ds);
            }

            return ds;
        }

        internal void UpdateProperties(DataAvail.XtraBindings.Calculator.ObjectProperties ObjectProperties)
        {
            foreach (var i in Items.Where(p=>p.Value.ExtBindingProperties != null))
            {
                i.Value.ExtBindingProperties.Reset(_controller.TableContext[i.Key.Name], ObjectProperties);
            }
        }
    }
}
