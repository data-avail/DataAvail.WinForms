using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;

namespace DataAvail.DX.XtraEditors
{
    public class MemoEdit : BaseTextEdit<DataAvail.DXEditors.DX.MemoEdit>
    {
        public MemoEdit(XOFieldContext FieldContext)
            : base(FieldContext)
        {
            this.MaximumSize = new System.Drawing.Size(0, 0);
            this.MinimumSize = new System.Drawing.Size(0, 0);
        }
    }
}
