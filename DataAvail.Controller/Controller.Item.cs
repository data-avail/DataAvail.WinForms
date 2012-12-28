using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;
using DataAvail.Utils;
using DataAvail.Controllers.Binding;
using DataAvail.XtraBindings;
using System.ComponentModel;

namespace DataAvail.Controllers
{
    partial class Controller
    {
        public event System.EventHandler ItemInitializeUI;

        public event System.EventHandler ItemShowUI;

        public event System.EventHandler ItemCloseUI;

        private IControllerUI _itemUI;

        private bool _suspendFieldChanged = false;

        private List<Controller> _childrenControllers = new List<Controller>();

        private bool SuspendFieldChanged
        {
            get { return _suspendFieldChanged; }

            set 
            {
                if (SuspendFieldChanged != value)
                {
                    

                    if (!value)
                    {
                        XtraBinding.ResumeControlsBinding();

                        if (_itemUI is ISupportInitialize)
                            ((ISupportInitialize)_itemUI).EndInit();

                    }
                    else
                    {
                        if (_itemUI is ISupportInitialize)
                            ((ISupportInitialize)_itemUI).BeginInit();

                        XtraBinding.SuspendControlsBinding();
                    }

                    _suspendFieldChanged = value;
                 
                }
            }
        }

        public IControllerUI ItemUI
        {
            get { return _itemUI; }
        }

        private bool ItemCreate()
        {
            OnItemCreateUI();

            return true;
        }

        private bool ItemInitialize()
        {
            OnItemInitializeUI();

            //3this.RefereshAvailableStates();

            //this.ChildrenRefreshAvailableStatesChildren();

            return true;
        }

        protected virtual bool ItemShow(DataAvail.Controllers.Commands.IControllerCommandItem CommandItem)
        {
            return ItemShow();        
        }

        protected virtual bool ItemSelect(DataAvail.Controllers.Commands.IControllerCommandItem CommandItem)
        {
            return ItemShow();
        }

        private bool ItemShow()
        {
            TableContext.ActiveType = XOTableContextActiveType.Item;

            SuspendFieldChanged = true;

            ItemLoad();

            this.CalculatorReset();

            XtraBinding.CalculateInitialize();

            UpdateStates();

            bool uiFilterWasActive = SetListUIFilterActive(false);

            OnItemShowUI();

            if (uiFilterWasActive)
                SetListUIFilterActive(true);

            return true;
        }

        private bool ItemClose()
        {
            TableContext.ResetActiveType();

            this.UpdateStates();

            return ItemClose(true);
        }

        private bool ItemClose(bool Validate)
        {
            if (!Validate || ValidateExit(true))
            {
                this.SuspendFieldChanged = true;

                this.CalculatorReset();

                ItemUnload();

                OnItemCloseUI();

                return true;
            }
            else
            {
                return false;
            }
        }

        private void OnItemCreateUI()
        {
            if (uiCreator != null)
            {
                IControllerUI controllerUI = uiCreator.Create(this, true);

                _itemUI = controllerUI;

                controllerUI.UIDataBound +=new EventHandler(controllerUI_DataBound);

                controllerUI.UIKeyDown += new System.Windows.Forms.KeyEventHandler(controllerUI_UIKeyDown);

                controllerUI.UIKeyUp += new System.Windows.Forms.KeyEventHandler(controllerUI_UIKeyUp);

            }

        }

        private void OnItemInitializeUI()
        {
            if (ItemInitializeUI != null)
                ItemInitializeUI(this, EventArgs.Empty);
        }


        private void OnItemShowUI()
        {
            if (ItemShowUI != null)
                ItemShowUI(this, EventArgs.Empty);
        }

        private void OnItemCloseUI()
        {
            if (ItemCloseUI != null)
                ItemCloseUI(this, EventArgs.Empty);
        }

        private void ItemLoad()
        {
            if (XtraBinding is DataAvail.XtraBindings.XtraBindingContainer)
                ((DataAvail.XtraBindings.XtraBindingContainer)XtraBinding).FillChildren();

            foreach (Controller childController in _childrenControllers)
                childController.ItemLoad();
        }

        private void ItemUnload()
        {
            foreach (Controller childController in _childrenControllers)
                childController.ItemUnload();

            if (XtraBinding is DataAvail.XtraBindings.XtraBindingContainer)
                ((DataAvail.XtraBindings.XtraBindingContainer)XtraBinding).ClearChildren();
        }

        private void ChildrenResetBindings()
        {
            foreach (Controller childController in _childrenControllers)
            {
                childController.ResetBindings();
            }
        }

