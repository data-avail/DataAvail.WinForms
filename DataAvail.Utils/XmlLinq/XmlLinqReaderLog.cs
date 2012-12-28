using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DataAvail.Utils.XmlLinq
{
    public static class XmlLinqReaderLog
    {
        public static bool IsThrowException(IXmlLinqReaderLog XmlReaderLog) { return XmlReaderLog == null || !XmlReaderLog.ContinueOnException; }

        public static void WriteToLog(IXmlLinqReaderLog ReaderLog, string Text, bool Critical)
        {
            WriteToLog(ReaderLog, null, (XAttribute)null, Text, Critical);
        }

        public static void WriteToLog(IXmlLinqReaderLog ReaderLog, XElement Element, string AttributeName, string Text, bool Critical)
        {
            WriteToLog(ReaderLog, Element, AttributeName != null ? Element.Attribute(AttributeName) : null, Text, Critical);
        }

        public static void WriteToLog(IXmlLinqReaderLog ReaderLog, XElement Element, XAttribute Attribute,  string Text, bool Critical)
        {
            if (ReaderLog != null)
            {
                ReaderLog.Write(Element, Attribute, Text, (int)XmlLinqReaderLogInfoType.UnknownError);
            }

            if (Critical && (ReaderLog == null || !ReaderLog.ContinueOnException))
                throw new XmlLinqReaderException(Text);
        }

        public static void WriteToLog(IXmlLinqReaderLog ReaderLog, XElement Element, XAttribute XAttribute, XmlLinqReaderLogInfoType InfoType)
        {
            if (ReaderLog != null)
            {
                ReaderLog.Write(Element, XAttribute, null, (int)InfoType);
            }
        }

        public static void WriteToLog(IXmlLinqReaderLog ReaderLog, XElement Element, XAttribute XAttribute, Exception Exception)
        {
            if (ReaderLog != null)
            {
                ReaderLog.WriteException(Element, XAttribute, Exception);
            }
        }

        public static void WriteToLogUnexpectedValueType(IXmlLinqReaderLog ReaderLog, XElement Element, XAttribute XAttribute, Type ExpectedType)
        {
            if (ReaderLog != null)
            {
                ReaderLog.WriteUnexpectedValueType(Element, XAttribute, ExpectedType);
            }            
        }

        public static void WriteToLogUnexpectedEnumValue(IXmlLinqReaderLog ReaderLog, XElement Element, XAttribute XAttribute, Type EnumType)
        {
            if (ReaderLog != null)
            {
                ReaderLog.WriteUnexpectedEnumValue(Element, XAttribute, EnumType);
            }
        }
    }
}
