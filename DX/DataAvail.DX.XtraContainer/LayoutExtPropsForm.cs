using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataAvail.DX.XtraContainer
{
    public partial class LayoutExtPropsForm : Form
    {
        public LayoutExtPropsForm()
        {
            InitializeComponent();
        }

        public LayoutExtPropsForm(DevExpress.XtraLayout.BaseLayoutItem[] LayoutControlItems)
            : this()
        {
            Initialize(LayoutControlItems);
        }

        internal void Initialize(DevExpress.XtraLayout.BaseLayoutItem[] LayoutControlItems)
        {
            if (LayoutControlItems != null)
            {
                this.propertyGrid1.SelectedObject = new ExtendeLayoutControlProperties(LayoutControlItems);
            }
            else
            {
                this.propertyGrid1.SelectedObject = null;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            ApplyCahnges();
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            ApplyCahnges();
        }

        private void ApplyCahnges()
        {
            if (this.propertyGrid1.SelectedObject != null)
                ((ExtendeLayoutControlProperties)this.propertyGrid1.SelectedObject).EndEdit();
        }

        public class ExtendeLayoutControlProperties
        {
            internal ExtendeLayoutControlProperties(DevExpress.XtraLayout.BaseLayoutItem[] LayoutControlItems)
            {
                layoutControlItems = LayoutControlItems;

                Padding = LayoutControlItems[0].Padding;
                TextToControlDistance = LayoutControlItems[0].TextToControlDistance;
                if (LayoutControlItems[0] is DevExpress.XtraLayout.LayoutControlItem)
                    this.TextAlignMode = ((DevExpress.XtraLayout.LayoutControlItem)LayoutControlItems[0]).TextAlignMode;

                foreach (DevExpress.XtraLayout.BaseLayoutItem item in layoutControlItems)
                {
                    if (Padding == item.Padding)
                        Padding = item.Padding;

                    if (TextToControlDistance == item.TextToControlDistance)
                        TextToControlDistance = item.TextToControlDistance;

                    if (item is DevExpress.XtraLayout.LayoutControlItem)
                        this.TextAlignMode = ((DevExpress.XtraLayout.LayoutControlItem)item).TextAlignMode;
                }
            }

            internal void EndEdit()
            {
                foreach (DevExpress.XtraLayout.BaseLayoutItem item in layoutControlItems)
                {
                    item.Padding = Padding;

                    item.TextToControlDistance = TextToControlDistance;

                    if (item is DevExpress.XtraLayout.LayoutControlItem)
                        ((DevExpress.XtraLayout.LayoutControlItem)item).TextAlignMode = this.TextAlignMode;
                }
            }

            private readonly DevExpress.XtraLayout.BaseLayoutItem[] layoutControlItems;

            private DevExpress.XtraLayout.Utils.Padding _padding;

            private int _textToControlDistance;

            private DevExpress.XtraLayout.TextAlignModeItem _textAlignMode;

            public DevExpress.XtraLayout.TextAlignModeItem TextAlignMode
            {
                get { return _textAlignMode; }
                set { _textAlignMode = value; }
            }

            public int TextToControlDistance
            {
                get { return _textToControlDistance; }
                set { _textToControlDistance = value; }
            }

            public DevExpress.XtraLayout.Utils.Padding Padding
            {
                get { return _padding; }
                set { _padding = value; }
            }
        }
    }
}
