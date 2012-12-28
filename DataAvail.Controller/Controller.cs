using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.Controllers.Commands;
using DataAvail.XObject;
using DataAvail.Controllers.Binding;
using DataAvail.XtraBindings;

namespace DataAvail.Controllers
{
    public partial class Controller
    {
        public Controller(XtraBinding XtraBinding, XOTableContext XOTableContext)
            : this(XtraBinding, new ControllerDynamicContext(XOTableContext))
        { }

        public Controller(XtraBinding XtraBinding, IControllerDynamicContext ControllerDynamicContext)
        {
            _xtraBinding = XtraBinding;

            _dynamicContext = ControllerDynamicContext;

            _dataSources = new ControllerDataSourcesCollection(this);

            _commands = new ControllerCommands(this);

            _itemSelector = new ItemSelector(this);

            _xtraBinding.Calculator.SetAppItemContext(TableContext);

            CreateUI();

            XtraBinding.xtraBindingOperation.EndFill += new XtraBindingOperationEndFillHandler(xtraBindingOperation_EndFill);

            XtraBinding.CalulatorCalculated += new EventHandler(XtraBinding_CalulatorCalculated);

            DynamicContext.TableContextChanged += new EventHandler(DynamicContext_TableContextChanged);
        }

        public static System.ComponentModel.ISynchronizeInvoke synchronizeInvoke;

        public static IControllerUICreator uiCreator;

        public static ControllerContext controllerContext;

        private readonly IControllerDynamicContext _dynamicContext;

        private readonly XtraBinding _xtraBinding;

        private FillStates _fillState = FillStates.None;

        private ControllerStates _state = ControllerStates.None;

        private ControllerStates _availableStates = ControllerStates.None;

        private bool _itemMoving = false;

        private readonly ControllerCommands _commands;

        private readonly ItemSelector _itemSelector;

        private readonly ControllerDataSourcesCollection _dataSources;

        public IControllerDynamicContext DynamicContext
        {
            get { return _dynamicContext; }
        }

        private ControllerDataSourcesCollection DataSources
        {
            get { return _dataSources; }
        } 
 
        private void CreateUI()
        {
            ItemCreate();

            ItemInitialize();

            if (!(TableContext.IsChildContext))
            {
                ListCreate();

                ListInitialize();
            }

        }

        private bool ItemValidate()
        {
            return ItemUI != null ? ItemUI.Validate() : true;
        }


        #region public

        public XOTableContext TableContext
        {
            get { return _dynamicContext.CurrentTableContext; }
        }

        public ControllerCommands Commands
        {
            get { return _commands; }
        }

        public ItemSelector itemSelector
        {
            get { return _itemSelector; }
        }

        public ControllerStates State
        {
            get { return _state; }
        }

        public ControllerStates AvailableState
        {
            get { return _availableStates; }
        }

        private FillStates FillState
        {
            get { return _fillState; }

            set { _fillState = value; }

        }

        public static event ControllerAskExitHandler AskExit;

        public static event ControllerShowInfo ShowInfo;

        public static event ControllerAskConfirmationHandler AskConfirmation;

        #endregion

        protected internal XtraBinding XtraBinding
        {
            get { return _xtraBinding; }
        }

        private void OnAskConfirmation(ControllerAskConfirmationEventArgs args)
        {
            if (AskConfirmation != null)
                AskConfirmation(this, args);
        }

        private void OnAskExit(ControllerAskExitEventArgs args)
        {
            if (AskExit != null)
                AskExit(this, args);
        }

        private void OnShowInfo(ControllerShowInfoEventArgs args)
        {
            if (ShowInfo != null)
                ShowInfo(this, args);
        }

        internal void SetDataSourceFilter(string Filter)
        {
            this.XtraBinding.SetDataSourceFilter(Filter);
        }

        protected bool Confirm(string ConfirmationString)
        {
            ControllerAskConfirmationEventArgs args = new ControllerAskConfirmationEventArgs(ConfirmationString);

            OnAskConfirmation(args);

            return args.confirm;
        }

