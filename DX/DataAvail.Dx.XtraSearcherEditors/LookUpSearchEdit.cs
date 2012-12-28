﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;

namespace DataAvail.DX.XtraSearcherEditors
{
    public class LookUpSearchEdit : DataAvail.DX.XtraEditors.LookUpEdit, DataAvail.XtraSearcherEditors.IXtraSearchEdit, DataAvail.XtraSearcherEditors.IXtraSearchBaseEdit
    {
        public LookUpSearchEdit(XOFieldContext XOFieldContext)
            : base(XOFieldContext)
        {
            _searchEditDgtr = new DataAvail.XtraSearcherEditors.BaseSearchEdit<LookUpSearchEdit>(this, new CheckEdit());

            _searchEditDgtr.Initialize();
        }

        private readonly DataAvail.XtraSearcherEditors.BaseSearchEdit<LookUpSearchEdit> _searchEditDgtr;

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
    }
}
