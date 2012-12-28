using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraObjectProperties
{
    public class XtraObjectFunctionParameter
    {
        public XtraFieldProperties mappedField;

        public string name;

        public System.Type type;

        public int size;

        public System.Data.ParameterDirection direction = System.Data.ParameterDirection.Input;

        public object val;
    }


}
