using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraObjectProperties.XOPCreator
{
    internal class FKFieldPropertiesDictionary : FKFieldPropertiesBase
    {
        internal FKFieldPropertiesDictionary(XtraFieldProperties XtraFieldProperties, Dictionary<string, string> KeyNameDict)
            : base(XtraFieldProperties)
        {
            this.ValueMember = "Key";

            this.DisplayMember = "Value";

            SetDataSource(KeyNameDict, null);
        }
    }
}
