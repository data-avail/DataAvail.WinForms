using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraObjectProperties
{
    public class XtraFieldProperties : System.ComponentModel.INotifyPropertyChanged
    {
        public XtraFieldProperties(XtraObjectProperties XtraObjectProperties, string FieldName, Type FieldType)
        {
            this._xtraObjectProperties = XtraObjectProperties;

            this.FieldName = FieldName;

            Type uddType = System.Nullable.GetUnderlyingType(FieldType);

            if (uddType != null)
            {
                this.FieldTypeNullable = FieldType;

                this.FieldType = uddType;
            }
            else
            {
                this.FieldType = FieldType;
            }
        }

        public XtraFieldProperties(XtraFieldProperties XtraFieldProperties)
        {
            this._xtraObjectProperties = XtraFieldProperties.XtraObjectProperties;
            this.IsPK = XtraFieldProperties.IsPK;
            this.FieldName = XtraFieldProperties.FieldName;
            this.FieldType = XtraFieldProperties.FieldType;
            this.Caption = XtraFieldProperties.Caption;
            this.AllowUserEdit = XtraFieldProperties.AllowUserEdit;
            this.AllowUserNull = XtraFieldProperties.AllowUserNull;
            this.IsMapped = XtraFieldProperties.IsMapped;
            this.MappedParamName = XtraFieldProperties.MappedParamName;
            this._fieldCalculator = XtraFieldProperties.FieldCalculator;
            this.SelectFkItemType = XtraFieldProperties.SelectFkItemType;
            this.SpecifiedControlType = XtraFieldProperties.SpecifiedControlType;
            this.FKCommands = XtraFieldProperties.FKCommands;
        }

        internal void EndInit()
        {
            this._fieldCalculator = new DataAvail.XOP.XOPFieldCalculator(this);
        }

        private readonly DataAvail.XtraObjectProperties.XtraObjectProperties _xtraObjectProperties;

        private string _caption;

        private bool _allowUserEdit = true;

        private bool _allowUserNull = true;

        private DataAvail.Utils.EditModeType _selectFkItemType = DataAvail.Utils.EditModeType.View;

        private XOP.XOPFieldCalculator _fieldCalculator;

        public readonly string FieldName;

        public readonly Type FieldType;

        public readonly Type FieldTypeNullable;

        public bool IsPK = false;

        public bool IsMapped = true;

        public string MappedParamName;

        public object DefaultValue;

        public string CalculatorExpression;

        public string SpecifiedControlType;

        public XOPFieldFkCommands FKCommands = XOPFieldFkCommands.selectKey | XOPFieldFkCommands.selectKeySearch;

        public DataAvail.XtraObjectProperties.XtraObjectProperties XtraObjectProperties
        {
            get { return _xtraObjectProperties; }
        } 

        public string Caption
        {
            get { return _caption; }

            set
            {
                if (_caption != value)
                {
                    _caption = value;

                    OnNotifyPropertyChanged("Caption");
                }
            }
        }

        public bool AllowUserEdit 
        {
            get { return _allowUserEdit; }

            set 
            {
                if (_allowUserEdit != value)
                {
                    _allowUserEdit = value;

                    OnNotifyPropertyChanged("AllowUserEdit");
                }
            }
        }

        public bool AllowUserNull
        {
            get { return _allowUserNull; }

            set
            {
                if (_allowUserNull != value)
                {
                    _allowUserNull = value;

                    OnNotifyPropertyChanged("AllowUserNull");
                }
            }
        }

        public DataAvail.Utils.EditModeType SelectFkItemType
        {
            get { return _selectFkItemType; }
         
            set 
            {
                if (SelectFkItemType != value)
                {
                    _selectFkItemType = value;

                    if (SelectFkItemType != DataAvail.Utils.EditModeType.None &&
                        (SelectFkItemType & DataAvail.Utils.EditModeType.View) != DataAvail.Utils.EditModeType.View)
                        _selectFkItemType |= DataAvail.Utils.EditModeType.View;

                    OnNotifyPropertyChanged("ItemSelectorType");
                }
            }
        }

        public string GetCaption()
        {
            return !string.IsNullOrEmpty(Caption) ? Caption : FieldName;
        }

        protected void OnNotifyPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(PropertyName));
        }

        public bool IsFilled { get { return string.IsNullOrEmpty(this.CalculatorExpression); } }


        #region INotifyPropertyChanged Members

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        #endregion

        public DataAvail.XtraObjectProperties.XtraObjectRelation ParentRelation
        {
            get
            {
                XOP.AppContext.AppItem parentAppItem = XtraObjectProperties.Container.AppContext.GetParentAppItem(this);

                if (parentAppItem != null)
                {
                    return parentAppItem.XtraObjectProperties.GetChildRelation(this.FieldName);
                }
                else
                {
                    return null;
                }
            }
        }


        public XOP.XOPFieldCalculator FieldCalculator
        {
            get 
            {
                if (!string.IsNullOrEmpty(CalculatorExpression) && _fieldCalculator == null)
                {
                    _fieldCalculator = new DataAvail.XOP.XOPFieldCalculator(this);
                }

                return _fieldCalculator;
            }
        }
    }

    [Flags]
    public enum XOPFieldFkCommands
    { 
        none = 0x00,
        addKey = 0x10,
        addButton = 0x20,
        selectKey = 0x40,
        selectButton = 0x80,
        comboKey = 0x100,
        comboButton = 0x200,
        selectKeySearch = 0x400,
        selectButtonSearch = 0x800,
        all = addKey | addButton | selectKey | selectButton | comboKey | comboButton | selectKeySearch | selectButtonSearch
    }
}
