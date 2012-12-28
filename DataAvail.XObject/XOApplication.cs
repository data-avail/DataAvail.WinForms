using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.XObject.XOP;
using DataAvail.XObject.XWP;
using DataAvail.XObject.XSP;
using DataAvail.XObject.XContexts;

namespace DataAvail.XObject
{
    public class XOApplication
    {
        public static IXmlReaderLog xmlReaderLog;

        public XOApplication(XElement ModelElement, XElement ViewElement, XElement SecurityElement, XOApplicationConfigParams ConfigParams)
        {
            if (ModelElement == null)
            {
                throw new ArgumentException("ModelElement can't be null"); 
            }

            _xopDataSet = new XOPDataSet(ModelElement);

            if (ViewElement != null)
                _xwpApplication = new XWPApplication(ViewElement);

            if (SecurityElement != null)
                _xspApplication = new XSPApplication(SecurityElement);

            _tables = _xopDataSet.Tables.Select(p => new XOTable(this, p, GetXWPTable(XwpApplication, p.Name), GetXSPTable(XspApplication, p.Name))).ToArray();

            _configParams = ConfigParams;

            this.EndInit();
        }

        private static XWPTable GetXWPTable(XWPApplication XWPApplication, string TableName)
        {
            return XWPApplication != null && XWPApplication.DataView != null && XWPApplication.DataView.Tables != null ? XWPApplication.DataView.Tables.FirstOrDefault(s => s.TableName == TableName) : null;
        }

        private static XSPTable GetXSPTable(XSPApplication XSPApplication, string TableName)
        {
            return XSPApplication != null && XSPApplication.Tables != null ? XSPApplication.Tables.FirstOrDefault(s => s.TableName == TableName) : null;
        }

        private readonly XOPDataSet _xopDataSet;

        private readonly XWPApplication _xwpApplication;

        private readonly XSPApplication _xspApplication;

        private readonly XOTable[] _tables;

        private readonly List<XOTableContext> _tablesContext = new List<XOTableContext>();

        private readonly XOApplicationConfigParams _configParams;

        private void EndInit()
        {
            foreach (XOTable table in Tables)
                table.EndInit();
        }

        private XOTableContext this[XContext XContext, XOTable XOTable]
        {
            get
            {
                return _tablesContext.FirstOrDefault(p => p.Context.Equals(XContext) && p.XOTable == XOTable);
            }
        }

        internal XOTableContext GetTableContext(XContext XContext, XOTable XOTable)
        {
            XOTableContext tableContext = this[XContext, XOTable];

            if (tableContext == null)
            {
                tableContext = new XOTableContext(XOTable, XContext);

                _tablesContext.Add(tableContext);
            }

            return tableContext;
        }

        public XOPDataSet XopDataSet
        {
            get { return _xopDataSet; }
        }

        public XWPApplication XwpApplication
        {
            get { return _xwpApplication; }
        }

        public XSPApplication XspApplication
        {
            get { return _xspApplication; }
        }

        public XOTable[] Tables
        {
            get { return _tables; }
        }

        public XOMode Mode
        {
            get
            {
                return XspApplication != null ? XspApplication.Mode : XOMode.All;
            }
        }


        public XOTableContext[] MenuTables
        {
            get 
            {
                return 
                    FlatMenuItems.Select(p => Tables.FirstOrDefault(s => s.Name == p.XWPMenuItem.TableName)).
                    Where(p=>p != null).
                    Select(p => GetTableContext(XContext.GetDefaultContext(), p)).ToArray();

            }
        }

        public XOTableContext[] AutoFillTables
        {
            get
            {
                return Tables.Where(p => p.XopTable.AutoFill).Select(p => GetTableContext(XContext.GetDefaultContext(), p)).ToArray();
            }
        }

                
        public XOMenuItem [] MenuItems
        {
            get
            {
                XOMenuItem [] menuItems = new XOMenuItem[] { };

                if (this.XwpApplication != null && this.XwpApplication.AppView != null && XwpApplication.AppView.MenuItems != null && XwpApplication.AppView.MenuItems.MenuItems != null)
                {
                    menuItems = XwpApplication.AppView.MenuItems.MenuItems.Select(p => new XOMenuItem(p, XspApplication)).Where(p=>p.IsVisible).ToArray();
                }

#if DEBUG
                if (menuItems.Length == 0)
                { 
                    menuItems = new XOMenuItem [] { new XOMenuItem(
                        new XWPMenuItem("Items", null, Tables.Select(p=>new XWPMenuItem(p.Name, p.Name, new XWPMenuItem[] {})).ToArray()), 
                        XspApplication)};
                }
#endif
                return menuItems;
            }
        }

        public string Name
        {
            get { return XwpApplication == null || string.IsNullOrEmpty(XwpApplication.Name) ? "Data Avail Constructor" : XwpApplication.Name; }
        }

        private IEnumerable<XOKey> Keys
        {
            get
            {
                if (XwpApplication != null && XwpApplication.AppView != null && XwpApplication.AppView.Keys != null)
                {
                    return XwpApplication.AppView.Keys.KeyCommands.SelectMany(p => p.Keys.Union(p.KeyContexts.SelectMany(s => s.Keys))).Select(p => new XOKey(p));
                }
                else
                {
                    return new XOKey[] { };
                }
            }
        }

        public XOKey [] GetKeys(XWPKeyContextType XWPKeyContextType)
        {
            IEnumerable<XOKey> keys = Keys;

            var contextKeys = keys.Where(s => s.ContextType == XWPKeyContextType);

            return keys.Where(p => contextKeys.FirstOrDefault(s => s.CommandType == p.DefaultKeyCommand.CommandType) == null) 
                .Union(contextKeys).ToArray();
        }


        private XOMenuItem [] FlatMenuItems
        {
            get { return GetDecendants(MenuItems).ToArray(); }
        }

        private IEnumerable<XOMenuItem> GetDecendants(IEnumerable<XOMenuItem> XOMenuItems)
        {
            return XOMenuItems.Union(XOMenuItems.SelectMany(p => GetDecendants(p.Children)));
        }

        public XOApplicationConfigParams ConfigParams
        {
            get { return _configParams; }
        }
    }

    public class XOApplicationConfigParams
    {
        public string pluginsDirectory;

        public string sharedPluginsDirectory;
    }
}
