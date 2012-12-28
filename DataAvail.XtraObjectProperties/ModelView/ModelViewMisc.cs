using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DataAvail.XOP.ModelView
{
    public class ModelViewMisc
    {
        internal ModelViewMisc()
        {
        }

        private string _skinName;

        internal void Parse(XElement MiscElement)
        {
            if (MiscElement != null && MiscElement.Element("AppSkin") != null)
            {
                _skinName = DataAvail.Utils.XmlLinq.XmlLinq.GetAttribute(MiscElement.Element("AppSkin"), "name");
            }
        }

        public string SkinName
        {
            get { return _skinName; }
        }
    }
}
