using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DataAvail.XOP.ModelView
{
    public class ModelViewAppKey
    {
        internal ModelViewAppKey(string Key)
        {
            string [] strs = Key.Split(',');

            if (strs.Length == 1)
            {
                key = strs[0].Trim().ToUpper();
            }
            else
            {
                modifyers = strs[0].Split('|');
                key = strs[1].Trim().ToUpper();
            }
        }

        public readonly string [] modifyers;

        public readonly string key;
    }
}
