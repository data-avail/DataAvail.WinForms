using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.DX.XtraSearcherEditors
{
    internal class CheckEdit : DataAvail.DXEditors.DX.CheckEdit, DataAvail.XtraSearcherEditors.ICheckEdit
    {
        internal CheckEdit()
        {
            this.Text = null;   
        }

        protected override void OnParentVisibleChanged(EventArgs e)
        {
            base.OnParentVisibleChanged(e);

            this.Size = new System.Drawing.Size(this.ViewInfo.CheckInfo.GlyphRect.Size.Width + 3, this.ViewInfo.CheckInfo.GlyphRect.Size.Height);
        }

        

    }
}
