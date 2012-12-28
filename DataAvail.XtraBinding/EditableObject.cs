using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraBindings
{
    public abstract class EditableObject : System.ComponentModel.IEditableObject, IEditableObjectBaseOpers, IObjectStateProvider, IComparable
    {
        private EditableObject _orig;

        private bool _isNew = true;

        public virtual void BeginEdit()
        {
            BeginEditInt();
        }

        public virtual void CancelEdit()
        {
            _orig.CopyTo(this);

            OnEditCanceled();
        }

        public virtual void EndEdit()
        {
            _isNew = false;

            BeginEditInt();

            this.CopyTo(_orig);

            OnEditEnd();
        }

        #region IEditableObjectBaseOpers Members

        public abstract EditableObject CreateInstance();

        public abstract void CopyTo(EditableObject ObjectTo);

        #endregion

        internal void BeginEditInt()
        {
            if (_orig == null)
            {
                _orig = CreateInstance();

                this.CopyTo(_orig);
            }
        }

        internal event System.EventHandler EditCanceled;

        internal event System.EventHandler EditEnd;

        private void OnEditCanceled()
        {
            if (EditCanceled != null)
            {
                EditCanceled(this, EventArgs.Empty);
            }
        }

        private void OnEditEnd()
        {
            if (EditEnd != null)
            {
                EditEnd(this, EventArgs.Empty);
            }
        }


        #region IObjectStateProvider Members

        public bool IsEdit
        {
            get { return this.CompareTo(_orig) != 0; }
        }

        public bool IsNew
        {
            get { return _isNew; }
        }

        #endregion

        #region IComparable Members

        public abstract int CompareTo(object obj);

        #endregion
    }

}
