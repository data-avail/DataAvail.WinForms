using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Linq;
using DataAvail.XObject;
using DataAvail.XObject.XWP;

namespace DataAvail.AppShell
{
    public partial class MainFrame : DataAvail.XtraForm.XtraForm
    {
        public MainFrame()
        {
            InitializeComponent();
        }

        public void Initialize(DataAvailable.DACProperties DACProperties)
        {
            /*nimpl
            DataAvail.Data.DataSet.Log = new DebugLog();

            DataAvail.XtraBinding.Controllers.Controller.synchronizeInvoke = this;
            DataAvail.XtraBinding.Controllers.Controller.uiCreator = new ControllerUICreator(this);

            DataAvail.XtraBinding.Controllers.Controller.AskExit += new DataAvail.XtraBinding.Controllers.ControllerAskExitHandler(Controller_AskExit);
            DataAvail.XtraBinding.Controllers.Controller.ShowInfo += new DataAvail.XtraBinding.Controllers.ControllerShowInfo(Controller_ShowInfo);
            DataAvail.XtraBinding.Controllers.Controller.AskConfirmation += new DataAvail.XtraBinding.Controllers.ControllerAskConfirmationHandler(Controller_AskConfirmation);

            AppShellInitializer.AppShellInitializerResult result = AppShellInitializer.Initialize(DACProperties);

            _appContext = result.appContext;

            _controllers = result.controllers;

            BuildMenu();        
             */
        }

     
        private XOApplication _appContext;

        private IEnumerable<DataAvail.XtraBinding.Controllers.Controller> _controllers;

        private Dictionary<string, DevExpress.XtraBars.BarButtonItem> _menuButtons = new Dictionary<string, DevExpress.XtraBars.BarButtonItem>();

        private XOApplication AppContext
        {
            get { return _appContext; }
        }

        private IEnumerable<DataAvail.XtraBinding.Controllers.Controller> Controllers
        {
            get { return _controllers; }
        }

        protected override void OnFirstLoad()
        {
            base.OnFirstLoad();

            this.Text = AppContext.XwpApplication.Name;
        }

        protected override string SerializationName
        {
            get
            {
                return "MainFrame";
            }
        }

        #region Build Menu

        private void BuildMenu()
        {
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();

            foreach(XWPMenuItem menuItem in AppContext.XwpApplication.AppView.MenuItems.MenuItems) 
                BuildMenuItem(null, menuItem);

            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
        }

        private void BuildMenuItem(DevExpress.XtraBars.BarSubItem ParentBarSubItem, XWPMenuItem MenuItem)
        {
            if (MenuItem.Children.Length > 0)
            {
                DevExpress.XtraBars.BarSubItem barSubItem = AddMenuSubItem(ParentBarSubItem, MenuItem);

                foreach (XWPMenuItem menuItem in MenuItem.Children)
                    BuildMenuItem(barSubItem, menuItem);
            }
            else
            {
                _menuButtons.Add(MenuItem.TableName, AddMenuButtonItem(ParentBarSubItem, MenuItem));
            }
        }

        private DevExpress.XtraBars.BarSubItem AddMenuSubItem(DevExpress.XtraBars.BarSubItem ParentBarSubItem, XWPMenuItem MenuItem)
        {
            DevExpress.XtraBars.BarSubItem barSubItem = new DevExpress.XtraBars.BarSubItem();

            barSubItem.Caption = MenuItem.Caption;

            //barSubItem.Hint = MenuItem.Hint;

            barSubItem.Name = "barSubItem" + MenuItem.Caption;

            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {barSubItem});

