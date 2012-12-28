using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;

namespace DataAvail.DX.XtraSearcherEditors
{
    public class DateSearchEdit : DataAvail.DX.XtraEditors.DateEdit, DataAvail.XtraSearcherEditors.IXtraSearchEdit, DataAvail.XtraSearcherEditors.IXtraSearchBaseEdit
    {
        public DateSearchEdit(XOFieldContext XOFieldContext)
            : base(XOFieldContext)
        {
            _searchEditDgtr = new DataAvail.XtraSearcherEditors.BaseSearchEdit<DateSearchEdit>(this, new CheckEdit());

            _searchEditDgtr.Initialize();
        }

        private readonly DataAvail.XtraSearcherEditors.BaseSearchEdit<DateSearchEdit> _searchEditDgtr;

        #region IXtraSearchEdit Members

        public string GetExpression() 
        {
            return _searchEditDgtr.GetExpression(); 
        }

        #endregion

        public override void Serialize(System.Runtime.Serialization.SerializationInfo SerializationInfo)
        {
            _searchEditDgtr.Serialize(SerializationInfo);
        }

        public override void Deserialize(System.Runtime.Serialization.SerializationInfo SerializationInfo)
        {
            _searchEditDgtr.Deserialize(SerializationInfo);
        }

        protected override IEnumerable<DataAvail.Controllers.Commands.IControllerCommandItem> CreateCommandItems()
        {
            return new DataAvail.Controllers.Commands.IControllerCommandItem[] { };
        }

    }
}
