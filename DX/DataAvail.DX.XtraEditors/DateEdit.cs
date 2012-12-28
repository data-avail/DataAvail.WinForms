using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;

namespace DataAvail.DX.XtraEditors
{
    public class DateEdit : BaseTextEdit<DataAvail.DXEditors.DX.DateEdit>
    {
        public DateEdit(XOFieldContext XOFieldContext)
            : base(XOFieldContext)
        {
            this.DxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
        }

        #region ISearchEditValueFormatter Members

        public override string FormattedValue
        {
            get
            {
                return GetFormattedValue(this.DxEdit);
            }

            set
            {
                SetFormattedValue(this.DxEdit, value);
            }
        }

        #endregion

        public static string GetFormattedValue(DevExpress.XtraEditors.DateEdit DateEdit)
        {
            return DateEdit.DateTime != System.DateTime.MinValue ? DateEdit.DateTime.ToString("dd-MM-yy") : null;
        }

        public static void SetFormattedValue(DevExpress.XtraEditors.DateEdit DateEdit, string Value)
        {
            System.DateTime dt;

            if (DateTime.TryParseExact(Value, "dd-MM-yy", null, System.Globalization.DateTimeStyles.None, out dt))
            {
                DateEdit.DateTime = dt;
            }
        }


    }
}
