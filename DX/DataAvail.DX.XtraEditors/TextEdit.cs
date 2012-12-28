using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;

namespace DataAvail.DX.XtraEditors
{
    public class TextEdit : BaseTextEdit<DataAvail.DXEditors.DX.TextEdit>
    {
        public TextEdit(XOFieldContext XOFieldContext)
            : base(XOFieldContext)
        {
        }
    }
}
