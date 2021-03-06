using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using DataAvail.XObject;
using DataAvail.XObject.XWP;
using DataAvail.Controllers;
using DataAvail.Controllers.Commands;
using DataAvail.WinUtils;
using DataAvail.WinUtils.InfoForm;
using DataAvail.Attributes;
using DataAvail.Utils;

namespace DataAvail.AppShell
{
    public partial class MainFrame : DataAvail.XtraForm.XtraForm
    {
        public MainFrame()
        {
            InitializeComponent();

            DataAvail.Controllers.Controller.synchronizeInvoke = this;
            DataAvail.Controllers.Controller.uiCreator = new ControllerUICreator(this);

            _appContext = AppShellInitializer.InitializeFromAppConfig();

            if (AppContext != null)
            {
                BuildMenu();

                DataAvail.Data.DataSet.Log = new DebugLog();

                DataAvail.Controllers.Controller.AskExit += new DataAvail.Controllers.ControllerAskExitHandler(Controller_AskExit);
                DataAvail.Controllers.Controller.ShowInfo += new DataAvail.Controllers.ControllerShowInfo(Controller_ShowInfo);
                DataAvail.Controllers.Controller.AskConfirmation += new DataAvail.Controllers.ControllerAskConfirmationHandler(Controller_AskConfirmation);
            }
        }

        private XOApplication _appContext;

        private Dictionary<string, BarButtonItem> _menuButtons = new Dictionary<string, BarButtonItem>();

        private XOApplication AppContext
        {
            get { return _appContext; }
        }

