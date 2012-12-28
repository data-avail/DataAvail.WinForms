﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.Utils;
using DataAvail.XObject.XOP;
using DataAvail.XObject.XWP;
using DataAvail.XObject.XSP;


namespace DataAvail.XObject
{
    public class XOField
    {
        internal XOField(XOTable XOTable, XOPField XOPField, XWPField XWPField, XSPField XSPField)
        {
            _xoTable = XOTable;

            _xopField = XOPField;

            _xwpField = XWPField;

            _xspField = XSPField;


        }

        private readonly XOTable _xoTable;

        private readonly XOPField _xopField;

        private readonly XWPField _xwpField;

        private readonly XSPField _xspField;

        private XOFieldCalculator _calculator;

        internal void EndInit()
        {
            try
            {
                if (!string.IsNullOrEmpty(XopField.Calculator))
                    _calculator = new XOFieldCalculator(this);
            }
            catch (NullReferenceException)
            { }
        }

        public XOTable XoTable
        {
            get { return _xoTable; }
        }

        public XOPField XopField
        {
            get { return _xopField; }
        }

        public XWPField XwpField
        {
            get { return _xwpField; }
        }

        public XSPField XspField
        {
            get { return _xspField; }
        }

        public XOMode Mode
        {
            get
            {
                XOMode mode = XOMode.None;

                if (XoTable.IsCanView)
                    mode |= XOMode.View;

                if (XoTable.IsCanEdit)
                    mode |= XOMode.Edit;

                if (!XopField.IsMapped || 
                    XopField.Calculator != null || 
                    (XopField.IsPk && XopField.IsPkAutoGenerated))
                        mode = XOMode.View;

                if (XwpField != null && XwpField.ReadOnly)
                    mode = XOMode.View;

                if (XspField != null)
                {
                    if ((XspField.Mode & XOMode.Edit) != XOMode.Edit)
                    {
                        mode |= XOMode.View;
                    }

                    if ((XspField.Mode & XOMode.View) != XOMode.View)
                    {
                        mode |= XOMode.Edit;
                    }
                }

                return mode;
            }
        }

        public XORelation ParentRelation
        {
            get
            {
                return XoTable.XoApplication.Tables.SelectMany(p => p.ChildrenRelations).FirstOrDefault(p => p.ChildTable == this.XoTable && p.ChildField == this);
            }
        }

        public string Name
        {
            get { return XopField.Name; }
        }

        public string Caption
        {
            get { return XwpField == null ? Name : XwpField.Caption; }
        }

        public XOFieldCalculator Calculator
        {
            get { return _calculator; }
        }

        public string SpecifiedControlType
        {
            get 
            {
                if (XwpField != null && !string.IsNullOrEmpty(XwpField.ControlType))
                {
                    return XwpField.ControlType.Split(',')[0];
                }
                else
                {
                    return null;
                }
            }
        }

        public int SpecifiedControlIndex
        {
            get
            {
                if (XwpField != null && !string.IsNullOrEmpty(XwpField.ControlType))
                {
                    string [] strs = XwpField.ControlType.Split(',');

                    if (strs.Length > 1)
                    {
                        int i;

                        if (!int.TryParse(strs[1], out i))
                            throw new Exception("SpecifiedType has wrong format. (Format must be Type[,Index]");

                        return i;
                    }
                }

                return -1;
            }

        }


        public Type FieldType
        {
            get { return XopField.Type.Type; }
        }

        public int FieldSize
        {
            get { return XopField.Type.Size; }
        }

        public bool IsFkChildField
        {
            get 
            {
                return XoTable.XoApplication.XopDataSet.Relations.Relations.FirstOrDefault(p => p.ChildTable == XoTable.Name && p.ChildField == this.Name) != null;
            }
        }

        public bool IsCanView
        {
            get {
                return this.XoTable.IsCanView && EnumFlags.IsContain(Mode, XOMode.View) && 
                (this.XspField == null || EnumFlags.IsContain(this.XspField.Mode, XOMode.View)); 
            }
        }

        public bool IsCanEdit
        {
            get
            {
                return this.XoTable.IsCanEdit && 
                    EnumFlags.IsContain(Mode, XOMode.Edit) && 
                    (this.XspField == null || EnumFlags.IsContain(this.XspField.Mode, XOMode.Edit));
            }
        }

        public XWPFieldMask Mask
        {
            get { return XwpField == null ? DefaultMask : XwpField.Mask; }
        }

        public bool IsPk
        {
            get { return XopField.IsPk; }
        }

        public bool IsPkAutoGenerated
        {
            get { return XopField.IsPkAutoGenerated; }
        }

        public bool IsMapped
        {
            get { return XopField.IsMapped && string.IsNullOrEmpty(XopField.Calculator); }
        }

        public object DefaultValue
        {
            get { return XopField.DefaultValue; }
        }

        public XWPFieldFkInterfaceType FkInterfaceType
        {
            get { return XwpField != null && XwpField.FkInterfaceType != XWPFieldFkInterfaceType.Default ? 
                            XwpField.FkInterfaceType : 
                            XWPFieldFkInterfaceType.SelectItemKey | XWPFieldFkInterfaceType.SelectItemKeySearch; }
        }

        public XOMode FkSelectItemMode
        {
            get
            {
                return XwpField != null ? XwpField.FkSelectItemMode : XOMode.View;
            }
        }


        //Get default mask based on fields type
        private XWPFieldMask DefaultMask
        {
            get
            {
                if (FieldType == typeof(int) || 
                    FieldType == typeof(long) || 
                    FieldType == typeof(uint) || 
                    FieldType == typeof(ulong) 
                    )
                {
                    return new XWPFieldMask(XWPFieldMask.STD_NUMERIC_MASK, XWPFieldMaskType.Numeric);
                }

                if (FieldType == typeof(float) ||
                    FieldType == typeof(decimal) ||
                    FieldType == typeof(double)
                    )
                {
                    return new XWPFieldMask(XWPFieldMask.STD_DECIMAL_MASK, XWPFieldMaskType.Numeric);
                }

                if (FieldType == typeof(System.DateTime))
                {
                    return new XWPFieldMask(XWPFieldMask.STD_DATE_MASK, XWPFieldMaskType.Date);
                }

                return null;
            }
        }

        public string BindingProperty 
        {
            get { return XwpField.BindingField; }
        }

        public XWPFieldDisplayType DisplayType
        {
            get { return XwpField != null ? XwpField.DisplayType : XWPFieldDisplayType.Anywhere; }
        }
    }
}
