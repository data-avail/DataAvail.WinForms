using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataAvail.XObject;
using DataAvail.XtraContainerBuilder;

namespace DataAvail.XtraContainer
{
    partial class XtraContainer
    {
        protected override void InitializeFieldControl(object Control, XtraContainerControlProperties ControlProperties)
        {
            ((System.Windows.Forms.Control)Control).Name = string.Format("ctl{0}", ControlProperties.fieldContext.Name);

            base.InitializeFieldControl(Control, ControlProperties);
        }


        protected override object OnCreateTextFieldControl(XOFieldContext XOFieldContext)
        {
            return new System.Windows.Forms.TextBox();
        }

        protected override object OnCreateNumericFieldControl(XOFieldContext XOFieldContext)
        {
            return new System.Windows.Forms.TextBox();
        }

        protected override object OnCreateDateFieldControl(XOFieldContext XOFieldContext)
        {
            return new System.Windows.Forms.DateTimePicker();
        }

        protected override object OnCreateComboboxFieldControl(XOFieldContext XOFieldContext)
        {
            return new System.Windows.Forms.ComboBox();
        }

        protected override void InitializeChildRelationControl(object Control, XORelation XORelation)
        {
            ((System.Windows.Forms.Control)Control).Name = string.Format("ctl{0}_{1}_{2}", XORelation.ParentTable.Name, XORelation.ChildTable.Name, XORelation.ChildField.Name);

            base.InitializeChildRelationControl(Control, XORelation);
        }

        
     
    }
}
