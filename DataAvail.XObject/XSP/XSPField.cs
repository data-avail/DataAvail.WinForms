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
    public class XSPField
    {
        public XSPField(XElement XElement)
        {
            XmlLinqElementReader reader = new XmlLinqElementReader(XElement, XOApplication.xmlReaderLog);

            _fieldName = reader.ReadAttribute("name", true);

            _mode = reader.ReadAttributeEnumFlags("mode", XOMode.All, XOMode.View | XOMode.Edit);

            if (EnumFlags.IsContain(_mode, XOMode.Add | XOMode.Delete))
                XmlLinqReaderLog.WriteToLog(XOApplication.xmlReaderLog, XElement, "mode", "Field mode can't be nor Add neither Remove, they will be ignored", false);
        }

        private readonly string _fieldName;

        private readonly XOMode _mode;

        public string FieldName
        {
            get { return _fieldName; }
        }

        public XOMode Mode
        {
            get { return _mode; }
        } 

    }
}
