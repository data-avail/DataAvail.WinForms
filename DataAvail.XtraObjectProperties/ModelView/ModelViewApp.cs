using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils.XmlLinq;

namespace DataAvail.XOP.ModelView
{
    public class ModelViewApp
    {
        public ModelViewApp()
        {
        }

        internal ModelViewApp(XElement XApp)
        {
            this.Parse(XApp);
        }

        private string _name;

        private readonly ModelViewMainMenu _mainMenu = new ModelViewMainMenu();

        private readonly ModelViewAppItems _appItems = new ModelViewAppItems();

        private readonly ModelViewAppKeys _appKeys = new ModelViewAppKeys();

        private readonly ModelViewMisc _appMisc = new ModelViewMisc();

        internal void Parse(XElement XApp)
        {
            _name = XmlLinq.GetAttribute(XApp, "name");

            MainMenu.Parse(XApp.Element("MainMenu"));

            AppItems.Parse(XApp.Element("AppItems"));

            AppKeys.Parse(XApp.Element("Keys"));

            AppMisc.Parse(XApp.Element("Misc"));

            foreach (var i in from m in AllMenuItems
                              join a in AppItems.Items on m.AppItemId equals a.Id
                              select new { menu = m, action = a })
            {
                i.menu.SetAppItem(i.action);
            }
        }

        private IEnumerable<ModelViewMenuItem> AllMenuItems
        {
            get
            {
                return _mainMenu.Items.Union(_mainMenu.Items.SelectMany(p=>GetMenuItemDescendants(p)));
            }
        }

        private IEnumerable<ModelViewMenuItem> GetMenuItemDescendants(ModelViewMenuItem MenuItem)
        {
            return MenuItem.Items.Union(MenuItem.Items.SelectMany(p => GetMenuItemDescendants(p)));
        }

        public static ModelViewApp Load(string FileName)
        {
            XDocument xdoc = System.Xml.Linq.XDocument.Load(FileName);

            return new ModelViewApp(xdoc.Root);
        }

        public string Name
        {
            get { return _name; }
        }

        public ModelViewMainMenu MainMenu
        {
            get { return _mainMenu; }
        }

        public ModelViewAppItems AppItems
        {
            get { return _appItems; }
        }

        public ModelViewAppKeys AppKeys
        {
            get { return _appKeys; }
        }

        public ModelViewMisc AppMisc
        {
            get { return _appMisc; }
        } 


    }
}
