using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;
using DataAvail.Controllers.Binding;
using DataAvail.Controllers;
using System.ComponentModel;
using DevExpress.XtraEditors.Repository;

namespace DataAvail.DX.XtraGrid
{
    public partial class XtraGrid : DataAvail.XtraGrid.XtraGrid
    {
        public XtraGrid(DataAvail.Controllers.Controller Controller)
            : base(Controller)
        {
        }

        private List<DevExpress.XtraEditors.NavigatorButton> _buttonsBeforeReadOnly 
            = new List<DevExpress.XtraEditors.NavigatorButton>();

        protected override void InitializeUI()
        {
            InitializeComponent();

            this.gridControl1.EmbeddedNavigator.Buttons.CancelEdit.Visible =
            this.gridControl1.EmbeddedNavigator.Buttons.Edit.Visible =
            this.gridControl1.EmbeddedNavigator.Buttons.EndEdit.Visible = false;

            base.InitializeUI();

            UpdateColumnsReadOnly(true);

            Controller.SetControlDataSource(this.gridControl1);

            this.gridControl1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(gridControl1_PreviewKeyDown);
        }

        public override System.Windows.Forms.ContextMenuStrip ContextMenuStrip
        {
            get
            {
                return this.contextMenuStrip1;
            }
        }

        protected override IEnumerable<DataAvail.Controllers.Commands.IControllerCommandItem> OnCreateCommands()
        {
            return new DataAvail.Controllers.Commands.IControllerCommandItem[] { 
                new XGNavButtonCommandItemStub(gridControl1.EmbeddedNavigator, gridControl1.EmbeddedNavigator.Buttons.Append, DataAvail.Controllers.Commands.ControllerCommandTypes.Add),
                new XGNavButtonCommandItemStub(gridControl1.EmbeddedNavigator, gridControl1.EmbeddedNavigator.Buttons.Remove, DataAvail.Controllers.Commands.ControllerCommandTypes.Remove),
                new XGShowItemCommandItemStub(gridControl1)
            }.Union(base.OnCreateCommands());
        }

        private void UpdateColumnsReadOnly(bool ReadOnly)
        {
            foreach (DevExpress.XtraGrid.Columns.GridColumn col in this.gridView1.Columns)
            {
                col.OptionsColumn.AllowEdit = !ReadOnly;
            }
        }

        #region Fields building

        protected override object CreateFieldControl(DataAvail.XtraContainerBuilder.XtraContainerBuilderControlType ControlType, XOFieldContext FieldContext)
        {
            /*
            if (ControlType != DataAvail.XtraContainerBuilder.XtraContainerBuilderControlType.Custom)
                return new DevExpress.XtraGrid.Columns.GridColumn();
            else
                return null;
             */

            return new DevExpress.XtraGrid.Columns.GridColumn();
        }

        protected override void Build(XOTableContext TableContext)
        {
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();

            base.Build(TableContext);

            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);
        }

        protected override void InitializeFieldControl(object Control, DataAvail.XtraContainerBuilder.XtraContainerControlProperties ControlProperties)
        {
            DevExpress.XtraGrid.Columns.GridColumn col = (DevExpress.XtraGrid.Columns.GridColumn)Control;

            col.Name = string.Format("col{0}", ControlProperties.fieldContext.Name);
            col.Caption = ControlProperties.fieldContext.Caption;
            col.Visible = true;
            col.VisibleIndex = gridView1.Columns.Count;


            col.OptionsColumn.ReadOnly = !ControlProperties.fieldContext.IsCanEdit;

            if (ControlProperties.fieldContext.Mask != null)
            {
                DevExpress.Utils.FormatInfo formatInfo = DataAvail.DX.Utils.Converter.GetFormatInfo(ControlProperties.fieldContext.Mask);

                col.DisplayFormat.Assign(formatInfo);
            }


            base.InitializeFieldControl(Control, ControlProperties);
        }

        protected override void BindFieldControl(object Control, string FieldName, string BindingProperty)
        {
            ((DevExpress.XtraGrid.Columns.GridColumn)Control).FieldName = FieldName;

            base.BindFieldControl(Control, FieldName, BindingProperty);
        }

