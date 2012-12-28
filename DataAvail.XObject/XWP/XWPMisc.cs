using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;

/*
Тэг	Misc
Аттрибуты	 
Вложенные тэги	AppSkin
Класс	XWPMisc
Описание	Определяет визуальное представление различных элементов интерфейса пользователя.
 */

namespace DataAvail.XObject.XWP
{
    public class XWPMisc
    {
        public XWPMisc(XElement XElement)
        {
            _appSkin = XmlLinq.GetChildren(XElement, "AppSkin", new Range(-1, 1), XOApplication.xmlReaderLog).Select(p => new XWPAppSkin(p)).FirstOrDefault();
        }

        private readonly XWPAppSkin _appSkin;

        public XWPAppSkin AppSkin
        {
            get { return _appSkin; }
        } 

    }
}
