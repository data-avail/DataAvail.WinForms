using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraMenu
{
    public interface IXtraMenuButton
    {
        XtraMenuButtonType ButtonType { get; }

        event System.EventHandler ButtonClick;

        bool Visible { get; set; }

        bool Enabled { get; set; }
    }

    public enum XtraMenuButtonType
    { 
        AcceptChanges,

        RejectChanges,

        EndEdit,

        CancelEdit,
         
        MovePerv,

        MoveNext, 

        StartFill,

        StopFill, 

        Clone,

        Refill,

        UploadToExcel,

        Custom = UploadToExcel + 1
    }
}
