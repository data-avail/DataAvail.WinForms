using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;

/*
 Тэг	Columns
Аттрибуты	
Вложенные тэги	
Класс	XWPColumns
Описание	Содержит описание набора визульных характеристик полей данных таблицы.
 */

namespace DataAvail.XObject.XWP
{
    public class XWPFields
    {
        public XWPFields(XWPTable XWPTable, XElement XElement)
        {
            _xWPTable = XWPTable;

            _parentDisplayField = XmlLinq.ReadAttribute(XElement, "parentDisplayField", XOApplication.xmlReaderLog); 

            _fields = XmlLinq.GetChildren(XElement, "Field", Range.NotBound, XOApplication.xmlReaderLog).Select(p=>new XWPField(this, p)).ToArray();
        }

        private readonly XWPTable _xWPTable;

        private readonly string _parentDisplayField;

        private readonly XWPField[] _fields;

        public XWPTable XWPTable
        {
            get { return _xWPTable; }
        } 

        public string ParentDisplayField
        {
            get { return _parentDisplayField; }
        }

        public XWPField[] Fields
        {
            get { return _fields; }
        } 

    }
}
