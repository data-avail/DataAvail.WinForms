using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;
using DataAvail.Controllers;
using DataAvail.Controllers.Binding;
using System.Windows.Forms;

namespace DataAvail.DX.XtraSearcherContainer
{
    public class XtraSearchContainer : DataAvail.DX.XtraContainer.XtraContainer, DataAvail.Serialization.ISerializableObject, DataAvail.XtraSearcherContainer.IXtraSearchContainer
    {
        public XtraSearchContainer(DataAvail.Controllers.Controller Controller)
            : base(Controller)
        {
            Controller.GetFillExpression += new DataAvail.Controllers.ControllerFilterExpressionHandler(Controller_GetFillExpression);
        }

        void Controller_GetFillExpression(object sender, DataAvail.Controllers.ControllerFilterExpressionEventArgs args)
        {
            args.filterExpression = SearchExpression;
        }

        private DevExpress.XtraEditors.SimpleButton _searchButton;

        public string SearchExpression 
        {
            get 
            {
                string expr = DataAvail.XtraSearcherContainer.XtraSearchContainer.BuildExpression(this);

                if (!string.IsNullOrEmpty(expr))
                {
                    return expr;
                }
                else
                {
                    return null;
                }
            }
        }

        #region Building

        protected override void Build(XOTableContext AppItemContext)
        {
            base.Build(AppItemContext);

            _searchButton = new DevExpress.XtraEditors.SimpleButton() { Name = "searchButton", Text = "Искать" };

            this.AddControl(_searchButton, _searchButton.Name, "Поиск");
        }

        protected override object CreateFieldControl(DataAvail.XtraContainerBuilder.XtraContainerBuilderControlType ControlType, XOFieldContext AppFieldContext)
        {
            switch (ControlType)
            {
                case DataAvail.XtraContainerBuilder.XtraContainerBuilderControlType.Text:
                    return new DataAvail.DX.XtraSearcherEditors.TextSearchEdit(AppFieldContext);
                case DataAvail.XtraContainerBuilder.XtraContainerBuilderControlType.Numeric:
                    return new DataAvail.DX.XtraSearcherEditors.TextSearchEdit(AppFieldContext);
                case DataAvail.XtraContainerBuilder.XtraContainerBuilderControlType.Date:
                    return new DataAvail.DX.XtraSearcherEditors.DateRangeSearchEdit(AppFieldContext);
                case DataAvail.XtraContainerBuilder.XtraContainerBuilderControlType.Combo:
                    return new DataAvail.DX.XtraSearcherEditors.LookUpSearchEdit(AppFieldContext);
                case DataAvail.XtraContainerBuilder.XtraContainerBuilderControlType.Custom:
                    return base.CreateFieldControl(ControlType, AppFieldContext);
            }

            return null;
        }

        protected override void BindFieldControl(object Control, string FieldName, string BindingProperty)
        {
            if (Control is DataAvail.Controllers.Binding.IControllerDataSourceControl)
            {
                ControllerDataSource ctrDataSource = Controller.GetControllerDataSource(FieldName);

                if (Control is IControllerDataSourceControl)
                    ((Control)Control).DataBindings.Add(new Binding("DataSourceProperties", ctrDataSource, "DataSourceProperties"));
            }

        }

        protected override void InitializeFieldControl(object Control, DataAvail.XtraContainerBuilder.XtraContainerControlProperties ControlProperties)
        {
            base.InitializeFieldControl(Control, ControlProperties);
        }

        protected override object CreateChildRelationControl(XORelation XORelation)
        {
            return null;
        }

        protected override bool IsAdmittedField(string FieldName)
        {
            return true;
        }

        #endregion

        public override string SeraializationType
        {
            get
            {
                return "SrchLayout";
            }
        }

        protected override IEnumerable<DataAvail.Controllers.Commands.IControllerCommandItem> CommandItems
        {
            get
            {
                return base.CommandItems.Union(new DataAvail.Controllers.Commands.IControllerCommandItem []
                    {
                        new DataAvail.Controllers.Commands.UICommandItems.ControlCommandItem(DataAvail.Controllers.Commands.ControllerCommandTypes.Fill, _searchButton)
                    });
            }
        }


        #region IXtraSearchContainer Members

        public object SearchButton
        {
            get { return this._searchButton; }
        }

        #endregion

        /*
        protected override void OnSubscribeControllerEvents()
        {
        }
         */

        #region Custom field controls

        protected override object OnCreateCustomFieldControl(XOFieldContext AppFieldContext)
        {
            switch (AppFieldContext.SpecifiedControlType)
            {
                case "AutoRefCombo":
                case "LikeLookUpEdit":
                    return new XtraSearcherEditors.AutoRefComboSearchEdit(AppFieldContext);
            }

            return null;
        }

        #endregion
    }
}
