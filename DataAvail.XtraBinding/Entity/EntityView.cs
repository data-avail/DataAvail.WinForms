using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baio.XtraBinding.Entity
{
    public class EntityView<T> : System.ComponentModel.IEditableObject where T : class, new()
    {
        public EntityView()
        {
        }

        public void Initialize(T Entity)
        {
            _entity = Entity;

            _editableEntity = new T();
        }

        private T _entity;

        private T _editableEntity;

        #region IEditableObject Members

        public void BeginEdit()
        {
            CancelEdit();
        }

        public void CancelEdit()
        {
            Baio.Utils.Reflection.CopyProperties(_entity, _editableEntity);
        }

        public void EndEdit()
        {
            Baio.Utils.Reflection.CopyProperties(_editableEntity, _entity);
        }

        #endregion
    }
}
