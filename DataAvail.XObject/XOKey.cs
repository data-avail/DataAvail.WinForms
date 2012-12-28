using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject.XWP;

namespace DataAvail.XObject
{
    public class XOKey
    {
        internal XOKey(XWPKey XWPKey)
        {
            _xwpKey = XWPKey;
        }

        private readonly XWPKey _xwpKey;

        public XWPKey XwpKey
        {
            get { return _xwpKey; }
        }

        public XWPKeyCommand DefaultKeyCommand
        {
            get { return (XwpKey.KeyCommand is XWPKeyCommandContext) ? ((XWPKeyCommandContext)XwpKey.KeyCommand).DefaultKeyCommand : XwpKey.KeyCommand; }
        } 

        public XWPKeyCommandType CommandType
        {
            get { return DefaultKeyCommand.CommandType; }
        }

        public XWPKeyContextType ContextType
        {
            get
            { return (XwpKey.KeyCommand is XWPKeyCommandContext) ? ((XWPKeyCommandContext)XwpKey.KeyCommand).ContextType : XWPKeyContextType.undefined; }
        }

        public string Key
        {
            get { return XwpKey.Key; }
        }

        public string[] Modifyers
        {
            get { return XwpKey.Modifyers; }
        }


    }
}
