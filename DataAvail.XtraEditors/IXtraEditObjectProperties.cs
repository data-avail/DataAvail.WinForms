using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraEditors
{
    public interface IXtraEditObjectProperties
    {
        void SetObjectProperties(Dictionary<string, object> NameValue);
    }
}
