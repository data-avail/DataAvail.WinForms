using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;

/*
 * Тэг	DataView
Аттрибуты	
Вложенные тэги	Table
Класс	XWPDataView
Описание	Определяет визульное представление и поведение интерфейса пользователя для данных проекта.

 */

namespace DataAvail.XObject.XWP
{
    public class XWPDataView
    {
        public XWPDataView(XElement XElement)
        {
            XmlLinqElementReader reader = new XmlLinqElementReader(XElement, XOApplication.xmlReaderLog);

            _parentDisplayField = reader.ReadAttribute("parentDisplayField");

            _saveMode = reader.ReadAttributeEnumFlags("saveMode", XWPTableSaveMode.All, XWPTableSaveMode.Default);

            _childSaveMode = reader.ReadAttributeEnumFlags("childSaveMode", XWPTableSaveMode.All, XWPTableSaveMode.Default);

            _tables = reader.GetChildren("Table", Range.NotBound).Select(p => new XWPTable(this, p)).ToArray();

            _relations = reader.GetChildren("Relations", new Range(0, 1)).Select(p => new XWPRelations(p)).FirstOrDefault();
        }

        private readonly XWPTable[] _tables;

        private readonly XWPRelations _relations;

        private readonly string _parentDisplayField;

        private readonly XWPTableSaveMode _saveMode;

        private readonly XWPTableSaveMode _childSaveMode;

        public XWPTable[] Tables
        {
            get { return _tables; }
        }

        public XWPRelations Relations
        {
            get { return _relations; }
        }

        public string ParentDisplayField
        {
            get { return _parentDisplayField; }
        }


        public XWPTableSaveMode SaveMode
        {
            get { return _saveMode; }
        }

        public XWPTableSaveMode ChildSaveMode
        {
            get { return _childSaveMode; }
        }

    }
}
