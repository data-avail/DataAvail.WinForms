using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;

/*
 * Тэг	MenuItem
Аттрибуты	caption, tableName
Вложенные тэги	MenuItem
Класс	XWPMenuItem
Описание	Определяет элемент меню на главной форме приложения.

Имя 	Описание
caption*	Заголовок меню, будет показан на элементе меню интерфейса пользователя.
tableName	Если данный аттрибут задан то при активизации данного меню пользователем будет открыта форма списка записей для данной таблицы.

Возможны вложеные в друг друга MenuItem в этом случае по ним будет построено  иерархическое меню.
 */

namespace DataAvail.XObject.XWP
{
    public class XWPMenuItem
    {
        public XWPMenuItem(XElement XElement)
        {
            XmlLinqElementReader reader = new XmlLinqElementReader(XElement, XOApplication.xmlReaderLog);

            _caption = reader.ReadAttribute("caption", true);

            _tableName = reader.ReadAttribute("tableName");

            _children = reader.GetChildren("MenuItem", Range.NotBound).Select(p=>new XWPMenuItem(p)).ToArray();

            if (!string.IsNullOrEmpty(_tableName) && _children.Length > 0)
                XmlLinqReaderLog.WriteToLog(XOApplication.xmlReaderLog, XElement, (string)null, "Menu item can't have tableName not null along with children menu items, tableName will be ignored.", false);

            if (string.IsNullOrEmpty(_tableName) && _children.Length == 0)
                XmlLinqReaderLog.WriteToLog(XOApplication.xmlReaderLog, XElement, (string)null, "Menu item tableName is empty and doesn't contain children menu items.", false);

            _key = new XWPKeyCommand(XElement, false);

        }

        internal XWPMenuItem(string Caption, string TableName, XWPMenuItem [] Children)
        {
            _caption = Caption;

            _tableName = TableName;

            _children = Children;
        }
        
        private readonly string _tableName;

        private readonly string _caption;

        private readonly XWPKeyCommand _key;

        private readonly XWPMenuItem[] _children;

        public string Caption
        {
            get { return _caption; }
        }

        public string TableName
        {
            get { return _tableName; }
        } 

        public XWPMenuItem[] Children
        {
            get { return _children; }
        }

        public XWPKeyCommand KeyCommand
        {
            get { return _key; }
        } 
    }
}
