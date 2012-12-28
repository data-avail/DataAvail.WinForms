using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraBindings
{
    internal class XtraBindingUpdateSetEntity
    {
        internal XtraBindingUpdateSetEntity(XtraBinding XtraBinding)
        {
            xtraBinding = XtraBinding;
        }

        internal readonly XtraBinding xtraBinding;

        internal readonly List<XtraBindingStoredItem> StoredItems = new List<XtraBindingStoredItem>();
    }
}
