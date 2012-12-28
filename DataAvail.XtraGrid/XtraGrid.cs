using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataAvail.XObject.XWP;

namespace DataAvail.XtraGrid
{
    public partial class XtraGrid : 
        DataAvail.XtraContainerBuilder.XtraContainerBuilder, 
        DataAvail.Serialization.ISerializableObject, 
        DataAvail.XtraEditors.IXtraControl,
        DataAvail.Controllers.Commands.IXtraEditCommand
    {
        public XtraGrid(Controllers.Controller Controller)
        {
            _controller = Controller;

            InitializeUI();

            Controller.ListUIUploadToExcel += new DataAvail.Controllers.ControllerListUIUploadToExcelHandler(Controller_ListUIUploadToExcel);
            Controller.ListUIFilter += new DataAvail.Controllers.ControllerFilterExpressionHandler(Controller_ListUIFilter);
            Controller.ListUIFilterActive += new DataAvail.Controllers.ControllerListUIFilterActiveHandler(Controller_ListUIFilterActive);
            Controller.ListUIFilterFormatter += new DataAvail.Controllers.ControllerListUIFilterFormatterHandler(Controller_ListUIFilterFormatter);
            Controller.EndFill += new EventHandler(Controller_EndFill);

            Controller.Commands.AddCommandItems(SupportedCommands);
        }

        private readonly Controllers.Controller _controller;

        private DataAvail.Serialization.SerializationTag _serializationTag;

        private bool _readOnly = false;

        private DataAvail.Controllers.Commands.IControllerCommandItem[] _supportedCommands;

        protected Controllers.Controller Controller { get { return _controller; } }

        protected virtual void InitializeUI()
        {
            InitializeComponent();

            this.Build(Controller.TableContext);

            _serializationTag = new DataAvail.Serialization.SerializationTag("Grid", Controller.TableContext.SerializationName);

            this.ContextMenuStrip.Items.Add("Показать");
        }


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
            get { return new DataAvail.Serialization.ISerializableObject[] { (DataAvail.Serialization.ISerializableObject)Controller.ItemUI.UI }; }
        }

        #endregion

        protected override bool IsAdmittedField(string FieldName)
        {
            return Controller == null || DefaultTableContext[FieldName].IsCanView;
        }

        void Controller_ListUIFilter(object sender, DataAvail.Controllers.ControllerFilterExpressionEventArgs args)
        {
            OnListUIFilter(args.filterExpression);
        }

        void Controller_ListUIFilterActive(object sender, DataAvail.Controllers.ControllerListUIFilterActiveEventArgs args)
        {
            args.filterActive = OnListUIFilterActive(args.filterActive);
        }

        void Controller_ListUIFilterFormatter(object sender, DataAvail.Controllers.ControllerListUIFilterFormatterEventArgs args)
        {
            args.filterFormatter = OnListUIFilterFormatter();
        }

        void Controller_EndFill(object sender, EventArgs e)
        {
            OnEndFill();
        }

        void Controller_ListUIUploadToExcel(object sender, DataAvail.Controllers.ControllerListUIUploadToExcelEventArgs args)
        {   
            OnListUIUploadToExcel(args.fileName);
        }

        protected virtual void OnListUIFilter(string ExpressionFilter)
        { }

        protected virtual bool OnListUIFilterActive(bool FilterActive)
        {
            return FilterActive;
        }

        protected virtual DataAvail.Data.DbContext.IDbContextWhereFormatter OnListUIFilterFormatter()
        {
            return null;
        }

        protected virtual void OnListUIUploadToExcel(string FileName)
        { 
            
        }

        protected virtual void OnEndFill()
        { }

        #region IXtraControl Members

        public bool ReadOnly
        {
            get
            {
                return _readOnly;
            }

            set
            {
                if (ReadOnly != value)
                {
                    _readOnly = value;

                    OnReadOnlyChanged();
                }
            }
        }

        #endregion

        protected virtual void OnReadOnlyChanged()
        { 
            
        }

        #region IXtraEditCommand Members

        public IEnumerable<DataAvail.Controllers.Commands.IControllerCommandItem> SupportedCommands
        {
            get 
            { 
                if (_supportedCommands == null)
                {
                    _supportedCommands = OnCreateCommands().ToArray();
                }

                return _supportedCommands;
            }
        }

        #endregion

        protected virtual IEnumerable<DataAvail.Controllers.Commands.IControllerCommandItem> OnCreateCommands()
        {
            return new DataAvail.Controllers.Commands.IControllerCommandItem[] {
                            new DataAvail.Controllers.Commands.UICommandItems.ToolStripCommandItem(DataAvail.Controllers.Commands.ControllerCommandTypes.ItemShow, this.ContextMenuStrip.Items[0])
                        };
        }
    
    }
}
