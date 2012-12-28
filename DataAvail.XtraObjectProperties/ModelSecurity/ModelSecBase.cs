using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;


namespace DataAvail.XOP.ModelSecurity
{
    public class ModelSecBase
    {
        internal ModelSecBase(XElement XSec)
        {
            Parse(XSec);
        }

        private DefaultBoolType _isHidden;

        private DefaultBoolType _isReadOnly;

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public DefaultBoolType IsHidden
        {
            get { return _isHidden; }
            set { _isHidden = value; }
        }

        public DefaultBoolType IsReadOnly
        {
            get { return _isReadOnly; }
            set { _isReadOnly = value; }
        }

        protected virtual void Parse(XElement XSec)
        {
            Name = XmlLinq.GetAttribute(XSec, "name");

            IsHidden = new DefaultBool(XmlLinq.GetAttribute(XSec, "hidden"));

            IsReadOnly = new DefaultBool(XmlLinq.GetAttribute(XSec, "readOnly"));
        }
    }
}
