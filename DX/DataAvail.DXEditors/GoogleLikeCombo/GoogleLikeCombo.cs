using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Registrator;
using DataAvail.DXEditors.DX;


namespace DataAvail.DXEditors
{
    public partial class GoogleLikeCombo : DX.PopupContainerEdit, System.ComponentModel.ISupportInitialize
    {
        public GoogleLikeCombo()
        {
            InitializeComponent();

            _dataBindingSource.DataSource = _data;

            listBoxControl1.DataSource = _dataBindingSource;

            this.PreviewKeyDown += new PreviewKeyDownEventHandler(GoogleLikeCombo_PreviewKeyDown);
            
            this.KeyDown += new KeyEventHandler(PopupContainerEdit_KeyDown);

            this.Popup += new EventHandler(PopupContainerEdit_Popup);

            ((DX.ListBoxControl)this.ListBoxControl).RequestItemHighlight += new DX.RequestItemHighlightHandler(GoogleLikeCombo_RequestItemHighlight);
        }

        private readonly GoogleLikeComboOptions _options = new GoogleLikeComboOptions();

        private bool _initializing = false;

        private bool _textUpdating = false;

        private bool _isManualInput = false;

        private IGoogleLikeComboDataProvider _dataProvider;

        private List<GoogleLikeComboData> _data = new List<GoogleLikeComboData>();

        private readonly BindingSource _dataBindingSource = new BindingSource();

        private object _keyValue;

        private string _textValue;

        private string _dropDownText;

        private GoogleLikeComboData _selectedItem;

        public event System.EventHandler KeyValueChanged;

        public event System.EventHandler TextValueChanged;

        public event System.EventHandler DropDownTextChanged;

        public event System.EventHandler SelectedItemChanged;

        public GoogleLikeComboOptions Options
        {
            get { return _options; }
        }

        protected DevExpress.XtraEditors.ListBoxControl ListBoxControl
        {
            get { return listBoxControl1; }
        }

        public virtual object KeyValue
        {
            get { return _keyValue; }

            set
            {
                if (KeyValue != value && (KeyValue == null
                    || !KeyValue.Equals(value)))
                {
                    _keyValue = value;

                    if (!_initializing)
                        OnKeyValueChanged();
                }
            }

        }

        public string TextValue
        {
            get { return _textValue; }

            set
            {
                if (TextValue != value)
                {
                    _textValue = value;

                    this.UpdatePopupContainerEditText();

                    if (!_initializing)
                    {
                        OnTextValueChanged();
                    }
                }
            }
        }

        public string DropDownText
        {
            get 
            {
                return _dropDownText;
            }

            set
            {
                if (DropDownText != value)
                {
                    _dropDownText = value;

                    OnDropDownTextChanged();
                }
            }
        }


        public GoogleLikeComboData SelectedItem
        {
            get
            {
                return _selectedItem;
            }

            set
            {
                if (SelectedItem != value)
                {
                    _selectedItem = value;

                    OnSelectedItemChanged();
                }
            }
        }

        private void UpdateDropDownText()
        {
            if (SelectedItem != null)
            {
                GoogleLikeComboData item = ListBoxControl.SelectedItem as GoogleLikeComboData;

                DropDownText = item != null ? item.DropDownText : null;        
            }
        }

        private void UpdateSelectedItem()
        {
            SelectedItem = ListBoxControl.SelectedItem as GoogleLikeComboData;

            UpdateDropDownText();
        }

        public IGoogleLikeComboDataProvider DataProvider
        {
            get
            {
                return _dataProvider;
            }

            set
            {
                if (DataProvider != value)
                {
                    _dataProvider = value;

                    if (!_initializing)
                        OnDataProviderChanged();
                }
            }
        }

        protected virtual void OnDropDownTextChanged()
        {
            if (DropDownTextChanged != null)
                DropDownTextChanged(this, EventArgs.Empty);            
        }

