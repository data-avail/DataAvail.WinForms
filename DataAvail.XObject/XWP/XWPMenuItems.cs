using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;

/*
Тэг	MenuItems
Аттрибуты	
Вложенные тэги	XWPMenuItem
Класс	XOPDataSet
Описание	Определяет представление меню на главной форме приложения.
 */

namespace DataAvail.XObject.XWP
{
    public class XWPMenuItems
    {
        public XWPMenuItems(XElement XElement)
        {
            _menuItems = XmlLinq.GetChildren(XElement, "MenuItem", new Range(1, -1), XOApplication.xmlReaderLog).Select(p=>new XWPMenuItem(p)).ToArray();
        }

        private readonly XWPMenuItem[] _menuItems;

        public XWPMenuItem[] MenuItems
        {
            get { return _menuItems; }
        } 

    }
}
