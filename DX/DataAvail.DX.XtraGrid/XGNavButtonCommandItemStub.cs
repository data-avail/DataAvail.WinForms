using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.DX.XtraGrid
{
    /// <summary>
    /// Xtra grid navigator menu button stub for IControllerCommandItem
    /// </summary>
    internal class XGNavButtonCommandItemStub : DataAvail.Controllers.Commands.UICommandItems.UICommandItem
    {
        internal XGNavButtonCommandItemStub(DevExpress.XtraEditors.ControlNavigator ControlNavigator, DevExpress.XtraEditors.NavigatorButton NavigatorButton, DataAvail.Controllers.Commands.ControllerCommandTypes CommandType)
            : base(CommandType)
        {
            ControlNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(ControlNavigator_ButtonClick);

            _navigatorButton = NavigatorButton;
        }

        void ControlNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            if (_navigatorButton == e.Button)
            {
                e.Handled = true;

                Exec();
            }
        }

        private readonly DevExpress.XtraEditors.NavigatorButton _navigatorButton;

        public override bool Available
        {
            set { _navigatorButton.Visible = value; }
        }

        public override bool Enabled
        {
            set { _navigatorButton.Enabled = value; }
        }
    }
}