        protected virtual void OnSelectedItemChanged()
        {
            if (SelectedItemChanged != null)
                SelectedItemChanged(this, EventArgs.Empty);
        }

        protected virtual void OnTextValueChanged()
        {
            if (TextValueChanged != null)
                TextValueChanged(this, EventArgs.Empty);

            if (_isManualInput)
            {
                if (LoadData())
                {
                    this.ShowPopup();
                }
                else
                {
                    this.ClosePopup(false);
                }

                SelectListBoxItem();
            }

            //try to find corresponded edit value
            GoogleLikeComboData GoogleLikeComboData = DataItems.FirstOrDefault(p => p.Text == this.TextValue);

            this.KeyValue = GoogleLikeComboData != null ? (object)GoogleLikeComboData.Key : null;
        }

        protected virtual void OnKeyValueChanged()
        {
            if (KeyValueChanged != null)
                KeyValueChanged(this, EventArgs.Empty);
        }


        protected virtual void OnDataProviderChanged()
        {
            LoadData();
        }

        protected IEnumerable<GoogleLikeComboData> DataItems
        {
            get { return _data; }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
        }

        private void PopupContainerEdit_TextChanged(object sender, EventArgs e)
        {
            if (!_textUpdating)
            {
                _isManualInput = true;
                this.TextValue = this.Text;
                _isManualInput = false;
            }
        }

        private void UpdatePopupContainerEditText()
        {
            _textUpdating = true;
            this.Text = TextValue;
            _textUpdating = false;
        }


        protected virtual string FindSuitableExpression
        {
            get { return this.TextValue; }
        }

        protected GoogleLikeComboData ListBoxSelectedItem
        {
            get { return (GoogleLikeComboData)this.ListBoxControl.SelectedValue; }
        }


        /// <summary>
        /// Select appropriate item, if exists, and highlight filters.
        /// </summary>
        protected bool SelectListBoxItem()
        {
            if (DataProvider != null && !string.IsNullOrEmpty(FindSuitableExpression))
            {
                GoogleLikeComboData data = DataItems.FirstOrDefault(p => p.IsMarked);

                if (data != null)
                {
                    this.ListBoxControl.SelectedValue = data;

                    this.ListBoxControl.Invalidate();

                    return true;
                }
            }

            this.ListBoxControl.SelectedValue = null;

            this.ListBoxControl.Invalidate();

            return false;

        }

        void GoogleLikeCombo_RequestItemHighlight(object sender, DX.RequestItemHighlightEventArgs args)
        {
            if (DataProvider != null && !string.IsNullOrEmpty(FindSuitableExpression) && args.text != FindSuitableExpression)
            {
                GoogleLikeComboData item = this.DataItems.FirstOrDefault(p => p.DropDownText == args.text);

                if (item != null && item.IsMarked)
                {
                    args.TextSelection = new DX.TextSelectionData(this.Options.SelectedTextColorListControl);

                    args.TextSelection.Markers = item.Markers.Select(p => new TextSelectionData.Marker() { Start = p.Start, End = p.End }).ToArray();
                }
            }
        }


        bool LoadData()
        {
            return LoadData(Text);
        }

        protected bool LoadData(string LikeExpression)
        {
            if (DataProvider != null)
            {
                //Load either all data suitable to LikeExpression or greater than it, or exacly that suited to defined filter
                GoogleLikeComboData[] data = DataProvider.GetData(LikeExpression, Options.MaxItemsCount).ToArray();

                if (data.Length != 0)
                {
                    _data.Clear();

                    _data.AddRange(data);

                    _dataBindingSource.ResetBindings(false);

                    return true;
                }
            }

            return false;
        }

        protected void AddData(GoogleLikeComboData GoogleLikeComboData)
        {
            _data.Add(GoogleLikeComboData);
        }

        void GoogleLikeCombo_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = HandlePreviewKey(e.KeyCode, e.IsInputKey);
        }

