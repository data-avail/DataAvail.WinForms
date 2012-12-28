using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraObjectProperties
{
    public class XtraFieldMask
    {
        public XtraFieldMask()
        { }

        public XtraFieldMask(string Mask, XtraFieldMaskType MaskType)
        {
            this.Mask = Mask;

            this.MaskType = MaskType;
        }

        public readonly string Mask;

        public readonly XtraFieldMaskType MaskType;

        #region Constants

        public const string STD_DECIMAL_MASK = ",################0.00##########";

        public const string STD_NUMERIC_MASK = "n0";

        public const string STD_DATE_MASK = "d";

        public const string STD_PHONE_MASK = @"\(\d\d\d\)\d{1,3}-\d\d-\d\d";

        public const string STD_EMAIL_MASK = @"\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b";

        public readonly static XtraFieldMask StdDecimal = new XtraFieldMask(STD_DECIMAL_MASK, XtraFieldMaskType.Numeric);

        public readonly static XtraFieldMask StdNumeric = new XtraFieldMask(STD_NUMERIC_MASK, XtraFieldMaskType.Numeric);

        public readonly static XtraFieldMask StdDate = new XtraFieldMask(STD_DATE_MASK, XtraFieldMaskType.Date);

        #endregion

        public string Format()
        {
            string maskType = FormatMaskType(this.MaskType);

            if (!string.IsNullOrEmpty(maskType) && !string.IsNullOrEmpty(Mask))
                return string.Format("{0}{1}", maskType, Mask);
            else
                return null;

        }

        public static XtraFieldMask Parse(string Mask)
        {
            if (Mask == "none")
                return null;

            DataAvail.XtraObjectProperties.XtraFieldMaskType maskType = DataAvail.XtraObjectProperties.XtraFieldMaskType.None;

            string tag = Mask.Length > 1 && Mask[1] == ':' ? Mask.Substring(0, 2) : null;

            string mask = string.IsNullOrEmpty(tag) ? Mask : Mask.Remove(0, 2);

            switch (tag)
            {
                case "d:":
                    maskType = DataAvail.XtraObjectProperties.XtraFieldMaskType.Date;
                    break;
                case "r:":
                    maskType = DataAvail.XtraObjectProperties.XtraFieldMaskType.RegEx;
                    break;
                default:
                    maskType = DataAvail.XtraObjectProperties.XtraFieldMaskType.Numeric;
                    break;
            }

            if (maskType != DataAvail.XtraObjectProperties.XtraFieldMaskType.None)
            {
                if (maskType == XtraFieldMaskType.RegEx)
                {
                    string stdMask = GetStdRegExMaskByName(mask);

                    if (!string.IsNullOrEmpty(stdMask))
                    {
                        mask = stdMask;
                    }
                }

                return new DataAvail.XtraObjectProperties.XtraFieldMask(mask, maskType);
            }
            else
            {
                return null;
            }
        }

        private static string FormatMaskType(XtraFieldMaskType MaskType)
        {
            switch (MaskType)
            { 
                case XtraFieldMaskType.RegEx:
                    return "r:";
                case XtraFieldMaskType.Numeric:
                    return "n:";
                case XtraFieldMaskType.Date:
                    return "d:";
                default:
                    return null;
            }
        }

        private static string GetStdRegExMaskByName(string MaskName)
        {
            switch (MaskName)
            { 
                case "phone":
                    return STD_PHONE_MASK;
                case "email":
                    return STD_EMAIL_MASK;
            }

            return null;
        }

    }

    public enum XtraFieldMaskType
    {
        Numeric,
        Date,
        RegEx,
        None
    }

}
