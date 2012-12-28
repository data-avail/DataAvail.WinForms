using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;

namespace DataAvail.DX.XtraEditors
{
    internal abstract class ButtonCommandItem : DataAvail.Controllers.Commands.UICommandItems.UICommandItem
    {
        internal ButtonCommandItem(
            DataAvail.XtraEditors.IXtraEdit XtraEdit,
            XOFieldContext AppFieldContext,
            DevExpress.XtraEditors.ButtonEdit ButtonEdit,
            DevExpress.XtraEditors.Controls.EditorButton Button,
            DataAvail.Controllers.Commands.ControllerCommandTypes CommandType) :
            base(CommandType)
        {
            _xtraEdit = XtraEdit;

            _appFieldContext = AppFieldContext;

            _buttonEdit = ButtonEdit;

            _button = Button;

            _buttonEdit.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(Properties_ButtonClick);
        }

        private readonly DataAvail.XtraEditors.IXtraEdit _xtraEdit;

        private readonly XOFieldContext _appFieldContext;

        private readonly DevExpress.XtraEditors.ButtonEdit _buttonEdit;

        private readonly DevExpress.XtraEditors.Controls.EditorButton _button;

        protected DataAvail.XtraEditors.IXtraEdit XtraEdit
        {
            get { return _xtraEdit; }
        }

        protected virtual object EditValue
        {
            get { return XtraEdit.EditValue; }

            set { XtraEdit.EditValue = value; }
        
        }

        void Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button == _button)
            {
                Exec();
            }
        }

        protected override void OnExecuted(object ReturnValue)
        {
            this.EditValue = ReturnValue;
        }

        public override object Argument
        {
            get
            {
                return this.EditValue;
            }
        }

        public override object Context
        {
            get
            {
                return _appFieldContext;
            }
        }

        public override bool Enabled
        {
            set
            {
                _button.Enabled = value;
            }
        }

        public override bool Available
        {
            set
            {
                if (!_buttonEdit.Properties.ReadOnly || !value)
                    _button.Visible = value;
            }
        }
    }

}
