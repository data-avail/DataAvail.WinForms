using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;

namespace DataAvail.XObject.XOP
{
    public class XOPFieldType
    {
        public XOPFieldType(XElement XElement)
            : this(XElement, true)
        { 
            
        }

        public XOPFieldType(XElement XElement, bool Obrigatory)
        {
            XAttribute attr;

            string strType = XmlLinq.ReadAttribute(XElement, "type", Obrigatory, XOApplication.xmlReaderLog, out attr);

            try
            {
                if (!string.IsNullOrEmpty(strType))
                {
                    string[] strs = strType.Split(',');

                    _type = (Type)Reflection.Parse(typeof(Type), strs[0]);

                    _size = 1;

                    if (strs.Length == 2)
                    {
                        _size = int.Parse(strs[1]);
                    }
                    else if(strs.Length > 2)
                    {
                        throw new XmlLinqReaderException("Incorrect type format.");
                    }
                }
            }
            catch (System.Exception e)
            {
                XmlLinqReaderLog.WriteToLog(XOApplication.xmlReaderLog, XElement, attr, e);

                if (XmlLinqReaderLog.IsThrowException(XOApplication.xmlReaderLog))
                    throw;
            }
           
        }

        private readonly Type _type;

        private readonly int _size;

        public Type Type
        {
            get { return _type; }
        }

        public int Size
        {
            get { return _size; }
        } 

    }
}
