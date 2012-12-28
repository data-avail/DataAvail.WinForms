using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataAvail.XtraMenu;

namespace DataAvail.XtraForm
{
    public partial class XtraForm : Form, DataAvail.Serialization.ISerializableObject
    {
        public XtraForm()
        {
            InitializeComponent();

            OnSerializationNameChanged();
        }

        private bool _isLoaded = false;

        private IXtraMenu _formMenu;

        private bool _isAutoSerialize = true;

        /// <summary>
        /// Define whether the form would be srialized on load and deserialized on close.
        /// </summary>
        public bool IsAutoSerialize
        {
            get { return _isAutoSerialize; }

            set { _isAutoSerialize = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode && !_isLoaded)
            {
                _isLoaded = true;

                OnFirstLoad();
            }

            base.OnLoad(e);

        }

        /// <summary>
        /// Invokes only when form is opened first time. Usefull when it is needed to intialize or get some objects in derived class, 
        /// after they have been created in constructor of base class.
        /// </summary>
        protected virtual void OnFirstLoad()
        {
            CreateFormMenu();

            if (IsAutoSerialize)
                this.Deserialize();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (IsAutoSerialize)
                this.Serialize();

            base.OnFormClosed(e);
        }

        protected void Serialize()
        {
            if (!_serializationTag.IsEmpty)
                DataAvail.Serialization.SerializationManager.Serialize(this, _serializationTag.ToString());        
        }

        protected void Deserialize()
        {
            if (!string.IsNullOrEmpty(SerializationName))
            {
                DataAvail.Serialization.SerializationManager.Deserialize(this, _serializationTag.ToString());
            }
        }

        private DataAvail.Serialization.SerializationTag _serializationTag;

        protected virtual string SerializationName { get { return null; } }

        #region ISerializableObject Members

        public virtual void Serialize(System.Runtime.Serialization.SerializationInfo SerializationInfo)
        {
            SerializationInfo.AddValue("Width", this.Width);
            SerializationInfo.AddValue("Height", this.Height);
            SerializationInfo.AddValue("WindowState", (int)this.WindowState);
            SerializationInfo.AddValue("LocationX", (int)this.Location.X);
            SerializationInfo.AddValue("LocationY", (int)this.Location.Y);
        }

        public virtual void Deserialize(System.Runtime.Serialization.SerializationInfo SerializationInfo)
        {
            try
            {
                this.Width = (int)SerializationInfo.GetValue("Width", typeof(int));
                this.Height = (int)SerializationInfo.GetValue("Height", typeof(int));
                this.WindowState = (FormWindowState)SerializationInfo.GetValue("WindowState", typeof(int));
                this.Location = new Point( (int)SerializationInfo.GetValue("LocationX", typeof(int)), (int)SerializationInfo.GetValue("LocationY", typeof(int)));

            }
            catch (System.Runtime.Serialization.SerializationException)
            { }
        }

        public DataAvail.Serialization.SerializationTag SerializationTag
        {
            get { return _serializationTag; }
        }

        public virtual DataAvail.Serialization.ISerializableObject[] ChildrenSerializable
        {
            get { return null; }
        }

        #endregion

        #region Form Menu

        public IXtraMenu FormMenu { get { return _formMenu; } }

        protected void CreateFormMenu()
        {
            if (_formMenu == null)
                _formMenu = OnCreateFormMenu();
        }

        protected virtual IXtraMenu OnCreateFormMenu()
        {
            return null;
        }


        protected void SetMenuButtonsState(bool Enabled)
        {
            foreach (IXtraMenuButton button in FormMenu)
            {
                button.Enabled = Enabled;
            }            
        }

        #endregion

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            //Only here data could be actually bound
            if (this.Visible)
                OnDataBound();

        }

        protected virtual void OnDataBound()
        {
            if (DataBound != null)
                DataBound(this, EventArgs.Empty);
        }

        public event System.EventHandler DataBound;

        protected void OnSerializationNameChanged()
        {
            _serializationTag = new DataAvail.Serialization.SerializationTag("Form", SerializationName); 
        }
    }
}
