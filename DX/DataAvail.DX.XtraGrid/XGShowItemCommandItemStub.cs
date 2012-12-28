using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.DX.XtraGrid
{
    internal class XGShowItemCommandItemStub : DataAvail.Controllers.Commands.UICommandItems.UICommandItem
    {
        internal XGShowItemCommandItemStub(DevExpress.XtraGrid.GridControl GridControl)
            : base(DataAvail.Controllers.Commands.ControllerCommandTypes.ItemSelect)
        {
            GridControl.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(GridControl_MouseDoubleClick);
        }

        private bool _enabled = true;

        private bool _visible = true;

        void GridControl_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (_enabled && _visible)
                Exec();
        }

        public override bool Available
        {
            set { _visible = value; }
        }

        public override bool Enabled
        {
            set { _enabled = value; }
        }
    }
}
