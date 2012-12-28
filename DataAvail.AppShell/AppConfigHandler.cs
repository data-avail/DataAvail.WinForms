using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml.Linq;
using DataAvail.Utils.XmlLinq;
using DataAvail.Utils;

namespace DataAvail.AppShell
{
    public class AppConfigHandler : IConfigurationSectionHandler
    {
        public AppConfigHandler()
        { 
        }

        #region Defaul Constants


        private const string DefModelBasePath = "Model";

        private const string DefSerializationPath = "XML";

        private const string DefModelFileName = "model.xml";

        private const string DefViewFileName = "view.xml";

        private const string DefSecurityFileName = "security.xml";

        private const string DefPluginsFolderPath = "Plugins";

        #endregion


        internal static StartupBunch Initialize()
        {
            StartupBunch sttpBunch = ConfigurationSettings.GetConfig("dataAvailConfig") as StartupBunch;

            if (sttpBunch == null)
            {
                sttpBunch = new StartupBunch();   
            }

            return sttpBunch;
        }

        #region IConfigurationSectionHandler Members

        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            XElement configEl = section.GetXElement();

            StartupBunch sttpBunch = new StartupBunch()
            {
                BaseFolder = XmlLinq.GetAttributeSafe(configEl, "path")
            };

            XElement xApps = configEl.Element("apps");

            string sharedPluginsPath = XmlLinq.GetAttributeSafe(xApps, "sharedPluginsPath");

            if (!string.IsNullOrEmpty(sharedPluginsPath))
                sttpBunch.SharedPluginsFolder = DataAvail.Utils.Path.GetFullPath(sttpBunch.BaseFolder, sharedPluginsPath);

            sttpBunch.AppConfigs = GetAppConfigs(sttpBunch, xApps).Where(p => p.IsActive).ToArray();

            return sttpBunch;
        }

        #endregion

