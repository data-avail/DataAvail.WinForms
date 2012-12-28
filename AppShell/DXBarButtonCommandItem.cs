using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.AppShell
{
    internal class DXBarButtonCommandItem : DataAvail.XtraBinding.Controllers.Commands.UICommandItems.UICommandItem
    {
        internal DXBarButtonCommandItem(DataAvail.XtraBinding.Controllers.Commands.ControllerCommandTypes Type, DevExpress.XtraBars.BarButtonItem BarButtonItem) 
            : base(Type)
        {
            _barButtonItem = BarButtonItem;

            _barButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(_barButtonItem_ItemClick);
        }

        private readonly DevExpress.XtraBars.BarButtonItem _barButtonItem;

        void _barButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Exec();
        }

        public override bool Available
        {
            set { _barButtonItem.Visibility = value ?  DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never; }
        }

        public override bool Enabled
        {
            set { _barButtonItem.Enabled = value; }
        }
    }
}
