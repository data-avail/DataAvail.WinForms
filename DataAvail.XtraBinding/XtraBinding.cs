using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;

namespace DataAvail.XtraBindings
{
    public class XtraBinding
    {
        public XtraBinding(object DataSource, XtraBindingOperation XtraBindingOperation)
        {
            if (ItemAdapter == null)
                ItemAdapter = new ItemAdapter.DataTable.XBIADataTable();

            _dataSource = DataSource;

            _bindingSource = new BindingSource(ItemAdapter.GetBindingListView(DataSource), XtraBindingOperation.ObjectCalculator);

            System.Type itemType = ItemAdapter != null ? ItemAdapter.AdaptedItemType : GetDataSourceCustomItemType(DataSource);

            _isCanEdit = itemType.GetInterfaces().Contains(typeof(System.ComponentModel.IEditableObject));

            _isCanModify = itemType.GetInterfaces().Contains(typeof(IModifyableObjectProvider));

            _isEditStatusProvided = itemType.GetInterfaces().Contains(typeof(IObjectStateProvider));

            if (_isCanModify)
            {
                System.Reflection.MemberInfo memberInfo = itemType.GetMembers().First(p => p.Name == "ModifyableObject");

                _isModifyedStatusProvided = memberInfo.DeclaringType.GetInterfaces().Contains(typeof(IObjectStateProvider));

                _bindingSource.RemovingOld += new System.ComponentModel.AddingNewEventHandler(_bindingSource_RemovingOld);
            }

            xtraBindingOperation = XtraBindingOperation;

            if (xtraBindingOperation != null)
            {
                xtraBindingOperation.EndFill += new XtraBindingOperationEndFillHandler(xtraBindingOperation_EndFill);
            }

            EndEdit();
        }

        public object _dataSource;

        private readonly BindingSource _bindingSource;

        private bool _isCanEdit = false;

        private bool _isCanModify = false;

        private bool _isEditStatusProvided = false;

        private bool _isModifyedStatusProvided = false;

        private bool _suspendCalculatorEvents = false;

        private static IXtraBindingItemAdapter _itemAdapter;

        private static XtraBindingUpdateSet _updateSet;

        public System.Windows.Forms.BindingSource BindingSource { get { return _bindingSource; } }

        public readonly XtraBindingOperation xtraBindingOperation;

        public event System.EventHandler CalulatorCalculated;


        private bool SuspendCalculatorEvents
        {
            get { return _suspendCalculatorEvents; }
            set { _suspendCalculatorEvents = value; }
        }

        public object DataSource
        {
            get { return _dataSource; }
        }

        internal virtual XtraBinding Clone()
        {
            return new XtraBinding(this.DataSource, this.xtraBindingOperation);
        }

        public void SetDataSourceFilter(string Filter)
        {
            if (BindingSource.Filter != Filter)
            {
                BindingSource.Filter = Filter;
            }
        }

        private static XtraBindingUpdateSet UpdateSet
        {
            get { return _updateSet; }
        }

        public static IXtraBindingItemAdapter ItemAdapter
        {
            get { return _itemAdapter; }

            set 
            {
                _updateSet = new XtraBindingUpdateSet(value.DataConverter);

                _itemAdapter = value;
            }
        }

        private object GetAdaptedItem(object DataSourceItem)
        {
            if (ItemAdapter == null)
            {
                return DataSourceItem;
            }
            else
            {
                return ItemAdapter.GetAdaptedDataSourceItem(DataSourceItem);
            }
        }

        private object GetUnderlyingItem(object DataSourceItem)
        {
            if (ItemAdapter == null)
            {
                return DataSourceItem;
            }
            else
            {
                return ItemAdapter.GetUnderlyingDataSourceItem(DataSourceItem);
            }
        }

        public void EndEdit()
        {
            if (IsCanEdit)
            {
                foreach (object item in BindingSource)
                {
                    ((System.ComponentModel.IEditableObject)item).EndEdit();
                }
            }
        }

        public static Type GetDataSourceCustomItemType(object DataSource)
        {
            return DataAvail.Utils.Reflection.GetItemType(DataSource.GetType());
        }

