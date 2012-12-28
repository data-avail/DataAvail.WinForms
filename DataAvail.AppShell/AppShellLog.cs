using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.AppShell
{
    internal class AppShellLog : DataAvail.XObject.IXmlReaderLog
    {
        internal static AppShellLog Log = new AppShellLog();


        internal void Write(string Text, System.Exception ex)
        { 
        
        }

        #region IXmlLinqReaderLog Members

        public void Write(string Text, params object[] prms)
        {

        }


        public void Write(System.Xml.Linq.XElement XElement, System.Xml.Linq.XAttribute Attribute, string Text, int InfoType)
        {
            
        }

        public void WriteException(System.Xml.Linq.XElement XElement, System.Xml.Linq.XAttribute Attribute, Exception Exception)
        {
            
        }

        public void WriteUnexpectedValueType(System.Xml.Linq.XElement XElement, System.Xml.Linq.XAttribute Attribute, Type ExpectedType)
        {
            
        }

        public void WriteUnexpectedEnumValue(System.Xml.Linq.XElement XElement, System.Xml.Linq.XAttribute Attribute, Type EnumType)
        {
            
        }

        public bool ContinueOnException
        {
            get
            {
                return false;
            }

            set
            {
            }
        }

        #endregion
    }
}
