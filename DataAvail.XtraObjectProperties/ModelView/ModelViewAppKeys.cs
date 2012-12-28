using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DataAvail.XOP.ModelView
{
    public class ModelViewAppKeys
    {
        internal ModelViewAppKeys()
        {
        }

        private ModelViewAppKeyCommand [] _keyCommands;

        public ModelViewAppKeyCommand [] KeyCommands
        {
            get { return _keyCommands; }
        }

        internal void Parse(XElement XElement)
        {
            _keyCommands = XElement.Elements("Key").Select(p => new ModelViewAppKeyCommand(p)).ToArray();
        }
    }
}
