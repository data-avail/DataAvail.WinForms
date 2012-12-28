using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.ViewInfo;

namespace DataAvail.DXEditors.DX
{
    public class ListBoxControl : DevExpress.XtraEditors.ListBoxControl, ITextSelectionDataProvider
    {
        public ListBoxControl() 
            : base() 
        {
            this.HotTrackItems = true;
        }

        public event RequestItemHighlightHandler RequestItemHighlight;

        private void OnRequestItemHighlight(RequestItemHighlightEventArgs args)
        {
            if (RequestItemHighlight != null)
                RequestItemHighlight(this, args);
        }

        protected override BaseStyleControlViewInfo CreateViewInfo(
            ) { return new BaseListBoxViewInfo(this, this); }

        public TextSelectionData GetTextSelection(string Text)
        {
            RequestItemHighlightEventArgs args = new RequestItemHighlightEventArgs(Text);

            OnRequestItemHighlight(args);

            return args.TextSelection;
        }
    }

    public class RequestItemHighlightEventArgs : System.EventArgs
    {
        internal RequestItemHighlightEventArgs(string Text)
        {
            this.text = Text;     
        }

        public readonly string text;

        public TextSelectionData TextSelection;
    }

    public delegate void RequestItemHighlightHandler(object sender, RequestItemHighlightEventArgs args);
}