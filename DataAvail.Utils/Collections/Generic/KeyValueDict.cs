using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.Linq;

namespace DataAvail.Utils.Collections.Generic
{
    public class KeyValueDict<T> : IEnumerable<KeyValueItem<T>>
    {
        private readonly Dictionary<string, KeyValueItem<T>> _dict = new Dictionary<string, KeyValueItem<T>>();

        public void Assign(KeyValueDict<T> Dict)
        {
            Clear();

            foreach (KeyValueItem<T> kvi in Dict)
            {
                this[kvi.key] = kvi.value;
            }
        }

        #region IEnumerable<KeyValueItem<T>> Members

        public IEnumerator<KeyValueItem<T>> GetEnumerator()
        {
            return _dict.Values.GetEnumerator();   
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _dict.Values.GetEnumerator();
        }

        #endregion

        public bool Contains(string Name)
        {
            return _dict.Keys.Contains(Name);
        }

        public T this[string Key]
        {
            get 
            {
                return Contains(Key) ? _dict[Key].value : default(T);
            }

            set
            {
                if (!Contains(Key))
                {
                    _dict.Add(Key, new KeyValueItem<T>() { key = Key, value = value });
                }
                else
                {
                    _dict[Key].value = value;
                }
            }
        }

        public void Clear()
        {
            _dict.Clear();
        }

        public Dictionary<string, T> ToDictionary()
        {
            return this.ToDictionary(p => p.key, s => s.value);
        }

    }
}
