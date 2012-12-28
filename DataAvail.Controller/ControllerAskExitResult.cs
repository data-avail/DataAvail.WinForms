using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Controllers
{
    //http://dotnetperls.com/enum-flags
    [Flags]
    public enum ControllerAskExitResult
    {
        None = 0x00,
        Save = 0x01,
        Reject = 0x02,
        EndEdit = 0x04,
        CancelEdit = 0x08,
        CancelExit = 0x10,
        JustExit = 0x20
    }

    public class ControllerAskExitEventArgs : System.EventArgs
    {
        internal ControllerAskExitEventArgs(ControllerAskExitResult EnabledResults)
        {
            enabledResults = EnabledResults;
        }

        public readonly ControllerAskExitResult enabledResults;

        public ControllerAskExitResult result;
    }

    public delegate void ControllerAskExitHandler(object sender, ControllerAskExitEventArgs args);

}
