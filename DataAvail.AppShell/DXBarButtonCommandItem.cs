using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraBars;
using DataAvail.Controllers.Commands;
using DataAvail.Controllers.Commands.UICommandItems;

namespace DataAvail.AppShell
{
    internal class DXBarButtonCommandItem : UICommandItem
    {
        internal DXBarButtonCommandItem(ControllerCommandTypes Type, BarButtonItem BarButtonItem) 
            : base(Type)
        {
            _barButtonItem = BarButtonItem;

            _barButtonItem.ItemClick += new ItemClickEventHandler(_barButtonItem_ItemClick);
        }

        private readonly BarButtonItem _barButtonItem;

        void _barButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            Exec();
        }

        public override bool Available
        {
            set { _barButtonItem.Visibility = value ?  BarItemVisibility.Always : BarItemVisibility.Never; }
        }

        public override bool Enabled
        {
            set { _barButtonItem.Enabled = value; }
        }
    }
}