        protected bool ValidateExit(bool ItemExit)
        {
            ControllerAskExitResult enabledResults = ControllerAskExitResult.None;

            if (ItemExit)
            {
                if (DataAvail.Utils.EnumFlags.IsContain(State, ControllerStates.CanAcceptChanges))
                    enabledResults |= ControllerAskExitResult.Save;

                if (DataAvail.Utils.EnumFlags.IsContain(State, ControllerStates.CanRejectChanges))
                    enabledResults |= ControllerAskExitResult.Reject;

                if (DataAvail.Utils.EnumFlags.IsContain(State, ControllerStates.CanEndEdit))
                {
                    enabledResults |= ControllerAskExitResult.EndEdit;
                }
                else if (DataAvail.Utils.EnumFlags.IsContain(State, ControllerStates.CanAcceptChanges) && TableContext.IsCanSaveInCache)
                {
                    enabledResults |= ControllerAskExitResult.JustExit;
                }

                if (DataAvail.Utils.EnumFlags.IsContain(State, ControllerStates.CanCancelEdit))
                    enabledResults |= ControllerAskExitResult.CancelEdit;

            }
            else
            {
                if (DataAvail.Utils.EnumFlags.IsContain(State, ControllerStates.CanBatchAcceptChanges))
                    enabledResults |= ControllerAskExitResult.Save;

                if (DataAvail.Utils.EnumFlags.IsContain(State, ControllerStates.CanBatchRejectChanges))
                    enabledResults |= ControllerAskExitResult.Reject;
            }

            if (enabledResults == ControllerAskExitResult.None)
            {
                return true;
            }
            else
            {
                enabledResults |= ControllerAskExitResult.CancelExit;
            }

            ControllerAskExitEventArgs args = new ControllerAskExitEventArgs(enabledResults);

            OnAskExit(args);

            if (ItemExit)
            {
                switch (args.result)
                {
                    case ControllerAskExitResult.Save:
                        return Commands.Execute(ControllerCommandTypes.AcceptChanges);
                    case ControllerAskExitResult.Reject:
                        return Commands.Execute(ControllerCommandTypes.RejectChanges);
                    case ControllerAskExitResult.EndEdit:
                        return Commands.Execute(ControllerCommandTypes.EndEdit);
                    case ControllerAskExitResult.CancelEdit:
                        return Commands.Execute(ControllerCommandTypes.CancelEdit);
                    case ControllerAskExitResult.JustExit:
                        return true;
                    case ControllerAskExitResult.CancelExit:
                        return false;
                }
            }
            else
            {
                switch (args.result)
                {
                    case ControllerAskExitResult.Save:
                        return Commands.Execute(ControllerCommandTypes.BatchAcceptChanges);
                    case ControllerAskExitResult.Reject:
                        return Commands.Execute(ControllerCommandTypes.BatchRejectChanges);
                    case ControllerAskExitResult.JustExit:
                        return true;
                    case ControllerAskExitResult.CancelExit:
                        return false;
                }
            }

            return true;
        }

        protected void DisplayInfo(System.Exception Exception)
        {
            OnShowInfo(new ControllerShowInfoEventArgs(Exception));
        }

        protected void DisplayInfo(string Message)
        {
            DisplayInfo(Message);
        }

        #region Commands

        protected bool CurrentEndEdit()
        {
            try
            {
                if (!this.ItemValidate()) return false;

                XtraBinding.CurrentEndEdit();
            }
            catch (System.Data.ConstraintException e)
            {
                DisplayInfo(e);

                return false;
            }

            this.RefreshState();

            return true;
        }

        protected bool CurrentCancelEdit()
        {
            bool isNew = XtraBinding.CurrentIsNew;

            this.SuspendFieldChanged = true;

            XtraBinding.CurrentCancelEdit();

            if (isNew && !_itemMoving)
            {
                /*
                XtraBinding.CurrentEndEdit();

                XtraBinding.CurrentRejectChanges();
                 */

                this.RefreshState();

                this.ItemClose(false);
            }
            else
            {
                
                //XtraBinding.CurrentCancelEdit();

                this.CalculatorReset();

                this.SuspendFieldChanged = false;

                this.RefreshState();
            }

            return true;
        }

        protected virtual bool CurrentAcceptChanges()
        {


            SuspendFieldChanged = true;

            try
            {
                XtraBinding.CurrentEndEdit();
                XtraBinding.CurrentAcceptChanges();

            }
            catch (System.Exception ex)
            {
                DisplayInfo(ex);

                return false;
            }
            finally
            {
                SuspendFieldChanged = false;
            }

            this.RefreshState();

            return true;
        }

