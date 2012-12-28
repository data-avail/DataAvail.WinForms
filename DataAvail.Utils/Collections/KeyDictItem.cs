using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Utils.Collections
{
    public class KeyDictItem : DataAvail.Utils.Collections.Generic.KeyDictItem<object>
    {
        public static implicit operator KeyDictItem(Generic.KeyValueItem<Generic.KeyDictItem<object>> Val)
        {
            return (KeyDictItem)Val;
        }
 
    }
}