        public void InvokeFieldChanged(string FieldName)
        {
            if (!SuspendFieldChanged)
            {
                System.Diagnostics.Debug.WriteLine(FieldName);

                CalculateField(FieldName);

                if (!this.TableContext.IsCanSaveInCache)
                {
                    this.CurrentEndEdit();
                }
                else
                {
                    this.RefreshState();
                }
            }
        }

        private void CalculateField(string FieldName)
        {
            this.SuspendFieldChanged = true;

            XtraBinding.CalculateField(FieldName);

            this.SuspendFieldChanged = false;
        }

        public Binding.ControllerDataSource GetControllerDataSource(string FieldName)
        {
            return DataSources.GetDataSourceProperty(this.TableContext.Fields.Single(p => p.Name == FieldName)); 
        }

        public IBindingListView GetParentRelationDataSource(XORelation ParentRelation)
        {
            object dataSource = Controller.controllerContext.GetDataSource(ParentRelation.ParentTable);

            if (dataSource == null)
                throw new Exception("Uh huh data source fro parent table not found. Please confirm it is defined in model file");

            IBindingListView listView = XtraBinding.ItemAdapter.GetBindingListView(dataSource);

            if (!string.IsNullOrEmpty(ParentRelation.Filter))
            {
                listView.Filter = ParentRelation.Filter;
            }

            return listView;
        }


        public Controller GetChildController(string ParentFieldName, string ChildObjectName, string ChildFieldName)
        {
            if (XtraBinding is XtraBindingContainer)
            {
                XtraBindingChild xtraBindingChild = ((XtraBindingContainer)XtraBinding).Children
                    .FirstOrDefault(p => p.ChildProperties.parentFieldName == ParentFieldName && 
                        p.ChildProperties.fkFieldName == ChildFieldName && 
                        p.ChildProperties.childObjectName == ChildObjectName);

                if (xtraBindingChild != null)
                {
                    XOTableContext itemContext =
                        TableContext.XOTable.ChildrenRelations.First(p => p.ParentField.Name == ParentFieldName &&
                            p.ChildTable.Name == ChildObjectName &&
                            p.ChildField.Name == ChildFieldName).ChildTable.CreateTableChildContext(TableContext.Fields.First(p => p.Name == ParentFieldName));

                    Controller childController = new Controller(xtraBindingChild, itemContext);

                    childController.StateChanged += new EventHandler(childController_StateChanged);

                    _childrenControllers.Add(childController);

                    return childController;
                }
            }

            return null;
        }

        void childController_StateChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Child state changed");

            if (XtraBinding.Current != null && XtraBinding.CurrentIsNew && XtraBinding.CurrentIsModifyed)
            {
                this.CurrentEndEdit();
            }

            this.RefreshState();            
        }

        private void RefreshAvailableStatesChildren()
        {
            foreach (Controller childController in _childrenControllers)
            {
                childController.RefereshAvailableStates(true);
            }
        }

        void DynamicContext_TableContextChanged(object sender, EventArgs e)
        {
            foreach (Controller childController in _childrenControllers)
            {
                XOTableContext tableContext = childController.DynamicContext.CurrentTableContext;

                ((XObject.XContexts.XChildContext)tableContext.Context).ParentFieldContext = this.TableContext.Fields.Single(p => p.Name == ((XObject.XContexts.XChildContext)tableContext.Context).ParentFieldContext.Name);
            }
        }

        void controllerUI_DataBound(object sender, EventArgs e)
        {
            OnItemUIDataBound();
        }

        protected virtual void OnItemUIDataBound()
        {
            System.Diagnostics.Debug.WriteLine("Item ui data bound " + this.TableContext.Caption);

            if (!this.XtraBinding.CurrentIsNew)
            {
                this.XtraBinding.CurrentCancelEdit();
            }
            else
            {
                if (!this.TableContext.IsCanSaveInCache)
                    this.XtraBinding.CurrentEndEdit();
            }

            this.RefreshState();

            RefreshAvailableStatesChildren();

            this.UpdateBindings();

            SuspendFieldChanged = false;
            
        }

        void XtraBinding_CalulatorCalculated(object sender, EventArgs e)
        {
            this.UpdateBindings();
        }

        private void UpdateBindings()
        {
            DataSources.UpdateProperties(XtraBinding.ObjectProperties);
        }
    }

    public class ControllerItemUISetReadOnlyEventArgs : System.EventArgs
    {
        internal ControllerItemUISetReadOnlyEventArgs(bool ReadOnly)
        {
            readOnly = ReadOnly;
        }

        public readonly bool readOnly;
    }


    public delegate void ControllerItemUISetReadOnlyHandler(object sender, ControllerItemUISetReadOnlyEventArgs args);
}
