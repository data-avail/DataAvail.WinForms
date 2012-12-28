using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Controllers
{
    public class ControllerAskConfirmationEventArgs : System.EventArgs 
    {
        public ControllerAskConfirmationEventArgs(string ConfirmationString)
        {
            this.confirmationString = ConfirmationString;
        }

        public readonly string confirmationString;

        public bool confirm = true;
    }

    public delegate void ControllerAskConfirmationHandler(object sender, ControllerAskConfirmationEventArgs args);
}
