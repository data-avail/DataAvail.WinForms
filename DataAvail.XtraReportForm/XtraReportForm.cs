using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataAvail.XtraFormController;
using DataAvail.Controllers.Commands;


namespace DataAvail.XtraReportForm
{
    public partial class XtraReportForm : DataAvail.XtraFormController.XtraFormController
    {
        public XtraReportForm(DataAvail.Controllers.Controller Controller)
            : base(Controller, true)
        {
            OnSerializationNameChanged();
        }

        public DataAvail.XtraContainer.XtraContainer XtraContainer { get { return xtraContainer1; } }

        protected override void InitializeUI()
        {
            InitializeComponent();

            base.InitializeUI();
        }

        #region Create children

        public event DataAvail.XtraEditors.ObjectCreatingHandler XtraContainerCreating;

        public event DataAvail.XtraEditors.ObjectCreatedHandler XtraContainerCreated;

        protected virtual void OnXtraContainerCreating(DataAvail.XtraEditors.ObjectCreatingEventArgs args)
        {
            if (XtraContainerCreating != null)
            {
                XtraContainerCreating(this, args);
            }
        }

        protected virtual void OnXtraContainerCreated(DataAvail.XtraEditors.ObjectCreatedEventArgs args)
        {
            if (XtraContainerCreated != null)
            {
                XtraContainerCreated(this, args);
            }
        }

        protected DataAvail.XtraContainer.XtraContainer CreateXtraContainer()
        {
            DataAvail.XtraEditors.ObjectCreatingEventArgs args = new DataAvail.XtraEditors.ObjectCreatingEventArgs(Controller);

            OnXtraContainerCreating(args);

            if (args.Object == null)
                throw new Exception("Can't create container");

            OnXtraContainerCreated(new DataAvail.XtraEditors.ObjectCreatedEventArgs(args.Object));

            return (DataAvail.XtraContainer.XtraContainer)args.Object;
        }

        #endregion

        #region Serilaization

        protected override string SerializationName
        {
            get
            {
                return TableContext != null ? TableContext.Name : null;
            }
        }

        public override DataAvail.Serialization.ISerializableObject[] ChildrenSerializable
        {
            get
            {
                return new DataAvail.Serialization.ISerializableObject[] { xtraContainer1 };
            }
        }

        #endregion

        protected override void OnShow()
        {
            this.ShowDialog();
        }

        protected override void OnClose()
        {
            this.Close();
        }

        protected override IEnumerable<IControllerCommandItem> CommandItems
        {
            get
            {
                return new IControllerCommandItem[] { 
                    GetCommandItem(ControllerCommandTypes.AcceptChanges, FormMenu[DataAvail.XtraMenu.XtraMenuButtonType.AcceptChanges]),
                    GetCommandItem(ControllerCommandTypes.RejectChanges, FormMenu[DataAvail.XtraMenu.XtraMenuButtonType.RejectChanges]),
                    GetCommandItem(ControllerCommandTypes.EndEdit, FormMenu[DataAvail.XtraMenu.XtraMenuButtonType.EndEdit]),
                    GetCommandItem(ControllerCommandTypes.CancelEdit, FormMenu[DataAvail.XtraMenu.XtraMenuButtonType.CancelEdit]),
                    GetCommandItem(ControllerCommandTypes.MoveNext, FormMenu[DataAvail.XtraMenu.XtraMenuButtonType.MoveNext]),
                    GetCommandItem(ControllerCommandTypes.MovePerv, FormMenu[DataAvail.XtraMenu.XtraMenuButtonType.MovePerv]),
                    GetCommandItem(ControllerCommandTypes.Clone, FormMenu[DataAvail.XtraMenu.XtraMenuButtonType.Clone])
                };
            }
        }
    }
}

