using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.Controllers.Commands;
using DataAvail.XObject;
using DataAvail.XtraBindings;

namespace DataAvail.Controllers
{
    partial class Controller
    {
        public event ControllerFilterExpressionHandler GetFillExpression;

        public event ControllerFilterExpressionHandler ListUIFilter;

        public event ControllerListUIFilterActiveHandler ListUIFilterActive;

        public event ControllerListUIFilterFormatterHandler ListUIFilterFormatter;

        public event ControllerListUIUploadToExcelHandler ListUIUploadToExcel;

        public event System.EventHandler ListUIFocusSearch;

        public event System.EventHandler ListUIFocusList;

        public event System.EventHandler EndFill;

        public event System.EventHandler ListInitializeUI;

        public event System.EventHandler ListShowUI;

        public event System.EventHandler ListCloseUI;

        private IControllerUI _listUI;

        private DataAvail.Data.DbContext.IDbContextWhereFormatter _listUIFilterFormatter;

        public IControllerUI ListUI
        {
            get { return _listUI; }
        }

        private bool ListCreate()
        {
            OnListCreateUI();

            return true;
        }

        private bool ListInitialize()
        {
            OnListInitializeUI();

            //this.RefereshAvailableStates();

            return true;
        }

        private bool ListShow()
        {
            this.TableContext.ActiveType = XOTableContextActiveType.List;

            this.ResetBindings();

            this.UpdateStates();

            OnListShowUI();

            return true;
        }

        private bool ListClose()
        {
            if (ValidateExit(false))
            {
                SetListUIFilterActive(false);

                this.TableContext.ResetActiveType();

                this.UpdateStates();

                OnListCloseUI();

                return true;
            }
            else
            {
                return false;
            }
        }

        private void OnListCreateUI()
        {
            if (uiCreator != null)
            {
                IControllerUI controllerUI = uiCreator.Create(this, false);

                _listUI = controllerUI;

                controllerUI.UIDataBound += new EventHandler(controllerUI_ListUIDataBound);

                controllerUI.UIKeyDown += new System.Windows.Forms.KeyEventHandler(controllerUI_UIKeyDown);

                controllerUI.UIKeyUp +=new System.Windows.Forms.KeyEventHandler(controllerUI_UIKeyUp);
            }
        }

        private void OnListInitializeUI()
        {
            if (ListInitializeUI != null)
                ListInitializeUI(this, EventArgs.Empty);
        }


        private void OnListShowUI()
        {
            if (ListShowUI != null)
                ListShowUI(this, EventArgs.Empty);
        }

        private void OnListCloseUI()
        {
            if (ListCloseUI != null)
                ListCloseUI(this, EventArgs.Empty);
        }

        private ControllerAskExitResult ListGetEnabledExitResults(ref string Reason)
        {
            if (this.FillState != FillStates.None)
            {
                Reason = "Please wait till fill operation stops, or cancel it.";

                return ControllerAskExitResult.None;
            }

            ControllerAskExitResult enabledResults = ControllerAskExitResult.None;

            if (DataAvail.Utils.EnumFlags.IsContain(State, ControllerStates.CanBatchRejectChanges))
            {
                enabledResults |= ControllerAskExitResult.Reject;
            }

            if (DataAvail.Utils.EnumFlags.IsContain(State, ControllerStates.CanBatchAcceptChanges))
            {
                enabledResults |= ControllerAskExitResult.Save;
            }

            return enabledResults;
        }

        private bool ListHandleAskExitResult(ControllerAskExitResult Result)
        {
            switch (Result)
            {
                case ControllerAskExitResult.Save:
                    this.Commands.Execute(ControllerCommandTypes.BatchAcceptChanges);
                    return true;
                case ControllerAskExitResult.Reject:
                    this.Commands.Execute(ControllerCommandTypes.BatchRejectChanges);
                    return true;
                case ControllerAskExitResult.CancelExit:
                    return false;
            }

            return true;
        }

        public void SetControlDataSource(System.Windows.Forms.Control Control)
        {
            Control.GetType().GetProperty("DataSource").SetValue(Control, XtraBinding.BindingSource, null);
        }

