using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Registrator;

namespace DataAvail.DXEditors.DX
{
    internal class XtraEditorPainter
    {
        private TextSelectionData _textSelection;

        internal TextSelectionData TextSelection
        {
            get { return _textSelection; }

            set { _textSelection = value; }
        }

        internal void SetTextSelection(string Filter, System.Drawing.Color Color)
        {
            this.TextSelection = new TextSelectionData(Filter, Color);
        }

        internal void SetTextSelection(int FirstIndex, int Length, System.Drawing.Color Color)
        {
            this.TextSelection = new TextSelectionData(FirstIndex, Length, Color);
        }

        internal void SetTextSelection(string Text, string Filter, System.Drawing.Color Color)
        {
            SetTextSelection(null, Color);

            TextSelection.Calculate(Text, Filter);
        }

        internal void DrawTextSelection(ControlGraphicsInfoArgs info)
        {
            DrawTextSelection(info.ViewInfo.DisplayText, 0, info, TextSelection);
        }

        internal void DrawTextSelection(string PaintText, int Offset, ControlGraphicsInfoArgs info)
        {
            DrawTextSelection(PaintText, Offset, info, TextSelection);
        }

        internal static void DrawTextSelection(string PaintText, int Offset, ControlGraphicsInfoArgs info, TextSelectionData SelectTextData)
        {
            if (SelectTextData != null && SelectTextData.Markers != null)
            {
                foreach (var i in SelectTextData.Markers)
                {
                    DrawTextSelection(PaintText, Offset, info, SelectTextData.color, i.Start, i.Length);
                }
            }
        }

        private static void DrawTextSelection(string PaintText, int Offset, ControlGraphicsInfoArgs info, Color Color, int StartIndex, int Length)
        {
            if (StartIndex != -1)
            {
                string strPrec = PaintText.Substring(0, StartIndex);

                string str = PaintText.Substring(StartIndex, Length);

                SizeF sizePrec = info.ViewInfo.PaintAppearance.CalcTextSize(info.Cache, strPrec, info.Bounds.Width);

                SizeF size = info.ViewInfo.PaintAppearance.CalcTextSize(info.Cache, str, info.Bounds.Width);

                RectangleF rect = new RectangleF(Offset + info.ViewInfo.ContentRect.X + sizePrec.Width, info.Bounds.Y + info.ViewInfo.ContentRect.Y, size.Width, size.Height);

                info.Cache.FillRectangle(new SolidBrush(Color), rect);
            }        
        }
    }
}
