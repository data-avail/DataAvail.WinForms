using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DataAvail.XOP.ModelView
{
    public class ModelViewAppItems : ModelViewAppItemBase
    {
        internal ModelViewAppItems()
            : base()
        {
        }

        internal override void Parse(XElement XAction)
        {
            base.Parse(XAction);

            _items.AddRange(XAction.Elements("AppItem").Select(p => new ModelViewAppItem(this, p)));
        }

        private readonly List<ModelViewAppItem> _items = new List<ModelViewAppItem>();

        public IEnumerable<ModelViewAppItem> Items { get { return _items; } }

    }
}
