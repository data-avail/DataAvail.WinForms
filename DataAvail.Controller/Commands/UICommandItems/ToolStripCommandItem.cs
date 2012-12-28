using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Controllers.Commands.UICommandItems
{
    public class ToolStripCommandItem : UICommandItem
    {
        public ToolStripCommandItem(Controllers.Commands.ControllerCommandTypes CommandType, System.Windows.Forms.ToolStripItem ToolStripItem)
            : base(CommandType)
        {
            _toolStripItem = ToolStripItem;

            _toolStripItem.Click += new EventHandler(_toolStripItem_Click);
        }

        private readonly System.Windows.Forms.ToolStripItem _toolStripItem;

        void _toolStripItem_Click(object sender, EventArgs e)
        {
            Exec();
        }

        public override bool Available
        {
            set { _toolStripItem.Visible = value; }
        }

        public override bool Enabled
        {
            set { _toolStripItem.Enabled = value; }
        }
    }
}
