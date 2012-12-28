using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Controllers
{
    [Flags]
    public enum ControllerStates
    {
        None = 0x00,
        CanEndEdit = 0x01,
        CanCancelEdit = 0x02,
        CanAcceptChanges = 0x04,
        CanRejectChanges = 0x08,
        CanBatchAcceptChanges = 0x10,
        CanBatchRejectChanges = 0x20,
        CanClone = 0x80,
        CanMovePerv = 0x100,
        CanMoveNext =0x200,
        CanAdd = 0x400,
        CanRemove = 0x800,
        CanEdit = 0x1000,
        ItemCanShow = 0x2000,
        CanFill = 0x4000,
        CanCancelFill = 0x8000,
        ListCanShow = 0x10000,
        CanAll = CanEndEdit | CanCancelEdit | CanAcceptChanges | CanRejectChanges 
            | CanBatchAcceptChanges | CanBatchRejectChanges | CanClone | CanMovePerv 
            | CanMoveNext | CanAdd | CanRemove | CanEdit | ItemCanShow | CanFill | CanCancelEdit | ListCanShow
    }
}
