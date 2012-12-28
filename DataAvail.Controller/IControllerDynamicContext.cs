using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;
using DataAvail.XObject.XContexts;


namespace DataAvail.Controllers
{
    public interface IControllerDynamicContext
    {
        XOTableContext CurrentTableContext { get; }

        event System.EventHandler TableContextChanged;
    }

    public class ControllerDynamicContext : IControllerDynamicContext
    {
        public ControllerDynamicContext(XOTableContext XOTableContext)
        {
            AddAppItemContext(XOTableContext, true);
        }

        private readonly List<XOTableContext> _items = new List<XOTableContext>();

        private XOTableContext _current;

        public void AddAppItemContext(XOTableContext TableContext, bool SetCurrent)
        {
            _items.Add(TableContext);

            if (SetCurrent)
                SetCurrentAppItemContext(TableContext);
        }

        public void SetCurrentAppItemContext(XOTableContext TableContext)
        {
            if (CurrentTableContext != TableContext)
            {
                _current = TableContext;

                OnTableContextChanged();
            }
        }

        public XOTableContext GetAppItemContext(XContext Context)
        {
            return _items.FirstOrDefault(p => p.Context.Equals(Context));
        }

        private void OnTableContextChanged()
        {
            if (TableContextChanged != null)
            {
                TableContextChanged(this, EventArgs.Empty);
            }
        }

        #region IControllerDynamicContext Members

        public event System.EventHandler TableContextChanged;

        public XOTableContext CurrentTableContext
        {
            get { return _current; }
        }

        #endregion
    }
}
