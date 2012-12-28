using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Controllers.Commands
{
    public interface IControllerCommandItem
    {
        string CommandName { get; }

        object Argument { get; }

        object Context { get; }

        bool Enabled { set; }

        bool Available { set; }

        event ControllerCommandItemExecuteHandler Execute;

        object Exec();
    }

    public class ControllerCommandItemExecuteEventArgs : System.EventArgs
    {
        internal ControllerCommandItemExecuteEventArgs()
        {
        }

        public object result;
    }

    public delegate void ControllerCommandItemExecuteHandler(object sender, ControllerCommandItemExecuteEventArgs args);
}
