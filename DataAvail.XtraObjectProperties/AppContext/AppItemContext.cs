using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XOP.AppContext
{
    public class AppItemContext
    {
        internal AppItemContext(AppItem Container, ModelView.ModelViewAppItem ModelViewAppItem, ModelSecurity.ModelSecObject ModelSecObject, Context Context)
        {
            _container = Container;

            _modelViewAppItem = ModelViewAppItem;

            _modelSecObject = ModelSecObject;
                 
            _context = Context;
        }

        private readonly List<AppFieldContext> _appFields = new List<AppFieldContext>();

        private readonly ModelView.ModelViewAppItem _modelViewAppItem;

        private readonly ModelSecurity.ModelSecObject _modelSecObject;

        private readonly AppItem _container;

        private readonly Context _context;

        public Context Context { get { return _context; } }

        public AppItem Container { get { return _container; } }

        private ModelView.ModelViewAppItem ModelViewAppItem { get { return _modelViewAppItem; } }

        private ModelSecurity.ModelSecObject ModelSecObject { get { return _modelSecObject; } }

        private AppItemContext Parent 
        {
            get 
            {
                if (_context is ChildContext)
                    return ((ChildContext)_context).parent;
                else
                    return null;

            } 
        }

        public string Name { get { return Container.Name; } }

        public string Caption { get { return Container.XtraObjectProperties.Caption; } }

        public string ItemCaption { get { return Container.XtraObjectProperties.ItemCaption; } }

        public Type ItemType { get { return Container.XtraObjectProperties.ItemType; } }

        public bool IsCanEdit
        {
            get
            {
                return Container.IsCanEdit 
                    && IsTrue(DataAvail.Utils.EditModeType.Edit) 
                    && FkItemSelectorIsTrue(DataAvail.Utils.EditModeType.Edit);
            }
        }

        public bool IsCanView
        {
            get { return Container.IsCanView 
                && IsTrue(DataAvail.Utils.EditModeType.View)
                && FkItemSelectorIsTrue(DataAvail.Utils.EditModeType.View);
            }
        }

        public bool IsCanRemove
        {
            get { return Container.IsCanRemove 
                && IsTrue(DataAvail.Utils.EditModeType.Delete)
                && FkItemSelectorIsTrue(DataAvail.Utils.EditModeType.Delete)
                && !FkItemAddIsTrue();
            }
        }

        public bool IsCanAdd
        {
            get
            {
                return Container.IsCanAdd
                    && IsTrue(DataAvail.Utils.EditModeType.Add)
                    && FkItemSelectorIsTrue(DataAvail.Utils.EditModeType.Add)
                    && !FkItemAddIsTrue();
            }
        }

        public bool IsCanClone
        {
            get
            {
                return IsCanAdd;
            }
        }

        public bool IsCanMove
        {
            get
            {
                return !FkItemAddIsTrue();
            }
        }

        public bool IsCanSaveInCache
        {
            get
            {
                return (SaveOnExitMode & DataAvail.XOP.ModelView.SaveMode.Cache) == DataAvail.XOP.ModelView.SaveMode.Cache;
            }
        }

        public bool IsCanSaveInStorage
        {
            get
            {
                return (SaveOnExitMode & DataAvail.XOP.ModelView.SaveMode.Repository) == DataAvail.XOP.ModelView.SaveMode.Repository;
            }
        }

        public AppItemContext GetChild(string ChildObjectName)
        {
            return this.Container.Container.GetAppItemContext(ChildObjectName, new DataAvail.XOP.AppContext.ChildContext(this));
        }

        private ModelView.ModelViewAppItemBase RelevantModelViewItem
        {
            get 
            {
                if (ModelViewAppItem != null && ModelViewAppItem.Mode != DataAvail.Utils.EditModeType.Default) return ModelViewAppItem;

                if (Parent != null) return Parent.RelevantModelViewItem;

                return Container.Container.ModelViewApp.AppItems;
            }
        }

        private bool IsTrue(DataAvail.Utils.EditModeType EditModeType)
        {
            return RelevantModelViewItem == null ||
                RelevantModelViewItem.Mode == DataAvail.Utils.EditModeType.Default ||
                (RelevantModelViewItem.Mode & EditModeType) == EditModeType;
        }

        private ModelView.SaveMode SaveOnExitMode
        {
            get
            {
                if (ModelViewAppItem != null && ModelViewAppItem.SaveMode != DataAvail.XOP.ModelView.SaveMode.Default) 
                    return ModelViewAppItem.SaveMode;

                if (Parent != null)
                {
                    return Parent.ChildSaveOnExitMode;
                }
                else
                {
                    return Container.Container.ModelViewApp.AppItems.SaveMode;    
                }
            }
        }

        private ModelView.SaveMode ChildSaveOnExitMode
        {
            get
            {
                if (ModelViewAppItem != null && ModelViewAppItem.ChildSaveMode != DataAvail.XOP.ModelView.SaveMode.Default)
                    return ModelViewAppItem.ChildSaveMode;

                if (Parent != null)
                    return Parent.ChildSaveOnExitMode;

                return Container.Container.ModelViewApp.AppItems.ChildSaveMode;
            }
        }

        public AppFieldContext this[string FieldName]
        {
            get 
            {
                AppFieldContext appFieldContext = _appFields.FirstOrDefault(p => p.FieldName == FieldName);

                if (appFieldContext == null)
                {
                    appFieldContext = new AppFieldContext(this, Container.XtraObjectProperties.Fields.First(p => p.FieldName == FieldName), ModelSecObject != null ? ModelSecObject.Fields.FirstOrDefault(p => p.Name == FieldName) : null);

                    _appFields.Add(appFieldContext);
                }

                return appFieldContext;
            }
        }

        public IEnumerable<AppFieldContext> Fields
        {
            get { return Container.XtraObjectProperties.Fields.Select(p=>this[p.FieldName]); }
        }

        public IEnumerable<DataAvail.XtraObjectProperties.XtraObjectRelation> ShownChildrenRelations
        {
            get { return Container.XtraObjectProperties.ShownChildrenRelations; }
        }

        public DataAvail.XtraObjectProperties.XtraObjectRelation GetChildRelation(string FieldName)
        {
            return Container.XtraObjectProperties.GetChildRelation(FieldName); 
        }


        public bool FkItemSelectorIsTrue(DataAvail.Utils.EditModeType EditModeType)
        {
            if (Context.GetType() == typeof(SelectFkItemContext))
            {
                return DataAvail.Utils.EnumFlags.IsContain(((SelectFkItemContext)Context).childAppFieldContext.ItemSelectorType, EditModeType);
            }
            else
            {
                return true;
            }
        }

        public bool FkItemAddIsTrue()
        {
            return Context is AddFkItemContext;
        }


        public bool CommandIsTrue(DataAvail.XtraObjectProperties.XOPFieldFkCommands XOPFieldFkCommands)
        {
            if (Context is SelectFkItemContext)
            {
                return DataAvail.Utils.EnumFlags.IsContain(((SelectFkItemContext)Context).childAppFieldContext.XtraFieldProperties.FKCommands, XOPFieldFkCommands);
            }
            else
            {
                return true;
            }
        }

        public string SerializationName
        {
            get
            {
                if (this.Context is DataAvail.XOP.AppContext.SelectFkItemContext)
                { 
                    string relSzName = this.GetChildRelation(((DataAvail.XOP.AppContext.SelectFkItemContext)Context).childAppFieldContext.FieldName).SerializationName;

                    if (!string.IsNullOrEmpty(relSzName))
                    {
                        return relSzName;
                    }
                }
                
                if (XtraObjectProperties.separateSerialization 
                    && (this.Context.GetType() != typeof(DataAvail.XOP.AppContext.DefaultContext)) 
                    && (this.Context.GetType() != typeof(DataAvail.XOP.AppContext.ChildContext)))
                {    
                    return string.Format("AUX_{0}", Name);
                }
                 

                return Name;
            }
        }

        public bool PersistFill
        {
            get { return this.Container.XtraObjectProperties.PersistFill; }
        }

        public DataAvail.XtraObjectProperties.XtraObjectProperties XtraObjectProperties
        {
            get { return Container.XtraObjectProperties; }
        }

    }
}
