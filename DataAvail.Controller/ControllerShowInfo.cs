using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Controllers
{
    public class ControllerShowInfoEventArgs : System.EventArgs
    {
        internal ControllerShowInfoEventArgs(System.Exception Exception)
        {
            exception = Exception;
        }

        internal ControllerShowInfoEventArgs(string InfoMessage)
        {
            infoMessage = InfoMessage;
        }

        public readonly System.Exception exception;

        public readonly string infoMessage;

        public override string ToString()
        {
            return !string.IsNullOrEmpty(exception.Message) ? exception.Message : infoMessage;
        }
    }

    public delegate void ControllerShowInfo(object sender, ControllerShowInfoEventArgs args);
}
