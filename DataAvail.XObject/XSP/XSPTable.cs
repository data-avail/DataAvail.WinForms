using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;
using DataAvail.XObject;


namespace DataAvail.XObject.XSP
{
    public class XSPTable
    {
        public XSPTable(XElement XElement)
        {
            XmlLinqElementReader reader = new XmlLinqElementReader(XElement, XOApplication.xmlReaderLog);

            _tableName = reader.ReadAttribute("name", true);

            _mode = reader.ReadAttributeEnumFlags("mode", XOMode.All, XOMode.All);

            _fields = reader.GetChildren("Field", Range.NotBound).Select(p => new XSPField(p)).ToArray();
        }

        private readonly string _tableName;

        private readonly XOMode _mode;

        private readonly XSPField[] _fields;

        public string TableName
        {
            get { return _tableName; }
        }

        public XOMode Mode
        {
            get { return _mode; }
        }

        public XSPField[] Fields
        {
            get { return _fields; }
        } 
    }
}