        public object Current 
        { 
            get
            {
                System.Diagnostics.Debug.Assert(!(BindingSource.Position != -1 && BindingSource.Current == null));

                return BindingSource.Position != -1 
                    && BindingSource.Current != null 
                    ? GetAdaptedItem(BindingSource.Current) : null;
            } 
        }

        public object CurrentUnderlying
        { 
            get
            {
                return this.GetUnderlyingItem(this.BindingSource.Current);
            } 
        }

        public Calculator.XtraBindingCalculator Calculator
        {
            get
            {
                return ((BindingSource)BindingSource).Calculator;
            }
        }

        public void AddNew()
        {
            object newItem = this.BindingSource.AddNew();

            this.CalculateInitializeNew();

            if (this is XtraBindingChild)
                ((XtraBindingChild)this).UpdateRelationFK(newItem);
        }

        public void CurrentBeginEdit()
        {
            if (IsCanEdit)
            {
                ((System.ComponentModel.IEditableObject)_bindingSource.Current).BeginEdit();
                
                OnAction(XtraBindingAction.BeginEdit);
            }

        }

        public void CurrentEndEdit()
        {
            if (IsCanEdit)
            {
                BindingSource.EndEdit();

                StoreCurrentItem();

                OnAction(XtraBindingAction.EndEdit);
            }
        }

        public void CurrentCancelEdit()
        {
            if (IsCanEdit)
            {
                //Why ((System.ComponentModel.IEditableObject)bindingSource.Current).CancelEdit() doesn't work???
                BindingSource.CancelEdit();

                OnAction(XtraBindingAction.CancelEdit);
            }
        }

        public void CurrentClone()
        {
            SuspendCalculatorEvents = true;

            CalculateClone();

            DataAvail.XtraBindings.Calculator.ObjectProperties op = this.ObjectProperties;

            AddNew();

            this.ObjectProperties.Merge(op);

            _bindingSource.UpdateValuesFromCalculator();

            SuspendCalculatorEvents = false;

            OnCalculatorCalculated();

        }

        public bool CurrentIsCanMoveNext
        {
            get { return BindingSource.Position != BindingSource.Count - 1; }
        }

        public void CurrentMoveNext()
        {
            this.BindingSource.MoveNext();

            CalculateInitialize();
        }

        public bool CurrentIsCanMovePerv
        {
            get { return BindingSource.Position != 0; }
        }

        public void CurrentMovePerv()
        {
            this.BindingSource.MovePrevious();

            CalculateInitialize();
        }

        public void CurrentRemove()
        {

            if (this.CurrentIsAdded)
            {
                BatchReject(UpdateSet.GetObjectsPKS(this, new object[] { CurrentUnderlying }));
            }
            else
            {
                this.BindingSource.RemoveCurrent();
            }
        }


        public void CurrentAcceptChanges()
        {
            CurrentEndEdit();

            if (IsCanModify)
            {
                BatchUpdate(UpdateSet.GetObjectsPKS(this, new object[] { CurrentUnderlying }));

                CalculateAfterSave();

                CalculateInitialize();
            }
        }

        public void CurrentRejectChanges()
        {
            CurrentCancelEdit();

            if (IsCanModify)
            {
                object [] pks = UpdateSet.GetObjectsPKS(this, new object[] { CurrentUnderlying }).ToArray();

                ((IModifyableObjectProvider)Current).ModifyableObject.RejectChanges();

                BatchReject(pks);
            }
        }

        public bool CurrentIsEdit
        {
            get
            {
                if (!IsEditStatusProvided)
                    throw new Exception("Isn't supported");

                return ((IObjectStateProvider)Current).IsEdit;
            }
        }

        /// <summary>
        /// Item was assed and EndEdit() still not invoked on it
        /// </summary>
        public bool CurrentIsNew
        {
            get
            {
                if (!IsEditStatusProvided)
                    throw new Exception("Isn't supported");

                return ((IObjectStateProvider)Current).IsNew;
            }
        }

