using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.Utils;
using DataAvail.XObject.XSP;
using DataAvail.XObject.XWP;

namespace DataAvail.XObject
{
    public class XOMenuItem
    {
        public XOMenuItem(XWPMenuItem MenuItem, XSPApplication XSPApplication)
        {
            _xwpMenuItem = MenuItem;

            _xspTable = XSPApplication != null ? XSPApplication.Tables.FirstOrDefault(p=>p.TableName == MenuItem.TableName) : null;

            _menuItems = MenuItem.Children.Select(p => new XOMenuItem(p, XSPApplication)).ToArray();
        }

        private readonly XSPTable _xspTable;

        private readonly XWPMenuItem _xwpMenuItem;

        private readonly XOMenuItem [] _menuItems;

        private XOMenuItem[] MenuItems
        {
            get { return _menuItems; }
        }

        public XOMenuItem[] Children
        {
            get { return MenuItems.Where(p => p.IsVisible).ToArray(); }
        }

        public XWPMenuItem XWPMenuItem
        {
            get { return _xwpMenuItem; }
        }

        public XSPTable XSPTable
        {
            get { return _xspTable; }
        }

        public bool IsVisible
        {
            get { return XSPTable == null || EnumFlags.IsContain(XSPTable.Mode, XOMode.View); }
        }

        public string Caption
        {
            get { return _xwpMenuItem.Caption; }
        }

        public string TableName
        {
            get { return _xwpMenuItem.TableName; }
        }
    }
}
