using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Utils.Collections.Generic
{
    public class KeyValueItem<T>
    {
        public KeyValueItem()
        { }

        public KeyValueItem(string Key, T Value)
        {
            this.key = Key;

            this.value = Value;
        }

        public string key;

        public T value;
    }
}
