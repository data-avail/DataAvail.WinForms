using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraSearcherEditors
{
    public interface ICheckEdit
    {
        bool Checked { get; set; }

        event System.EventHandler CheckedChanged;
    }
}