        protected virtual bool CurrentRejectChanges()
        {
            SuspendFieldChanged = true;

            bool isNew = XtraBinding.CurrentIsAdded || XtraBinding.CurrentIsNew;

            XtraBinding.CurrentEndEdit();
            XtraBinding.CurrentRejectChanges();

            if (isNew && !_itemMoving)
            {
                this.RefreshState();

                this.ItemClose(false);
            }
            else
            {
                SuspendFieldChanged = false;

                this.ChildrenResetBindings();

                this.CalculatorReset();

                this.RefreshState();
            }

            return true;
        }

        protected bool CurrentMoveNext()
        {
            if (CurrentIsCanMoveNext)
            {
                _itemMoving = true;

                if (ValidateExit(true))
                {
                    SuspendFieldChanged = true;

                    ItemUnload();

                    this.XtraBinding.CurrentMoveNext();

                    ItemLoad();

                    this.RefreshState();

                    SuspendFieldChanged = false;

                    _itemMoving = false;

                    return true;
                }

                _itemMoving = false;
            }

            return false;
        }

        protected bool CurrentMovePerv()
        {
            if (this.CurrentIsCanMovePerv)
            {
                _itemMoving = true;

                if (ValidateExit(true))
                {
                    SuspendFieldChanged = true;

                    ItemUnload();

                    this.XtraBinding.CurrentMovePerv();

                    ItemLoad();

                    this.RefreshState();

                    SuspendFieldChanged = false;

                    _itemMoving = false;

                    return true;
                }

                _itemMoving = false;
            }

            return false;

        }

        protected bool CurrentClone()
        {
            if (ValidateExit(true))
            {
                SuspendFieldChanged = true;

                ItemUnload();

                this.XtraBinding.CurrentClone();

                if (!this.TableContext.IsCanSaveInCache)
                    this.CurrentEndEdit();

                SuspendFieldChanged = false;

                this.RefreshState();

                return true;
            }

            return false;
        }

        private bool Add()
        {
            bool uiFilterWasActive = SetListUIFilterActive(false);
            this.ResetBindings();

            bool f = OnAdd();

            if (f)
            {
                CurrentEndEdit();

                f = ItemShow();
            }

            if (uiFilterWasActive)
                SetListUIFilterActive(true);

            return f;
        }

        protected virtual bool OnAdd()
        {
            SuspendFieldChanged = true;

            ItemUnload();

            XtraBinding.AddNew();

            this.RefreshState();

            SuspendFieldChanged = false;

            return true;
        }

        protected bool Remove()
        {

            if (!this.TableContext.IsCanSaveInCache)
            {
                //Ask confirmation
                if (!Confirm("Запись будет удалена! Продолжить?"))
                {
                    return false;
                }
            }

            //SuspendFieldChanged = true;

            XtraBinding.CurrentRemove();

            bool f = false;

            if (!this.TableContext.IsCanSaveInCache)
            {
                int pos = this.XtraBinding.BindingSource.Position;

                f = this.BatchAcceptChanges();

                if (!f)
                {
                    this.BatchRejectChanges();

                    this.XtraBinding.BindingSource.Position = pos;
                }
            }
            else
            {
                OnStateChanged();

                this.RefreshState();

                f = true;
            }

            //SuspendFieldChanged = false;

            return f;
        }

        protected bool BatchAcceptChanges()
        {
            try
            {
                XtraBinding.UpdateBatch();
            }
            catch (System.Exception ex)
            {
                DisplayInfo(ex);

                return false;
            }

            this.RefreshState();

            return true;
        }

        protected bool BatchRejectChanges()
        {
            XtraBinding.RejectBatch();

            this.RefreshState();

            return true;
        }

        #endregion

        public void UpdateStates()
        {
            this.RefereshAvailableStates(true);

            this.RefreshState(true);

            foreach (Controller child in _childrenControllers)
                child.UpdateStates();
        }

        #region Enabled commands

        protected void RefreshState()
        {
            RefreshState(false);
        }

