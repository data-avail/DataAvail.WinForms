using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XOP.AppContext
{
    public class AppFieldContext 
    {
        internal AppFieldContext(DataAvail.XOP.AppContext.AppItemContext AppItemContext, DataAvail.XtraObjectProperties.XtraFieldProperties XtraFieldProperties, DataAvail.XOP.ModelSecurity.ModelSecField ModelSecField)
        {
            _appItemContext = AppItemContext;

            _xtraFieldProperties = XtraFieldProperties;

            _modelSecField = ModelSecField;
        }

        private readonly DataAvail.XOP.AppContext.AppItemContext _appItemContext;

        private readonly DataAvail.XtraObjectProperties.XtraFieldProperties _xtraFieldProperties;

        private readonly DataAvail.XOP.ModelSecurity.ModelSecField _modelSecField;

        public DataAvail.XOP.AppContext.AppItemContext AppItemContext { get { return _appItemContext; } }

        public DataAvail.XtraObjectProperties.XtraFieldProperties XtraFieldProperties { get { return _xtraFieldProperties; } }

        private DataAvail.XOP.ModelSecurity.ModelSecField ModelSecField { get { return _modelSecField; } }

        public string FieldName
        {
            get
            {
                return XtraFieldProperties.FieldName;
            }
        }

        public string Caption
        {
            get
            {
                return XtraFieldProperties.Caption;
            }
        }

        public Type FieldType
        {
            get
            {
                return XtraFieldProperties.FieldType;
            }
        }

        public DataAvail.XtraObjectProperties.XtraFieldMask XtraFieldMask
        {
            get
            {
                if (XtraFieldProperties is DataAvail.XtraObjectProperties.XtraTextFieldProperties)
                {
                    return ((DataAvail.XtraObjectProperties.XtraTextFieldProperties)XtraFieldProperties).XtraFieldMask;
                }
                else
                {
                    return null;
                }
            }
        }

        public DataAvail.XtraObjectProperties.IFKFieldProperties FKFieldProperties
        {
            get { return XtraFieldProperties as DataAvail.XtraObjectProperties.IFKFieldProperties; }
        }

        public bool IsCanEdit 
        { 
            get 
            {
                return AppItemContext.IsCanEdit && XtraFieldProperties.AllowUserEdit &&
                    (ModelSecField == null || !(ModelSecField.IsReadOnly == DataAvail.Utils.DefaultBoolType.True));
            } 
        }

        public bool IsCanView 
        { 
            get 
            {
                return ModelSecField == null || !(ModelSecField.IsHidden == DataAvail.Utils.DefaultBoolType.True);
            } 
        }

        public bool IsCanNull
        {
            get
            {
                return XtraFieldProperties.AllowUserNull;
            }
        }

        public Type FieldTypeNullable
        {
            get { return XtraFieldProperties.FieldTypeNullable; }
        }

        
        public XOP.AppContext.AppItemContext ParentRelationAppItemContext
        {
            get 
            {
                AppItem appItem = this.AppItemContext.Container.Container.GetParentAppItem(this.XtraFieldProperties);

                if (appItem != null)
                {
                    return appItem[new DefaultContext()];
                }
                else
                {
                    return null;
                }

            }
        }

        public DataAvail.XtraObjectProperties.XtraObjectRelation ParentRelation
        {
            get
            {
                return XtraFieldProperties.ParentRelation;
            }
        }


        public DataAvail.Utils.EditModeType ItemSelectorType
        {
            get { return XtraFieldProperties.SelectFkItemType; }
        }

        public bool IsPK
        {
            get { return XtraFieldProperties.IsPK; }
        }

        public string CalculatorExpression
        {
            get { return XtraFieldProperties.CalculatorExpression; }
        }

        public XOPFieldCalculator FieldCalculator
        {
            get { return XtraFieldProperties.FieldCalculator; }
        }


    }
}
