using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraContainerBuilder
{
    public interface IXtraContinerObjectParser
    {
        XtraContainerObjectProperties ObjectProperies { get; }

        XtraContainerFieldProperties GetNextFieldProperties();
    }
}
