using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Baio.XtraBinding.Entity
{
    public class EditableObject<T> : System.ComponentModel.IEditableObject, IObjectStateProvider  where T : class, new()
    {

        public EditableObject(T Entity)
        {
            _entity = Entity;

            _clonedEntity = new T();
        }

        private readonly T _entity;

        private readonly T _clonedEntity;

        #region IEditableObject Members

        public void BeginEdit()
        {
            EndEdit();
        }

        public void CancelEdit()
        {
            Baio.Utils.Reflection.CopyProperties(_clonedEntity, _entity);
        }

        public void EndEdit()
        {
            Baio.Utils.Reflection.CopyProperties(_entity, _clonedEntity);
        }

        #endregion

        #region IObjectStateProvider Members

        public bool IsEdit
        {
            get { return Baio.Utils.Reflection.CompareByProperties(_entity, _clonedEntity); }
        }

        public bool IsNew
        {
            get { return false; }
        }

        #endregion
    }
}
