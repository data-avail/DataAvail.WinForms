using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Utils.Collections.Generic
{
    public class KeyDictDict<T> : KeyValueDict<KeyDictItem<T>>
    {
        public T this[string Key1, string Key2]
        {
            get
            {
                KeyDictItem<T> item = this[Key1];

                if (item != null)
                {
                    return item.value[Key2];
                }
                else
                {
                    return default(T);
                }
            }

            set
            {
                if (!Contains(Key1))
                {
                    this[Key1] = new KeyDictItem<T>() { key = Key1, value = new KeyValueDict<T>() };
                }

                this[Key1].value[Key2] = value;
            }
        }

        public void Assign(KeyDictDict<T> Dict)
        {
            Clear();

            foreach (KeyValueItem<KeyDictItem<T>> kdi in Dict)
            {
                this[kdi.key] = new KeyDictItem<T>() { key = kdi.key, value = new KeyValueDict<T>() };

                this[kdi.key].value.Assign(kdi.value.value);
            }
        }

    }
}
