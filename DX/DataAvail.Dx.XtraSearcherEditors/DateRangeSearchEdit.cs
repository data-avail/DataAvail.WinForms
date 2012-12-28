using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataAvail.XObject;

namespace DataAvail.DX.XtraSearcherEditors
{
    public partial class DateRangeSearchEdit : UserControl, DataAvail.XtraSearcherEditors.IXtraSearchEdit, DataAvail.XtraSearcherEditors.IXtraSearchBaseEdit, DataAvail.Serialization.ISerializableObject
    {
        public DateRangeSearchEdit(XOFieldContext XOFieldContext)
        {
            InitializeComponent();

            _searchEditDgtr = new DataAvail.XtraSearcherEditors.BaseSearchEdit<DateRangeSearchEdit>(this, new CheckEdit());

            _fieldName = XOFieldContext.Name;

            _fieldType = XOFieldContext.FieldType;

            _appFieldContext = XOFieldContext;

            _searchEditDgtr.Initialize();

            dateEdit2.PreviewKeyDown += new PreviewKeyDownEventHandler(dateEdit2_PreviewKeyDown);
        }

        private readonly string _fieldName;

        private readonly Type _fieldType;

        private readonly XOFieldContext _appFieldContext;

        private readonly DataAvail.XtraSearcherEditors.BaseSearchEdit<DateRangeSearchEdit> _searchEditDgtr;

        #region IXtraSearchEdit Members

        public string GetExpression()
        {
            IFormatProvider formatProvider = System.Globalization.CultureInfo.InvariantCulture;

            if (_searchEditDgtr.Checked)
            {
                if (this.dateEdit1.DateTime != System.DateTime.MinValue && this.dateEdit2.DateTime != System.DateTime.MinValue)
                {
                    return string.Format("{0} >= {1} AND {0} < {2}", this._appFieldContext.Name, this.dateEdit1.DateTime.ToString(formatProvider),
                        this.dateEdit2.DateTime.AddDays(1).ToString(formatProvider));
                }
                else if (this.dateEdit1.DateTime != System.DateTime.MinValue)
                {
                    return string.Format("{0} >= {1}", this._appFieldContext.Name, this.dateEdit1.DateTime.ToString(formatProvider));
                }
                else if (this.dateEdit2.DateTime != System.DateTime.MinValue)
                {
                    return string.Format("{0} < {1}", this._appFieldContext.Name, this.dateEdit2.DateTime.AddDays(1).ToString(formatProvider));
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        #endregion


        #region IXtraSearchBaseEdit Members

        public bool ReadOnly
        {
            get
            {
                return this.dateEdit1.Properties.ReadOnly;
            }
            set
            {
                this.dateEdit1.Properties.ReadOnly = this.dateEdit2.Properties.ReadOnly = value;

                this.TabStop = this.layoutControl1.TabStop = !ReadOnly;
            }
        }

        public string FormattedValue
        {
            get
            {
                return string.Format("{0}:{1}", DataAvail.DX.XtraEditors.DateEdit.GetFormattedValue(dateEdit1), DataAvail.DX.XtraEditors.DateEdit.GetFormattedValue(dateEdit2));
            }
            set
            {
                string[] strs = value.Split(new char[] { ':' });

                if (strs.Length > 0)
                {
                    DataAvail.DX.XtraEditors.DateEdit.SetFormattedValue(dateEdit1, strs[0]);
                }

                if (strs.Length > 1)
                {
                    DataAvail.DX.XtraEditors.DateEdit.SetFormattedValue(dateEdit2, strs[1]);
                }
            }
        }

        #endregion

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            this.Size = new Size(this.Width, this.dateEdit1.Height + 2);
        }

        #region ISerializableObject Members

        public void Serialize(System.Runtime.Serialization.SerializationInfo SerializationInfo)
        {
            _searchEditDgtr.Serialize(SerializationInfo);
        }

        public void Deserialize(System.Runtime.Serialization.SerializationInfo SerializationInfo)
        {
            _searchEditDgtr.Deserialize(SerializationInfo);
        }


        public DataAvail.Serialization.SerializationTag SerializationTag
        {
            get { return new DataAvail.Serialization.SerializationTag("Edit", _fieldName); }
        }

        public DataAvail.Serialization.ISerializableObject[] ChildrenSerializable
        {
            get { return null; }
        }

        #endregion

        void dateEdit2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {            
            if (e.Modifiers == Keys.None && e.KeyCode == Keys.Tab)
            {
                if (Parent is DevExpress.XtraLayout.LayoutControl)
                {
                    Control ctl = ((DevExpress.XtraLayout.LayoutControl)this.Parent).FocusHelper.GetNextControl(this);

                    ctl.Focus();

                    e.IsInputKey = false;
                }
            }
        }

    }
}