        private IEnumerable<AppConfig> GetAppConfigs(StartupBunch StartupBunch, XElement AppsElements)
        {
            foreach (XElement appEl in AppsElements.Elements("app"))
            {
                XElement modelEl = appEl.Element("modelFolder");
                XElement plugEl = appEl.Element("plugins");

                AppConfig config = new AppConfig(StartupBunch)
                    {
                        Caption = XmlLinq.GetAttributeSafe(appEl, "caption"),
                        BaseFolder = XmlLinq.GetAttributeSafe(appEl, "path"),
                        ImgSplashFile = XmlLinq.GetAttributeSafe(appEl, "imgSplash"),
                        TmpFolder = XmlLinq.GetAttributeSafe(appEl.Element("tmpFolder"), "path"),
                        XmlFolder = XmlLinq.GetAttributeSafe(appEl.Element("xmlFolder"), "path"),
                        PluginsFolder = XmlLinq.GetAttributeSafe(plugEl, "path"),
                        IsActive = XmlLinq.ReadAttributeBool(appEl, "active", true, null)
                    };

                if (plugEl != null)
                {
                    config.ClaulatorFile = XmlLinq.GetAttributeSafe(plugEl.Element("calculator"), "path");
                }

                if (modelEl != null)
                {
                    config.ModelFolder = XmlLinq.GetAttributeSafe(modelEl, "path");
                    config.ModelFile = XmlLinq.GetAttributeSafe(modelEl.Element("model"), "path");
                    config.ViewFile = XmlLinq.GetAttributeSafe(modelEl.Element("view"), "path");
                    config.SecurityFile = XmlLinq.GetAttributeSafe(modelEl.Element("security"), "path");
                }

                if (string.IsNullOrEmpty(config.ModelFolder))
                    config.ModelFolder = DefModelBasePath;

                if (string.IsNullOrEmpty(config.ModelFile))
                    config.ModelFile = DefModelFileName;

                if (string.IsNullOrEmpty(config.ViewFile))
                    config.ViewFile = DefViewFileName;

                if (string.IsNullOrEmpty(config.SecurityFile))
                    config.SecurityFile = DefSecurityFileName;

                if (string.IsNullOrEmpty(config.XmlFolder))
                    config.XmlFolder = DefSerializationPath;

                if (string.IsNullOrEmpty(config.PluginsFolder))
                    config.PluginsFolder = DefPluginsFolderPath;

                yield return config;
            }
        }
    }

    internal class StartupBunch
    {
        private string _baseFolder;

        private string _sharedPluginsFolder;

        private AppConfig[] _appConfigs;

        internal string BaseFolder
        {
            get { return _baseFolder; }
            set { _baseFolder = value; }
        }

        internal string SharedPluginsFolder
        {
            get { return _sharedPluginsFolder; }
            set { _sharedPluginsFolder = value; }
        }

        internal AppConfig[] AppConfigs
        {
            get { return _appConfigs; }
            set { _appConfigs = value; }
        }
    }

    internal class AppConfig
    {
        internal AppConfig(StartupBunch StartupBunch)
        {
            _startupBunch = StartupBunch;
        }

        private readonly StartupBunch _startupBunch;

        private string _caption;

        private string _baseFolder;

        private string _modelFolder;

        private string _modelFile;

        private string _viewFile;

        private string _securityFile;

        private string _xmlFolder;

        private string _tmpFolder;

        private string _claulatorFile;

        private string _imgSplashFile;

        private bool _isActive;

        private string _pluginsFolder;

        internal StartupBunch StartupBunch
        {
            get { return _startupBunch; }
        }

        public string Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }

        public string BaseFolder
        {
            get { return _baseFolder; }
            set { _baseFolder = value; }
        }

        public string ModelFolder
        {
            get { return _modelFolder; }
            set { _modelFolder = value; }
        }

        public string ModelFile
        {
            get { return _modelFile; }
            set { _modelFile = value; }
        }

        public string ViewFile
        {
            get { return _viewFile; }
            set { _viewFile = value; }
        }

        public string SecurityFile
        {
            get { return _securityFile; }
            set { _securityFile = value; }
        }

        public string XmlFolder
        {
            get { return _xmlFolder; }
            set { _xmlFolder = value; }
        }

        public string TmpFolder
        {
            get { return _tmpFolder; }
            set { _tmpFolder = value; }
        }

        public string ClaulatorFile
        {
            get { return _claulatorFile; }
            set { _claulatorFile = value; }
        }

        public string ImgSplashFile
        {
            get { return _imgSplashFile; }
            set { _imgSplashFile = value; }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public string PluginsFolder
        {
            get { return _pluginsFolder; }
            set { _pluginsFolder = value; }
        }

        internal string FullXmlFolderPath
        {
            get { return Path.GetFullPath(StartupBunch.BaseFolder, BaseFolder, XmlFolder); }
        }

        internal string FullTmpFolderPath
        {
            get { return Path.GetFullPath(StartupBunch.BaseFolder, BaseFolder, XmlFolder); }
        }

        internal string FullPluginsFolderPath
        {
            get { return Path.GetFullPath(StartupBunch.BaseFolder, BaseFolder, PluginsFolder); }
        }

        internal string FullCalculatorFileName
        {
            get { return Path.GetFullPath(FullPluginsFolderPath, ClaulatorFile); }
        }

        internal string FullModelFileName
        {
            get { return Path.GetFullPath(StartupBunch.BaseFolder, BaseFolder, ModelFolder, ModelFile); }
        }

        internal string FullViewFileName
        {
            get { return Path.GetFullPath(StartupBunch.BaseFolder, BaseFolder, ModelFolder, ViewFile); }
        }

        internal string FullSecurityFileName
        {
            get { return Path.GetFullPath(StartupBunch.BaseFolder, BaseFolder, ModelFolder, SecurityFile); }
        }

        internal string FullImgSplashFileName
        {
            get { return !string.IsNullOrEmpty(ImgSplashFile) ? Path.GetFullPath(StartupBunch.BaseFolder, BaseFolder, ImgSplashFile) : null; }
        }

        public override string ToString()
        {
            return Caption;
        }
    }
}
