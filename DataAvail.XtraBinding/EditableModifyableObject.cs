using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraBindings
{
    public abstract class EditableModifyableObject : EditableObject, IModifyableObjectProvider, IModifyableObject
    {
        public EditableModifyableObject()
        {
        }

        private EditableObject _modifyableObject;

        #region IModifyableObjectProvider Members

        public IModifyableObject ModifyableObject
        {
            get 
            {
                return (IModifyableObject)_modifyableObject;
            }
        }

        #endregion

        public override void BeginEdit()
        {
            base.BeginEdit();

            if (_modifyableObject == null)
            {
                _modifyableObject = CreateInstance();

                this.CopyTo(_modifyableObject);

                _modifyableObject.BeginEditInt();

                _modifyableObject.EditCanceled += new EventHandler(_modifyableObject_EditCanceled);

                _modifyableObject.EditEnd += new EventHandler(_modifyableObject_EditEnd);
            }
        }

        public override void EndEdit()
        {
            base.EndEdit();

            if (_modifyableObject != null)
                this.CopyTo(_modifyableObject);
        }

        public override void CancelEdit()
        {
            base.CancelEdit();
        }

        #region IModifyableObject Members

        public void AcceptChanges()
        {
            // EditableObject.EndEdit() -> copy values from controls to EditableObject -> copy values from EditableObject to ModifyableObject (this)
            this.EndEdit();

            //Update origin values
            this.EndEdit();
        }

        public void RejectChanges()
        {
            this.CancelEdit();
        }

        #endregion

        void _modifyableObject_EditCanceled(object sender, EventArgs e)
        {
            this.CancelEdit();

            _modifyableObject.CopyTo(this);

            this.EndEdit();
        }

        void _modifyableObject_EditEnd(object sender, EventArgs e)
        {
            this.EndEdit();

            _modifyableObject.CopyTo(this);
        }
    }
}
