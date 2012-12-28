using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DataAvail.Utils.XmlLinq
{
    public class XmlLinqReaderException : System.Exception
    {
        internal XmlLinqReaderException()
        {}


        public XmlLinqReaderException(string Text)
            : base(Text)
        { }
    }

    public class XmlLinqReaderObrigatoryAttrNotFoundException : XmlLinqReaderException
    {
        public XmlLinqReaderObrigatoryAttrNotFoundException(XElement XElement,  string AttrName)
            : base()
        {
            this.element = XElement;

            this.attributeName = AttrName;
        }

        public readonly XElement element;

        public readonly string attributeName;
    }

}
