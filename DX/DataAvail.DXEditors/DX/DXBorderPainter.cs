using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DataAvail.DXEditors.DX
{
    internal class DXBorderPinter : DevExpress.Utils.Drawing.BorderPainter
    {
        public override void DrawObject(DevExpress.Utils.Drawing.ObjectInfoArgs e)
        {
            e.Cache.DrawRectangle(new Pen(Color.Gold, 2), new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, e.Bounds.Width - 1, e.Bounds.Height - 1));
        }

    }

    internal class DXCheckEditBorderPinter : DevExpress.Utils.Drawing.BorderPainter
    {
        public override Color GetBorderColor(DevExpress.Utils.Drawing.ObjectInfoArgs e)
        {
            return base.GetBorderColor(e);
        }

    }
}
