using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace DataAvail.DXEditors
{
    /// <summary>
    /// Select list of items from provider by text typed in the control.
    /// If typed text can be defined as filter for an intem in selected list, this item become selected.
    /// </summary>

    public partial class DynamicCombo : GoogleLikeCombo
    {
        public DynamicCombo()
            : base()
        {
            this.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

            this.KeyUp += new System.Windows.Forms.KeyEventHandler(PopupContainerEdit_KeyUp);

            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(PopupContainerEdit_KeyPress);
        }

        private string _filterString;

        private bool _filterStingChanging = false;

        private System.Windows.Forms.Keys _lastPressedKey;

        private System.Windows.Forms.Keys LastPressedKey
        {
            get { return _lastPressedKey; }
        }

        private string FilterString
        {
            get { return _filterString; }

            set
            {
                if (FilterString != value)
                {
                    _filterString = value;

                    OnFilterStingChanged();
                }
            }
        }

        protected virtual void OnFilterStingChanged()
        {
            _filterStingChanging = true;

            SelectListBoxItem();

            if (ListBoxSelectedItem != null)
            {
                this.TextValue = ListBoxSelectedItem.Text;
            }

            SelectText();

            _filterStingChanging = false;
        }

        private void SelectText()
        {
            this.SelectText(FilterString, this.Options.SelectedTextColorTextEdit);
        }

        protected override string FindSuitableExpression
        {
            get
            {
                return this.FilterString != null ? this.FilterString : base.FindSuitableExpression;
            }
        }

        protected override bool HandleKeyDown(KeyEventArgs args)
        {
            _lastPressedKey = Keys.None;

            if (args.Modifiers == Keys.None)
            {
                Keys Key = args.KeyCode;

                _lastPressedKey = args.KeyCode;

                string newFilterStr = null;

                bool f = false;

                if (Key == System.Windows.Forms.Keys.Delete)
                {
                    newFilterStr = null;

                    f = true;
                }
                else if (Key == System.Windows.Forms.Keys.Back)
                {
                    if (!string.IsNullOrEmpty(_filterString))
                        newFilterStr = FilterString.Remove(_filterString.Length - 1, 1);
                    else
                        newFilterStr = null;

                    f = true;
                }

                if (f)
                {
                    this.FilterString = newFilterStr;

                    return true;
                }
            }

            return base.HandleKeyDown(args);
        }

        void PopupContainerEdit_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (LastPressedKey != Keys.None)
                HandlePressedKey(LastPressedKey, e.KeyChar);
        }


        private void HandlePressedKey(System.Windows.Forms.Keys Key, char KeyChar)
        {
            string chr = KeyChar.ToString();

            string newFilterStr = FilterString + chr;

            if (System.Char.IsSymbol((char)Key)
                || System.Char.IsLetterOrDigit((char)Key)
                || Key == System.Windows.Forms.Keys.Space)
            {
                if (!string.IsNullOrEmpty(newFilterStr))
                {
                    LoadData(newFilterStr);

                    if (DataProvider != null)
                    {
                        GoogleLikeComboData data = DataItems.FirstOrDefault(p => p.IsMarked);

                        if (data == null)
                        {
                            LoadData(chr);

                            data = DataItems.FirstOrDefault(p => p.IsMarked);

                            newFilterStr = data != null ? chr : null;
                        }
                    }

                }
            }

            this.FilterString = newFilterStr;

            this.ShowPopup();
        }

        private static string ConvertKeyCodeToString(System.Windows.Forms.Keys Key)
        {
            switch (Key)
            {
                case System.Windows.Forms.Keys.Space:
                    return " ";
            }

            return Key.ToString();
        }

        protected override void OnTextValueChanged()
        {
            base.OnTextValueChanged();

            //If text was changed somehow extarnally not through FilterString, reset filterString to null
            if (!_filterStingChanging)
            {
                _filterString = null;

                SelectText();
            }
        }

        void PopupContainerEdit_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {

            if (!IsPopupOpen)
            {
                switch (e.KeyCode)
                {
                    case System.Windows.Forms.Keys.Up:
                    case System.Windows.Forms.Keys.Down:
                        e.Handled = HandleKeyDown(e);
                        break;

                }
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if(!IsPopupOpen)
                FilterString = null;

            base.OnLostFocus(e);
        }
    }
}
