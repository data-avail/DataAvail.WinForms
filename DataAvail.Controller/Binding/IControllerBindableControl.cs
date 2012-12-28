using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Controllers.Binding
{
    public interface IControllerBindableControl
    {
        string FieldName { get; }

        string ValuePropertyName { get; }

        CCProperties ControlProperties { get; set; }
    }
}
