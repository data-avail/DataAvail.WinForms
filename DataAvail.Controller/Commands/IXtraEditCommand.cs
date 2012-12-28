using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Controllers.Commands
{
    public interface IXtraEditCommand
    {
        IEnumerable<DataAvail.Controllers.Commands.IControllerCommandItem> SupportedCommands { get; }
    }
}
