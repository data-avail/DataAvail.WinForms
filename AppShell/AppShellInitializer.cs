using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XtraBinding.Controllers;
using DataAvail.XObject;
using DataAvail.XObject.XWP;

namespace DataAvail.AppShell
{
    internal class AppShellInitializer
    {
        internal static AppShellInitializerResult Initialize(DataAvailable.DACProperties DACProperties)
        {
            DataAvail.Serialization.FileSerializationStream.DeafaultDirectory = DACProperties.LayoutsFolder;

            DataAvail.Data.DbContext.IDbContext dbContext = null;
            
            switch(DACProperties.AdapterType)
            {
                case DataAvailable.DACDataAdapterType.Oracle:
                    //dbContext = new DataAvail.DevArt.Data.Oracle.DbContext();
                    break;
                case DataAvailable.DACDataAdapterType.MSSQL:
                    dbContext = new DataAvail.DevArt.Data.MSSQL.DbContext();
                    break;
                case DataAvailable.DACDataAdapterType.SQLite:
                    dbContext = new DataAvail.DevArt.Data.SQLite.DbContext();
                    break;
            }

            dbContext.ObjectCreator.Connection.ConnectionString = DACProperties.ConnectionString;
            DataAvail.Data.DbContext.DbContext.CurrentContext = dbContext;
            DataAvail.XtraBinding.Controllers.Controller.properties.TempFolder = DACProperties.TempFolder;

            PrepareDirectories(DACProperties.TempFolder);

            return Initialize(DACProperties.Model, DACProperties.ModelView, DACProperties.ModelSecurity, DACProperties.CalculatorManager);
        }

        private static AppShellInitializerResult Initialize( 
            string ModelFileName, 
            string ModelViewFileName, 
            string ModelSecFileName,
            DataAvail.XtraBinding.Calculator.IObjectCalculatorManager ObjectCalulatorManager)
        {

            System.Data.DataSet dataSet = BuildMetadata(ModelFileName);

            XtraBindingControllerProvider controllerProvider = BuildControllerProvider(dataSet, ObjectCalulatorManager);

            DataAvail.XtraBinding.Controllers.Controller.controllerContext = new ControllerContext(controllerProvider);

            XOApplication xopCol = BuildStorage(ModelFileName, ModelViewFileName, ModelSecFileName, dataSet);

            /*nimpl
            InitializeControllersContext(xopCol);

            FillStorage(dataSet, controllerProvider, xopCol);

            InitializeAppApperance(xopCol.AppContext.ModelViewApp.AppMisc);

            return new AppShellInitializerResult(xopCol.AppContext, DataAvail.XtraBinding.Controllers.Controller.controllerContext.Controllers);
             */

            return null;
        }

        private static void InitializeAppApperance(XWPAppSkin AppSkin)
        {
            if (!string.IsNullOrEmpty(AppSkin.SkinName))
            {
                switch (AppSkin.SkinName)
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
                }

                DevExpress.LookAndFeel.DefaultLookAndFeel lookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel();

                DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(AppSkin.SkinName);
            }
        }

        private static void PrepareDirectories(string TempDirectory)
        {
            if (!string.IsNullOrEmpty(TempDirectory))
            {
                if (!System.IO.Directory.Exists(TempDirectory))
                {
                    System.IO.Directory.CreateDirectory(TempDirectory);
                }
                else
                {
                    foreach (string fileName in System.IO.Directory.GetFiles(TempDirectory))
                    {
                        System.IO.File.Delete(fileName);  
                    }
                }
            }

        }

        private static System.Data.DataSet BuildMetadata(string ModelFileName)
        {
            return DataAvail.DataSetParser.DataSetBuilder.Build(ModelFileName);
        }

        private static XtraBindingControllerProvider BuildControllerProvider(
            System.Data.DataSet DataSet, 
            DataAvail.XtraBinding.Calculator.IObjectCalculatorManager ObjectCalulatorManager)
        {
            return new XtraBindingControllerProvider(DataSet, ObjectCalulatorManager);
        }

        private static XOApplication BuildStorage(string ModelFileName, string ModelViewFileName, string ModelSecFileName, System.Data.DataSet DataSet)
        {
            /*nimpl
            return DataAvail.XtraObjectProperties.XOPCreator.XOPCreator.Create(
                    new DataAvail.XtraObjectProperties.XOPCreator.XOPCreatorDataSet.XOPCreatorProviderDataSet(DataSet),
                    ModelFileName,
                    ModelViewFileName,
                    ModelSecFileName);
             */

            return null;
   
        }

        private static void FillStorage(System.Data.DataSet DataSet, XtraBindingControllerProvider ControllerProvider, XOApplication XOApplication)
        {
            foreach (XOTableContext xop in XOApplication.AutoFillTables)
            {
                ControllerProvider.GetDataAdapter(xop).Fill(DataSet.Tables[xop.Name], null);
            }
        }

        private static void InitializeControllersContext(XOApplication XOApplication)
        {
            foreach (XOTableContext appItemContext in XOApplication.MenuTables)
            {
                DataAvail.XtraBinding.Controllers.Controller.controllerContext.AddController(appItemContext);
            }
        }

        internal class AppShellInitializerResult
        {
            internal AppShellInitializerResult(XOApplication XOApplication, IEnumerable<Controller> Controllers)
            {
                application = XOApplication;

                controllers = Controllers;
            }

            internal readonly XOApplication application;

            internal readonly IEnumerable<Controller> controllers;
        }
    }
}
