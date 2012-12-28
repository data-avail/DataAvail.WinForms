using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Controllers.Commands.UICommandItems
{
    public abstract class UICommandItem : IControllerCommandItem
    {
        public UICommandItem(ControllerCommandTypes CommandType) 
            : this(CommandType.ToString())
        {
        }

        public UICommandItem(string CommandName)
        {
            _commandName = CommandName;
        }

        private readonly string _commandName;

        public object Exec()
        {
            Commands.ControllerCommandItemExecuteEventArgs args = new ControllerCommandItemExecuteEventArgs();

            if (Execute != null)
            {
                Execute(this, args);
            }

            OnExecuted(args.result);

            return args.result;
        }

        protected virtual void OnExecuted(object ReturnValue)
        { }

        #region IControllerCommandItem Members

        public string CommandName
        {
            get { return _commandName; }
        }

        public virtual object Argument
        {
            get { return null; }
        }

        public virtual object Context
        {
            get { return null; }
        }

        public abstract bool Enabled {set;}

        public abstract bool Available {set;}

        public event Commands.ControllerCommandItemExecuteHandler Execute;

        #endregion
    }
}
