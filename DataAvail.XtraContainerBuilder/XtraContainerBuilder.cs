using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataAvail.XObject;

namespace DataAvail.XtraContainerBuilder
{
    public partial class XtraContainerBuilder : UserControl
    {
        public XtraContainerBuilder()
        {
            InitializeComponent();
        }

        private readonly Dictionary<string, object> _fakeContainerControls = new Dictionary<string, object>();

        private readonly Dictionary<XOField, object> _containerControls = new Dictionary<XOField, object>();

        private XOTableContext _defaultTableContext;

        protected Dictionary<XOField, object> ContainerControls { get { return _containerControls; } }

        protected Dictionary<string, object> FakeContainerControls { get { return _fakeContainerControls; } }

        public IEnumerable<object> FieldControls { get { return ContainerControls.Values.Union(FakeContainerControls.Values); } }

        private object GetSpecifiedControl(XOField XOField)
        {
            if (XOField.SpecifiedControlType == null && XOField.SpecifiedControlIndex == -1)
                return null;

            object ctl = ContainerControls.FirstOrDefault(
                p => p.Key.SpecifiedControlType == XOField.SpecifiedControlType
                    && p.Key.SpecifiedControlIndex == XOField.SpecifiedControlIndex).Value;

            if (ctl == null)
            {

            }

            return ctl;
        }

        protected XOTableContext DefaultTableContext
        {
            get { return _defaultTableContext; }
        }

        protected virtual void Build(XOTableContext TableContext)
        {
            _defaultTableContext = TableContext;

            this.SuspendLayout();

            foreach (XOFieldContext fieldContext in TableContext.Fields)
            {
                BuildField(fieldContext);
            }

            foreach (XORelation childRelation in _defaultTableContext.ShownChildrenRelations)
            {
                BuildChildRelation(childRelation);
            }

            this.ResumeLayout();
        }

        protected virtual void BeginBuild() { }

        protected virtual void EndBuild() { }

        protected virtual object BuildField(XOFieldContext FieldContext)
        {
            bool isAdmitted = IsAdmittedField(FieldContext.Name);

            if (isAdmitted)
            {
                object ctl = GetSpecifiedControl(FieldContext.XOField);

                if (ctl == null)
                {
                    XtraContainerBuilderControlType controlType = GetControlType(FieldContext);

                    ctl = CreateFieldControl(controlType, FieldContext);

                    if (ctl != null)
                    {

                        XtraContainerControlProperties props = new XtraContainerControlProperties(ctl, controlType, FieldContext);

                        InitializeFieldControl(ctl, props);

                        BindFieldControl(ctl, FieldContext.Name, FieldContext.BindingProperty);

                        AddFieldControl(ctl, props);

                        ContainerControls.Add(FieldContext.XOField, ctl);
                    }

                    return ctl;
                }
                else
                {
                    BindFieldControl(ctl, FieldContext.Name, FieldContext.BindingProperty);

                    return null;
                }

                

            }
            else
            {
                return CreateEmptyControl();
            }
        }

        protected virtual object CreateFieldControl(XtraContainerBuilderControlType ControlType, XOFieldContext FieldContext)
        {
            switch (ControlType)
            {
                case XtraContainerBuilderControlType.Text:
                    return OnCreateTextFieldControl(FieldContext);
                case XtraContainerBuilderControlType.Numeric:
                    return OnCreateNumericFieldControl(FieldContext);
                case XtraContainerBuilderControlType.Date:
                    return OnCreateDateFieldControl(FieldContext);
                case XtraContainerBuilderControlType.Combo:
                    return OnCreateComboboxFieldControl(FieldContext);
                case XtraContainerBuilderControlType.Custom:
                    return OnCreateCustomFieldControl(FieldContext);
            }

            return null;
        }

        protected virtual void InitializeFieldControl(object Control, XtraContainerControlProperties FieldProperties)
        {
            switch (FieldProperties.controlType)
            {
                case XtraContainerBuilderControlType.Text:
                    OnInitializeTextFieldControl(Control, FieldProperties);
                    break;
                case XtraContainerBuilderControlType.Numeric:
                    OnInitializeNumericFieldControl(Control, FieldProperties);
                    break;
                case XtraContainerBuilderControlType.Date:
                    OnInitializeDateFieldControl(Control, FieldProperties);
                    break;
                case XtraContainerBuilderControlType.Combo:
                    OnInitializeComboboxFieldControl(Control, FieldProperties);
                    break;
                case XtraContainerBuilderControlType.Custom:
                    OnInitializeCustomFieldControl(Control, FieldProperties);
                    break;
            }
        }

