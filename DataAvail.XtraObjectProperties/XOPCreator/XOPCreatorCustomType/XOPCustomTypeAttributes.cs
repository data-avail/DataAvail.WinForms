using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraObjectProperties.XOPCreator.XOPCreatorCustomType
{
    public class FKDataSourceAttribute : Attribute
    {
        public FKDataSourceAttribute(string ThisField)
        { 
            this.ThisField = ThisField;
        }

        public string ThisField;
    }
 
}