        protected override void OnInitializeComboboxFieldControl(object Control, DataAvail.XtraContainerBuilder.XtraContainerControlProperties FieldProperties)
        {
            DevExpress.XtraGrid.Columns.GridColumn col = (DevExpress.XtraGrid.Columns.GridColumn)Control;

            DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();

            ControllerDataSource controllerDataSource = Controller.GetControllerDataSource(FieldProperties.fieldContext.Name);

            RepositoryItemLookUpEditDataSourceProperties dataSourceProperties = new RepositoryItemLookUpEditDataSourceProperties(lookUpEdit);
            dataSourceProperties.Assign((ControllerDataSourceProperties)controllerDataSource.DataSourceProperties);

            //
            lookUpEdit.ShowFooter = lookUpEdit.ShowHeader = false;
            lookUpEdit.NullText = null;

            col.ColumnEdit = lookUpEdit;

            gridControl1.RepositoryItems.Add(lookUpEdit);
        }

        protected override void AddFieldControl(object Control, DataAvail.XtraContainerBuilder.XtraContainerControlProperties XtraContainerControlProperties)
        {
            this.gridView1.Columns.Add((DevExpress.XtraGrid.Columns.GridColumn)Control);
        }

        protected override object CreateEmptyControl()
        {
            return new DevExpress.XtraGrid.Columns.GridColumn();
        }

        #endregion

        protected override void OnListUIFilter(string ExpressionFilter)
        {
            gridView1.ActiveFilterString = ExpressionFilter;
        }

        protected override bool OnListUIFilterActive(bool FilterActive)
        {
            bool f = gridView1.ActiveFilterEnabled;

            gridView1.ActiveFilterEnabled = FilterActive;

            return f;
        }

        protected override DataAvail.Data.DbContext.IDbContextWhereFormatter OnListUIFilterFormatter()
        {
            return new DataAvail.Data.DbContext.DbContextWhereFormatter("yyyy-MM-ddTHH:mm:ss", "#{0}#");
        }

        protected override void OnEndFill()
        {
            base.OnEndFill();

            this.gridView1.ExpandAllGroups();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.gridView1.ExpandAllGroups();
        }

        protected override void OnReadOnlyChanged()
        {
            if (ReadOnly)
            {
                _buttonsBeforeReadOnly.Clear();

                if (this.gridControl1.EmbeddedNavigator.Buttons.Append.Visible)
                    _buttonsBeforeReadOnly.Add(this.gridControl1.EmbeddedNavigator.Buttons.Append);

                if (this.gridControl1.EmbeddedNavigator.Buttons.Remove.Visible)
                    _buttonsBeforeReadOnly.Add(this.gridControl1.EmbeddedNavigator.Buttons.Remove);

                this.gridControl1.EmbeddedNavigator.Buttons.Append.Visible =
                    this.gridControl1.EmbeddedNavigator.Buttons.Remove.Visible = false;

            }
            else
            {
                foreach (var b in _buttonsBeforeReadOnly)
                    b.Visible = true;
            
            }


            base.OnReadOnlyChanged();
        }

        protected override void OnListUIUploadToExcel(string FileName)
        {
            gridControl1.MainView.ExportToXls(FileName, new DevExpress.XtraPrinting.XlsExportOptions(DevExpress.XtraPrinting.TextExportMode.Text));    
        }

        void gridControl1_PreviewKeyDown(object sender, System.Windows.Forms.PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Return)
                e.IsInputKey = true;
        }

        private class RepositoryItemLookUpEditDataSourceProperties : ControllerDataSourceProperties
        {
            internal RepositoryItemLookUpEditDataSourceProperties(RepositoryItemLookUpEdit RepositoryItemLookUpEdit)
            {
                _repositoryItemLookUpEdit = RepositoryItemLookUpEdit;
            }

            private readonly RepositoryItemLookUpEdit _repositoryItemLookUpEdit;

            public RepositoryItemLookUpEdit RepositoryItemLookUpEdit
            {
                get { return _repositoryItemLookUpEdit; }
            } 

            protected override void OnDataSourceChanged()
            {
                RepositoryItemLookUpEdit.DataSource = this.DataSource;
            }

            protected override void OnValueMemberChanged()
            {
                RepositoryItemLookUpEdit.ValueMember = this.ValueMember;
            }

            protected override void OnDisplayMemberChanged()
            {
                RepositoryItemLookUpEdit.DisplayMember = this.DisplayMember;

                RepositoryItemLookUpEdit.Columns.Clear();
                RepositoryItemLookUpEdit.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(this.DisplayMember));
            }

            protected override void OnFilterChanged()
            {
                if (!string.IsNullOrEmpty(this.Filter) && RepositoryItemLookUpEdit.DataSource is IBindingListView)
                    ((IBindingListView)RepositoryItemLookUpEdit.DataSource).Filter = this.Filter;
            }
        }

    }
}