        protected virtual void BindFieldControl(object Control, string FieldName, string BindingProperty)
        {
        }

        protected virtual void AddFieldControl(object Control, XtraContainerControlProperties XtraContainerControlProperties)
        {
        }

        public void AddFieldControl(object Control, string FakeFieldName, string Caption)
        {
            if (FakeFieldName != null)
            {
                this.FakeContainerControls.Add(FakeFieldName, Control);
            }

            AddControl(Control, Caption, Caption);
        }

        public virtual void AddControl(object Control, string Name, string Caption)
        {
        }



        #region Create field control virtuals

        protected virtual object OnCreateTextFieldControl(XOFieldContext FieldContext)
        {
            return null;
        }

        protected virtual object OnCreateNumericFieldControl(XOFieldContext FieldContext)
        {
            return null;
        }

        protected virtual object OnCreateDateFieldControl(XOFieldContext FieldContext)
        {
            return null;
        }

        protected virtual object OnCreateComboboxFieldControl(XOFieldContext FieldContext)
        {
            return null;
        }

        protected virtual object OnCreateCustomFieldControl(XOFieldContext FieldContext)
        {
            return null;
        }


        #endregion

        #region Initialize field control virtuals

        protected virtual void OnInitializeTextFieldControl(object Control, XtraContainerControlProperties FieldProperties)
        {
        }

        protected virtual void OnInitializeNumericFieldControl(object Control, XtraContainerControlProperties FieldProperties)
        {
        }

        protected virtual void OnInitializeDateFieldControl(object Control, XtraContainerControlProperties FieldProperties)
        {
        }

        protected virtual void OnInitializeComboboxFieldControl(object Control, XtraContainerControlProperties FieldProperties)
        {
        }

        protected virtual void OnInitializeCustomFieldControl(object Control, XtraContainerControlProperties FieldProperties)
        {
        }

        #endregion

        protected virtual XtraContainerBuilderControlType GetControlType(XOFieldContext FieldContext)
        {
            if (!string.IsNullOrEmpty(FieldContext.SpecifiedControlType))
                return XtraContainerBuilderControlType.Custom;

            if (FieldContext.FieldType == typeof(string))
            {
                return XtraContainerBuilderControlType.Text;
            }
            else if (FieldContext.FieldType == typeof(System.DateTime))
            {
                return XtraContainerBuilderControlType.Date;
            }
            else if (FieldContext.FieldType == typeof(int)
                || FieldContext.FieldType == typeof(long)
                || FieldContext.FieldType == typeof(double)
                || FieldContext.FieldType == typeof(decimal)
                || FieldContext.FieldType == typeof(float))
            {
                if (FieldContext.IsFkChildField)
                {
                    return XtraContainerBuilderControlType.Combo;
                }
                else
                {
                    return XtraContainerBuilderControlType.Numeric;
                }
            }
            else
            {
                return XtraContainerBuilderControlType.Custom;
            }
        }

        protected bool IsShowCaption { get { return true; } }

        #region Child relation

        protected virtual object BuildChildRelation(XORelation XtraObjectRelation)
        {
            bool isAdmitted = IsAdmittedChildRelation(XtraObjectRelation);

            object ctl = isAdmitted ? CreateChildRelationControl(XtraObjectRelation) : CreateEmptyControl();

            if (ctl != null)
            {
                InitializeChildRelationControl(ctl, XtraObjectRelation);

                if (isAdmitted)
                    BindChildRelationControl(ctl, XtraObjectRelation);

                AddFieldControl(ctl, string.Format("{0}_{1}_{2}", XtraObjectRelation.ParentTable.Name, XtraObjectRelation.ChildTable.Name, XtraObjectRelation.ChildField.Name), XtraObjectRelation.ChildTable.Caption);
            }

            return ctl;

        }

        protected virtual object CreateChildRelationControl(XORelation XtraObjectRelation)
        {
            return null;
        }

        protected virtual void InitializeChildRelationControl(object Control, XORelation XtraObjectRelation)
        {

        }

        protected virtual void BindChildRelationControl(object Control, XORelation XtraObjectRelation)
        {
        }

        #endregion

        protected virtual bool IsAdmittedField(string FileName)
        {
            return true;
        }

        protected virtual bool IsAdmittedChildRelation(XORelation XtraObjectRelation)
        {
            return true;
        }


        protected virtual object CreateEmptyControl()
        {
            return null;
        }
    }

    public enum XtraContainerBuilderControlType
    {
        Text,
        Numeric,
        Combo,
        Date,
        Custom
    }

}
