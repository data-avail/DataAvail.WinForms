using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Controllers.Commands.UICommandItems
{
    public class ControlCommandItem : UICommandItem
    {
        public ControlCommandItem(Controllers.Commands.ControllerCommandTypes CommandType, System.Windows.Forms.Control Control)
            : base(CommandType)
        {
            _control = Control;

            _control.Click += new EventHandler(_control_Click);
        }

        private readonly System.Windows.Forms.Control _control;

        void _control_Click(object sender, EventArgs e)
        {
            Exec();
        }

        public override bool Available
        {
            set { _control.Visible = value; }
        }

        public override bool Enabled
        {
            set { _control.Enabled = value; }
        }
    }
}
