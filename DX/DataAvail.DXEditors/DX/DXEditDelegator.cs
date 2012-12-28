using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DataAvail.DXEditors.DX
{
    public class DXEditDelegator
    {
        public DXEditDelegator(DevExpress.XtraEditors.BaseEdit BaseEdit)
        { 
            _baseEdit = BaseEdit;

            _readOnly = BaseEdit.Properties.ReadOnly;

            this.BaseEdit.Properties.AppearanceReadOnly.BackColor = DXEdit.readOnlyBackColor;

            this.BaseEdit.TabStop = !Properties.ReadOnly;

            //this.BaseEdit.GotFocus += new EventHandler(BaseEdit_GotFocus);

            Properties.PropertiesChanged += new EventHandler(Properties_PropertiesChanged);

            #region key timer 

            _keyTimer.SynchronizingObject = this.BaseEdit;

            _keyTimer.AutoReset = false;

            _keyTimer.Interval = 1000;

            _keyTimer.Elapsed += new System.Timers.ElapsedEventHandler(_keyTimer_Elapsed);

            #endregion
        }

        private bool _readOnly;

        private readonly DevExpress.XtraEditors.BaseEdit _baseEdit;

        private System.Timers.Timer _keyTimer = new System.Timers.Timer();

        internal event FocusedFontChangingHandler FocusedFontChanging;

        private void OnFocusedFontChanging(FocusedFontChangingEventArgs args)
        {
            if (FocusedFontChanging != null)
                FocusedFontChanging(this, args);
        }

        public DevExpress.XtraEditors.BaseEdit BaseEdit
        {
            get { return _baseEdit; }
        }

        public DevExpress.XtraEditors.Repository.RepositoryItem Properties
        {
            get { return BaseEdit.Properties; }
        }

        private DevExpress.Utils.AppearanceObject _apperanceBeforeGotFocus = new DevExpress.Utils.AppearanceObject();

        #region Focused font

        private bool UseFocusedFont
        {
            get 
            {
                IDataAvailStyleController sc = BaseEdit.StyleController as IDataAvailStyleController;

                return sc != null ? sc.UseFocusedFont : false;
            }
        }

        void _keyTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SetFontToFocused();
        }

        private bool CheckFocusedFontChanging(bool GotFocus)
        {
            FocusedFontChangingEventArgs args = new FocusedFontChangingEventArgs(GotFocus);

            OnFocusedFontChanging(args);

            return !args.cancel;
        }

        void BaseEdit_GotFocus(object sender, EventArgs e)
        {
            if (UseFocusedFont && CheckFocusedFontChanging(true))
            {
                //_keyTimer.Start();
                this.SetFontToFocused();
            }
        }

        #region UI

        private DevExpress.XtraLayout.LayoutControl LayoutControl
        {
            get { return this.BaseEdit.StyleController as DevExpress.XtraLayout.LayoutControl; }
        }

        private void SuspendLayout()
        {
            DevExpress.XtraLayout.LayoutControl lc = LayoutControl;

            if (lc != null)
            {
                //lc.SuspendLayout();
                ((DevExpress.XtraLayout.LayoutControl)lc).BeginInit();
            }
        }

        private void ResumeLayout()
        {
            DevExpress.XtraLayout.LayoutControl lc = LayoutControl;

            if (lc != null)
            {
                //lc.ResumeLayout();
                ((DevExpress.XtraLayout.LayoutControl)lc).EndInit();
            }
        }

        internal void SetFontToFocused()
        {
            this.SuspendLayout();
            this.ResumeLayout();
        }

        #endregion

        #endregion

        void Properties_PropertiesChanged(object sender, EventArgs e)
        {
            if (_readOnly != Properties.ReadOnly)
            {
                _readOnly = Properties.ReadOnly;

                OnReadOnlyCahnged();
            }
        }

        protected virtual void OnReadOnlyCahnged()
        {
            this.BaseEdit.TabStop = !Properties.ReadOnly;
        }
    }

    internal class FocusedFontChangingEventArgs : System.EventArgs
    {
        internal FocusedFontChangingEventArgs(bool GotFocus)
        {
            gotFocus = GotFocus;
        }

        internal readonly bool gotFocus = false; 

        internal bool cancel = false;
    }

    internal delegate void FocusedFontChangingHandler(object sender, FocusedFontChangingEventArgs args);
}
