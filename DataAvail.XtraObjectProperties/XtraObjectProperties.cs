using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraObjectProperties
{
    public class XtraObjectProperties
    {
        public XtraObjectProperties(string ObjectName, Type ItemType)
        {
            this.ObjectName = ObjectName;

            this.ObjectSource = ObjectName;

            this.Caption = this.ObjectName + "s";

            this.ItemCaption = this.ObjectName;

            this.ItemType = ItemType;
        }

        private XtraObjectPropertiesCollection _container;

        private XtraObjectFunctions _functions = new XtraObjectFunctions();

        public string Caption;

        public string ItemCaption;

        public bool ShowFieldCaption;

        public bool AutoFill = false;

        public bool PersistFill = true;

        public bool separateSerialization;

        public readonly string ObjectName;

        public string ObjectSource;

        public readonly Type ItemType;

        public DataAvail.Utils.EditModeType UseCommands = DataAvail.Utils.EditModeType.Default;

        public string ObjectSourceUpdate;

        internal readonly XtraObjectRelationsCollection ChildrenRelations = new XtraObjectRelationsCollection();

        public readonly XtraFieldPropertiesCollection Fields = new XtraFieldPropertiesCollection();

        public XtraObjectFunctions Functions
        {
            get { return _functions; }

            set { _functions = value; }
        }

        internal void SetContainer(XtraObjectPropertiesCollection Container)
        {
            _container = Container;
        }

        public XtraObjectPropertiesCollection Container
        {
            get { return _container; }
        }

        public IEnumerable<XtraObjectRelation> ShownChildrenRelations
        {
            get { return ChildrenRelations.Where(p=>p.IsShown);}
        }

        public XtraObjectRelation GetChildRelation(string ChildFieldName)
        { 
            return ChildrenRelations.FirstOrDefault(p=>p.ChildField.FieldName == ChildFieldName);
        }

        public XtraObjectRelation GetChildRelation(string ChildObjectName, string ChildFieldName)
        {
            return ChildrenRelations.FirstOrDefault(p => p.ChildObject.ObjectName == ChildObjectName && p.ChildField.FieldName == ChildFieldName);
        }

        public DataAvail.Utils.EditModeType AvailableOpertaions
        {
            get 
            {
                DataAvail.Utils.EditModeType opers = DataAvail.Utils.EditModeType.View;

                if (DataAvail.Utils.EnumFlags.IsContain(UseCommands, DataAvail.Utils.EditModeType.Add) || Functions.createFunction != null)
                    opers |= DataAvail.Utils.EditModeType.Add;

                if (DataAvail.Utils.EnumFlags.IsContain(UseCommands, DataAvail.Utils.EditModeType.Edit) || Functions.updateFunction != null)
                    opers |= DataAvail.Utils.EditModeType.Edit;

                if (DataAvail.Utils.EnumFlags.IsContain(UseCommands, DataAvail.Utils.EditModeType.Delete) || Functions.createFunction != null)
                    opers |= DataAvail.Utils.EditModeType.Delete;


                return opers;
            }
        }
    }
}
