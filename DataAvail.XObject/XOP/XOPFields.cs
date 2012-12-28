using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils.XmlLinq;


/*
Тэг	Columns
Аттрибуты	 
Вложенные тэги	Column[1..]
Класс	XOPColumns
Описание	Определяет набор полей таблиы.

 */

namespace DataAvail.XObject.XOP
{
    public class XOPFields
    {
        public XOPFields(XElement XElement)
        {
            if (XElement != null)
                _fields = XmlLinq.GetChildren(XElement, "Field", new DataAvail.Utils.Range(1, -1), XOApplication.xmlReaderLog).Select(p => new XOPField(p)).ToArray();
            else
                _fields = new XOPField [] { };
        }

        private readonly XOPField[] _fields;

        public XOPField[] Fields
        {
            get { return _fields; }
        } 

    }
}
