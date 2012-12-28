using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.DXEditors.DX
{
    public interface ITextSelectionDataProvider
    {
        TextSelectionData GetTextSelection(string Text);
    }
}
