using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;


namespace DataAvail.DX.XtraEditors
{
    public partial class AutoRefCombo : BaseEdit<DataAvail.DXEditors.AutoRefCombo>
    {
        public AutoRefCombo(XOFieldContext FieldContext)
            : base(FieldContext)
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

            this.DxEdit.KeyValueChanged += new EventHandler(DxEdit_KeyValueChanged);
        }


        private readonly DevExpress.XtraEditors.Controls.EditorButton _selectItemButton;

        private readonly DevExpress.XtraEditors.Controls.EditorButton _addItemButton;

        private DevExpress.XtraEditors.Controls.EditorButton SelectItemButton
        {
            get { return _selectItemButton; }
        }

        private DevExpress.XtraEditors.Controls.EditorButton AddItemButton
        {
            get { return _addItemButton; }
        }

        public override object EditValue
        {
            get
            {
                return DxEdit.KeyValue;
            }
            set
            {
                DxEdit.KeyValue = value;
            }
        }

        protected override void OnEditValueChanged()
        {
            
        }

        void DxEdit_KeyValueChanged(object sender, EventArgs e)
        {
            base.OnEditValueChanged();
        }


        public string Filter { get { return null; } }

        #region Commands

        protected override IEnumerable<DataAvail.Controllers.Commands.IControllerCommandItem> CreateCommandItems()
        {
            return new DataAvail.Controllers.Commands.IControllerCommandItem[] { new ItemSelectorCommandItem(this), new AddItemCommandItem(this) };
        }

        private class ItemSelectorCommandItem : AutoRefComboCommandItem
        {
            internal ItemSelectorCommandItem(AutoRefCombo AutoRefCombo) :
                base(AutoRefCombo,
                AutoRefCombo.SelectItemButton,
                DataAvail.Controllers.Commands.ControllerCommandTypes.SelectFkItem
                )
            {
            }
        }

        private class AddItemCommandItem : AutoRefComboCommandItem
        {
            internal AddItemCommandItem(AutoRefCombo AutoRefCombo) :
                base(AutoRefCombo,
                AutoRefCombo.AddItemButton,
                DataAvail.Controllers.Commands.ControllerCommandTypes.AddFkItem
                )
            {
            }
        }

        private class AutoRefComboCommandItem : ButtonCommandItem
        {

            protected AutoRefComboCommandItem
                (AutoRefCombo AutoRefCombo,
                DevExpress.XtraEditors.Controls.EditorButton Button,
                DataAvail.Controllers.Commands.ControllerCommandTypes CommandType) :
                base(AutoRefCombo,
                        AutoRefCombo.AppFieldContext,
                        AutoRefCombo.DxEdit,
                        Button,
                        CommandType)
            { }

            protected override object EditValue
            {
                get
                {
                    return ((DataAvail.DX.XtraEditors.AutoRefCombo)XtraEdit).DxEdit.KeyValue;
                }
                set
                {
                    if (!((DataAvail.DX.XtraEditors.AutoRefCombo)XtraEdit).DxEdit.KeyValue.Equals(value))
                    {
                        ((DataAvail.DX.XtraEditors.AutoRefCombo)XtraEdit).DxEdit.RefreshData();
                        ((DataAvail.DX.XtraEditors.AutoRefCombo)XtraEdit).DxEdit.KeyValue = value;
                    }
                }
            }

            public override object Context
            {
                get
                {
                    return new DataAvail.Controllers.Commands.UICommandItems.FKCommandItemContext(
                        (XOFieldContext)base.Context, ((AutoRefCombo)XtraEdit).Filter);
                }
            }
        }

        #endregion
    }
}
