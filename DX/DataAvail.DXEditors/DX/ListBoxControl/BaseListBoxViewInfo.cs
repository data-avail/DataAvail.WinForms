using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.ViewInfo;


namespace DataAvail.DXEditors.DX
{
    public class BaseListBoxViewInfo : DevExpress.XtraEditors.ViewInfo.BaseListBoxViewInfo
    {
        public BaseListBoxViewInfo(BaseListBoxControl owner, ITextSelectionDataProvider TextSelectionDataProvider) 
            : base(owner) 
        {
            _textSelectionDataProvider = TextSelectionDataProvider;
        }

        private readonly ITextSelectionDataProvider _textSelectionDataProvider;


        protected override BaseListBoxItemPainter CreateItemPainter()
        {
            return new ListBoxItemPainter(_textSelectionDataProvider);
        }
    }
}
