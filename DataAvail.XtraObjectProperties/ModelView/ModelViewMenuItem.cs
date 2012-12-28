using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;

namespace DataAvail.XOP.ModelView
{
    public class ModelViewMenuItem
    {
        internal ModelViewMenuItem()
        { 
        }

        internal ModelViewMenuItem(XElement XMenuItem)
        {
            this.Parse(XMenuItem);
        }

        private string _caption;

        private string _hint;

        private string _appItemId;

        private ModelViewAppItem _appItem;

        private ModelViewAppKey _key;

        private readonly List<ModelViewMenuItem> _items = new List<ModelViewMenuItem>();

        internal void Parse(XElement XMenuItem)
        {
            _caption = XmlLinq.GetAttribute(XMenuItem, "caption");

            _appItemId = XmlLinq.GetAttribute(XMenuItem, "appItem");

            _hint = XmlLinq.GetAttribute(XMenuItem, "hint");

            _items.AddRange(XMenuItem.Elements("MenuItem").Select(p => new ModelViewMenuItem(p)));

            string key = XmlLinq.GetAttribute(XMenuItem, "key");

            if (!string.IsNullOrEmpty(key))
                _key = new ModelViewAppKey(key);
        }

        internal void SetAppItem(ModelViewAppItem AppItem)
        {
            _appItem = AppItem;
        }

        public ModelViewAppKey Key
        {
            get { return _key; }
        }

        public string Caption
        {
            get { return _caption; }
        }

        public string Hint
        {
            get { return _hint; }
        }

        public string AppItemId
        {
            get { return _appItemId; }
        }

        public ModelViewAppItem AppItem
        {
            get { return _appItem; }
        }

        public IEnumerable<ModelViewMenuItem> Items
        {
            get { return _items; }
        }

        public IEnumerable<ModelViewMenuItem> Descendants { get { return Items.Union(Items.SelectMany(p => p.Descendants)); } }
    }
}
