using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XOP.AppContext
{
    public class AppItem
    {
        internal AppItem(AppContext Container, string Name, ModelSecurity.ModelSecObject ModelSecObject)
        {
            _container = Container;

            _name = Name;

            _modelSecObject = ModelSecObject;

            _appItemContexts = new AppItemContextsCollection(this);
        }

        private readonly AppContext _container;

        private readonly string _name;

        private readonly AppItemContextsCollection _appItemContexts;

        private readonly ModelSecurity.ModelSecObject _modelSecObject;

        internal string Name { get { return _name; } }

        public AppContext Container { get { return _container; } }

        internal ModelSecurity.ModelSecObject ModelSecObject { get { return _modelSecObject; } }

        internal DataAvail.XtraObjectProperties.XtraObjectProperties XtraObjectProperties { get { return Container.XtraObjectsProperties.First(p => p.ObjectName == Name); } }

        private AppItemContextsCollection AppItemContexts
        {
            get { return _appItemContexts; }
        } 

        public bool IsCanEdit 
        {
            get 
            {
                return DataAvail.Utils.EnumFlags.IsContain(XtraObjectProperties.AvailableOpertaions, DataAvail.Utils.EditModeType.Edit) && //XtraObjectProperties.Functions.updateFunction != null &&
                    (ModelSecObject == null || !IsTrue(ModelSecObject.IsReadOnly, false));
            }
        }

        public bool IsCanView
        {
            get
            {
                return ModelSecObject == null || !IsTrue(ModelSecObject.IsHidden, false);
            }
        }

        public bool IsCanAdd
        {
            get
            {
                return DataAvail.Utils.EnumFlags.IsContain(XtraObjectProperties.AvailableOpertaions, DataAvail.Utils.EditModeType.Add) &&//XtraObjectProperties.Functions.createFunction != null && 
                    (ModelSecObject == null || IsTrue(ModelSecObject.IsCanAdd));
            }
        }

        public bool IsCanRemove
        {
            get
            {
                return DataAvail.Utils.EnumFlags.IsContain(XtraObjectProperties.AvailableOpertaions, DataAvail.Utils.EditModeType.Delete) &&//XtraObjectProperties.Functions.deleteFunction != null && 
                    (ModelSecObject == null || IsTrue(ModelSecObject.IsCanRemove));
            }
        }

        public DataAvail.XtraObjectProperties.XtraFieldProperties PKField
        {
            get
            {
                return XtraObjectProperties.Fields.FirstOrDefault(p => p.IsPK == true);
            }                                                            
        }

        private bool IsTrue(DataAvail.Utils.DefaultBoolType Value)
        {
            return DataAvail.Utils.DefaultBool.Convert(Value, true);
        }

        private bool IsTrue(DataAvail.Utils.DefaultBoolType Value, bool DefaultAsTrue)
        {
            return DataAvail.Utils.DefaultBool.Convert(Value, DefaultAsTrue);
        }

        public AppItemContext AddAppItemContext(ModelView.ModelViewAppItem ModelViewAppItem, AppItemContext ParentAppItemContext)
        {
            return this.AppItemContexts.Add(ModelViewAppItem, ParentAppItemContext == null ? (Context)new DefaultContext() : (Context)new ChildContext(ParentAppItemContext));
        }

        public AppItemContext AddAppItemContext(ModelView.ModelViewAppItem ModelViewAppItem, Context Context)
        {
            return this.AppItemContexts.Add(ModelViewAppItem, Context);
        }

        public AppItemContext this[Context Context]
        {
            get
            {
                return this.AppItemContexts[Context];
            }
        }

    }
}
