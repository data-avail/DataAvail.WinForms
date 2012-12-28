using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraObjectProperties.XOPCreator
{
    public abstract class XOPCreatorProviderBase<O,F> : IXOPCreatorProvider
    {
        public XOPCreatorProviderBase(IEnumerable<O> ObjectItems)
        {
            _objectItemsEnum = ObjectItems;

            _objectItems = ObjectItems.GetEnumerator();
        }

        IEnumerable<O> _objectItemsEnum;

        IEnumerator<O> _objectItems;

        IEnumerator<F> _fieldItems;

        XtraObjectProperties _currentXtraObject;

        #region abstract

        protected abstract IEnumerable<F> GetFieldItems(O ObjectItem);

        protected abstract XtraObjectProperties GetObjectProperties(O ObjectItem);

        protected abstract XtraFieldProperties GetFieldPropertiesCore(F FieldItem);

        #endregion

        #region virtual

        protected virtual XtraFieldProperties GetFieldProperties(F Item)
        {
            XtraFieldProperties props = GetFieldPropertiesCore(Item);

            XtraTextFieldProperties textProps = GetTextFieldProperties(props, Item);

            if (textProps != null)
                props = textProps;

            IFKFieldProperties fkProps = GetFKFieldProperties(props, Item);

            if (fkProps != null)
                props = (XtraFieldProperties)fkProps;

            return props;
        }

        protected virtual XtraTextFieldProperties GetTextFieldProperties(XtraFieldProperties XtraFieldProperties, F Item)
        {
            if (XtraFieldProperties.FieldType == typeof(string))
            {
                return new DataAvail.XtraObjectProperties.XtraTextFieldProperties(XtraFieldProperties);
            }
            else
            {
                return null;
            }
        }

        protected virtual IFKFieldProperties GetFKFieldProperties(XtraFieldProperties XtraFieldProperties, F Item)
        {
            return null;
        }

        #endregion

        #region IXOPCreatorProvider Members

        public XtraObjectProperties NextObject()
        {
            if (_objectItems.MoveNext())
            {
                _fieldItems = GetFieldItems(_objectItems.Current).GetEnumerator();

                _currentXtraObject = GetObjectProperties(_objectItems.Current);

                return _currentXtraObject;
            }
            else
            {
                return null;
            }
        }

        public XtraFieldProperties NextField()
        {
            if (_fieldItems.MoveNext())
            {
                return GetFieldProperties(_fieldItems.Current);
            }
            else
            {
                return null;
            }
        }

        public virtual void FinalizeCreation(IEnumerable<XtraObjectProperties> XtraObjects)
        { }

        #endregion

        protected XtraObjectProperties CurrentXtraObject
        {
            get
            {
                return _currentXtraObject;
            }
        }

        protected IEnumerable<O> ObjectItems
        {
            get
            {
                return _objectItemsEnum;
            }
        }

        #region IXOPXmlFieldCreator Members

        public virtual XtraFieldProperties CreateFkField(XtraFieldProperties XtraFieldProperties, FKFieldDescriptor FKFieldDescriptor)
        {
            if (FKFieldDescriptor.GetType() == typeof(FKFieldDescriptorDictionary))
            {
                return new FKFieldPropertiesDictionary(XtraFieldProperties, ((FKFieldDescriptorDictionary)FKFieldDescriptor).valueNameDictionary);
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
