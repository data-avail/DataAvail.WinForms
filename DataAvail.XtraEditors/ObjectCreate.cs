using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraEditors
{
    public delegate void ObjectCreatingHandler(object sender, ObjectCreatingEventArgs args);

    public class ObjectCreatingEventArgs
    {
        public ObjectCreatingEventArgs(object Controller)
        {
            controller = Controller;
        }

        public readonly object controller;

        public object Object;
    }

    public delegate void ObjectCreatedHandler(object sender, ObjectCreatedEventArgs args);

    public class ObjectCreatedEventArgs
    {
        public ObjectCreatedEventArgs(object Object)
        {
            this.Object = Object;
        }

        public readonly object Object;
    }

}
