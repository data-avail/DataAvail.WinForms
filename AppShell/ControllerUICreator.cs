using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.AppShell
{
    internal class ControllerUICreator : DataAvail.XtraBinding.Controllers.IControllerUICreator
    {
        internal ControllerUICreator(System.Windows.Forms.Form MdiParent)
        {
            _mdiParent = MdiParent;
        }

        private readonly System.Windows.Forms.Form _mdiParent;


        #region IControllerUICreator Members

        public DataAvail.XtraBinding.Controllers.IControllerUI Create(DataAvail.XtraBinding.Controllers.Controller Controller, bool ItemUI)
        {
            if (ItemUI)
            {
                DataAvail.DX.XtraReportForm.XtraReportForm reportForm = new DataAvail.DX.XtraReportForm.XtraReportForm((DataAvail.XtraBinding.Controllers.Controller)Controller);

                return new CountrollerUI(reportForm);
            }
            else
            {
                DataAvail.DX.XtraListForm.XtraListForm listForm = new DataAvail.DX.XtraListForm.XtraListForm((DataAvail.XtraBinding.Controllers.Controller)Controller);

                if (Controller.TableContext.IsDefaultContext)
                {
                    listForm.MdiParent = this._mdiParent;
                }

                return new CountrollerUI(listForm);
            }
        }

        #endregion

        internal class CountrollerUI : DataAvail.XtraBinding.Controllers.IControllerUI
        {
            internal CountrollerUI(DataAvail.XtraForm.XtraForm Form)
            {
                _form = Form;

                _form.KeyPreview = true;

                _form.KeyDown += new System.Windows.Forms.KeyEventHandler(_form_KeyDown);

                Form.DataBound += new EventHandler(Form_DataBound);
            }

            void _form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
            {
                if (UIKeyDown != null)
                    UIKeyDown.Invoke(this, e);
            }

            void Form_DataBound(object sender, EventArgs e)
            {
                if (UIDataBound != null)
                    UIDataBound(this, EventArgs.Empty);                
            }

            private readonly DataAvail.XtraForm.XtraForm _form;

            #region IControllerUI Members

            public event EventHandler UIDataBound;

            public event System.Windows.Forms.KeyEventHandler UIKeyDown;

            public bool IsFocused
            {
                get { return _form.Focused || DataAvail.WinUtils.Control.GetFocusedDescendant(_form) != null; }
            }

            public DataAvail.XtraBinding.Controllers.Commands.IXtraEditCommand FocusedEditCommand
            {
                get 
                { 
                    System.Windows.Forms.Control ctl = DataAvail.WinUtils.Control.GetFocusedDescendant(_form);

                    if (ctl is DataAvail.XtraBinding.Controllers.Commands.IXtraEditCommand)
                    {
                        return (DataAvail.XtraBinding.Controllers.Commands.IXtraEditCommand)ctl;
                    }
                    else if (ctl.Parent is DataAvail.XtraBinding.Controllers.Commands.IXtraEditCommand)
                    {
                        return ((DataAvail.XtraBinding.Controllers.Commands.IXtraEditCommand)ctl.Parent);
                    }

                    return null;
                }
            }

            public object UI
            {
                get { return _form; }
            }

            #endregion
        }
    }
}
