using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;
using DataAvail.XtraBindings;

namespace DataAvail.Controllers
{
    public class ControllerContext
    {
        public ControllerContext(DataAvail.Controllers.IControllerProvider ControllerProvider)
        {
            _controllerProvider = ControllerProvider;

            _controllers = new List<Controller>();
        }

        private readonly List<Controller> _controllers;

        private readonly DataAvail.Controllers.IControllerProvider _controllerProvider;

        public IEnumerable<Controller> Controllers
        {
            get { return _controllers; }
        } 

        private DataAvail.Controllers.IControllerProvider ControllerProvider
        {
            get { return _controllerProvider; }
        }

        public Controller AddController(XOTableContext TableContext)
        {
            return AddController(TableContext, null);
        }

        public Controller AddController(XOTableContext TableContext, Func<XtraBinding, XOTableContext, Controller> CreateControllerInstanceFunc)
        {
            if (CreateControllerInstanceFunc == null)
                CreateControllerInstanceFunc = (p, x) => new Controller(p, x);

            Controller controller = DataAvail.Controllers.XtraBindingCreator.CreateController(TableContext, ControllerProvider, CreateControllerInstanceFunc);

            _controllers.Add(controller);

            return controller;
        }

        public object GetDataSource(XOTable AppItem)
        {
            return ControllerProvider.GetDataSource(AppItem);
        }
    }
}
