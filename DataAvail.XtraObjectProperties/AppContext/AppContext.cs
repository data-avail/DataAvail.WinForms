using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XOP.AppContext
{
    public class AppContext
    {
        internal AppContext(IEnumerable<DataAvail.XtraObjectProperties.XtraObjectProperties> XtraObjectsProperties, ModelView.ModelViewApp ModelViewApp, ModelSecurity.ModelSecApp ModelSecApp)
        {
            _xtraObjectsProperties = XtraObjectsProperties;

            _modelViewApp = ModelViewApp;

            IEnumerable<ModelView.ModelViewAppItem> mvItems = ModelViewApp != null ? ModelViewApp.AppItems.Items : new ModelView.ModelViewAppItem[] { };

            _secObjs = ModelSecApp != null ? ModelSecApp.Objects : new ModelSecurity.ModelSecObject[] { };

            foreach(DataAvail.XtraObjectProperties.XtraObjectProperties xop in XtraObjectsProperties)
            {
                AddAppItem(xop.ObjectName, mvItems.FirstOrDefault(p => p.Name == xop.ObjectName), null);
            }

        }

        private readonly ModelView.ModelViewApp _modelViewApp;

        public ModelView.ModelViewApp ModelViewApp { get { return _modelViewApp; } }

        private readonly IEnumerable<DataAvail.XtraObjectProperties.XtraObjectProperties> _xtraObjectsProperties;

        private readonly List<AppItem> _appItems = new List<AppItem>();

        private readonly IEnumerable<ModelSecurity.ModelSecObject> _secObjs;

        private void AddAppItem(string Name, ModelView.ModelViewAppItem ModelViewAppItem, AppItemContext ParentAppItemContext)
        {
            AppItem appItem = GetAppItem(Name);

            AppItemContext itemContext = appItem.AddAppItemContext(ModelViewAppItem, ParentAppItemContext);
        }

        internal IEnumerable<DataAvail.XtraObjectProperties.XtraObjectProperties> XtraObjectsProperties { get { return _xtraObjectsProperties; } }

        public AppItemContext GetAppItemContext(string Name, Context Context)
        {
            AppItem appItem = GetAppItem(Name);
    
            AppItemContext itemContext = appItem[Context];

            if (itemContext == null)
            {
                itemContext = appItem.AddAppItemContext(null, Context);
            }

            return itemContext;
        }

        public AppItem GetParentAppItem(DataAvail.XtraObjectProperties.XtraFieldProperties XtraFieldProperties)
        {
            return _appItems.FirstOrDefault(p => p.XtraObjectProperties.ChildrenRelations.FirstOrDefault(k => k.ChildField == XtraFieldProperties) != null);
        }

        private AppItem GetAppItem(string Name)
        {
            AppItem appItem = _appItems.FirstOrDefault(p => p.Name == Name);

            if (appItem == null)
            {
                appItem = new AppItem(this, Name, _secObjs.FirstOrDefault(p => p.Name == Name));

                _appItems.Add(appItem);
            }

            return appItem;
        
        }

        public string AppName { get { return ModelViewApp != null ? ModelViewApp.Name : null; } }

        public IEnumerable<ModelView.ModelViewMenuItem> AppMenuItems
        { 
            get
            {
                return ModelViewApp != null ? ModelViewApp.MainMenu.Items : new ModelView.ModelViewMenuItem[] {};
            }
        }

        public bool IsMenuItemVisible(ModelView.ModelViewMenuItem ModelViewMenuItem)
        {
            return ModelViewMenuItem.AppItem == null || GetAppItemContext(ModelViewMenuItem.AppItem.Name, new DefaultContext()).IsCanView;
        }

        public IEnumerable<ModelView.ModelViewMenuItem> VisibleActionAppMenuItems
        {
            get
            {
                return ModelViewApp != null ? ModelViewApp.MainMenu.Descendants.Where(p => p.AppItem != null && this.IsMenuItemVisible(p)) : new ModelView.ModelViewMenuItem[] {};
            }
        }

    }
}
