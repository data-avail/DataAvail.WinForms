using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.Controllers.Commands;

namespace DataAvail.XtraListForm
{
    public partial class XtraListForm : DataAvail.XtraFormController.XtraFormController
    {
        public XtraListForm(DataAvail.Controllers.Controller Controller)
            : base(Controller, false)
        {
            OnSerializationNameChanged();

            Controller.ListUIFocusSearch += new EventHandler(Controller_ListUIFocusSearch);

            Controller.ListUIFocusList += new EventHandler(Controller_ListUIFocusList);
        }

        private DataAvail.XtraContainer.XtraContainer _xtraSearchContainer;

        private DataAvail.XtraGrid.XtraGrid _xtraGrid;

        public DataAvail.XtraContainer.XtraContainer XtraSearchContainer { get { return _xtraSearchContainer; } }

        public DataAvail.XtraGrid.XtraGrid XtraGrid { get { return _xtraGrid; } }

        #region Create children
        
        public event DataAvail.XtraEditors.ObjectCreatingHandler XtraGridCreating;

        public event DataAvail.XtraEditors.ObjectCreatingHandler XtraSearchContainerCreating;

        public event DataAvail.XtraEditors.ObjectCreatedHandler XtraGridCreated;

        public event DataAvail.XtraEditors.ObjectCreatedHandler XtraSearchContainerCreated;

        protected virtual void OnXtraGridCreating(DataAvail.XtraEditors.ObjectCreatingEventArgs args)
        {
            if (XtraGridCreating != null)
            {
                XtraGridCreating(this, args);
            }
        }

        protected virtual void OnXtraSearchContainerCreating(DataAvail.XtraEditors.ObjectCreatingEventArgs args)
        {
            if (XtraSearchContainerCreating != null)
            {
                XtraSearchContainerCreating(this, args);
            }
        }

        protected virtual void OnXtraGridCreated(DataAvail.XtraEditors.ObjectCreatedEventArgs args)
        {
            if (XtraGridCreated != null)
            {
                XtraGridCreated(this, args);
            }
        }

        protected virtual void OnXtraSearchContainerCreated(DataAvail.XtraEditors.ObjectCreatedEventArgs args)
        {
            if (XtraSearchContainerCreated != null)
            {
                XtraSearchContainerCreated(this, args);
            }
        }

        protected DataAvail.XtraGrid.XtraGrid CreateXtraGrid()
        {
            DataAvail.XtraEditors.ObjectCreatingEventArgs args = new DataAvail.XtraEditors.ObjectCreatingEventArgs(Controller);

            OnXtraGridCreating(args);

            _xtraGrid = (DataAvail.XtraGrid.XtraGrid)args.Object;

            OnXtraGridCreated(new DataAvail.XtraEditors.ObjectCreatedEventArgs(_xtraGrid));

            return _xtraGrid;
        }

        protected DataAvail.XtraContainer.XtraContainer CreateXtraSearchContainer()
        {
            DataAvail.XtraEditors.ObjectCreatingEventArgs args = new DataAvail.XtraEditors.ObjectCreatingEventArgs(Controller);

            OnXtraSearchContainerCreating(args);

            _xtraSearchContainer = (DataAvail.XtraContainer.XtraContainer)args.Object;

            OnXtraSearchContainerCreated(new DataAvail.XtraEditors.ObjectCreatedEventArgs(args.Object));

            return (DataAvail.XtraContainer.XtraContainer)args.Object;
        }

        #endregion

        #region Serialization

        protected override string SerializationName
        {
            get
            {
                return this.TableContext != null ? this.TableContext.SerializationName + "s" : null;
            }
        }

        public override DataAvail.Serialization.ISerializableObject[] ChildrenSerializable
        {
            get
            {
                return new DataAvail.Serialization.ISerializableObject[] { XtraSearchContainer, XtraGrid };
            }
        }

        #endregion

        protected override void OnShow()
        {
            if (Controller.TableContext.IsDefaultContext)
            {
                if (this.Visible)
                {
                    this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

                    this.Focus();
                }
                else
                {
                    this.Show();
                }
            }
            else 
            {
                this.ShowDialog();
            }
        }


        protected override void OnClose()
        {
            this.Serialize();

            if (Controller.TableContext.IsDefaultContext)
                this.Hide();
            else
                this.Close();    
        }

        protected override IEnumerable<IControllerCommandItem> CommandItems
        {
            get
            {
                return new IControllerCommandItem[] { 
                    GetCommandItem(ControllerCommandTypes.BatchAcceptChanges, FormMenu[DataAvail.XtraMenu.XtraMenuButtonType.AcceptChanges]),
                    GetCommandItem(ControllerCommandTypes.BatchRejectChanges, FormMenu[DataAvail.XtraMenu.XtraMenuButtonType.RejectChanges]),
                    GetCommandItem(ControllerCommandTypes.CancelFill, FormMenu[DataAvail.XtraMenu.XtraMenuButtonType.StopFill]),
                    GetCommandItem(ControllerCommandTypes.Refill, FormMenu[DataAvail.XtraMenu.XtraMenuButtonType.Refill]),
                    GetCommandItem(ControllerCommandTypes.UploadToExcel, FormMenu[DataAvail.XtraMenu.XtraMenuButtonType.UploadToExcel])
                };
            }
        }

        protected override void InitializeUI()
        {
            this.Deserialize();

            base.InitializeUI();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            this.AcceptButton = (System.Windows.Forms.IButtonControl)((DataAvail.XtraSearcherContainer.IXtraSearchContainer)XtraSearchContainer).SearchButton;

            this.XtraGrid.TabStop = false;
        }

        #region Controller events

        void Controller_ListUIFocusList(object sender, EventArgs e)
        {
            this.XtraGrid.Focus();
        }

        void Controller_ListUIFocusSearch(object sender, EventArgs e)
        {
            if (this.XtraSearchContainer.Height != 0)
                this.XtraSearchContainer.Focus();
        }

        #endregion
    }
}
