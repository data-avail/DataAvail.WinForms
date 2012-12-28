using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraObjectProperties
{
    public interface IFKFieldProperties
    {
        string ValueMember { get; set; }

        string DisplayMember { get; set; }

        object DataSource { get; }

        string Filter { get; } 
    }
}
