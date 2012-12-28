using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;


namespace DataAvail.XObject.XWP
{
    public class XWPFieldMask
    {
        public XWPFieldMask()
        { }

        public XWPFieldMask(XElement XElement)
        {
            XAttribute attr;

            string maskStr = XmlLinq.ReadAttribute(XElement, "mask", false, XOApplication.xmlReaderLog, out attr);

            if (!string.IsNullOrEmpty(maskStr))
            {
                string tag = maskStr.Length > 1 && maskStr[1] == ':' ? maskStr.Substring(0, 2) : null;

                _mask = string.IsNullOrEmpty(tag) ? maskStr : maskStr.Remove(0, 2);

                switch (tag)
                {
                    case "d:":
                        _maskType = XWPFieldMaskType.Date;
                        break;
                    case "r:":
                        _maskType = XWPFieldMaskType.RegEx;
                        break;
                    default:
                        _maskType = XWPFieldMaskType.Numeric;
                        break;
                }

                if (_maskType == XWPFieldMaskType.RegEx)
                {
                    string stdMask = GetStdRegExMaskByName(_mask);

                    if (!string.IsNullOrEmpty(stdMask))
                    {
                        _mask = stdMask;
                    }
                }

                if (_maskType != XWPFieldMaskType.None && string.IsNullOrEmpty(_mask))
                {
                    XmlLinqReaderLog.WriteToLog(XOApplication.xmlReaderLog, XElement, attr, "MaskType is defined but mask string is empty", false);
                }
            }
                
        }


        public XWPFieldMask(string Mask, XWPFieldMaskType MaskType)
        {
            _mask = Mask;

            _maskType = MaskType;
        }

        public static XWPFieldMask Parse(string Mask)
        {
            if (Mask == "none")
                return null;

            XWPFieldMaskType maskType = XWPFieldMaskType.None;

            string tag = Mask.Length > 1 && Mask[1] == ':' ? Mask.Substring(0, 2) : null;

            string mask = string.IsNullOrEmpty(tag) ? Mask : Mask.Remove(0, 2);

            switch (tag)
            {
                case "d:":
                    maskType = XWPFieldMaskType.Date;
                    break;
                case "r:":
                    maskType = XWPFieldMaskType.RegEx;
                    break;
                default:
                    maskType = XWPFieldMaskType.Numeric;
                    break;
            }

            if (maskType != XWPFieldMaskType.None)
            {
                if (maskType == XWPFieldMaskType.RegEx)
                {
                    string stdMask = GetStdRegExMaskByName(mask);

                    if (!string.IsNullOrEmpty(stdMask))
                    {
                        mask = stdMask;
                    }
                }

                return new XWPFieldMask(mask, maskType);
            }
            else
            {
                return null;
            }
        }

        private readonly string _mask;

        private readonly XWPFieldMaskType _maskType;

        public string Mask
        {
            get { return _mask; }
        }

        public XWPFieldMaskType MaskType
        {
            get { return _maskType; }
        } 


        #region Constants

        public const string STD_DECIMAL_MASK = ",################0.00##########";

        public const string STD_NUMERIC_MASK = "n0";

        public const string STD_DATE_MASK = "D";

        public const string STD_PHONE_MASK = @"\(\d\d\d\)\d{1,3}-\d\d-\d\d";

        public const string STD_EMAIL_MASK = @"\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b";

        #endregion

        private static string FormatMaskType(XWPFieldMaskType MaskType)
        {
            switch (MaskType)
            {
                case XWPFieldMaskType.RegEx:
                    return "r:";
                case XWPFieldMaskType.Numeric:
                    return "n:";
                case XWPFieldMaskType.Date:
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
 
        public string Format()
        {
            string maskType = FormatMaskType(this.MaskType);

            if (!string.IsNullOrEmpty(maskType) && !string.IsNullOrEmpty(Mask))
                return string.Format("{0}{1}", maskType, Mask);
            else
                return null;

        }
    }
    public enum XWPFieldMaskType
    {
        None,
        Numeric,
        Date,
        RegEx
    }
}