        /// <summary>
        /// Item was added and EndEdit() was invoked on it
        /// </summary>
        public bool CurrentIsAdded
        {
            get
            {
                if (!IsModifyedStatusProvided)
                    throw new Exception("Isn't supported");

                return ((IObjectStateProvider)((IModifyableObjectProvider)Current).ModifyableObject).IsNew;
            }
        }


        public virtual bool CurrentIsModifyed
        {
            get
            {
                if (!IsModifyedStatusProvided)
                    throw new Exception("Isn't supported");

                return UpdateSet.GetStoredItemsBatch(this, UpdateSet.GetObjectsPKS(this, new object[] {CurrentUnderlying})).Count() != 0;
            }
        }

        public ObjectState CurrentObjectState
        {
            get
            {
                if (Current == null)
                    return ObjectState.Undetermined;

                if (IsModifyedStatusProvided && IsEditStatusProvided)
                {
                    if (CurrentIsNew)
                    {
                        return ObjectState.New;
                    }
                    else if (CurrentIsEdit && CurrentIsModifyed)
                    {
                        return ObjectState.EditModifyed;
                    }
                    else if (CurrentIsEdit)
                    {
                        return ObjectState.Edit;
                    }
                    else if (CurrentIsModifyed)
                    {
                        return ObjectState.Modifyed;
                    }
                    else
                    {
                        return ObjectState.Unchanged;
                    }
                }
                else if (IsEditStatusProvided)
                {
                    if (CurrentIsNew)
                    {
                        return ObjectState.New;
                    }
                    else if (CurrentIsEdit)
                    {
                        return ObjectState.Edit;
                    }
                    else
                    {
                        return ObjectState.Unchanged;
                    }
                }
                else
                {
                    return ObjectState.Unchanged;
                }
            }
        }

        public bool IsCanModify
        {
            get
            {
                return _isCanModify;
            }
        }

        public bool IsCanEdit
        {
            get
            {
                return _isCanEdit;
            }
        }

        public bool IsEditStatusProvided
        {
            get
            {
                return _isEditStatusProvided;
            }
        }

        public bool IsModifyedStatusProvided
        {
            get
            {
                return _isModifyedStatusProvided;
            }
        }

        public event XtraBindingActionHandler Action;

        protected void OnAction(XtraBindingAction XtraBindingAction)
        {
            if (Action != null)
            {
                Action(this, new XtraBindingActionEventArgs(XtraBindingAction));
            }
        }

        public enum XtraBindingAction
        { 
            BeginEdit,
            EndEdit, 
            CancelEdit,
            RejectChanges,
            AcceptChanges,
            ChildItemRemoved
        }

        public class XtraBindingActionEventArgs : System.EventArgs
        {
            internal XtraBindingActionEventArgs(XtraBindingAction XtraBindingAction)
            {
                this.xtraBindingAction = XtraBindingAction;
            }

            public readonly XtraBindingAction xtraBindingAction;
        }

        public delegate void XtraBindingActionHandler(object sender, XtraBindingActionEventArgs args);

        #region Batch update

        public bool IsCanBatchUpdate
        {
            get { return ItemAdapter is IXtraBindingBatchItemsAdapter; }
        }

        public void UpdateBatch()
        {
            BatchUpdate(null);
        }

        public void RejectBatch()
        {
            BatchReject(null);
        }

        public virtual bool IsBatchModifyed()
        {
            return UpdateSet.GetStoredItemsBatch(this, null).Count() != 0;
        }

        #endregion

        #region Fill

        public void StopFill()
        {
            xtraBindingOperation.StopFill();
        }

        public object DataSourceCloneAndFill(string Filter)
        {
            object clone = ItemAdapter.Clone(this.DataSource);

            xtraBindingOperation.FillSync(clone, Filter);

            return clone;
        }

        public IEnumerable<IDictionary<string, object>> GetItemsFromDataAdapter(string Filter)
        {
            return UpdateSet.ConvertToItems(DataSourceCloneAndFill(Filter));
        }


        public void Fill(string Filter, bool ForcedSync)
        {
            if (xtraBindingOperation != null)
            {
                _bindingSource.Suspend();

                xtraBindingOperation.Fill(Filter, ForcedSync);
            }
            else
                OnEndFill(new XtraBindingOperationEndFillEventArgs(true, null));
        }

