using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baio.XtraBinding
{
    public class XtraBindingOperationForm : Baio.XtraBinding.XtraBindingOperation
    {
        public XtraBindingOperationForm(object DataSource, Baio.Data.IDataAdapter DataAdapter, Baio.XtraBinding.IObjectCalculator ObjectCalculator)
            : base(DataSource, DataAdapter, ObjectCalculator)
        {
        }

        private System.Windows.Forms.Form _form;

        public System.Windows.Forms.Form Form
        {
            get { return _form; }

            set { _form = value; }
        }

        public override void Show()
        {
            if (_form != null)
            {
                _form.ShowDialog();
            }
        }

        public override bool IsCanShow
        {
            get { return _form != null; }
        }
    }
}
