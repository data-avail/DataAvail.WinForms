using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Data.Function
{
    public class CommandExecutedEventArgs : System.EventArgs 
    {
        public CommandExecutedEventArgs(object ExecutionResult)
        {
            this.executionResult = ExecutionResult;
        }

        public object executionResult;
    }

    public delegate void CommandExecutedHandler(object sender, CommandExecutedEventArgs args);
}