        public event XtraBindingOperationEndFillHandler EndFill;

        void xtraBindingOperation_EndFill(object sender, XtraBindingOperationEndFillEventArgs args)
        {
            OnEndFill(args);
        }

        public void EndFillSafely()
        {
            UpdateSet.RefreshItems(this);

            _bindingSource.Resume();
        }

        protected virtual void OnEndFill(XtraBindingOperationEndFillEventArgs args)
        {
            if (EndFill != null)
            {
                EndFill(this, args);
            }
        }

        #endregion

        public void SuspendControlsBinding()
        {
            foreach (System.Windows.Forms.Binding binding in BindingSource.CurrencyManager.Bindings)
            {
                binding.ControlUpdateMode = System.Windows.Forms.ControlUpdateMode.Never;
            }
        }

        public void ResumeControlsBinding()
        {
            foreach (System.Windows.Forms.Binding binding in BindingSource.CurrencyManager.Bindings)
            {
                binding.ControlUpdateMode = System.Windows.Forms.ControlUpdateMode.OnPropertyChanged;
            }
        }

        public void ResetBindings()
        {
            BindingSource.ResetBindings(false);
        }

        private void BatchUpdate(IEnumerable<object> PKs)
        {
            object[] pks = PKs != null ? PKs.ToArray() : null;

            BatchEndUpdate(UpdateSet.Update(this, pks));

            UpdateSet.Reject(this, pks);

            OnAction(XtraBindingAction.AcceptChanges);
        }

        private static void BatchEndUpdate(IEnumerable<XtraBindingUpdateSetUpdateResult> UpdateResult)
        {
            foreach (XtraBindingUpdateSetUpdateResult upd in UpdateResult)
            {
                foreach (var i in upd.items.Where(p=>p.DataSourceItem != null).Select((p, i) => new { pk = upd.pksAfterUpdate.ElementAt(i), dataSourceItem = p.DataSourceItem }))
                {
                    if (i.pk != null)
                    {
                        upd.xtraBinding.SetDataSourceItemValue(i.dataSourceItem, upd.pkFieldName, i.pk);

                        upd.xtraBinding.FillDataSourceItem(i.dataSourceItem);
                    }

                    ((IModifyableObjectProvider)upd.xtraBinding.GetAdaptedItem(i.dataSourceItem)).ModifyableObject.AcceptChanges();
                }
            }
        }

        public void SetItemByValue(string FieldName, object Value)
        {
            int i = _bindingSource.IndexOf(FieldName, Value);

            if (i != -1)
            {
                _bindingSource.Position = i;
            }
        }

        internal IDictionary<string, object> GetItemValues(object BindingSourceItem)
        {
            return _bindingSource.GetItemValues(BindingSourceItem);
        }

        public object GetCurrentItemValue(string FieldName)
        {
            return _bindingSource.GetItemValue(_bindingSource.Current, FieldName);
        }

        internal IEnumerable<object> GetDataSourceItemsByValue(string FieldName, object Value)
        {
            return _bindingSource.Cast<object>().Where(p => _bindingSource.GetItemValue(p, FieldName).Equals(Value));
        }

        internal void SetDataSourceItemValue(object DataSourceItem, string FieldName, object Value)
        {
            _bindingSource.SetItemValue(DataSourceItem, FieldName, Value);
        }

        private void FillDataSourceItem(object DataSourceItem)
        {
            xtraBindingOperation.Fill(new object[] { GetUnderlyingItem(DataSourceItem) });
        }

        private void BatchReject(IEnumerable<object> PKs)
        {
            BatchEndReject(UpdateSet.Reject(this, PKs));

            OnAction(XtraBindingAction.RejectChanges);
        }


        private static void BatchEndReject(IEnumerable<XtraBindingUpdateSetRejectResult> RejectResult)
        {
            foreach (XtraBindingUpdateSetRejectResult rej in RejectResult)
            {
                foreach (object dataSourceItem in rej.items.Select(p=>p.DataSourceItem).Where(p=>p != null))
                {
                    ((IModifyableObjectProvider)rej.xtraBinding.GetAdaptedItem(dataSourceItem)).ModifyableObject.RejectChanges();
                }

                rej.xtraBinding.BindingSource.ResetBindings(false);
            }
        }

