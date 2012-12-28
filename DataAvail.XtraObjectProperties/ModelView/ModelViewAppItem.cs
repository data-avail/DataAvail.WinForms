using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;

namespace DataAvail.XOP.ModelView
{
    public class ModelViewAppItem : ModelViewAppItemBase
    {
        internal ModelViewAppItem()
            : base()
        { }

        internal ModelViewAppItem(ModelViewAppItemBase Parent, XElement XElement)
            : base(Parent, XElement)
        { 
        }

        internal override void Parse(XElement XAction)
        {
            base.Parse(XAction);

            _name = XmlLinq.GetAttribute(XAction, "name");

            _id = XmlLinq.GetAttribute(XAction, "id");

            _children.AddRange(XAction.Elements("ChildAppItem").Select(p => new ModelViewAppItem(this, p)));
        }

        private string _name;

        private string _id;

        private readonly List<ModelViewAppItem> _children = new List<ModelViewAppItem>();

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public IEnumerable<ModelViewAppItem> Children { get { return _children; } }
    }
}
