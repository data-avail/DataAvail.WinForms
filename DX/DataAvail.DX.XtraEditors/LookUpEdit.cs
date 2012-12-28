using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataAvail.XObject;
using DataAvail.Controllers.Binding;
using System.ComponentModel;

namespace DataAvail.DX.XtraEditors
{
    public class LookUpEdit : BaseTextEdit<DataAvail.DXEditors.DX.LookUpEdit>, IControllerDataSourceControl
    {
        public LookUpEdit(XOFieldContext XOFieldContext)
            : base(XOFieldContext)
        {
            _selectItemButton = new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis)
            {
                Visible = false
            };

            _addItemButton = new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Plus)
            {
                Visible = false
            };

            this.DxEdit.Properties.Buttons.Add(new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo));
            this.DxEdit.Properties.Buttons.Add(SelectItemButton);
            this.DxEdit.Properties.Buttons.Add(AddItemButton);

            this.DxEdit.Properties.ShowFooter = false;
            this.DxEdit.Properties.ShowHeader = false;

            _dataSourceProperties = new LookUpEditDataSourceProperties(this);
        }

        private readonly DevExpress.XtraEditors.Controls.EditorButton _selectItemButton;

        private readonly DevExpress.XtraEditors.Controls.EditorButton _addItemButton;

        private readonly LookUpEditDataSourceProperties _dataSourceProperties;

        private DevExpress.XtraEditors.Controls.EditorButton SelectItemButton
        {
            get { return _selectItemButton; }
        }

        private DevExpress.XtraEditors.Controls.EditorButton AddItemButton
        {
            get { return _addItemButton; }
        }

        protected override void OnControlPropertiesPropertyChanged(string PropertyName)
        {
            base.OnControlPropertiesPropertyChanged(PropertyName);

            switch (PropertyName)
            {
                case "DataSource":
                    this.DataSourceProperties.DataSource = ControlProperties.DataSource;
                    break;
                case "DataSourceFilter":
                    this.DataSourceProperties.Filter = ControlProperties.DataSourceFilter;
                    break;
            }
        }

        #region Commands

        protected override IEnumerable<DataAvail.Controllers.Commands.IControllerCommandItem> CreateCommandItems()
        {    
            return new DataAvail.Controllers.Commands.IControllerCommandItem[] 
                { new ItemSelectorCommandItem(this), new AddItemCommandItem(this) };
        }

        private class ItemSelectorCommandItem : LookUpCommandItem
        {
            internal ItemSelectorCommandItem(LookUpEdit LookUpEdit) :
                base(LookUpEdit,
                LookUpEdit.SelectItemButton,
                DataAvail.Controllers.Commands.ControllerCommandTypes.SelectFkItem
                )
            {
            }
        }

        private class AddItemCommandItem : LookUpCommandItem
        {
            internal AddItemCommandItem(LookUpEdit LookUpEdit) :
                base(LookUpEdit, 
                LookUpEdit.AddItemButton, 
                DataAvail.Controllers.Commands.ControllerCommandTypes.AddFkItem
                )
            {
            }
        }

        #endregion

        #region IControllerDataSourceControl Members

        public ControllerDataSourceProperties DataSourceProperties
        {
            get
            {
                return _dataSourceProperties;
            }

            set
            {
                _dataSourceProperties.Assign(value);
            }
        }

        #endregion

        #region LookUpEditDataSourceProperties

        private class LookUpEditDataSourceProperties : ControllerDataSourceProperties
        {
            internal LookUpEditDataSourceProperties(LookUpEdit LookUpEdit)
            {
                _lookUpEdit = LookUpEdit;
            }

            private readonly LookUpEdit _lookUpEdit;

            private LookUpEdit LookUpEdit
            {
                get { return _lookUpEdit; }
            }

            protected override void OnDataSourceChanged()
            {
                this.LookUpEdit.DxEdit.Properties.DataSource = new BindingSource(DataSource, null);
            }

            protected override void OnValueMemberChanged()
            {
                this.LookUpEdit.DxEdit.Properties.ValueMember = ValueMember;
            }

            protected override void OnDisplayMemberChanged()
            {
                this.LookUpEdit.DxEdit.Properties.DisplayMember = DisplayMember;

                this.LookUpEdit.DxEdit.Properties.Columns.Clear();
                this.LookUpEdit.DxEdit.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(DisplayMember));
            }

            protected override void OnFilterChanged()
            {
                if (DataSourceListView != null)
                    DataSourceListView.Filter = this.Filter;
            }

            private IBindingListView DataSourceListView
            {
                get { return ((BindingSource)this.LookUpEdit.DxEdit.Properties.DataSource).DataSource as IBindingListView; }
            }
        }


        #endregion
    }
}
