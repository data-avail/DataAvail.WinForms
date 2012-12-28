using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DataAvail.Utils.XmlLinq
{
    public interface IXmlLinqReaderLog
    {
        void Write(string Text, params object[] Prms);

        void Write(XElement XElement, XAttribute Attribute, string Text, int InfoType);

        void WriteException(XElement XElement, XAttribute Attribute, Exception Exception);

        void WriteUnexpectedValueType(XElement XElement, XAttribute Attribute, Type ExpectedType);

        void WriteUnexpectedEnumValue(XElement XElement, XAttribute Attribute, Type EnumType);

        bool ContinueOnException { get; set; }
    }

    public enum XmlLinqReaderLogInfoType
    {
        Success,

        AttributeNotFound,

        AttributeEmpty,

        UnexpectedValue,

        UnexpectedEnumValue,

        UnknownError
    }
}
