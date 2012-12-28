using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataAvail.Controllers;
using DataAvail.Controllers.Commands;
using System.ComponentModel;
 
namespace DataAvail.AppShell
{
    internal class ControllerUICreator : IControllerUICreator
    {
        internal ControllerUICreator(Form MdiParent)
        {
            _mdiParent = MdiParent;
        }

        private readonly Form _mdiParent;


        #region IControllerUICreator Members

        public IControllerUI Create(Controller Controller, bool ItemUI)
        {
            if (ItemUI)
            {
                DataAvail.DX.XtraReportForm.XtraReportForm reportForm = new DataAvail.DX.XtraReportForm.XtraReportForm((DataAvail.Controllers.Controller)Controller);

                return new CountrollerUI(reportForm);
            }
            else
            {
                DataAvail.DX.XtraListForm.XtraListForm listForm = new DataAvail.DX.XtraListForm.XtraListForm((Controller)Controller);

                if (Controller.TableContext.IsDefaultContext)
                {
                    listForm.MdiParent = this._mdiParent;
                }

                return new CountrollerUI(listForm);
            }
        }

        #endregion

        internal class CountrollerUI : IControllerUI, ISupportInitialize
        {
            internal CountrollerUI(DataAvail.XtraForm.XtraForm Form)
            {
                _form = Form;

                _form.KeyPreview = true;

                _form.KeyDown += new KeyEventHandler(_form_KeyDown);

                _form.KeyUp += new KeyEventHandler(_form_KeyUp);

                Form.DataBound += new EventHandler(Form_DataBound);
            }

            private Keys _lastKeyDown = Keys.None;

            void _form_KeyDown(object sender, KeyEventArgs e)
            {
                if (UIKeyDown != null)
                    UIKeyDown.Invoke(this, e);

                if (e.Handled)
                {
                    _lastKeyDown = e.KeyCode;
                }
            }

            void _form_KeyUp(object sender, KeyEventArgs e)
            {
                if (_lastKeyDown == e.KeyCode)
                {
                    if (UIKeyUp != null)
                        UIKeyUp.Invoke(this, e);
                }

                _lastKeyDown = Keys.None;
            }

            void Form_DataBound(object sender, EventArgs e)
            {
                if (UIDataBound != null)
                    UIDataBound(this, EventArgs.Empty);                
            }

            private readonly DataAvail.XtraForm.XtraForm _form;

            #region IControllerUI Members

            public event EventHandler UIDataBound;

            public event KeyEventHandler UIKeyDown;

            public event KeyEventHandler UIKeyUp;

            public bool IsFocused
            {
                get { return _form.Focused || DataAvail.WinUtils.Control.GetFocusedDescendant(_form) != null; }
            }

            public IXtraEditCommand FocusedEditCommand
            {
                get 
                { 
                    Control ctl = DataAvail.WinUtils.Control.GetFocusedDescendant(_form);

                    if (ctl is IXtraEditCommand)
                    {
                        return (IXtraEditCommand)ctl;
                    }
                    else if (ctl.Parent is IXtraEditCommand)
                    {
                        return ((IXtraEditCommand)ctl.Parent);
                    }

                    return null;
                }
            }

            public object UI
            {
                get { return _form; }
            }

            public bool Validate()
            {
                _form.Validate();

                return true;
            }

            #endregion

            #region ISupportInitialize

            public void BeginInit()
            {
                foreach (ISupportInitialize ctl in DataAvail.WinUtils.Control.DescndantControls(_form).OfType<ISupportInitialize>())
                    ctl.BeginInit();
            }

            public void EndInit()
            {
                foreach (ISupportInitialize ctl in DataAvail.WinUtils.Control.DescndantControls(_form).OfType<ISupportInitialize>())
                    ctl.EndInit();

            }

            #endregion
        }
    }
}
