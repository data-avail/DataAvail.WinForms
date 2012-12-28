using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraSearcherEditors
{
    public interface IXtraSearchBaseEdit
    {
        bool ReadOnly { get; set; }

        string FormattedValue { get; set; }
    }
}
