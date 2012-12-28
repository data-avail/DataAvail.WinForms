using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;
using DataAvail.XObject.XWP;

namespace DataAvail.DX.Utils
{
    public static class Converter
    {
        public static DevExpress.XtraEditors.Mask.MaskType ConvertMaskType(XWPFieldMaskType MaskType)
        {
            switch (MaskType)
            { 
                case XWPFieldMaskType.Numeric:
                    return DevExpress.XtraEditors.Mask.MaskType.Numeric;
                case XWPFieldMaskType.Date:
                    return DevExpress.XtraEditors.Mask.MaskType.DateTime;
                case XWPFieldMaskType.RegEx:
                    return DevExpress.XtraEditors.Mask.MaskType.RegEx;
            }

            return DevExpress.XtraEditors.Mask.MaskType.None;
        }

        public static DevExpress.Utils.FormatType GetFormatType(XWPFieldMaskType MaskType)
        {
            switch (MaskType)
            {
                case XWPFieldMaskType.Numeric:
                    return DevExpress.Utils.FormatType.Numeric;
                case XWPFieldMaskType.Date:
                    return DevExpress.Utils.FormatType.DateTime;
                case XWPFieldMaskType.RegEx:
                    return DevExpress.Utils.FormatType.Custom;
            }

            return DevExpress.Utils.FormatType.None;
        }

        public static DevExpress.Utils.FormatInfo GetFormatInfo(XWPFieldMask Mask)
        {
            DevExpress.Utils.FormatType formatType = GetFormatType(Mask.MaskType);

            if (formatType != DevExpress.Utils.FormatType.None)
            {
                return new DevExpress.Utils.FormatInfo() { FormatType = formatType, FormatString = DataAvail.Utils.String.FormatMask(Mask.Mask) };
            }
            else
            {
                return DevExpress.Utils.FormatInfo.Empty;
            }
        }



    }
}