            if (ParentBarSubItem != null)
            {
                ParentBarSubItem.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
                   new DevExpress.XtraBars.LinkPersistInfo(barSubItem)});
            }
            else
            {
                this.barManager1.MainMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
                    new DevExpress.XtraBars.LinkPersistInfo(barSubItem)});
            }

            return barSubItem;
        }

        private static Keys GetKeys(XWPMenuItem XWPMenuItem)
        {
            /*nimpl
            object k = Enum.Parse(typeof(Keys), ModelViewAppKey.key, true);

            if (k != null)
            {
                Keys key = (Keys)k;

                foreach (string str in ModelViewAppKey.modifyers)
                {
                    switch (str.ToUpper())
                    {
                        case "ALT":
                            key |= Keys.Alt;
                            break;
                        case "CTL":
                            key |= Keys.Control;
                            break;
                        case "SHIFT":
                            key |= Keys.Shift;
                            break;
                    }
                }

                return key;
            }
            else
            {
                return Keys.None;
            }
             */

            return Keys.None;
        }

        private DevExpress.XtraBars.BarButtonItem AddMenuButtonItem(DevExpress.XtraBars.BarSubItem ParentBarSubItem, XWPMenuItem MenuItem)
        {
            DevExpress.XtraBars.BarButtonItem barButtonItem = new DevExpress.XtraBars.BarButtonItem();

            barButtonItem.Caption = MenuItem.Caption;

        //    barButtonItem.Hint = MenuItem.Hint;

            barButtonItem.Name = "barSubItem" + MenuItem.Caption;

            /*
            if (MenuItem.Key != null)
            {
                Keys key = GetKeys(MenuItem.Key);

                if (key != Keys.None)
                    barButtonItem.ItemShortcut = new DevExpress.XtraBars.BarShortcut(key);
            }
             */

            this.barManager1.Items.Add(barButtonItem);

            if (ParentBarSubItem != null)
            {
                ParentBarSubItem.LinksPersistInfo.Add(new DevExpress.XtraBars.LinkPersistInfo(barButtonItem));
            }
            else
            {
                this.barManager1.MainMenu.LinksPersistInfo.Add(new DevExpress.XtraBars.LinkPersistInfo(barButtonItem));
            }

            DataAvail.XtraBinding.Controllers.Controller appItemController = Controllers.FirstOrDefault(p => p.TableContext.Name == MenuItem.TableName);

            if (appItemController != null)
            {
                appItemController.Commands.AddCommandItem(new DXBarButtonCommandItem(DataAvail.XtraBinding.Controllers.Commands.ControllerCommandTypes.ListShow, barButtonItem));

                appItemController.UpdateStates();
            }

            return barButtonItem;
        }

        #endregion

        #region Controller event

        void Controller_AskConfirmation(object sender, DataAvail.XtraBinding.Controllers.ControllerAskConfirmationEventArgs args)
        {
            args.confirm = System.Windows.Forms.MessageBox.Show(args.confirmationString, "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes;
        }

        void Controller_AskExit(object sender, DataAvail.XtraBinding.Controllers.ControllerAskExitEventArgs args)
        {
            args.result = GetXtraBindingControllerAskExitResult(DataAvail.WinUtils.AskExitForm.Ask(GetAskExitFormResult(args.enabledResults)));
        }

        void Controller_ShowInfo(object sender, DataAvail.XtraBinding.Controllers.ControllerShowInfoEventArgs args)
        {
            DataAvail.WinUtils.InfoForm.InfoForm.Show("Error", args.ToString(), DataAvail.WinUtils.InfoForm.InfoFormSendReportType.Default);
        }

        private DataAvail.WinUtils.AskExitFormResult GetAskExitFormResult(DataAvail.XtraBinding.Controllers.ControllerAskExitResult Result)
        {
            DataAvail.WinUtils.AskExitFormResult res = DataAvail.WinUtils.AskExitFormResult.None;

            if ((Result & DataAvail.XtraBinding.Controllers.ControllerAskExitResult.Save) == DataAvail.XtraBinding.Controllers.ControllerAskExitResult.Save)
                res |= DataAvail.WinUtils.AskExitFormResult.Save;

            if ((Result & DataAvail.XtraBinding.Controllers.ControllerAskExitResult.Reject) == DataAvail.XtraBinding.Controllers.ControllerAskExitResult.Reject)
                res |= DataAvail.WinUtils.AskExitFormResult.Reject;

            if ((Result & DataAvail.XtraBinding.Controllers.ControllerAskExitResult.EndEdit) == DataAvail.XtraBinding.Controllers.ControllerAskExitResult.EndEdit)
                res |= DataAvail.WinUtils.AskExitFormResult.EndEdit;

            if ((Result & DataAvail.XtraBinding.Controllers.ControllerAskExitResult.CancelEdit) == DataAvail.XtraBinding.Controllers.ControllerAskExitResult.CancelEdit)
                res |= DataAvail.WinUtils.AskExitFormResult.CancelEdit;

            if ((Result & DataAvail.XtraBinding.Controllers.ControllerAskExitResult.CancelExit) == DataAvail.XtraBinding.Controllers.ControllerAskExitResult.CancelExit)
                res |= DataAvail.WinUtils.AskExitFormResult.CancelExit;

            if ((Result & DataAvail.XtraBinding.Controllers.ControllerAskExitResult.JustExit) == DataAvail.XtraBinding.Controllers.ControllerAskExitResult.JustExit)
                res |= DataAvail.WinUtils.AskExitFormResult.JustExit;

            return res;
        }

        private DataAvail.XtraBinding.Controllers.ControllerAskExitResult GetXtraBindingControllerAskExitResult(DataAvail.WinUtils.AskExitFormResult Result)
        {
            DataAvail.XtraBinding.Controllers.ControllerAskExitResult res = DataAvail.XtraBinding.Controllers.ControllerAskExitResult.None;

            if ((Result & DataAvail.WinUtils.AskExitFormResult.Save) == DataAvail.WinUtils.AskExitFormResult.Save)
                res |= DataAvail.XtraBinding.Controllers.ControllerAskExitResult.Save;

            if ((Result & DataAvail.WinUtils.AskExitFormResult.Reject) == DataAvail.WinUtils.AskExitFormResult.Reject)
                res |= DataAvail.XtraBinding.Controllers.ControllerAskExitResult.Reject;

            if ((Result & DataAvail.WinUtils.AskExitFormResult.EndEdit) == DataAvail.WinUtils.AskExitFormResult.EndEdit)
                res |= DataAvail.XtraBinding.Controllers.ControllerAskExitResult.EndEdit;

            if ((Result & DataAvail.WinUtils.AskExitFormResult.CancelEdit) == DataAvail.WinUtils.AskExitFormResult.CancelEdit)
                res |= DataAvail.XtraBinding.Controllers.ControllerAskExitResult.CancelEdit;

            if ((Result & DataAvail.WinUtils.AskExitFormResult.CancelExit) == DataAvail.WinUtils.AskExitFormResult.CancelExit)
                res |= DataAvail.XtraBinding.Controllers.ControllerAskExitResult.CancelExit;

            if ((Result & DataAvail.WinUtils.AskExitFormResult.JustExit) == DataAvail.WinUtils.AskExitFormResult.JustExit)
                res |= DataAvail.XtraBinding.Controllers.ControllerAskExitResult.JustExit;

            return res;
        }

        #endregion
    }
}