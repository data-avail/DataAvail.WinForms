using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;


/*
 * Тэг	Application
Аттрибуты	name
Вложенные тэги	DataView, AppView
Класс	XWPApplication
Описание	Определяет параметры интерфейса пользователя в приложении.

Влияние аттрибутов на интерфейс пользователя. 
Имя 	Описание
name	Имя приложения. Будет показан как заголовок на главном окне приложения.
 */

namespace DataAvail.XObject.XWP
{
    public class XWPApplication
    {
        public XWPApplication(XElement XElement)
        {
            XmlLinqElementReader reader = new XmlLinqElementReader(XElement, XOApplication.xmlReaderLog);

            _name = reader.ReadAttribute("name");

            _dataView = reader.GetChildren("DataView", new Range(0, 1)).Select(p=>new XWPDataView(p)).FirstOrDefault();

            _appView = reader.GetChildren("AppView", new Range(0, 1)).Select(p => new XWPAppView(p)).FirstOrDefault();
        }

        private readonly string _name;

        private readonly XWPDataView _dataView;

        private readonly XWPAppView _appView;

        public string Name
        {
            get { return _name; }
        }

        public XWPDataView DataView
        {
            get { return _dataView; }
        }

        public XWPAppView AppView
        {
            get { return _appView; }
        }
    }
}