        private void RefreshState(bool ForceRefershCommandItems)
        {
            ControllerStates orig = this.State;

            _state = ControllerStates.None;

            if (CurrentIsCanShow)
                _state |= ControllerStates.ItemCanShow;

            if (ListIsCanShow)
                _state |= ControllerStates.ListCanShow;

            if (CurrentIsCanEdit)
                _state |= ControllerStates.CanEdit;

            if (CurrentIsCanCancelEdit)
                _state |= ControllerStates.CanCancelEdit;

            if (CurrentIsCanEndEdit)
                _state |= ControllerStates.CanEndEdit;

            if (CurrentIsCanAcceptChanges)
                _state |= ControllerStates.CanAcceptChanges;

            if (CurrentIsCanRejectChanges)
                _state |= ControllerStates.CanRejectChanges;

            if (CurrentIsCanClone)
                _state |= ControllerStates.CanClone;

            if (CurrentIsCanMoveNext)
                _state |= ControllerStates.CanMoveNext;

            if (CurrentIsCanMovePerv)
                _state |= ControllerStates.CanMovePerv;

            if (BatchIsCanAcceptChanges)
                _state |= ControllerStates.CanBatchAcceptChanges;

            if (BatchIsCanRejectChanges)
                _state |= ControllerStates.CanBatchRejectChanges;

            if (IsCanAdd)
                _state |= ControllerStates.CanAdd;

            if (IsCanRemove)
                _state |= ControllerStates.CanRemove;

            if (!BatchIsModifyed)
                _state |= ControllerStates.CanFill;

            if (FillState == FillStates.Filling)
                _state |= ControllerStates.CanCancelFill;


            if (orig != State || ForceRefershCommandItems)
            {
                Commands.RefreshEnabledDefaultCommands();

                OnStateChanged();
            }
        }

        private bool CurrentIsCanShow
        {
            get
            {
                return IsCommandAvailable(ControllerStates.ItemCanShow) && this.XtraBinding.Current != null;
            }
        }

        private bool ListIsCanShow
        {
            get
            {
                return IsCommandAvailable(ControllerStates.ListCanShow);
            }
        }

        private bool CurrentIsCanEdit
        {
            get
            {
                return IsCommandAvailable(ControllerStates.CanEdit);
            }
        }

        private bool CurrentIsCanEndEdit
        {
            get
            {
                return IsCommandAvailable(ControllerStates.CanEndEdit)
                    && CurrentIsEdit;
            }
        }

        private bool CurrentIsCanCancelEdit
        {
            get
            {
                return IsCommandAvailable(ControllerStates.CanCancelEdit)
                && CurrentIsEdit;
            }
        }

        private bool CurrentIsCanAcceptChanges
        {
            get
            {
                return IsCommandAvailable(ControllerStates.CanAcceptChanges)
                && CurrentIsModifyed;
            }
        }

        private bool CurrentIsCanRejectChanges
        {
            get
            {
                return IsCommandAvailable(ControllerStates.CanRejectChanges)
                && CurrentIsModifyed;
            }
        }

        protected virtual bool CurrentIsCanClone
        {
            get
            {
                return IsCommandAvailable(ControllerStates.CanClone)
                    && !CurrentIsEdit && !CurrentIsAdded;
            }
        }

        protected virtual bool CurrentIsCanMoveNext
        {
            get
            {
                return IsCommandAvailable(ControllerStates.CanMoveNext)
                    && XtraBinding.CurrentIsCanMoveNext;
            }
        }

        protected virtual bool CurrentIsCanMovePerv
        {
            get
            {
                return IsCommandAvailable(ControllerStates.CanMovePerv)
                    && XtraBinding.CurrentIsCanMovePerv;
            }
        }

        private bool BatchIsCanAcceptChanges
        {
            get
            {
                return IsCommandAvailable(ControllerStates.CanBatchAcceptChanges)
                && BatchIsModifyed;
            }
        }

        private bool BatchIsCanRejectChanges
        {
            get
            {
                return IsCommandAvailable(ControllerStates.CanBatchRejectChanges)
                && BatchIsModifyed;
            }
        }

        private bool IsCanAdd
        {
            get { return IsCommandAvailable(ControllerStates.CanAdd); }
        }

        private bool IsCanRemove
        {
            get { return IsCommandAvailable(ControllerStates.CanRemove); }
        }

        private DataAvail.XtraBindings.ObjectState CurrentObjectState
        {
            get { return XtraBinding.CurrentObjectState; }
        }

        private bool CurrentIsEdit
        {
            get
            {
                return CurrentObjectState == ObjectState.Edit ||
                    CurrentObjectState == ObjectState.EditModifyed ||
                    CurrentObjectState == ObjectState.New;
            }
        }

        private bool CurrentIsModifyed
        {
            get
            {
                return CurrentObjectState == ObjectState.EditModifyed ||
                    CurrentObjectState == ObjectState.Modifyed;
            }
        }

        private bool CurrentIsAdded
        {
            get
            {
                return XtraBinding.Current != null && (XtraBinding.CurrentIsAdded || XtraBinding.CurrentIsNew);
            }
        }


