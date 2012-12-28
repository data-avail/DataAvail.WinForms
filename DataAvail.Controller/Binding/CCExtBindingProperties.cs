using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;

namespace DataAvail.Controllers.Binding
{
    /// <summary>
    /// Controller control extended binding properties 
    /// </summary>
    internal class CCExtBindingProperties
    {
        internal CCExtBindingProperties(XOFieldContext FieldContext)
        {
            _field = FieldContext;

            _controlProperties = new CCProperties();

            _defProps = new CCBindingProperties();
        }

        private XOFieldContext _field;

        private readonly CCProperties _controlProperties = null;

        private DataAvail.XtraBindings.Calculator.ObjectProperties _objectProperties;

        private readonly CCBindingProperties _defProps;

        private DataAvail.XtraBindings.Calculator.ObjectProperties ObjectProperties
        {
          get { return _objectProperties; }
        }

        internal CCBindingProperties BindingDefaultProperties
        {
            get { return _defProps; }
        }

        internal XOFieldContext Field
        {
            get { return _field; }
        }

        public CCProperties ControlProperties
        {
            get { return _controlProperties; }
        }

        internal void Reset(XOFieldContext Field, DataAvail.XtraBindings.Calculator.ObjectProperties ObjectProperties)
        {
            this._field = Field;

            _objectProperties = ObjectProperties;

            BindingDefaultProperties.Reset(Field);

            this.ControlProperties.ReadOnly = GetReadOnly();
            this.ControlProperties.BackColor = GetBackColor();
            this.ControlProperties.DataSource = GetDataSource();
            this.ControlProperties.DataSourceFilter = GetDataSourceFilter();
            this.ControlProperties.Mask = GetMask();
        }

        private bool GetReadOnly()
        {
            if (Field.IsCanEdit)
            {
                object obj = ObjectProperties != null ? ObjectProperties.GetReadOnly(Field.Name) : null;

                if (obj == null)
                    return BindingDefaultProperties.ReadOnly;
                else
                    return (bool)obj;
            }
            else
            {
                return true;
            }

        }

        private System.Drawing.Color GetBackColor()
        {
            if (ObjectProperties != null)
            {
                DataAvail.XtraBindings.Calculator.CalulatorFieldState state = ObjectProperties.GS(Field.Name);

                switch (state)
                { 
                    case DataAvail.XtraBindings.Calculator.CalulatorFieldState.Calculated:
                        return System.Drawing.Color.Aquamarine;
                    case DataAvail.XtraBindings.Calculator.CalulatorFieldState.Manual:
                        return System.Drawing.Color.LightBlue;
                    default:
                        return BindingDefaultProperties.BackColor;
                }
            }
            else
            {
                return _defProps.BackColor;
            }
        }

        private object GetDataSource()
        {
            return null;
        }

        private string GetDataSourceFilter()
        {
            if (ObjectProperties != null)
            {
                string filter = ObjectProperties.GetFilter(Field.Name);

                if (!string.IsNullOrEmpty(filter))
                    return filter;
            }

            return null;

        }

        private string GetMask()
        {
            if (ObjectProperties != null)
            {
                string mask = ObjectProperties.GetMask(Field.Name);

                if (!string.IsNullOrEmpty(mask))
                    return mask;
            }

            return BindingDefaultProperties.Mask;
        }
    }
}
