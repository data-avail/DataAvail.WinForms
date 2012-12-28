using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataAvail.Controllers;
using DataAvail.Controllers.Binding;
using System.ComponentModel;

namespace DataAvail.XtraContainer
{
    partial class XtraContainer
    {
        List<Control> _notifyPropertyChangedControls = new List<Control>();

        protected override void BindFieldControl(object Control, string FieldName, string BindingProperty)
        {
            Control ctl = (Control)Control;

            ControllerDataSource ctrDataSource = Controller.GetControllerDataSource(FieldName);

            if (!string.IsNullOrEmpty(BindingProperty))
            {
                ctl.DataBindings.Add(new Binding(BindingProperty, ctrDataSource.EditValueDataSource, FieldName, true, DataSourceUpdateMode.OnPropertyChanged, null));

                if (ctl is INotifyPropertyChanged && !_notifyPropertyChangedControls.Contains(ctl))
                {
                    _notifyPropertyChangedControls.Add(ctl);
                    ((INotifyPropertyChanged)ctl).PropertyChanged +=new PropertyChangedEventHandler(XtraContainer_PropertyChanged);
                }

                return;
            }

            if (ctl is IControllerDataSourceControl)
                ctl.DataBindings.Add(new Binding("DataSourceProperties", ctrDataSource, "DataSourceProperties"));

            if (ctl is DataAvail.Controllers.Binding.IControllerBindableControl)
                ctl.DataBindings.Add(new Binding("ControlProperties", ctrDataSource, "ControlProperties"));

            if (ctl is DataAvail.XtraEditors.IXtraEdit)
            {
                string valPropertyName = "EditValue";

                if (Control is DataAvail.Controllers.Binding.IControllerBindableControl)
                {
                    valPropertyName = ((DataAvail.Controllers.Binding.IControllerBindableControl)Control).ValuePropertyName;
                }

                ctl.DataBindings.Add(new Binding(valPropertyName, ctrDataSource.EditValueDataSource, FieldName));

                ((DataAvail.XtraEditors.IXtraEdit)ctl).EditValueChanged += new EventHandler(XtraContainer_EditValueChanged);
            }
            else
            {
                string bindingPropertyName = null;

                if (ctl is System.Windows.Forms.TextBox)
                {
                    bindingPropertyName = "Text";

                    ((System.Windows.Forms.TextBox)ctl).TextChanged += new EventHandler(XtraContainer_TextChanged);
                }
                else if (ctl is System.Windows.Forms.DateTimePicker)
                {
                    bindingPropertyName = "Value";

                    ((System.Windows.Forms.DateTimePicker)ctl).ValueChanged += new EventHandler(XtraContainer_ValueChanged);
                }
                else if (ctl is System.Windows.Forms.ComboBox)
                {
                    bindingPropertyName = "SelectedValue";

                    ((System.Windows.Forms.ComboBox)ctl).SelectedValueChanged += new EventHandler(XtraContainer_SelectedValueChanged);
                }

                if (bindingPropertyName != null)
                {
                    ctl.DataBindings.Add(new Binding(bindingPropertyName, ctrDataSource.EditValueDataSource, FieldName));
                }
            }
        }

        void XtraContainer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetControlValue((Control)sender, e.PropertyName);
        }

        void XtraContainer_EditValueChanged(object sender, EventArgs e)
        {
            SetControlValue((Control)sender, "EditValue");
        }

        void XtraContainer_SelectedValueChanged(object sender, EventArgs e)
        {
            SetControlValue((System.Windows.Forms.Control)sender, "SelectedValue");
        }

        void XtraContainer_ValueChanged(object sender, EventArgs e)
        {
            SetControlValue((System.Windows.Forms.Control)sender, "Value");
        }

        void XtraContainer_TextChanged(object sender, EventArgs e)
        {
            SetControlValue((System.Windows.Forms.Control)sender, "Text");
        }

        protected void SetControlValue(System.Windows.Forms.Control Control, string ValuePropertyName)
        {
            Binding binding = Control.DataBindings[ValuePropertyName];

            if (binding != null)
            {
                binding.WriteValue();

                OnControlValueChanged(binding.BindingMemberInfo.BindingField);
            }
        }

        protected virtual void OnControlValueChanged(string FieldName)
        {
            Controller.InvokeFieldChanged(FieldName);
        }
    }
}
