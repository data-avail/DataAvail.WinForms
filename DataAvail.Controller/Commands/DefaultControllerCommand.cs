using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Controllers.Commands
{
    internal class DefaultControllerCommand : ControllerCommand
    {
        internal DefaultControllerCommand(ControllerCommandTypes CommandType, Func<Controller, IControllerCommandItem, object> Action)
            : this(CommandType, Action, true)
        {
        }

        internal DefaultControllerCommand(ControllerCommandTypes CommandType, Func<Controller, IControllerCommandItem, object> Action, bool IsKeyDownCommand)
            : base(CommandType.ToString(), Action)
        {
            this.commandType = CommandType;

            this.isKeyDownCommand = IsKeyDownCommand;
        }

        internal readonly ControllerCommandTypes commandType;

        internal readonly bool isKeyDownCommand;
    }
}
