using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraBindings
{
    public class XtraBindingUpdateSetUpdateResult : XtraBindingUpdateSetRejectResult
    {
        public XtraBindingUpdateSetUpdateResult(XtraBinding XtraBinding, string PkFieldName, IEnumerable<XtraBindingStoredItem> Items, IEnumerable<object> PKsAfterUpdate)
            : base(XtraBinding, PkFieldName, Items)
        {
            pksAfterUpdate = PKsAfterUpdate;
        }

        public readonly IEnumerable<object> pksAfterUpdate;

    }
}
