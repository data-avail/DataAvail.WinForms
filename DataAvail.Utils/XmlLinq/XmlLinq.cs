using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using System.Xml;

namespace DataAvail.Utils.XmlLinq
{
    public static class XmlLinq
    {
        #region Attributes 

        public static string GetAttribute(XElement Element, string AttributeName)
        {
            return GetAttribute(Element, AttributeName, false);
        }

        public static string GetAttributeSafe(XElement Element, string AttributeName)
        {
            return GetAttribute(Element, AttributeName, true);
        }

        public static string GetAttribute(XElement Element, string AttributeName, bool CheckElementNull)
        {
            if (CheckElementNull && Element == null) return null;
            
            XAttribute attr = Element.Attributes().FirstOrDefault(p => p.Name == AttributeName);

            return attr != null ? attr.Value : null;
        }

        public static string ReadAttribute(XElement Element, string AttributeName, IXmlLinqReaderLog ReaderLog)
        { 
            return ReadAttribute(Element, AttributeName, false, ReaderLog);
        }

        public static string ReadAttribute(XElement Element, string AttributeName, bool Obrigatory, IXmlLinqReaderLog ReaderLog)
        {
            XAttribute attr;

            return ReadAttribute(Element, AttributeName, Obrigatory, ReaderLog, out attr);
        }

        public static string ReadAttribute(XElement Element, string AttributeName, bool Obrigatory, IXmlLinqReaderLog ReaderLog, out XAttribute Attribute)
        {
            XmlLinqReaderLogInfoType infoType = XmlLinqReaderLogInfoType.Success;

            Attribute = null;

            try
            {
                Attribute = Element.Attributes().FirstOrDefault(p => p.Name == AttributeName);

                if (Attribute == null)
                    infoType = XmlLinqReaderLogInfoType.AttributeNotFound;
                else if (string.IsNullOrEmpty(Attribute.Value))
                    infoType = XmlLinqReaderLogInfoType.AttributeEmpty;

                XmlLinqReaderLog.WriteToLog(ReaderLog, Element, Attribute, infoType);
            }
            catch (System.Exception ex)
            {
                XmlLinqReaderLog.WriteToLog(ReaderLog, Element, Attribute, ex);

                if (XmlLinqReaderLog.IsThrowException(ReaderLog))
                    throw;

                return null;
            }

            if (Obrigatory && Attribute == null && XmlLinqReaderLog.IsThrowException(ReaderLog))
                throw new XmlLinqReaderObrigatoryAttrNotFoundException(Element, AttributeName);


            return Attribute != null ? Attribute.Value : null;
        }


        public static T ReadAttributeEnum<T>(XElement Element, string AttributeName, T DefaultValue, IXmlLinqReaderLog ReaderLog)
        {
            return ReadAttributeValue<T>(Element, AttributeName, p => (T)System.Enum.Parse(typeof(T), p),  p => XmlLinqReaderLog.WriteToLogUnexpectedEnumValue(ReaderLog, Element, p, typeof(T)), DefaultValue, ReaderLog); 
        }

        public static T ReadAttributeEnumFlags<T>(XElement Element, string AttributeName, T All, T DefaultValue, IXmlLinqReaderLog ReaderLog)
        {
            return ReadAttributeValue<T>(Element, AttributeName, p => (T)EnumFlags.Parse(p, All), p => XmlLinqReaderLog.WriteToLogUnexpectedEnumValue(ReaderLog, Element, p, typeof(T)), DefaultValue, ReaderLog);
        }

        public static object ReadAttributeValue(XElement Element, string AttributeName, Type Type, IXmlLinqReaderLog ReaderLog)
        {
            return ReadAttributeValue(Element, AttributeName, p => Reflection.Parse(Type, p), p => XmlLinqReaderLog.WriteToLogUnexpectedValueType(ReaderLog, Element, p, Type), null, ReaderLog);
        }

        public static bool ReadAttributeBool(XElement Element, string AttributeName, bool DefaultValue, IXmlLinqReaderLog ReaderLog)
        {
            return ReadAttributeValue<bool>(Element, AttributeName, DefaultValue, ReaderLog);
        }

        public static T ReadAttributeValue<T>(XElement Element, string AttributeName, T DefaultValue, IXmlLinqReaderLog ReaderLog)
        {
            return ReadAttributeValue<T>(Element, AttributeName, p => (T)Reflection.Parse(typeof(T), p), p => XmlLinqReaderLog.WriteToLogUnexpectedValueType(ReaderLog, Element, p, typeof(T)), DefaultValue, ReaderLog);
        }

        public static T ReadAttributeValue<T>(XElement Element, string AttributeName, Func<string, T> Parser, T DefaultValue, IXmlLinqReaderLog ReaderLog)
        {
            

            return ReadAttributeValue<T>(Element, AttributeName, Parser, p => XmlLinqReaderLog.WriteToLogUnexpectedValueType(ReaderLog, Element, p, typeof(T)), DefaultValue, ReaderLog);
        }

        private static T ReadAttributeValue<T>(XElement Element, string AttributeName, Func<string, T> Parser, Action<XAttribute> WriteLog, T DefaultValue, IXmlLinqReaderLog ReaderLog)
        {
            XAttribute attr;

            string attrStr = ReadAttribute(Element, AttributeName, false, ReaderLog, out attr);

            if (!string.IsNullOrEmpty(attrStr))
            {
                try
                {
                    return Parser(attrStr);
                }
                catch (System.ArgumentException)
                {
                    WriteLog(attr);

                    if (XmlLinqReaderLog.IsThrowException(ReaderLog))
                        throw;
                }
            }

            return DefaultValue;
        }

        #endregion

        #region Elements

        public static XElement [] GetChildren(XElement XElement, string ChildName, Range Range, IXmlLinqReaderLog ReaderLog)
        {
            XElement [] children = XElement.Elements(ChildName).ToArray();

            string err = Range.GetBoundErrorText(ChildName, children.Length);

            if (!string.IsNullOrEmpty(err))
            {
                XmlLinqReaderLog.WriteToLog(ReaderLog, err, true);
            }

            return children;
        }

        #endregion

        #region System.Xml <-> System.Xml.Linq

        //http://blogs.msdn.com/b/ericwhite/archive/2008/12/22/convert-xelement-to-xmlnode-and-convert-xmlnode-to-xelement.aspx

        public static XElement GetXElement(this XmlNode node)
        {
            XDocument xDoc = new XDocument();
            using (XmlWriter xmlWriter = xDoc.CreateWriter())
                node.WriteTo(xmlWriter);
            return xDoc.Root;
        }

        public static XmlNode GetXmlNode(this XElement element)
        {
            using (XmlReader xmlReader = element.CreateReader())
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlReader);
                return xmlDoc;
            }
        }

        #endregion
    }

}
