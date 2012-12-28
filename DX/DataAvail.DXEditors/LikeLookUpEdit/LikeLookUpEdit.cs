using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors.Registrator;
using System.Windows.Forms;

namespace DataAvail.DXEditors
{
    public partial class LikeLookUpEdit : DX.PopupContainerEdit
    {
        public LikeLookUpEdit()
        {
            InitializeComponent();

            this.PreviewKeyDown += new PreviewKeyDownEventHandler(GoogleLikeCombo_PreviewKeyDown);

            this.KeyDown += new KeyEventHandler(PopupContainerEdit_KeyDown);

            this.listBoxControl1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listBoxControl1_MouseClick);

            ((DX.ListBoxControl)this.ListBoxControl).RequestItemHighlight += new DX.RequestItemHighlightHandler(GoogleLikeCombo_RequestItemHighlight);
        }

        private object _dataSource;

        private string _valueMember;

        private string _displayMember;

        public object DataSource
        {
            get { return _dataSource; }

            set
            {
                if (DataSource != value)
                {
                    _dataSource = value;

                    OnDataSourceChanged();
                }
            }
        }

        public string ValueMember
        {
            get { return _valueMember; }

            set
            {
                if (ValueMember != value)
                {
                    _valueMember = value;

                    OnValueMemberChanged();
                }

            }
        }

        public string DisplayMember
        {
            get { return _displayMember; }

            set
            {
                if (DisplayMember != value)
                {
                    _displayMember = value;

                    OnDisplayMemberChanged();
                }
            }
        }

        protected virtual void OnValueMemberChanged()
        {
            ListBoxControl.ValueMember = ValueMember;
        }

        protected virtual void OnDisplayMemberChanged()
        {
            ListBoxControl.DisplayMember = DisplayMember;
        }

        protected virtual void OnDataSourceChanged()
        {
            ListBoxControl.DataSource = DataSource;
        }


        protected DevExpress.XtraEditors.ListBoxControl ListBoxControl
        {
            get { return listBoxControl1; }
        }

        #region events handling

        void GoogleLikeCombo_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = HandlePreviewKey(e.KeyCode, e.IsInputKey);
        }

        void PopupContainerEdit_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = HandleKeyDown(e);
        }


        private void listBoxControl1_MouseClick(object sender, MouseEventArgs e)
        {
            ClosePopup(true);
        }

        #endregion

        public override void ShowPopup()
        {
            if (!IsPopupOpen)
            {
                base.ShowPopup();

                this.Focus();
            }
        }

        private void ClosePopup(bool CatchValue)
        {
            if (IsPopupOpen)
            {
                if (CatchValue && this.ListBoxControl.SelectedValue != null)
                {
                    //this.TextValue = ((GoogleLikeComboData)this.ListBoxControl.SelectedValue).Text;

                    base.ClosePopup();
                }
                else
                {
                    this.ClosePopup();
                }
            }
        }

        protected virtual bool HandlePreviewKey(Keys Key, bool IsInputKey)
        {
            switch (Key)
            {
                case Keys.Tab:
                    ClosePopup(true);
                    return false;
            }

            return IsInputKey;
        }

        protected virtual bool HandleKeyDown(KeyEventArgs args)
        {
            if (args.Modifiers == Keys.None)
            {
                switch (args.KeyCode)
                {
                    case Keys.Return:
                        if (!IsPopupOpen)
                            ShowPopup();
                        else
                            ClosePopup(true);
                        return true;
                    case Keys.Escape:
                        ClosePopup(false);
                        return true;
                    case Keys.Down:
                        this.ListBoxControl.SelectedIndex++;
                        ShowPopup();
                        return true;
                    case Keys.Up:
                        this.ListBoxControl.SelectedIndex--;
                        ShowPopup();
                        return true;
                }
            }

            return false;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            this.ShowPopup();

            this.ListBoxControl.Invalidate();
        }

        void GoogleLikeCombo_RequestItemHighlight(object sender, DX.RequestItemHighlightEventArgs args)
        {
            if (!string.IsNullOrEmpty(this.Text) && args.text != this.Text)
            {
                int i = args.text.IndexOf(this.Text);

                if (i != -1)
                    args.TextSelection = new DX.TextSelectionData(i, this.Text.Length, System.Drawing.Color.FromArgb(80, System.Drawing.Color.Blue));
            }
        }
    }
}
