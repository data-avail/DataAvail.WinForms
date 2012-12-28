using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraBindings
{
    public class XtraBindingUpdateSetRejectResult
    {
        public XtraBindingUpdateSetRejectResult(XtraBinding XtraBinding, string PkFieldName, IEnumerable<XtraBindingStoredItem> Items)
        {
            xtraBinding = XtraBinding;

            pkFieldName = PkFieldName;

            items = Items;
        }

        public readonly XtraBinding xtraBinding;

        public readonly string pkFieldName;

        public readonly IEnumerable<XtraBindingStoredItem> items;
    }
}