        private void StoreCurrentItem()
        {
            if (IsCurrentDataSourceItemModifyed)
                UpdateSet.Add(this, BindingSource.Current, IsCurrentDataSourceItemAdded ? XtraBindingStoredItemState.Added : XtraBindingStoredItemState.Modifyed);
        }

        private bool IsCurrentDataSourceItemModifyed
        {
            get { return ((IObjectStateProvider)((IModifyableObjectProvider)Current).ModifyableObject).IsEdit; }
        }

        private bool IsCurrentDataSourceItemAdded
        {
            get { return ((IObjectStateProvider)((IModifyableObjectProvider)Current).ModifyableObject).IsNew; }
        }


        void _bindingSource_RemovingOld(object sender, System.ComponentModel.AddingNewEventArgs e)
        {
            UpdateSet.Add(this, e.NewObject, XtraBindingStoredItemState.Removed);
        }

        #region Calculators

        public Calculator.ObjectProperties ObjectProperties
        {
            get { return _bindingSource.Calculator.ObjectProperties; }
        }


        internal void Calculate(Calculator.ObjectCalulatorCalculateType CalculateType, string FieldName)
        {
            if (_bindingSource.IsCalculatorInitialized)
            {
                object item = this.GetAdaptedItem(BindingSource.Current);

                System.ComponentModel.IEditableObject editableObject = (System.ComponentModel.IEditableObject)item;

                DataAvail.XtraBindings.IModifyableObject modifyableObject = ((DataAvail.XtraBindings.IModifyableObjectProvider)item).ModifyableObject;

                bool fNew = ((DataAvail.XtraBindings.IObjectStateProvider)editableObject).IsNew;

                bool fEdit = ((DataAvail.XtraBindings.IObjectStateProvider)editableObject).IsEdit;

                bool fModifyed = ((DataAvail.XtraBindings.IObjectStateProvider)modifyableObject).IsEdit;

                _bindingSource.CalculatorCalculate(CalculateType, FieldName);

                if (!fNew)
                {
                    if (!fEdit && ((DataAvail.XtraBindings.IObjectStateProvider)editableObject).IsEdit)
                    {
                        editableObject.EndEdit();
                    }

                    if (!fModifyed && ((DataAvail.XtraBindings.IObjectStateProvider)modifyableObject).IsEdit)
                    {
                        modifyableObject.AcceptChanges();
                    }
                }

                OnCalculatorCalculated();
            }
        }

        public void CalculatorReset(XOTableContext TableContext)
        {
            if (_bindingSource.IsCalculatorInitialized)
            {
                _bindingSource.CalculatorReset(TableContext, BindingSource.Position);

                if (BindingSource.Position != -1)
                    CalculateInitialize();
            }
        }

        internal void CalculateAfterSave()
        {
            Calculate(DataAvail.XtraBindings.Calculator.ObjectCalulatorCalculateType.AfterSave, null);
        }

        public void CalculateInitialize()
        {
            Calculate(DataAvail.XtraBindings.Calculator.ObjectCalulatorCalculateType.Initialize, null);
        }

        public void CalculateInitializeNew()
        {
            Calculate(DataAvail.XtraBindings.Calculator.ObjectCalulatorCalculateType.InitializeNew, null);
        }

        public void CalculateClone()
        {
            Calculate(DataAvail.XtraBindings.Calculator.ObjectCalulatorCalculateType.Clone, null);
        }

        public void CalculateField(string FieldName)
        {
            Calculate(DataAvail.XtraBindings.Calculator.ObjectCalulatorCalculateType.Calculate, FieldName);
        }

        private void OnCalculatorCalculated()
        {
            if (!SuspendCalculatorEvents && CalulatorCalculated != null)
            {
                CalulatorCalculated.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion

        internal void Clear()
        {
            this.xtraBindingOperation.Clear();
        }

        public int ItemsCount
        {
            get { return BindingSource.Count; }
        }


    }
}
