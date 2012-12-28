using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;

namespace DataAvail.DX.XtraEditors
{
    internal class LookUpCommandItem : ButtonCommandItem
    {
        internal LookUpCommandItem(
            LookUpEdit LookUpEdit,
            DevExpress.XtraEditors.Controls.EditorButton Button,
            DataAvail.Controllers.Commands.ControllerCommandTypes CommandType
            ) :
            base(
            LookUpEdit,
            LookUpEdit.AppFieldContext,
            LookUpEdit.DxEdit,
            Button,
            CommandType)
        {
        }

        public override object Context
        {
            get
            {
                return new DataAvail.Controllers.Commands.UICommandItems.FKCommandItemContext(
                    (XOFieldContext)base.Context, ((LookUpEdit)XtraEdit).DataSourceProperties.Filter);
            }
        }
    }
}
