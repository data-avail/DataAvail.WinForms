using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraContainerBuilder
{
    public class XtraContainerObjectProperties
    {
        public XtraContainerObjectProperties(DataAvail.XtraObjectProperties.XtraObjectProperties ObjectProperties)
        {
            objectProperties = ObjectProperties;
        }

        public readonly DataAvail.XtraObjectProperties.XtraObjectProperties objectProperties;
    }
}