        protected override void OnFirstLoad()
        {
            if (AppContext == null)
            {
                this.Close();
            }
            else
            {
                base.OnFirstLoad();

                this.Text = AppContext.Name;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            SplashScreen.HideSplashScreen();
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

            foreach (XOMenuItem menuItem in AppContext.MenuItems)
                BuildMenuItem(null, menuItem);

            BuildMenuForPlugins();

            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
        }

        private void BuildMenuItem(DevExpress.XtraBars.BarSubItem ParentBarSubItem, XOMenuItem MenuItem)
        {
            if (MenuItem.Children.Length > 0)
            {
                BarSubItem barSubItem = AddMenuSubItem(ParentBarSubItem, MenuItem);

                foreach (XOMenuItem menuItem in MenuItem.Children)
                    BuildMenuItem(barSubItem, menuItem);
            }
            else
            {
                _menuButtons.Add(MenuItem.TableName, AddMenuButtonItem(ParentBarSubItem, MenuItem));
            }
        }

        private static Keys GetKeys(XWPMenuItem XWPMenuItem)
        {
            XWPKeyCommand keyCommand = XWPMenuItem.KeyCommand;

            if (keyCommand.Keys.Length > 0)
            {
                object k = Enum.Parse(typeof(Keys), keyCommand.Keys[0].Key, true);

                if (k != null)
                {
                    Keys key = (Keys)k;

                    foreach (string str in keyCommand.Keys[0].Modifyers)
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
            }
            return Keys.None;
        }

        private BarSubItem AddMenuSubItem(BarSubItem ParentBarSubItem, XOMenuItem MenuItem)
        {

            return AddMenuSubItem(ParentBarSubItem, MenuItem.Caption);
        }

        private BarSubItem AddMenuSubItem(BarSubItem ParentBarSubItem, string ButtonText)
        {
            BarSubItem barSubItem = new BarSubItem();

            barSubItem.Caption = ButtonText;

            barSubItem.Name = "barSubItem" + ButtonText;

            this.barManager1.Items.AddRange(new BarItem [] { barSubItem });

            if (ParentBarSubItem != null)
            {
                ParentBarSubItem.LinksPersistInfo.AddRange(new LinkPersistInfo[] {
                   new LinkPersistInfo(barSubItem)});
            }
            else
            {
                this.barManager1.MainMenu.LinksPersistInfo.AddRange(new LinkPersistInfo[] {
                    new LinkPersistInfo(barSubItem)});
            }

            return barSubItem;
        }

        private BarButtonItem AddMenuButtonItem(BarSubItem ParentBarSubItem, XOMenuItem MenuItem)
        {
            BarButtonItem buttonItem = AddMenuButtonItem(ParentBarSubItem, MenuItem.Caption, GetKeys(MenuItem.XWPMenuItem));

            if (!string.IsNullOrEmpty(MenuItem.TableName))
            {
                Controller appItemController = Controller.controllerContext.Controllers.FirstOrDefault(p => p.TableContext.Name == MenuItem.TableName);

                if (appItemController != null)
                {
                    appItemController.Commands.AddCommandItem(new DXBarButtonCommandItem(ControllerCommandTypes.ListShow, buttonItem));

                    appItemController.UpdateStates();
                }
            }

            return buttonItem;
        }

        private BarButtonItem AddMenuButtonItem(BarSubItem ParentBarSubItem, string ButtonText, Keys Key)
        {
            DevExpress.XtraBars.BarButtonItem barButtonItem = new DevExpress.XtraBars.BarButtonItem();

            barButtonItem.Caption = ButtonText;

            barButtonItem.Name = "barSubItem" + ButtonText;

            if (Key != Keys.None)
                barButtonItem.ItemShortcut = new DevExpress.XtraBars.BarShortcut(Key);

            this.barManager1.Items.Add(barButtonItem);

            if (ParentBarSubItem != null)
            {
                ParentBarSubItem.LinksPersistInfo.Add(new LinkPersistInfo(barButtonItem));
            }
            else
            {
                this.barManager1.MainMenu.LinksPersistInfo.Add(new LinkPersistInfo(barButtonItem));
            }

            return barButtonItem;
        }


        #endregion

        #region Controller event

        void Controller_AskConfirmation(object sender, DataAvail.Controllers.ControllerAskConfirmationEventArgs args)
        {
            args.confirm = System.Windows.Forms.MessageBox.Show(args.confirmationString, "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes;
        }

        void Controller_AskExit(object sender, DataAvail.Controllers.ControllerAskExitEventArgs args)
        {
            args.result = GetXtraBindingControllerAskExitResult(DataAvail.WinUtils.AskExitForm.Ask(GetAskExitFormResult(args.enabledResults)));
        }

        void Controller_ShowInfo(object sender, DataAvail.Controllers.ControllerShowInfoEventArgs args)
        {
            InfoForm.Show("Error", args.ToString(), InfoFormSendReportType.Default);
        }

        private DataAvail.WinUtils.AskExitFormResult GetAskExitFormResult(DataAvail.Controllers.ControllerAskExitResult Result)
        {
            AskExitFormResult res = AskExitFormResult.None;

            if ((Result & ControllerAskExitResult.Save) == ControllerAskExitResult.Save)
                res |= AskExitFormResult.Save;

            if ((Result & ControllerAskExitResult.Reject) == ControllerAskExitResult.Reject)
                res |= AskExitFormResult.Reject;

            if ((Result & ControllerAskExitResult.EndEdit) == ControllerAskExitResult.EndEdit)
                res |= DataAvail.WinUtils.AskExitFormResult.EndEdit;

            if ((Result & ControllerAskExitResult.CancelEdit) == ControllerAskExitResult.CancelEdit)
                res |= AskExitFormResult.CancelEdit;

            if ((Result & ControllerAskExitResult.CancelExit) == ControllerAskExitResult.CancelExit)
                res |= AskExitFormResult.CancelExit;

            if ((Result & ControllerAskExitResult.JustExit) == ControllerAskExitResult.JustExit)
                res |= AskExitFormResult.JustExit;

            return res;
        }

        private ControllerAskExitResult GetXtraBindingControllerAskExitResult(AskExitFormResult Result)
        {
            ControllerAskExitResult res = ControllerAskExitResult.None;

            if ((Result & AskExitFormResult.Save) == AskExitFormResult.Save)
                res |= ControllerAskExitResult.Save;

            if ((Result & AskExitFormResult.Reject) == AskExitFormResult.Reject)
                res |= ControllerAskExitResult.Reject;

            if ((Result & AskExitFormResult.EndEdit) == AskExitFormResult.EndEdit)
                res |= ControllerAskExitResult.EndEdit;

            if ((Result & AskExitFormResult.CancelEdit) == AskExitFormResult.CancelEdit)
                res |= ControllerAskExitResult.CancelEdit;

            if ((Result & AskExitFormResult.CancelExit) == AskExitFormResult.CancelExit)
                res |= ControllerAskExitResult.CancelExit;

            if ((Result & AskExitFormResult.JustExit) == AskExitFormResult.JustExit)
                res |= ControllerAskExitResult.JustExit;

            return res;
        }

        #endregion

        #region Plugins

        private void BuildMenuForPlugins()
        {
            BuildMenuForPlugins(AppContext.ConfigParams.pluginsDirectory);

            BuildMenuForPlugins(AppContext.ConfigParams.sharedPluginsDirectory);
        }


        private void BuildMenuForPlugins(string Dir)
        {
            if (!string.IsNullOrEmpty(Dir))
            {
                foreach (object obj in Plugins.LoadPlugins(Dir, typeof(PluginMenuItem)))
                {
                    PluginMenuItem attr = (PluginMenuItem)obj.GetType().GetCustomAttributes(false).First(p => p.GetType() == typeof(PluginMenuItem));

                    BarButtonItem item = BuildMenuItem(null, attr.Menu);

                    item.Tag = obj;

                    item.ItemClick += new ItemClickEventHandler(pluginMenuItem_ItemClick);
                }
            }
        }

        void pluginMenuItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            ((Form)e.Item.Tag).ShowDialog();
        }

        private BarButtonItem BuildMenuItem(DevExpress.XtraBars.BarSubItem ParentBarSubItem, string Path)
        {
            int i = Path.IndexOf('/');

            if (i != -1)
            {
                string txt = Path.Substring(0, i);

                BarSubItem barSubItem = AddMenuSubItem(ParentBarSubItem, txt);

                return BuildMenuItem(barSubItem, Path.Remove(0, i + 1));
            }
            else
            {
                return AddMenuButtonItem(ParentBarSubItem, Path, Keys.None);
            }
        }

        #endregion
    }
}