using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;

namespace DataAvail.XOP.ModelView
{
    public class ModelViewMainMenu
    {
        internal ModelViewMainMenu()
        {
        }

        private readonly List<ModelViewMenuItem> _items = new List<ModelViewMenuItem>();

        internal void Parse(XElement XMainMenu)
        {
            _items.AddRange(XMainMenu.Elements("MenuItem").Select(p => new ModelViewMenuItem(p)));            
        }

        public IEnumerable<ModelViewMenuItem> Items { get { return _items; } }

        public IEnumerable<ModelViewMenuItem> Descendants { get { return Items.Union(Items.SelectMany(p => p.Descendants)); } }
    }
}
