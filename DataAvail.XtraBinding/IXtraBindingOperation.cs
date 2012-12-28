using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraBindings
{
    public interface IXtraBindingOperation 
    {
        void Show();

        void Fill(System.Linq.Expressions.Expression Expression);

        void Update(IEnumerable<object> Objects);

        Calculator.IObjectCalculator ObjectCalculator { get; }

        event XtraBindingOperationEndFillHandler EndFill;

        void StopFill();

        void Fill(IEnumerable<object> Items);

        DataAvail.Data.DataAdapter.DataAdapterSuppotedOperations SuppotedOperations { get; }

        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();


    }

    public class XtraBindingOperationEndFillEventArgs : System.EventArgs
    {
        public XtraBindingOperationEndFillEventArgs(bool Canceled, System.Exception Exception)
        {
            canceled = Canceled;

            exception = Exception;
        }

        public readonly bool canceled;

        public readonly System.Exception exception;
    }

    public delegate void XtraBindingOperationEndFillHandler(object sender, XtraBindingOperationEndFillEventArgs args);
}
