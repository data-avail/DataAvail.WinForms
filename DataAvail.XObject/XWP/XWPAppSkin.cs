using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;


/*
 * Тэг	AppSkin
Аттрибуты	 name
Вложенные тэги	
Класс	XWPAppSkin
Описание	Определяет общий вид приложения через его имя.

Имя 	Описание
name	Имя скина программы. Смотри здесь 
http://www.devexpress.com/Products/NET/Controls/WinForms/Skins/
 */

namespace DataAvail.XObject.XWP
{
    public class XWPAppSkin
    {
        public XWPAppSkin(XElement XElement)
        {
            _skinName = XmlLinq.ReadAttribute(XElement, "skinName", XOApplication.xmlReaderLog);
        }

        private readonly string _skinName;

        public string SkinName
        {
            get { return _skinName; }
        } 

    }
}
