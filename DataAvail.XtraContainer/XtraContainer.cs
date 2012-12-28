using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataAvail.XObject;

namespace DataAvail.XtraContainer
{
    public partial class XtraContainer : DataAvail.XtraContainerBuilder.XtraContainerBuilder
    {
        public XtraContainer(DataAvail.Controllers.Controller Controller)
        {
            _controller = Controller;

            InitializeUI();
        }

        private readonly DataAvail.Controllers.Controller _controller;

        private DataAvail.Serialization.SerializationTag _serializationTag;

        List<DataAvail.XtraEditors.IXtraControl> _enabledControlsBeforeReadOnly = new List<DataAvail.XtraEditors.IXtraControl>();

        public Controllers.Controller Controller { get { return _controller; } }

        protected virtual void InitializeUI()
        {
            InitializeComponent();

            Build(Controller.TableContext);

            _serializationTag = new DataAvail.Serialization.SerializationTag(SeraializationType, DefaultTableContext.SerializationName);

            this.Controller.Commands.AddCommandItems(CommandItems);
        }

        #region Building

        protected override object CreateChildRelationControl(XORelation XORelation)
        {
            DataAvail.Controllers.Controller controller = Controller.GetChildController(XORelation.ParentField.Name, XORelation.ChildTable.Name, XORelation.ChildField.Name);

            return CreateChildRelationControl(controller);
        }

        protected virtual object CreateChildRelationControl(DataAvail.Controllers.Controller Controller)
        {
            return null;
        }

        protected override bool IsAdmittedField(string FieldName)
        {
            return DefaultTableContext[FieldName].IsCanView;
        }

        protected override bool IsAdmittedChildRelation(XORelation XORelation)
        {
            return DefaultTableContext.GetChildRelation(XORelation.ChildField).ChildTable.IsCanView;
        }

        protected override object CreateEmptyControl()
        {
            return new System.Windows.Forms.Control();
        }

        #endregion

        protected virtual IEnumerable<DataAvail.Controllers.Commands.IControllerCommandItem> CommandItems
        {
            get
            {
                return FieldControls.OfType<DataAvail.Controllers.Commands.IXtraEditCommand>().SelectMany(p => p.SupportedCommands);
            }
        }
    }
}
