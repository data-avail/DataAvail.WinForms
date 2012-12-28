using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraObjectProperties
{
    public class XtraTextFieldProperties : XtraFieldProperties
    {
        public XtraTextFieldProperties(XtraObjectProperties XtraObjectProperties, string FieldName, Type FieldType)
            : base(XtraObjectProperties, FieldName, FieldType)
        {
        }

        public XtraTextFieldProperties(XtraFieldProperties XtraFieldProperties)
            : base(XtraFieldProperties)
        {
        }

        private XtraFieldMask _xtraFieldMask;

        private int _maxLength;

        public XtraFieldMask XtraFieldMask
        {
            get { return _xtraFieldMask; }

            set
            {
                if (value != _xtraFieldMask)
                {
                    _xtraFieldMask = value;

                    OnNotifyPropertyChanged("XtraFieldMask");
                }
            }
        }

        public int MaxLength
        {
            get { return _maxLength; }

            set
            {
                if (_maxLength != value)
                {
                    _maxLength = value;

                    OnNotifyPropertyChanged("MaxLength");
                }
            }
        }
    }

}
