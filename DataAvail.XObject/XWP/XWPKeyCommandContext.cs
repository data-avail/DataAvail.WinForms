using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils.XmlLinq;

namespace DataAvail.XObject.XWP
{
    public class XWPKeyCommandContext : XWPKeyCommand
    {
        public XWPKeyCommandContext(XWPKeyCommand DefaultKey, XElement XElement) 
            : base(XElement, true)
        {
            _defaultKeyCommand = DefaultKey;

            XmlLinqElementReader reader = new XmlLinqElementReader(XElement, XOApplication.xmlReaderLog);

            _contextType = reader.ReadAttributeEnum("context", XWPKeyContextType.undefined);
        }

        private readonly XWPKeyCommand _defaultKeyCommand;

        private readonly XWPKeyContextType _contextType;

        public XWPKeyCommand DefaultKeyCommand
        {
            get { return _defaultKeyCommand; }
        } 

        public XWPKeyContextType ContextType
        {
            get { return _contextType; }
        } 
    }

    public enum XWPKeyContextType
    { 
        undefined,

        fkItem,

        searchPanel
    }
}
