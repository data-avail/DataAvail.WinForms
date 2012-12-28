using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils.XmlLinq;

namespace DataAvail.Serialization
{
    public class XMLSerializationFormatter : ISerializationFormatter
    {
        #region ISerializationFormatter Members

        public void Serialize(System.IO.Stream Stream, ISerializableObject SerializableObject)
        {
            XDocument xdoc = new XDocument();

            xdoc.Add(new XElement("Root"));

            Serialize(xdoc.Root, SerializableObject);

            System.Xml.XmlWriter xmlWriter = System.Xml.XmlWriter.Create(Stream, new System.Xml.XmlWriterSettings() { Indent = true });

            xdoc.WriteTo(xmlWriter);

            xmlWriter.Close();
        }

        public void Deserialize(System.IO.Stream Stream, ISerializableObject SerializableObject)
        {
            if (Stream.Length != 0)
            {
                XDocument xdoc = new XDocument();

                System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(Stream, new System.Xml.XmlReaderSettings() { IgnoreWhitespace = true });

                xdoc = XDocument.Load(xmlReader);

                xmlReader.Close();

                Deserialize((XElement)xdoc.Root.FirstNode, SerializableObject);
            }
        }

        #endregion

        private void Serialize(XElement XElement, ISerializableObject SerializableObject)
        {
            System.Runtime.Serialization.SerializationInfo si = new System.Runtime.Serialization.SerializationInfo(SerializableObject.GetType(), new System.Runtime.Serialization.FormatterConverter());

            SerializableObject.Serialize(si);

            XElement elem = new XElement(SerializableObject.SerializationTag.Type);

            elem.Add(new XAttribute("name", SerializableObject.SerializationTag.Name));        

            foreach (System.Runtime.Serialization.SerializationEntry entry in si)
            {
                elem.Add(new XElement(entry.Name, entry.Value));
            }

            if (SerializableObject.ChildrenSerializable != null)
            {
                foreach (ISerializableObject childSerializable in SerializableObject.ChildrenSerializable.Where(p=>p != null))
                {
                    Serialize(elem, childSerializable);
                }
            }

            if (!elem.IsEmpty)
                XElement.Add(elem);
        }

        private void Deserialize(XElement XElement, ISerializableObject SerializableObject)
        {
            System.Runtime.Serialization.SerializationInfo si = new System.Runtime.Serialization.SerializationInfo(SerializableObject.GetType(), new System.Runtime.Serialization.FormatterConverter());

            if (XElement != null)
            {
                foreach (XElement xelem in XElement.Elements())
                {
                    if (!xelem.HasElements && !string.IsNullOrEmpty(xelem.Value))
                    {
                        si.AddValue(xelem.Name.LocalName, xelem.Value);
                    }
                }

                if (SerializableObject.ChildrenSerializable != null)
                {
                    foreach (ISerializableObject childSerializable in SerializableObject.ChildrenSerializable.Where(p=> p!=null && !p.SerializationTag.IsEmpty))
                    {
                        XElement xelem = XElement.Elements().
                            Where(p => p.Name == childSerializable.SerializationTag.Type).
                            Where(p => XmlLinq.GetAttribute(p, "name") == childSerializable.SerializationTag.Name).FirstOrDefault();

                        Deserialize(xelem, childSerializable);
                    }
                }
            }
            
            SerializableObject.Deserialize(si);    
        }

    }
}
