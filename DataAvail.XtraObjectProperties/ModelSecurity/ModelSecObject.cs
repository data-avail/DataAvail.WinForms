using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;


namespace DataAvail.XOP.ModelSecurity
{
    public class ModelSecObject : ModelSecBase
    {
        internal ModelSecObject(XElement XSec)
            : base(XSec)
        { }

        private readonly List<ModelSecField> _items = new List<ModelSecField>();

        private DefaultBoolType _isCanAdd;

        private DefaultBoolType _isCanRemove;

        public DefaultBoolType IsCanAdd
        {
            get { return _isCanAdd; }
            set { _isCanAdd = value; }
        }

        public DefaultBoolType IsCanRemove
        {
            get { return _isCanRemove; }
            set { _isCanRemove = value; }
        }

        public IEnumerable<ModelSecField> Fields { get { return _items; } }

        protected override void Parse(XElement XSec)
        {
            base.Parse(XSec);

            EditModeType em = EditMode.Parse(XmlLinq.GetAttribute(XSec, "mode")); 

            if (em != EditModeType.Default)
            { 
                //mode attribute is alway override readOnly & hidden attributes

                IsReadOnly =  DefaultBool.Convert((em & EditModeType.Edit) != EditModeType.Edit);

                IsHidden = DefaultBool.Convert((em & EditModeType.View) != EditModeType.View);

                IsCanAdd = DefaultBool.Convert((em & EditModeType.Add) == EditModeType.Add);

                IsCanRemove = DefaultBool.Convert((em & EditModeType.Delete) == EditModeType.Delete);
            }

            _items.AddRange(XSec.Elements("FieldSecurity").Select(p => new ModelSecField(p)));            
        }
    }

}