        private bool CurrentIsChanged
        {
            get
            {
                return CurrentIsEdit || CurrentIsModifyed;
            }
        }

        private bool BatchIsModifyed
        {
            get { return XtraBinding.IsBatchModifyed(); }

        }

        #endregion

        #region Available commands

        private bool IsCommandAvailable(ControllerStates CommandType)
        {
            return (CommandType & AvailableState) == CommandType;
        }

        private void RefereshAvailableStates()
        {
            RefereshAvailableStates(false);
        }

        private void RefereshAvailableStates(bool ForceRefershCommandItems)
        {
            ControllerStates orig = AvailableState;

            _availableStates = ControllerStates.CanFill;

            if (CurrentIsShowAvailable)
                _availableStates |= ControllerStates.ItemCanShow;

            if (ListIsShowAvailable)
                _availableStates |= ControllerStates.ListCanShow;

            if (CurrentIsEditAvailable)
                _availableStates |= ControllerStates.CanEdit;

            if (CurrentIsEndEditAvailable)
            {
                _availableStates |= ControllerStates.CanEndEdit;
                _availableStates |= ControllerStates.CanCancelEdit;
            }

            if (CurrentIsModifyAvailable)
            {
                _availableStates |= ControllerStates.CanAcceptChanges;
                _availableStates |= ControllerStates.CanRejectChanges;
            }

            if (CurrentIsMoveAvailable)
            {
                _availableStates |= ControllerStates.CanMoveNext;
                _availableStates |= ControllerStates.CanMovePerv;
            }

            if (CurrentIsCloneAvailable)
            {
                _availableStates |= ControllerStates.CanClone;
            }

            if (IsAddAvailable)
            {
                _availableStates |= ControllerStates.CanAdd;
            }

            if (IsRemoveAvailable)
                _availableStates |= ControllerStates.CanRemove;

            if (BatchIsModifyAvailabe)
            {
                _availableStates |= ControllerStates.CanBatchAcceptChanges;
                _availableStates |= ControllerStates.CanBatchRejectChanges;
            }

            if (orig != AvailableState || ForceRefershCommandItems)
                Commands.RefreshAvailableDeafultCommands();
        }

        private bool CurrentIsShowAvailable
        {
            get
            {
                return TableContext.IsCanView;
            }
        }

        private bool ListIsShowAvailable
        {
            get
            {
                return TableContext.IsCanView;
            }
        }


        private bool CurrentIsEditAvailable
        {
            get
            {
                return TableContext.IsCanEdit;
            }
        }

        private bool CurrentIsEndEditAvailable
        {
            get
            {
                return CurrentIsEditAvailable && TableContext.IsCanSaveInCache && XtraBinding.IsCanEdit;
            }
        }

        private bool CurrentIsModifyAvailable
        {
            get
            {
                return CurrentIsEditAvailable && TableContext.IsCanSaveInStorage && XtraBinding.IsCanModify;
            }
        }

        private bool CurrentIsMoveAvailable
        {
            get
            {
                return TableContext.IsCanMove;
            }
        }

        private bool CurrentIsCloneAvailable
        {
            get
            {
                return TableContext.IsCanClone;
            }
        }

        private bool IsAddAvailable
        {
            get
            {
                return TableContext.IsCanAdd && XtraBinding.IsCanModify; ;
            }
        }

        private bool IsRemoveAvailable
        {
            get
            {
                return TableContext.IsCanRemove && XtraBinding.IsCanModify; ;
            }
        }

        private bool BatchIsModifyAvailabe
        {
            get
            {
                return CurrentIsModifyAvailable &&
                    XtraBinding.IsCanBatchUpdate &&
                    TableContext.IsCanSaveInCache;
            }
        }

        #endregion

        #region Child -> Parent communication

        private event System.EventHandler StateChanged;

        private void OnStateChanged()
        {
            if (StateChanged != null)
                StateChanged(this, EventArgs.Empty);
        }

        #endregion

        private enum FillStates
        {
            None,
            Filling,
            Canceling
        }

        private void ResetBindings()
        {
            this.XtraBinding.ResetBindings();
        }

        void controllerUI_UIKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            e.SuppressKeyPress = Commands.HandleKey(e.Modifiers, e.KeyCode, true);
        }

        void controllerUI_UIKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            e.SuppressKeyPress = Commands.HandleKey(e.Modifiers, e.KeyCode, false);
        }

        private void CalculatorReset()
        {
            this.XtraBinding.CalculatorReset(TableContext);
        }

    }
}
