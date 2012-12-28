using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraBindings
{
    public interface IEditableObjectBaseOpers
    {
        EditableObject CreateInstance();

        void CopyTo(EditableObject ObjectTo);
    }
}
