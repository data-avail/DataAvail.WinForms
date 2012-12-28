using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XObject
{
    [Flags]
    public enum XOMode
    {
        None = 0x00,
        View = 0x01,
        Edit = 0x02,
        Add = 0x04,
        Delete = 0x08,
        All = View | Edit | Add | Delete
    }
}
