using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XOP.AppContext
{
    public class Context
    {
        private ContextActiveType _activeType = ContextActiveType.None;

        private ContextActiveType _precActiveType = ContextActiveType.None;

        public void SetActiveType(ContextActiveType ActiveType)
        {
            _precActiveType = _activeType;

            _activeType = ActiveType;
        }

        public void ResetActiveType()
        {
            _activeType = _precActiveType;
        }

        public ContextActiveType ActiveType
        {
            get { return _activeType; }
        }
    }

    public enum ContextActiveType
    { 
        None,

        Item,

        List
    }

    public class DefaultContext : Context
    {
        public override bool Equals(object obj)
        {
            return obj.GetType() == typeof(DefaultContext);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static DefaultContext defaultContext = new DefaultContext();
    }

    public class ChildContext : Context
    {
        public ChildContext(AppItemContext Parent)
        {
            parent = Parent;
        }

        public readonly AppItemContext parent;

        public override bool Equals(object obj)
        {
            return obj.GetType() == typeof(ChildContext) && this.parent == ((ChildContext)obj).parent;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class SelectFkItemContext : Context
    {
        public SelectFkItemContext(DataAvail.XOP.AppContext.AppFieldContext ChildAppFieldContext)
        {
            childAppFieldContext = ChildAppFieldContext;
        }

        public readonly DataAvail.XOP.AppContext.AppFieldContext childAppFieldContext;

        public override bool Equals(object obj)
        {
            return obj.GetType() == typeof(SelectFkItemContext) && ((SelectFkItemContext)obj).childAppFieldContext == this.childAppFieldContext;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class AddFkItemContext : SelectFkItemContext
    {
        public AddFkItemContext(DataAvail.XOP.AppContext.AppFieldContext ChildAppFieldContext)
            : base(ChildAppFieldContext)
        {
        }

        public override bool Equals(object obj)
        {
            return obj.GetType() == typeof(AddFkItemContext) && ((AddFkItemContext)obj).childAppFieldContext == this.childAppFieldContext;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
