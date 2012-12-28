using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baio.XtraReportForm
{
    public class XtraBindingOperation<T> : Baio.XtraBinding.XtraBindingOperation where T : Baio.XtraReportForm.XtraReportForm
    {
        public XtraBindingOperation(Baio.XtraBinding.XtraBinding XtraBinding, Baio.Data.IDataAdapter DataAdapter, Baio.XtraBinding.IObjectCalculator ObjectCalculator)
            : base(XtraBinding.DataSource, DataAdapter, ObjectCalculator)
        {
        }

        private T _xtraReportForm;

        public override void Show()
        {
            if (_xtraReportForm == null)
            {
                
            }

            _xtraReportForm.ShowDialog();
        }

        public override bool IsCanShow
        {
            get { return _xtraReportForm != null; }
        }
    }
}
