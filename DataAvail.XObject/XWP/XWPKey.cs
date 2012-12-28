using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XObject.XWP
{
    public class XWPKey
    {
        internal XWPKey(XWPKeyCommand KeyCommand, string Key)
        {
            _keyCommand = KeyCommand;

            if (!string.IsNullOrEmpty(Key))
            {
                string[] strs = Key.Split(',');

                if (strs.Length == 1)
                {
                    _key = strs[0].Trim().ToUpper();
                }
                else
                {
                    _modifyers = strs[0].Split('|');
                    _key = strs[1].Trim().ToUpper();
                }
            }
        }

        private readonly XWPKeyCommand _keyCommand;

        private readonly string _key;

        private readonly string[] _modifyers;

        public string Key
        {
            get { return _key; }
        }

        public string[] Modifyers
        {
            get { return _modifyers; }
        }

        public XWPKeyCommand KeyCommand
        {
            get { return _keyCommand; }
        } 

    }
}
