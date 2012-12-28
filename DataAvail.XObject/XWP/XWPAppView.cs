using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;


/*
Тэг	AppView
Аттрибуты	
Вложенные тэги	MenuItems, Misc
Класс	XWPAppView
Описание	Описывает поведение приложения и его визуальное представление не связанное непосредственно с данными.
 */

namespace DataAvail.XObject.XWP
{
    public class XWPAppView
    {
        public XWPAppView(XElement XElement)
        {
            XmlLinqElementReader reader = new XmlLinqElementReader(XElement, XOApplication.xmlReaderLog);

            _menuItems = reader.GetChildren("MenuItems", new Range(1, 1)).Select(p=>new XWPMenuItems(p)).FirstOrDefault();

            _misc = reader.GetChildren("Misc", new Range(0, 1)).Select(p => new XWPMisc(p)).FirstOrDefault();

            _keys = reader.GetChildren("Keys", new Range(0, 1)).Select(p => new XWPKeyCommands(p)).FirstOrDefault();
        }

        private readonly XWPMenuItems _menuItems;

        private readonly XWPMisc _misc;

        private readonly XWPKeyCommands _keys;

        public XWPKeyCommands Keys
        {
            get { return _keys; }
        } 

        public XWPMenuItems MenuItems
        {
            get { return _menuItems; }
        }

        public XWPMisc Misc
        {
            get { return _misc; }
        } 

    }
}
