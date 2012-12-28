using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraFormController
{
    internal class MenuItemStub : DataAvail.Controllers.Commands.UICommandItems.UICommandItem
    {
        internal MenuItemStub(DataAvail.XtraMenu.IXtraMenuButton MenuButton, DataAvail.Controllers.Commands.ControllerCommandTypes CommandType)
            : base(CommandType)
        {
            _menuButton = MenuButton;

            _menuButton.ButtonClick += new EventHandler(_menuButton_ButtonClick);
        }

        private readonly DataAvail.XtraMenu.IXtraMenuButton _menuButton;


        void _menuButton_ButtonClick(object sender, EventArgs e)
        {
            Exec();
        }


        public override bool Enabled
        {
            set { _menuButton.Enabled = value; }
        }

        public override bool Available
        {
            set { _menuButton.Visible = value; }
        }
    }
}
