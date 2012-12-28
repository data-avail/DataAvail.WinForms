using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;

namespace DataAvail.XtraFormController
{
    public abstract class XtraFormController : DataAvail.XtraForm.XtraForm
    {
        public XtraFormController(DataAvail.Controllers.Controller Controller, bool ItemForm)
        {
            _controller = Controller;

            _isItemUI = ItemForm;

            if (ItemForm)
            {
                Controller.ItemInitializeUI += new EventHandler(Controller_InitializeUI);
            }
            else
            {
                Controller.ListInitializeUI += new EventHandler(Controller_InitializeUI);
            }

            this.IsAutoSerialize = false;
        }

        private readonly bool _isItemUI;

        private bool _userClosing = true;

        private readonly DataAvail.Controllers.Controller _controller;

        private bool IsItemUI
        {
            get { return _isItemUI; }
        } 

        protected DataAvail.Controllers.Controller Controller
        {
            get { return _controller; }
        }

        public XOTableContext TableContext
        {
            get { return Controller != null ? Controller.TableContext : null; }
        }

        protected abstract void OnShow();

        protected abstract void OnClose();

        protected override void OnFormClosing(System.Windows.Forms.FormClosingEventArgs e)
        {
            if (_userClosing)
            {
                bool f = Controller.Commands.Execute(IsItemUI ? 
                    DataAvail.Controllers.Commands.ControllerCommandTypes.ItemClose :
                    DataAvail.Controllers.Commands.ControllerCommandTypes.ListClose);

                e.Cancel = this.MdiParent == null || e.CloseReason != System.Windows.Forms.CloseReason.UserClosing ? !f 
                    : true;
            }

            base.OnFormClosing(e);
        }

        protected override void OnFirstLoad()
        {
            base.OnFirstLoad();

            if (IsItemUI)
                this.Text = TableContext.ItemCaption;
            else
                this.Text = TableContext.Caption;
        }

        protected virtual void InitializeUI()
        {
            CreateFormMenu();

            //this.Deserialize();

            Controller.Commands.AddCommandItems(CommandItems);

            if (IsItemUI)
            {
                Controller.ItemShowUI += new EventHandler(Controller_ShowUI);
                Controller.ItemCloseUI += new EventHandler(Controller_CloseUI);
            }
            else
            {
                Controller.ListShowUI += new EventHandler(Controller_ShowUI);
                Controller.ListCloseUI += new EventHandler(Controller_CloseUI);
            }
        }


        void Controller_InitializeUI(object sender, EventArgs e)
        {
            InitializeUI();
        }
        
        void Controller_ShowUI(object sender, EventArgs e)
        {
            OnShow();
        }

        void Controller_CloseUI(object sender, EventArgs e)
        {
            CloseForm();
        }

        private void CloseForm()
        {
            _userClosing = false;

            OnClose();

            _userClosing = true;        
        }

        protected static DataAvail.Controllers.Commands.IControllerCommandItem GetCommandItem(DataAvail.Controllers.Commands.ControllerCommandTypes Type, DataAvail.XtraMenu.IXtraMenuButton MenuButton)
        {
            return new MenuItemStub(MenuButton, Type);
        }

        protected virtual IEnumerable<DataAvail.Controllers.Commands.IControllerCommandItem> CommandItems
        {
            get
            {
                return new DataAvail.Controllers.Commands.IControllerCommandItem[] { };
            }
        }
    }
}
