using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraBindings
{
    public class XtraBindingContainer : XtraBinding
    {
        public XtraBindingContainer(object DataSource, XtraBindingOperation XtraBindingOperation)
            : base(DataSource, XtraBindingOperation)
        {
        }

        public readonly XtraBindingsCollection Children = new XtraBindingsCollection();

        internal override XtraBinding Clone()
        {
            XtraBindingContainer cloned = new XtraBindingContainer(DataSource, xtraBindingOperation);

            foreach(XtraBindingChild child in this.Children)
            {
                cloned.Children.Add((XtraBindingChild)child.Clone(cloned));
            }

            return cloned;
        }

        public void FillChildren()
        {
            FillChildren(this.BindingSource.Position);
        }

        public void ClearChildren()
        {
            ClearChildren(this.BindingSource.Position);
        }

        public void FillChildren(int Index)
        {
            foreach (XtraBindingChild childBinding in Children)
            {
                childBinding.Fill(this.BindingSource[Index]);
            }
        }

        public void ClearChildren(int Index)
        {
            foreach (XtraBindingChild childBinding in Children)
            {
                childBinding.Clear();
            }
        }

    }
}
