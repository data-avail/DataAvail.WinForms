using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataAvail.XtraSearcherEditors
{
    public class BaseSearchEdit<T> where T : Control, IXtraSearchBaseEdit
    {
        public BaseSearchEdit(T BaseEdit, ICheckEdit CheckEdit)
        {
            _baseEdit = BaseEdit;

            _checkEdit = CheckEdit;
        }

        private readonly T _baseEdit;

        private readonly ICheckEdit _checkEdit;

        private T BaseEdit { get { return _baseEdit; } }

        private ICheckEdit CheckEdit { get { return _checkEdit; } }

        void CheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            SetChecked(CheckEdit.Checked);
        }

        void SetChecked()
        {
            SetChecked(CheckEdit.Checked);
        }

        void SetChecked(bool Checked)
        {
            CheckEdit.Checked = Checked;

            BaseEdit.ReadOnly = !Checked;
        }

        public void Initialize()
        {
            CheckEdit.CheckedChanged += new EventHandler(CheckEdit_CheckedChanged);

            Control ckd = (Control)CheckEdit;

            ckd.Location = new System.Drawing.Point(0, 0);

            ckd.Name = "checkEdit";

            BaseEdit.Controls.Add(ckd);

            BaseEdit.Controls[1].Dock = System.Windows.Forms.DockStyle.Left;

            BaseEdit.Controls[0].Dock = System.Windows.Forms.DockStyle.Fill;

            BaseEdit.Controls[0].TabIndex = 1;

            ckd.TabIndex = 0;
        }


        public void Serialize(System.Runtime.Serialization.SerializationInfo SerializationInfo)
        {
            SerializationInfo.AddValue("Checked", CheckEdit.Checked);

            SerializationInfo.AddValue("Value", BaseEdit.FormattedValue);

        }

        public void Deserialize(System.Runtime.Serialization.SerializationInfo SerializationInfo)
        {
            foreach (var r in SerializationInfo)
            {
                switch (r.Name)
                { 
                    case "Checked":
                        SetChecked(bool.Parse((string)r.Value));
                        break;
                    case "Value":
                        BaseEdit.FormattedValue = (string)r.Value;
                        break;
                }
            }
        }

        public bool Checked { get { return CheckEdit.Checked; } }

        public string GetExpression()
        {
            DataAvail.XtraEditors.IXtraEdit xtraEdit = BaseEdit as DataAvail.XtraEditors.IXtraEdit;

            if (xtraEdit != null && xtraEdit.EditValue != null && Checked)
            {
                if (xtraEdit.FieldType == typeof(string))
                {
                    return string.Format("{0} LIKE '%{1}%'", xtraEdit.FieldName, xtraEdit.EditValue);
                }
                else
                {
                    return string.Format("{0} = {1}", xtraEdit.FieldName, xtraEdit.EditValue);
                }
            }
            else
            {
                return null;
            }
        }

    }
}
