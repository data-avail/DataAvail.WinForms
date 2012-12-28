using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;

namespace DataAvail.XtraBindings
{
    internal class BindingSource : System.Windows.Forms.BindingSource
    {
        internal BindingSource(object BindingDataSource, Calculator.IObjectCalculator ObjectCalculator)
            : base(BindingDataSource, null)
        {
            _objectCalculator = ObjectCalculator;

            _xtraBindingCalculator = new DataAvail.XtraBindings.Calculator.XtraBindingCalculator(ObjectCalculator, this);// { Object = this.Current };

            if (DataSource is DataAvail.Data.IDataSourceQueriable)
            {
                ((DataAvail.Data.IDataSourceQueriable)DataSource).DataSourceChanged += new EventHandler(BindingSource_DataSourceChanged);
            }
        }

        public event System.ComponentModel.AddingNewEventHandler RemovingOld;

        private readonly Calculator.IObjectCalculator _objectCalculator;

        private readonly Calculator.XtraBindingCalculator _xtraBindingCalculator;

        internal Calculator.XtraBindingCalculator Calculator { get { return _xtraBindingCalculator; } }

        internal bool IsCalculatorInitialized
        {
            get
            {
                return Calculator.IsCalculatorInitialized;
            }
        }

        internal void CalculatorReset(XOTableContext TableContext, int SourceIndex)
        {
            _xtraBindingCalculator.SetAppItemContext(TableContext);

            _xtraBindingCalculator.BindingSourceItem = null;

            if (SourceIndex != -1)
                _xtraBindingCalculator.BindingSourceItem = this[SourceIndex];
        }

        internal void CalculatorCalculate(Calculator.ObjectCalulatorCalculateType CalculateType, string FieldName)
        {
            _xtraBindingCalculator.BindingSourceItem = this.Current;

            _xtraBindingCalculator.Calculate(CalculateType, FieldName);

            UpdateValuesFromCalculator(this.Current);
        }

        internal void UpdateValuesFromCalculator()
        {
            UpdateValuesFromCalculator(this.Current);
        }

        void BindingSource_DataSourceChanged(object sender, EventArgs e)
        {
            this.ResetBindings(true);
        }

        /*
        protected override void OnCurrentChanged(EventArgs e)
        {
            if (_xtraBindingCalculator != null && _xtraBindingCalculator.BindingSourceItem != this.Current)
                _xtraBindingCalculator.BindingSourceItem = this.Current;

            base.OnCurrentChanged(e);
        }
         */

        private void UpdateValuesFromCalculator(object Item)
        {
            if (this.Calculator.ObjectProperties["value"] != null)
                SetItemValues(Item, this.Calculator.ObjectProperties["value"].value.ToDictionary());
        }

        private void SetItemValues(object Item, IDictionary<string, object> NameValues)
        {
            foreach (KeyValuePair<string, object> kvp in NameValues.Where(p=>p.Value != null))
                SetItemValue(Item, kvp.Key, kvp.Value);
        }

        internal void SetItemValue(object Item, string FieldName, object Value)
        {
            System.ComponentModel.ICustomTypeDescriptor descr = Item as System.ComponentModel.ICustomTypeDescriptor;

            if (descr != null && !descr.GetProperties()[FieldName].GetValue(Item).Equals(Value))
            {
                descr.GetProperties()[FieldName].SetValue(Item, Value);
            }
        }

        internal object GetItemValue(object Item, string FieldName)
        {
            System.ComponentModel.ICustomTypeDescriptor descr = Item as System.ComponentModel.ICustomTypeDescriptor;

            if (descr != null)
            {
                return descr.GetProperties()[FieldName].GetValue(Item);
            }
            else
            {
                return null;
            }
        }

        internal IDictionary<string, object> GetItemValues(object Item)
        {
            System.ComponentModel.ICustomTypeDescriptor descr = Item as System.ComponentModel.ICustomTypeDescriptor;

            if (descr != null)
            {
                return descr.GetProperties().Cast<System.ComponentModel.PropertyDescriptor>().ToDictionary(k => k.Name, v => v.GetValue(Item));
            }
            else
            {
                return null;
            }
        }


        //SuspendBinding + RaiseListChangedEvents = false
        public void Suspend()
        {
            this.RaiseListChangedEvents = false;

            base.SuspendBinding();
        }

        public void Resume()
        {
            this.ResumeBinding();

            base.RaiseListChangedEvents = true;
        }

        public override void RemoveAt(int index)
        {
            if (RemovingOld != null)
            { 
                RemovingOld(this, new System.ComponentModel.AddingNewEventArgs(this[index]));
            }

            base.RemoveAt(index);
        }

        //толилыжинеедуттолияебанут стандартный IndexOf не работает с DevArtDataRowView,  возможно они чтото с HashCode() нахимичили
        internal new int IndexOf(object Item)
        {
            return this.Cast<object>().Select((s, i) => new { s = s, i = i }).First(p => p.s == Item).i;
        }

        internal int IndexOf(string FieldName, object Value)
        {
            var r = this.Cast<object>().Select((x, i) => new { x = this.GetItemValue(x, FieldName), i = i }).FirstOrDefault(p => p.x.Equals(Value));

            if (r != null)
            {
                return r.i;
            }
            else
            {
                return -1;
            }

        }

    }
}
