using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils.XmlLinq;


namespace DataAvail.XOP.ModelView
{
    public class ModelViewAppKeyCommand
    {
        internal ModelViewAppKeyCommand(XElement XElement)
        {
            command = XmlLinq.GetAttribute(XElement, "command");

            keys = XmlLinq.GetAttribute(XElement, "key").Split('|').Select(p => new ModelViewAppKey(p)).ToArray();
        }

        public readonly string command;

        public readonly ModelViewAppKey[] keys;

    }
}
