using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;
using DataAvail.XObject;

/*
Тэг	AppSecurity
Аттрибуты	 mode
Вложенные тэги	TableSecurity
Класс	XSPAppSecurity
Описание	Определяет общие настройки безопасности для приложения.

Имя 	Описание
mode	Определяет какие действия доступны пользователю (определяется для всех таблиц проекта).
Значения:
view|edit|delete|add
view – пользователь может просматривать данные таблиц
edit – пользователь мoжет редактировать данные таблиц
delete – пользователь мoжет удалять данные таблиц
add – пользователь мoжет добавлять данные таблиц
 */

namespace DataAvail.XObject.XSP
{
    public class XSPApplication
    {
        public XSPApplication(XElement XElement)
        {
            XmlLinqElementReader reader = new XmlLinqElementReader(XElement, XOApplication.xmlReaderLog);

            _mode = reader.ReadAttributeEnumFlags("mode", XOMode.All, XOMode.All);

            if (_mode == XOMode.None)
                XmlLinqReaderLog.WriteToLog(XOApplication.xmlReaderLog, XElement, "mode", "Mode for application None, in this case project won't have any functionality.", false);

            _tables = reader.GetChildren("Table", Range.NotBound).Select(p => new XSPTable(p)).ToArray();
        }

        private readonly XOMode _mode;

        private readonly XSPTable [] _tables;

        public XSPTable [] Tables
        {
            get { return _tables; }
        } 


        public XOMode Mode
        {
            get { return _mode; }
        } 

    }
}
