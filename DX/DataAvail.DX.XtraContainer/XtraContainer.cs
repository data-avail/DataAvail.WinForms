using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataAvail.XtraContainerBuilder;
using DataAvail.XObject;

namespace DataAvail.DX.XtraContainer
{
    public partial class XtraContainer : DataAvail.XtraContainer.XtraContainer
    {
        public XtraContainer(DataAvail.Controllers.Controller Controller)
            : base(Controller)
        {
        }

        protected override void InitializeUI()
        {
            InitializeComponent();

            base.InitializeUI();

            this.layoutControl1.OptionsSerialization.RestoreLayoutItemPadding = true;
            this.layoutControl1.OptionsSerialization.RestoreTextToControlDistance = true;
            this.layoutControl1.OptionsSerialization.RestoreGroupPadding = true;

            this.layoutControl1.ShowContextMenu +=new DevExpress.XtraLayout.LayoutMenuEventHandler(layoutControl1_ShowContextMenu);
        }

        private List<DataAvail.XtraGrid.XtraGrid> _childrenGrids = new List<DataAvail.XtraGrid.XtraGrid>();

        public override void AddControl(object Control, string Name, string Caption)
        {
            System.Windows.Forms.Control ctl = (System.Windows.Forms.Control)Control;

            DevExpress.XtraLayout.LayoutControlItem layoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();

            layoutControlItem.Text = !string.IsNullOrEmpty(Caption) ? Caption : Name;
            layoutControlItem.CustomizationFormText = Name;
            layoutControlItem.Name = "layoutControlItem" + Name;

            layoutControlItem.TextLocation = DevExpress.Utils.Locations.Left;
            layoutControlItem.Control = ctl;

            layoutControlGroup1.AddItem(layoutControlItem);
            layoutControl1.Controls.Add(ctl);
        
        }

        protected override void AddFieldControl(object Control, XtraContainerControlProperties XtraContainerControlProperties)
        {
            AddControl(Control, XtraContainerControlProperties.fieldContext.Name, XtraContainerControlProperties.fieldContext.Caption);
        }

        protected override object CreateFieldControl(XtraContainerBuilderControlType ControlType, XOFieldContext AppFieldContext)
        {
            switch (ControlType)
            {
                case XtraContainerBuilderControlType.Text:
                    return new DataAvail.DX.XtraEditors.TextEdit(AppFieldContext);
                case XtraContainerBuilderControlType.Numeric:
                    return new DataAvail.DX.XtraEditors.TextEdit(AppFieldContext);
                case XtraContainerBuilderControlType.Date:
                    return new DataAvail.DX.XtraEditors.DateEdit(AppFieldContext);
                case XtraContainerBuilderControlType.Combo:
                    return new DataAvail.DX.XtraEditors.LookUpEdit(AppFieldContext);
                case XtraContainerBuilderControlType.Custom:
                    return base.CreateFieldControl(ControlType, AppFieldContext);
            }

            return null;
        }

        protected override void InitializeFieldControl(object Control, XtraContainerControlProperties ControlProperties)
        {
            base.InitializeFieldControl(Control, ControlProperties);

            if (Control is DevExpress.XtraEditors.ISupportStyleController)
            {
                ((DevExpress.XtraEditors.ISupportStyleController)Control).StyleController = this.layoutControl1;
            }
        }

        protected override void OnInitializeTextFieldControl(object Control, XtraContainerControlProperties FieldProperties)
        {
            base.OnInitializeTextFieldControl(Control, FieldProperties);
        }

        protected override object CreateChildRelationControl(DataAvail.Controllers.Controller Controller)
        {
            DataAvail.DX.XtraGrid.XtraGrid xtraGrid = new DataAvail.DX.XtraGrid.XtraGrid(Controller);

            _childrenGrids.Add(xtraGrid);

            return xtraGrid;
        }

        protected override void BeginBuild()
        {
            base.BeginBuild();

            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
        }

        protected override void EndBuild()
        {
            base.EndBuild();

            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (this.layoutControlGroup1.Items.Count > 0)
            {
                if (this.layoutControlGroup1.Items[0] is DevExpress.XtraLayout.TabbedControlGroup)
                {
                    ((DevExpress.XtraLayout.TabbedControlGroup)this.layoutControlGroup1.Items[0]).SelectedTabPageIndex = 0;

                    BeginInvoke(new MethodInvoker(delegate
                    {
                        layoutControl1.FocusHelper.FocusFirst();
                    }));
                }
            }

            layoutControl1.FocusHelper.FocusFirstInGroup(this.layoutControlGroup1);

            base.OnLoad(e);
        }

        private void layoutControl1_ShowContextMenu(object sender, DevExpress.XtraLayout.LayoutMenuEventArgs e)
        {
            if (e.Menu.Items[0].Caption == "Hide Customization Form")
            {
                DevExpress.XtraLayout.BaseLayoutItem[] items = this.layoutControl1.Items.Cast<DevExpress.XtraLayout.BaseLayoutItem>().Where(p => p.Selected).ToArray();

                if (items.Length != 0)
                {
                    e.Menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("Extended properties", new System.EventHandler(ShowExtendedProperties)) { Tag = items });
                }
            }
        }

        private void ShowExtendedProperties(object sender, EventArgs e)
        {
            using (LayoutExtPropsForm form = new LayoutExtPropsForm(((DevExpress.XtraLayout.BaseLayoutItem[])((DevExpress.Utils.Menu.DXMenuItem)sender).Tag)))
            {
                form.ShowDialog();
            }
        }

        #region Custom field controls

        protected override object OnCreateCustomFieldControl(XOFieldContext AppFieldContext)
        {
            switch (AppFieldContext.SpecifiedControlType)
            { 
                case "TextBoxMultiLine":
                    return new DataAvail.DX.XtraEditors.MemoEdit(AppFieldContext);
                case "AutoRefCombo":
                case "LikeLookUpEdit":
                    return new DataAvail.DX.XtraEditors.AutoRefCombo(AppFieldContext);
                case "HoohlShmoohlEditor":
                    return new HoohlShmoohlEditor.HoohlShmoohlEditor();
            }

            return base.OnCreateCustomFieldControl(AppFieldContext);
        }

        protected override void OnInitializeCustomFieldControl(object Control, XtraContainerControlProperties FieldProperties)
        {
            switch(FieldProperties.fieldContext.SpecifiedControlType)
            {
                case "AutoRefCombo":
                    ((DataAvail.DX.XtraEditors.AutoRefCombo)FieldProperties.control).DxEdit.DataProvider = new AutoRefComboDataProvider(FieldProperties.fieldContext);
                    break;
                case "LikeLookUpEdit":
                    ((DataAvail.DX.XtraEditors.AutoRefCombo)FieldProperties.control).DxEdit.DataProvider = new LikeLookUpEditDataProvider(
                        Controller.GetParentRelationDataSource(FieldProperties.fieldContext.ParentRelation),
                        FieldProperties.fieldContext);
                    break;
            }

            base.OnInitializeCustomFieldControl(Control, FieldProperties);
        }

        #endregion

        protected override IEnumerable<DataAvail.Controllers.Commands.IControllerCommandItem> CommandItems
        {
            get
            {
                return base.CommandItems.Except(this._childrenGrids.SelectMany(p => p.SupportedCommands));
            }
        }
    }
     
}
