using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Controllers
{
    public interface IControllerUI
    {
        object UI { get; }

        event System.EventHandler UIDataBound;

        event System.Windows.Forms.KeyEventHandler UIKeyDown;

        event System.Windows.Forms.KeyEventHandler UIKeyUp;

        bool IsFocused { get; }

        DataAvail.Controllers.Commands.IXtraEditCommand FocusedEditCommand { get; }

        bool Validate();
    }
}