        private void SetListUIFilter(string Filter)
        {
            OnLisUIFilter(new ControllerFilterExpressionEventArgs(Filter));
        }

        private bool SetListUIFilterActive(bool IsActive)
        { 
            ControllerListUIFilterActiveEventArgs args = new ControllerListUIFilterActiveEventArgs(IsActive);

            OnListUIFilterActive(args);

            return args.filterActive;
        }

        private DataAvail.Data.DbContext.IDbContextWhereFormatter GetListUIFilterFormatter()
        {
            ControllerListUIFilterFormatterEventArgs args = new ControllerListUIFilterFormatterEventArgs();

            OnListUIFilterFormatter(args);

            return args.filterFormatter;
        }

        #region Format where for context

        private DataAvail.Data.DbContext.IDbContextWhereFormatter ContextWhereFormatter
        {
            get 
            {
                if (TableContext.PersistFill)
                    return _listUIFilterFormatter;
                else
                    return DataAvail.Data.DbContext.DbContext.CurrentContext.WhereFormatter; 
            }
        }

        private string ContextFormattedWhereExpression
        {
            get
            {
                return DataAvail.Data.DbContext.DbContextWhereFormatter.FormatDateTime(ContextWhereFormatter,
                    FillExpression,
                    TableContext.Fields.Where(p => p.FieldType == typeof(System.DateTime)).Select(p => p.Name).ToArray());

            }
        }

        #endregion

        public bool Fill(string ContextFormattedFilter, bool ForcedSync)
        {
            FillState = FillStates.Filling;

            SuspendBindingListEvents();

            XtraBinding.Fill(ContextFormattedFilter, ForcedSync);

            return true;
        }

        #region Commands

        internal bool Fill()
        {
            string fillExpr = ContextFormattedWhereExpression;

            if (TableContext.PersistFill)
            {
                SetListUIFilter(fillExpr);
            }
            else
            {
                return Fill(fillExpr, false);
            }


            return true;
        }

        internal bool CancelFill()
        {
            FillState = FillStates.Canceling;

            XtraBinding.StopFill();

            return true;
        }

        #endregion

        private string FillExpression
        {
            get
            {
                ControllerFilterExpressionEventArgs args = new ControllerFilterExpressionEventArgs(null);

                OnGetFillExpression(args);

                return args.filterExpression;
            }
        }

        protected virtual void OnGetFillExpression(ControllerFilterExpressionEventArgs args)
        {
            if (GetFillExpression != null)
                GetFillExpression(this, args);
        }

        protected virtual void OnLisUIFilter(ControllerFilterExpressionEventArgs args)
        {
            if (ListUIFilter != null)
                ListUIFilter(this, args);
        }

        protected virtual void OnListUIFilterActive(ControllerListUIFilterActiveEventArgs args)
        {
            if (ListUIFilterActive != null)
            {
                ListUIFilterActive(this, args);
            }
        }

        protected virtual void OnListUIFilterFormatter(ControllerListUIFilterFormatterEventArgs args)
        {
            if (ListUIFilterFormatter != null)
            {
                ListUIFilterFormatter(this, args);
            }
        }


        void xtraBindingOperation_EndFill(object sender, XtraBindingOperationEndFillEventArgs args)
        {
            if (synchronizeInvoke != null && synchronizeInvoke.InvokeRequired)
            {
                synchronizeInvoke.Invoke(new XtraBindingOperationEndFillHandler(xtraBindingOperation_EndFill), new object[] { sender, args });
            }
            else
            {
                ResumeBindingListEvents();

                this.XtraBinding.EndFillSafely();

                FillState = FillStates.None;

                this.XtraBinding.ResetBindings();

                if (args.exception != null)
                    DisplayInfo(args.exception);

                this.RefreshState();

                OnEndFill();
            }
        }

        protected virtual void OnEndFill()
        {
            if (EndFill != null)
                EndFill(this, EventArgs.Empty);
        }

        void controllerUI_ListUIDataBound(object sender, EventArgs e)
        {
            OnListUIDataBound();
        }

