using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraObjectProperties.XOPCreator.XOPCreatorLinq
{
    public class XOPLinqAssociationAttribute : System.Attribute
    {
        public string ParentCoulmn;

        public Type ParentTableType;

        public System.Linq.Expressions.Expression Filter;
    }
}
