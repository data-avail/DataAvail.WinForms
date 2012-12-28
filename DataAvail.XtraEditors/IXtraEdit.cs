using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraEditors
{
    public interface IXtraEdit
    {
        object EditValue { get; set; }

        event System.EventHandler EditValueChanged;

        System.Type FieldType { get; }

        string FieldName { get; }
    }
}
