using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DataAvail.Utils.XmlLinq
{
    public class XmlLinqElementReader
    {
        public XmlLinqElementReader(XElement XElement, IXmlLinqReaderLog ReaderLog)
        {
            _element = XElement;

            _readerLog = ReaderLog;
        }

        private readonly XElement _element;

        private readonly IXmlLinqReaderLog _readerLog;

        public XElement Element
        {
            get { return _element; }
        } 

        public IXmlLinqReaderLog ReaderLog
        {
            get { return _readerLog; }
        }


        public string ReadAttribute(string AttrName)
        {
            return XmlLinq.ReadAttribute(Element, AttrName, ReaderLog);
        }

        public string ReadAttribute(string AttrName, bool Obrigatory)
        {
            return XmlLinq.ReadAttribute(Element, AttrName, Obrigatory, ReaderLog);
        }

        public T ReadAttributeEnum<T>(string AttrName, T DefaultValue)
        {
            return XmlLinq.ReadAttributeEnum<T>(Element, AttrName, DefaultValue, ReaderLog);
        }

        public T ReadAttributeEnumFlags<T>(string AttrName, T All, T DefaultValue)
        {
            return XmlLinq.ReadAttributeEnumFlags<T>(Element, AttrName, All, DefaultValue, ReaderLog);
        }

        public bool ReadAttributeBool(string AttrName, bool DefaultValue)
        {
            return XmlLinq.ReadAttributeBool(Element, AttrName, DefaultValue, ReaderLog);
        }

        public object ReadAttributeValue(string AttrName, Type Type)
        {
            return XmlLinq.ReadAttributeValue(Element, AttrName, Type, ReaderLog);
        }

        public XElement [] GetChildren(string ChildName, Range Range)
        {
            return XmlLinq.GetChildren(Element, ChildName, Range, ReaderLog);
        }
    }
}
