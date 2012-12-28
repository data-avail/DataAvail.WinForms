using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Controllers.Commands
{
    public class ControllerCommand : DataAvail.Controllers.Commands.IControllerCommand
    {
        public ControllerCommand(string CommandName, Func<Controller, IControllerCommandItem, object> Action)
        {
            _commandName = CommandName;

            _action = Action;
        }

        private readonly string _commandName;

        private readonly Func<Controller, IControllerCommandItem, object> _action;

        private bool _available = false;

        private bool _enabled = false;

        public void SetAvailable(bool Available, bool ForcedAvailableChanged)
        {
            if (this.Available != Available || ForcedAvailableChanged)
            {
                _available = Available;

                OnAvailableChanged();
            }
        }

        public void SetEnabled(bool Enabled, bool ForcedEnabledChanged)
        {
            if (this.Enabled != Enabled || ForcedEnabledChanged)
            {
                _enabled = Enabled;

                OnEnabledChanged();
            }
        }

        private void OnEnabledChanged()
        {
            if (EnabledChanged != null)
                EnabledChanged(this, EventArgs.Empty);
        }

        private void OnAvailableChanged()
        {
            if (AvailableChanged != null)
                AvailableChanged(this, EventArgs.Empty);
        }

        #region IControllerCustomCommand Members

        public string CommandName
        {
            get { return _commandName; }
        }

        public System.Func<Controller, IControllerCommandItem, object> Action
        {
            get { return _action; }
        }

        public bool Available
        {
            get { return _available; }
        }

        public bool Enabled
        {
            get { return _enabled; }
        }

        public event EventHandler AvailableChanged;

        public event EventHandler EnabledChanged;

        #endregion
    }
}
