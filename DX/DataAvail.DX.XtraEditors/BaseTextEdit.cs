using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;
using DataAvail.XObject.XWP;

namespace DataAvail.DX.XtraEditors
{
    public class BaseTextEdit<T> : BaseEdit<T> where T : DevExpress.XtraEditors.TextEdit, new()
    {
        public BaseTextEdit(XOFieldContext XOFieldContext)
            : base(XOFieldContext)
        {
            base.DxEdit.Properties.NullText = null;
        }

        private XWPFieldMask _mask;

        public XWPFieldMask Mask
        {
            get
            {
                return _mask;
            }

            set
            {
                if (value != Mask)
                {
                    _mask = value;

                    OnMaskChanged();
                }
            }
        }

        protected virtual void OnMaskChanged()
        {
            if (Mask != null)
            {
                this.DxEdit.Properties.Mask.EditMask = Mask.Mask;
                this.DxEdit.Properties.Mask.MaskType = DataAvail.DX.Utils.Converter.ConvertMaskType(Mask.MaskType);
                this.DxEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
            }
            else
            {
                this.DxEdit.Properties.Mask.EditMask = null;
                this.DxEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
                this.DxEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
            }
        }

        protected override void OnControlPropertiesPropertyChanged(string PropertyName)
        {
            base.OnControlPropertiesPropertyChanged(PropertyName);

            switch (PropertyName)
            { 
                case "Mask":
                    if (!string.IsNullOrEmpty(ControlProperties.Mask))
                        this.Mask = XWPFieldMask.Parse((string)ControlProperties.Mask);
                    else
                        this.Mask = null;
                    break;
            }
        }
    }
}
