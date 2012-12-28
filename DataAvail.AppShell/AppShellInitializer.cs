using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.XObject;
using DataAvail.Data;
using DataAvail.XObject.XOP;
using DataAvail.XObject.XWP;
using DataAvail.Controllers;
using DataAvail.XtraBindings.Calculator;
using System.Reflection;
using DataAvail.WinUtils;
using DataAvail.Attributes;

namespace DataAvail.AppShell
{
    public static class AppShellInitializer
    {
        private static AppConfig _appConfig;

        private static AppConfig AppConfig { get { return _appConfig; } }

        public static XOApplication InitializeFromAppConfig()
        {
            StartupBunch sttpBunch = AppConfigHandler.Initialize();

            _appConfig = SelectApp(sttpBunch);

            if (AppConfig == null) return null;

            SplashScreen.ShowSplashScreen(AppConfig.FullImgSplashFileName);

            DataAvail.Serialization.FileSerializationStream.DeafaultDirectory = AppConfig.FullXmlFolderPath;

            XElement Model; 
            XElement View;
            XElement Security;

            LoadModel(out Model, out View, out Security);

            if(Model == null)
            {
                throw new AppShellInitializerException("Can't reach the model, without it project can't be started.");
            }

            XOApplication.xmlReaderLog = AppShellLog.Log;

            XOApplication app = new XOApplication(Model, View, Security,
                new XOApplicationConfigParams() { pluginsDirectory = AppConfig.FullPluginsFolderPath, sharedPluginsDirectory = sttpBunch.SharedPluginsFolder });

            InitializeDbContext(AppConfig.FullModelFileName, app.XopDataSet);

            System.Data.DataSet dataSet = DataAvail.DataSetParser.DataSetBuilder.Build(AppConfig.FullModelFileName);

            FillStorage(app, dataSet);

            Controller.controllerContext = new ControllerContext(new XtraBindingControllerProvider(dataSet, LoadCalculatorManager()));

            InitializeControllerContext(app);

            if (app.XwpApplication != null)
                InitializeAppApperance(app.XwpApplication.AppView);

            return app;
        }

        private static AppConfig SelectApp(StartupBunch StartupBunch)
        {
            if (StartupBunch.AppConfigs.Length > 1)
            {
                return AppSelectForm.ShowForm(StartupBunch);
            }
            else
            {
                return StartupBunch.AppConfigs[0];
            }
        }

        private static void InitializeDbContext(string ModelFilePath, XOPDataSet XOPDataSet)
        {
            DataAvail.Data.DbContext.IDbContext dbContext = null;

            switch (XOPDataSet.AdapterType)
            {
                case XOPDataSetAdapterType.Oracle:
                    throw new AppShellInitializerException("Still not implemented, coming soon...");
                case XOPDataSetAdapterType.SqlServer:
                    dbContext = new DataAvail.DevArt.Data.MSSQL.DbContext();
                    break;
                case XOPDataSetAdapterType.SQLite:
                    dbContext = new DataAvail.DevArt.Data.SQLite.DbContext();
                    //throw new AppShellInitializerException("Temporary off");
                    break;
                default:
                    throw new AppShellInitializerException("Unknown data set's adapter type");
            }

            dbContext.ObjectCreator.Connection.ConnectionString = ConnectionStringMacros.Parse(ModelFilePath, XOPDataSet.ConnectionString);

            DataAvail.Data.DbContext.DbContext.CurrentContext = dbContext;
        }

        private static void InitializeControllerContext(XOApplication XOApplication)
        {
            foreach (XOTableContext appItemContext in XOApplication.MenuTables)
            {
                Controller.controllerContext.AddController(appItemContext);
            }
        }

        private static void FillStorage(XOApplication XOApplication, System.Data.DataSet DataSet)
        {
            foreach (XOTableContext appItemContext in XOApplication.AutoFillTables)
            {
                DataAvail.Data.DbContext.DbContext.CurrentContext.DataAdapter.Fill(DataSet.Tables[appItemContext.Name], null);
            } 
        }

        private static void InitializeAppApperance(XWPAppView AppView)
        {
            if (AppView != null && AppView.Misc != null && AppView.Misc.AppSkin != null)
            {
                if (!string.IsNullOrEmpty(AppView.Misc.AppSkin.SkinName))
                {
                    bool skinFound = true;

                    switch (AppView.Misc.AppSkin.SkinName)
                    {
                        case "Coffee":
                        case "Liquid Sky":
                        case "London Liquid Sky":
                        case "Glass Oceans":
                        case "Stardust":
                        case "Xmas 2008 Blue":
                        case "Valentine":
                        case "McSkin":
                        case "Summer 2008":
                        case "DevExpress Style":
                        case "Pumpkin":
                        case "DarkSide":
                        case "Springtime":
                        case "Darkroom":
                        case "Foggy":
                        case "High Contrast":
                        case "Seven":
                        case "Seven Classic":
                        case "Sharp":
                        case "Sharp Plus":
                            DevExpress.UserSkins.BonusSkins.Register();
                            break;
                        case "Office 2007 Blue":
                        case "Office 2007 Black":
                        case "Office 2007 Silver":
                        case "Office 2007 Green":
                        case "Office 2007 Pink":
                            DevExpress.UserSkins.OfficeSkins.Register();
                            break;
                        default:
                            skinFound = false;
                            AppShellLog.Log.Write("Can't determine skin [{0}] of project", AppView.Misc.AppSkin.SkinName);
                            break;
                    }

                    if (skinFound)
                    {
                        DevExpress.LookAndFeel.DefaultLookAndFeel lookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel();

                        DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(AppView.Misc.AppSkin.SkinName);
                    }
                }
            }
        }


        private static void LoadModel(out XElement Model, out XElement View, out XElement Security)
        {
            Model = null;
            View = null;
            Security = null;

            string modelPath = AppConfig.FullModelFileName;
            string viewPath = AppConfig.FullViewFileName;
            string securityPath = AppConfig.FullSecurityFileName;

            if (!string.IsNullOrEmpty(modelPath))
            {
                try
                {
                    Model = XDocument.Load(modelPath).Root;
                }
                catch(System.Exception e)
                {
                    AppShellLog.Log.Write("Model file failed to load", e);
                }

            }

            if (!string.IsNullOrEmpty(viewPath))
            {
                try
                {
                    View = XDocument.Load(viewPath).Root;
                }
                catch(System.Exception e)
                {
                    AppShellLog.Log.Write("View file failed to load", e);
                }

            }

            if (!string.IsNullOrEmpty(securityPath))
            {
                try
                {
                    Security = XDocument.Load(securityPath).Root;
                }
                catch(System.Exception e)
                {
                    AppShellLog.Log.Write("Security file failed to load", e);
                }

            }
        }

        private static IObjectCalculatorManager LoadCalculatorManager()
        {
            if (!string.IsNullOrEmpty(AppConfig.FullCalculatorFileName))
            {
                try
                {
                    return (IObjectCalculatorManager)Utils.Plugins.LoadPlugin(AppConfig.FullCalculatorFileName, typeof(ClaculatorManagerAttibute), true);
                }
                catch (System.Exception e)
                {
                    AppShellLog.Log.Write(string.Format(
                        "Exception while load the calculator (file name - {0}) : {1}.\n Project will be started without specified calculator.",
                        AppConfig.FullCalculatorFileName, e));

                }
            }

            return null;
        }
    }


}
