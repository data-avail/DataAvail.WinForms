using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Drawing;


namespace DataAvail.DXEditors.DX
{
    public class ListBoxItemPainter : DevExpress.XtraEditors.Drawing.ListBoxSkinItemPainter
    {
        internal ListBoxItemPainter(ITextSelectionDataProvider TextSelectionDataProvider)
        {
            _textSelectionDataProvider = TextSelectionDataProvider;
        }

        private readonly XtraEditorPainter _painter = new XtraEditorPainter();

        private readonly ITextSelectionDataProvider _textSelectionDataProvider;

        internal XtraEditorPainter Painter
        {
            get { return _painter; }
        }

        protected override void DrawItemBar(ListBoxItemObjectInfoArgs e)
        {
            DrawItemBarCore(e);
        }

        protected override void DrawItemText(ListBoxItemObjectInfoArgs e)
        {
            base.DrawItemText(e);

            if (_textSelectionDataProvider != null)
            {
                TextSelectionData ts = _textSelectionDataProvider.GetTextSelection(e.ItemText);

                if (ts != null)
                    Painter.TextSelection = ts;
                else
                    Painter.TextSelection = null;
            }


            if (Painter.TextSelection != null)
            {
                Painter.DrawTextSelection(e.ItemText, 4, new ControlGraphicsInfoArgs(e.ViewInfo, e.Cache, e.Bounds));
            }
        }
    }
}
