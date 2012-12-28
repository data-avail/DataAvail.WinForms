using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Controllers.Commands
{
    public enum ControllerCommandTypes
    {
        None = 0,
        ItemSelect,
        ItemShow,
        ListShow,
        ItemClose,
        ListClose,
        EndEdit,
        CancelEdit,
        AcceptChanges,
        RejectChanges, 
        Clone,
        Add,
        Remove,
        MoveNext,
        MovePerv,
        BatchAcceptChanges,
        BatchRejectChanges,
        Fill,
        CancelFill,
        SelectFkItem,
        AddFkItem,
        Refill,
        UploadToExcel,
        FocusList,
        FocusSearch
    }
}
