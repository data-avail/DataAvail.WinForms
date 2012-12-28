using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;


namespace DataAvail.XOP.ModelSecurity
{
    public class ModelSecApp : ModelSecBase
    {
        private ModelSecApp(XElement XSec)
            : base(XSec)
        { 
        }

        public static ModelSecApp Load(XDocument XDocument)
        {
            return new ModelSecApp(XDocument.Root);
        }

        public static ModelSecApp Load(string FileName)
        {
            return Load(XDocument.Load(FileName));
        }

        private readonly List<ModelSecObject> _items = new List<ModelSecObject>();

        protected override void Parse(XElement XSec)
        {
            _items.AddRange(XSec.Elements("ObjectSecurity").Select(p => new ModelSecObject(p)));            
        }

        public IEnumerable<ModelSecObject> Objects { get { return _items; } }

    }
}
