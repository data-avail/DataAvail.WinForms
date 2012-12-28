using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;

namespace DataAvail.XtraBindings.Calculator
{
    public class XtraBindingCalculator
    {
        internal XtraBindingCalculator(IObjectCalculator ObjectCalculator, BindingSource BindingSource)
        {
            _objectCalculator = ObjectCalculator;

            _bindingSource = BindingSource;
        }

        private object _bindingSourceItem;

        private ObjectProperties _objectProperties;

        private readonly IObjectCalculator _objectCalculator;

        private readonly BindingSource _bindingSource;

        private XOTableContext _tableContext;

        public void SetAppItemContext(XOTableContext TableContext)
        {
            _tableContext = TableContext;
        }

        internal object BindingSourceItem
        {
            get { return _bindingSourceItem; }

            set
            {
                if (_bindingSourceItem != value)
                {
                    _bindingSourceItem = value;

                    if (_bindingSourceItem != null)
                    {
                        _objectProperties = new ObjectProperties(
                            _tableContext, _bindingSource.GetItemValues(_bindingSourceItem));
                    }
                    else
                    {
                        _objectProperties = null;
                    }

                    OnObjectPropertiesChanged();
                }
                else if (_bindingSourceItem != null) 
                {
                    _objectProperties.MergeValues(_bindingSource.GetItemValues(_bindingSourceItem));
                }
            }
        }

        public ObjectProperties ObjectProperties
        {
            get { return _objectProperties; }
        }

        public bool IsCalculatorInitialized
        {
            get { return _objectCalculator != null; }
        }

        public void Calculate(ObjectCalulatorCalculateType CalculateType, string Field)
        {
            _objectCalculator.Calculate(ObjectProperties, Field, CalculateType);
        }

        private void CheckObjectPropertiesChanges()
        {
            OnObjectPropertiesChanged();
        }

        public event System.EventHandler ObjectPropertiesChanged;

        private void OnObjectPropertiesChanged()
        {
            if (ObjectPropertiesChanged != null)
            {
                ObjectPropertiesChanged(this, EventArgs.Empty);
            }
        }
    }
}
