using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Utils
{
    [Flags]
    public enum EditModeType
    {
        None = 0x00,
        View = 0x01,
        Edit = 0x02,
        Add = 0x04,
        Delete = 0x08,
        Default = 0x10,
        All = View | Edit | Add | Delete
    }

    public class EditMode
    {
        public static EditModeType Parse(string Value)
        {
            if (!string.IsNullOrEmpty(Value))
                return DataAvail.Utils.EnumFlags.Parse<EditModeType>(Value, EditModeType.All);
            else
                return EditModeType.Default;
        }
    }
}
