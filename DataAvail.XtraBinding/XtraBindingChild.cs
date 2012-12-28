using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraBindings
{
    public class XtraBindingChild : XtraBinding
    {
        public XtraBindingChild(object DataSource, XtraBindingOperation XtraBindingOperation, XtraBindingChildProperties XtraBindingChildProperties)
            : base(DataSource, XtraBindingOperation)
        {
            _xtraBindingChildProperties = XtraBindingChildProperties;
        }

        private readonly XtraBindingChildProperties _xtraBindingChildProperties;

        internal XtraBinding Clone(DataAvail.XtraBindings.XtraBindingContainer ParentXtraBindingContainer)
        {
            XtraBindingChild xtraBindingChild = new XtraBindingChild(DataSource, xtraBindingOperation, new XtraBindingChildProperties(ParentXtraBindingContainer,
                _xtraBindingChildProperties.parentObjectName, _xtraBindingChildProperties.parentFieldName,
                _xtraBindingChildProperties.childObjectName, _xtraBindingChildProperties.fkFieldName));

            return xtraBindingChild;
        }

        public void UpdateRelationFK(object DataSourceItem)
        {
            ((DataAvail.XtraBindings.BindingSource)this.BindingSource).SetItemValue(DataSourceItem, ChildProperties.fkFieldName, ParentPKValue);
        }

        internal object ParentPKValue { get { return (((DataAvail.XtraBindings.BindingSource)ChildProperties.parentBinding.BindingSource)).GetItemValue(ChildProperties.parentBinding.BindingSource.Current, ChildProperties.parentFieldName); } }

        public XtraBindingChildProperties ChildProperties { get { return _xtraBindingChildProperties; } }

        internal void Fill(object ParentDataSourceItem)
        {
            object parentVal = ((BindingSource)_xtraBindingChildProperties.parentBinding.BindingSource).GetItemValue(ParentDataSourceItem, _xtraBindingChildProperties.parentFieldName);

            string filter = string.Format("{0} = {1}", _xtraBindingChildProperties.fkFieldName, parentVal);
            
            base.Fill(filter, true);
        }

    }
}
