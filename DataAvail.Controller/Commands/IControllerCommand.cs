using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Controllers.Commands
{
    public interface IControllerCommand
    {
        string CommandName { get; }

        System.Func<Controller, IControllerCommandItem, object> Action { get; }

        bool Available { get; }

        bool Enabled { get; }

        event System.EventHandler AvailableChanged;

        event System.EventHandler EnabledChanged;
    }
}