        protected virtual void OnListUIDataBound()
        {
            System.Diagnostics.Debug.WriteLine("List ui loaded " + this.TableContext.Caption);

            if (this.TableContext.IsFkSelectItemContext//this.TableContext.Context is DataAvail.XOP.AppContext.SelectFkItemContext
                && this.XtraBinding.ItemsCount > 0)
            {
                OnListUIFocusList();
            }
            else
            {
                OnListUIFocusSearch();
            }

            this.ResetBindings();

            _listUIFilterFormatter = GetListUIFilterFormatter();
        }

        public object DataSourceCloneAndFill(string Filter)
        {
            return XtraBinding.DataSourceCloneAndFill(Filter);
        }

        public IEnumerable<IDictionary<string, object>> GetItemsFromDataAdapter(string Filter)
        {
            return XtraBinding.GetItemsFromDataAdapter(Filter);
        }

        public bool UploadToExcel()
        {
            string fileName = string.Format(@"{0}/{1}.xls", properties.TempFolder, System.Guid.NewGuid());

            OnListUIUploadToExcel(fileName);

            System.Diagnostics.Process.Start(fileName);

            return true;
        }

        public bool Refill()
        {
            string fillExpr = null;

            if (!TableContext.PersistFill)
            {
                fillExpr = ContextFormattedWhereExpression;
            }

            this.Fill(fillExpr, false);

            return true;
        }

        public bool FocusSearch()
        {
            //Is hidden???
            OnListUIFocusSearch();

            return true;
        }

        public bool FocusList()
        {
            if (this.XtraBinding.ItemsCount > 0)
            {
                OnListUIFocusList();

                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual void OnListUIFocusSearch()
        {
            if (ListUIFocusSearch != null)
                ListUIFocusSearch(this, EventArgs.Empty);
        }

        protected virtual void OnListUIFocusList()
        {
            if (ListUIFocusList != null)
                ListUIFocusList(this, EventArgs.Empty);        
        }

        protected virtual void OnListUIUploadToExcel(string FileName)
        {
            if (ListUIUploadToExcel != null)
                ListUIUploadToExcel(this, new ControllerListUIUploadToExcelEventArgs(FileName));
        }

        private void SuspendBindingListEvents()
        {
            foreach (var r in Controller.controllerContext.Controllers.Where(p => p.TableContext.Name == this.TableContext.Name))
            {
                r.XtraBinding.BindingSource.RaiseListChangedEvents = false;
            }
        }

        private void ResumeBindingListEvents()
        {
            foreach (var r in Controller.controllerContext.Controllers.Where(p => p.TableContext.Name == this.TableContext.Name))
            {
                r.XtraBinding.BindingSource.RaiseListChangedEvents = true;
            }        
        }
    }

    public class ControllerListUIUploadToExcelEventArgs : System.EventArgs
    { 
        internal ControllerListUIUploadToExcelEventArgs(string FileName)
        {
            fileName = FileName;
        }

        public readonly string fileName;
    }

    public delegate void ControllerListUIUploadToExcelHandler(object sender, ControllerListUIUploadToExcelEventArgs args);

    public delegate void ControllerFilterExpressionHandler(object sender, ControllerFilterExpressionEventArgs args);

    public class ControllerFilterExpressionEventArgs : System.EventArgs
    {
        public ControllerFilterExpressionEventArgs(string FilterExpression)
        {
            this.filterExpression = FilterExpression;
        }

        public string filterExpression;
    }

    public delegate void ControllerListUIFilterActiveHandler(object sender, ControllerListUIFilterActiveEventArgs args);

    public class ControllerListUIFilterActiveEventArgs : System.EventArgs
    {
        public ControllerListUIFilterActiveEventArgs(bool FilterActive)
        {
            this.filterActive = FilterActive;
        }

        public bool filterActive;
    }

    public delegate void ControllerListUIFilterFormatterHandler(object sender, ControllerListUIFilterFormatterEventArgs args);

    public class ControllerListUIFilterFormatterEventArgs : System.EventArgs
    {
        public ControllerListUIFilterFormatterEventArgs()
        { }

        public DataAvail.Data.DbContext.IDbContextWhereFormatter filterFormatter;
    }

}
