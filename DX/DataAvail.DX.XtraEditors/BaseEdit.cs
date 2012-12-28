using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataAvail.XObject;
using DataAvail.Controllers.Binding;

namespace DataAvail.DX.XtraEditors
{
    public partial class BaseEdit<T> : 
        DataAvail.DXEditors.XtraEditContainer,
        DataAvail.XtraEditors.IXtraEdit, 
        DataAvail.XtraEditors.IXtraEditObjectProperties, 
        DataAvail.Serialization.ISerializableObject, 
        DataAvail.XtraEditors.IXtraControl, 
        DataAvail.Controllers.Commands.IXtraEditCommand, 
        DataAvail.Controllers.Binding.IControllerBindableControl
            where T : DevExpress.XtraEditors.BaseEdit, new()
    {
        public BaseEdit(XOFieldContext XOFieldContext)
        {
            InitializeComponent();

            this.BaseEdit = this.DxEdit;

            dxEdit.EditValueChanged += new EventHandler(dxBaseEdit_EditValueChanged);

            this._appFieldContext = XOFieldContext;

            _serializationTag = new DataAvail.Serialization.SerializationTag("Edit", AppFieldContext.Name);

            this.TabStop = DxEdit.TabStop;

            DxEdit.TabStopChanged += new EventHandler(DxEdit_TabStopChanged);
        }

        private XOFieldContext _appFieldContext = null;

        private DataAvail.Serialization.SerializationTag _serializationTag;

        private List<DataAvail.Controllers.Commands.IControllerCommandItem> _editorCommandItems = null;

        private CCProperties _controllerControlProperties;

        void dxBaseEdit_EditValueChanged(object sender, EventArgs e)
        {
            OnEditValueChanged();
        }

        protected virtual void OnEditValueChanged()
        {
            if (EditValueChanged != null)
            {
                EditValueChanged(this, EventArgs.Empty);
            }        
        }

        private T CreateDxEdit()
        {
            return new T();
        }

        protected virtual IEnumerable<DataAvail.Controllers.Commands.IControllerCommandItem> CreateCommandItems()
        {
            return new DataAvail.Controllers.Commands.IControllerCommandItem[] { };
        }

        #region IXtraEdit Members

        public virtual object EditValue
        {
            get { return dxEdit.EditValue; }

            set { dxEdit.EditValue = value; }
        }

        public event EventHandler EditValueChanged;

        public Type FieldType
        {
            get { return _appFieldContext.FieldType; }
        }

        public string FieldName
        {
            get { return _appFieldContext.Name; }
        }

        #endregion

        public XOFieldContext AppFieldContext
        {
            get
            {

                return _appFieldContext;
            }
        }

        public T DxEdit { get { return dxEdit; } }


        #region ISerializableObject Members

        public virtual void Serialize(System.Runtime.Serialization.SerializationInfo SerializationInfo)
        {
        }

        public virtual void Deserialize(System.Runtime.Serialization.SerializationInfo SerializationInfo)
        {
        }

        public DataAvail.Serialization.SerializationTag SerializationTag
        {
            get { return _serializationTag; }
        }

        public DataAvail.Serialization.ISerializableObject[] ChildrenSerializable
        {
            get { return null; }
        }

        #endregion

        public virtual string FormattedValue
        {
            get
            {
                return this.EditValue != null ? this.EditValue.ToString() : null;
            }

            set
            {
                System.Runtime.Serialization.FormatterConverter formatterConverter = new System.Runtime.Serialization.FormatterConverter();

                this.EditValue = formatterConverter.Convert(value, FieldType);
            }
        }

        public bool ReadOnly
        {
            get
            {
                return this.dxEdit.Properties.ReadOnly;
            }

            set
            {
                this.DxEdit.Properties.ReadOnly = value;
            }
        }

        #region IXtraEditObjectProperties Members

        public void SetObjectProperties(Dictionary<string, object> NameValue)
        {
            foreach (KeyValuePair<string, object> kvp in NameValue)
            {
                OnSetProperty(kvp.Key, kvp.Value);
            }
        }

        #endregion

        protected virtual void OnSetProperty(string Name, object Value)
        {
            switch (Name)
            {
                case "value":
                    this.EditValue = Value;
                    break;
            }
        }

        #region IXtraEditCommand Members

        public IEnumerable<DataAvail.Controllers.Commands.IControllerCommandItem> SupportedCommands
        {
            get
            {
                if (_editorCommandItems == null)
                {
                    _editorCommandItems = new List<DataAvail.Controllers.Commands.IControllerCommandItem>();

                    _editorCommandItems.AddRange(CreateCommandItems());
                }

                return _editorCommandItems;
            }
        }

        #endregion

        #region IControllerBindableControl Members


        public CCProperties ControlProperties
        {
            get 
            { 
                return _controllerControlProperties; 
            }

            set
            {
                if (ControlProperties != value)
                {
                    _controllerControlProperties = value;

                    OnControlPropertiesChanged();
                }
            }
        }

        public string ValuePropertyName
        {
            get { return "EditValue"; }
        }
         


        #endregion

        public override Color BackColor
        {
            get
            {
                return dxEdit.BackColor;
            }
            set
            {
                dxEdit.BackColor = value;
            }
        }

        void DxEdit_TabStopChanged(object sender, EventArgs e)
        {
            if (this.TabStop != DxEdit.TabStop)
                this.TabStop = DxEdit.TabStop;
        }

        void OnControlPropertiesChanged()
        {
            if (ControlProperties != null)
            {
                ControlProperties.PropertyChanged += new PropertyChangedEventHandler(ControlProperties_PropertyChanged);
            }
        }

        void ControlProperties_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnControlPropertiesPropertyChanged(e.PropertyName);

        }

        protected virtual void OnControlPropertiesPropertyChanged(string PropertyName)
        { 
            switch (PropertyName)
            { 
                case "ReadOnly":
                    this.ReadOnly = ControlProperties.ReadOnly;
                    break;
                case "BackColor":
                    this.BackColor = ControlProperties.BackColor;
                    break;
            }        
        }

    }
}
