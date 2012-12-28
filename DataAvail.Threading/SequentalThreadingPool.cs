using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Threading
{
    public class SequentalThreadingPool : System.IDisposable
    {
        Queue<object> _queue = new Queue<object>();

        System.ComponentModel.BackgroundWorker _backgroundWorker = new System.ComponentModel.BackgroundWorker();

        bool _workInProgress = false;

        public SequentalThreadingPool()
        {
            _backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(_backgroundWorker_DoWork);

            _backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(_backgroundWorker_RunWorkerCompleted);
        }

        void _backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            e.Result = e.Argument;

            OnDoWork(e.Argument);
        }

        void _backgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            OnWorkComplete(e.Result);
        }

        public void Add(object Object)
        {
            _queue.Enqueue(Object);

            DoNextWork();
        }

        //Working frame
        protected virtual void OnDoWork(object Object)
        {
            if (DoWork != null)
            { 
                DoWork(this, new SequentalThreadingPoolWorkEventArgs(Object));
            }
        }

        protected virtual void OnWorkComplete(object Object)
        {
            if (this.EndWork != null)
            { 
                EndWork(this, new SequentalThreadingPoolWorkEventArgs(Object));
            }

            _workInProgress = false;

            DoNextWork();
        }

        protected virtual void OnDoNextWork(object Object)
        {
            if (BeginWork != null)
            {
                BeginWork(this, new SequentalThreadingPoolWorkEventArgs(Object));
            }
        }

        protected virtual void OnWorkComplete()
        {
            if (WorkComplete != null)
            {
                WorkComplete(this, EventArgs.Empty);
            }
        }

        private void DoNextWork()
        {
            if (!_workInProgress)
            {
                if (_queue.Count > 0)
                {
                    _workInProgress = true;

                    object obj = _queue.Dequeue();

                    OnDoNextWork(obj);

                    //_backgroundWorker.RunWorkerAsync(obj);
                    DoInTheSameThread(obj);
                }
                else
                {
                    OnWorkComplete();
                }
            }
        }

        public event SequentalThreadingPoolDoWorkHandler BeginWork;

        public event SequentalThreadingPoolDoWorkHandler DoWork;

        public event SequentalThreadingPoolDoWorkHandler EndWork;

        public event System.EventHandler WorkComplete;

        #region IDisposable Members

        public void Dispose()
        {
            _backgroundWorker.Dispose();
        }

        #endregion

        private void DoInTheSameThread(object Object)
        {
            _backgroundWorker_DoWork(this, new System.ComponentModel.DoWorkEventArgs(Object));

            _backgroundWorker_RunWorkerCompleted(this, new System.ComponentModel.RunWorkerCompletedEventArgs(Object, null, false));

            _workInProgress = false;
        }
    }

    public class SequentalThreadingPoolWorkEventArgs : System.EventArgs
    {
        internal SequentalThreadingPoolWorkEventArgs(object Param)
        {
            param = Param;
        }

        public readonly object param;
    }

    public delegate void SequentalThreadingPoolDoWorkHandler(object obj, SequentalThreadingPoolWorkEventArgs args);
}