        void PopupContainerEdit_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = HandleKeyDown(e);
        }

        protected virtual bool HandlePreviewKey(Keys Key, bool IsInputKey)
        {
            switch (Key)
            {
                case Keys.Tab:
                    this.ClosePopup(false);
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

        public void BeginInit()
        {
            _initializing = true;
        }

        public void EndInit()
        {
            _initializing = false;
        }

        public void RefreshData()
        {
            LoadData();
        }

        public override void ShowPopup()
        {
            if (!IsPopupOpen)
            {
                if (DataItems.Count() < Options.MaxItemsCount)
                {
                    LoadData();
                }

                this.popupContainerControl1.Width = this.Width;

                base.ShowPopup();

                this.Focus();

                this.ListBoxControl.SelectedItem = SelectedItem;

                this.SelectionStart = this.Text.Length;
            }
        }

        private void ClosePopup(bool CatchValue)
        {
            if (IsPopupOpen)
            {
                if (CatchValue && this.ListBoxControl.SelectedValue != null)
                {
                    this.TextValue = ((GoogleLikeComboData)this.ListBoxControl.SelectedValue).Text;

                    this.UpdateSelectedItem();

                    base.ClosePopup();
                }
                else
                {
                    this.ClosePopup();
                }
            }
        }

        void PopupContainerEdit_Popup(object sender, EventArgs e)
        {
            SelectListBoxItem();
        }

        private void listBoxControl1_MouseClick(object sender, MouseEventArgs e)
        {
            ClosePopup(true);
        }

        protected override void OnPopupClosed(PopupCloseMode closeMode)
        {
            base.OnPopupClosed(closeMode);

            this.SelectionStart = this.Text.Length;

            
        }

        #region InitializeComponents

        private void InitializeComponent()
        {
            this.popupContainerControl1 = new DevExpress.XtraEditors.PopupContainerControl();
            this.listBoxControl1 = new DataAvail.DXEditors.DX.ListBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).BeginInit();
            this.popupContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // popupContainerControl1
            // 
            this.popupContainerControl1.Controls.Add(this.listBoxControl1);
            this.popupContainerControl1.Location = new System.Drawing.Point(15, 26);
            this.popupContainerControl1.Name = "popupContainerControl1";
            this.popupContainerControl1.Size = new System.Drawing.Size(383, 121);
            this.popupContainerControl1.TabStop = false;
            // 
            // listBoxControl1
            // 
            this.listBoxControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxControl1.HighlightedItemStyle = DevExpress.XtraEditors.HighlightStyle.Skinned;
            this.listBoxControl1.Location = new System.Drawing.Point(0, 0);
            this.listBoxControl1.Name = "listBoxControl1";
            this.listBoxControl1.Size = new System.Drawing.Size(383, 121);
            this.listBoxControl1.TabStop = false;
            this.listBoxControl1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listBoxControl1_MouseClick);
            // 
            // popupContainerEdit1
            //
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "popupContainerEdit1";
            this.Properties.ShowPopupCloseButton = false;
            this.Properties.ShowPopupShadow = false;
            this.Properties.PopupResizeMode = DevExpress.XtraEditors.Controls.ResizeMode.LiveResize;
            this.Properties.PopupSizeable = false;
            this.Properties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.NoBorder;
            this.Size = new System.Drawing.Size(180, 20);
            this.TextChanged += new System.EventHandler(this.PopupContainerEdit_TextChanged);
            this.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.Properties.PopupControl = popupContainerControl1;
            // 
            // GoogleLikeCombo
            // 
            this.Controls.Add(this.popupContainerControl1);
            this.Name = "GoogleLikeCombo";
            this.Size = new System.Drawing.Size(180, 19);
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).EndInit();
            this.popupContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        private DevExpress.XtraEditors.PopupContainerControl popupContainerControl1;
        private DX.ListBoxControl listBoxControl1;

        #endregion

    }

}
