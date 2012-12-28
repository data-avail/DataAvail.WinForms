using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.DXEditors.DX
{
    public class DXButtonEditDelegator : DXEditDelegator
    {
        public DXButtonEditDelegator(DevExpress.XtraEditors.ButtonEdit ButtonEdit)
            : base(ButtonEdit)
        {
        }

        private List<DevExpress.XtraEditors.Controls.EditorButton> _beforeReadOnlyButtons = new List<DevExpress.XtraEditors.Controls.EditorButton>();

        public new DevExpress.XtraEditors.ButtonEdit BaseEdit
        {
            get { return (DevExpress.XtraEditors.ButtonEdit)base.BaseEdit; }
        }

        public new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit Properties
        {
            get { return (DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit)BaseEdit.Properties; }
        }

        protected override void OnReadOnlyCahnged()
        {
            IEnumerable<DevExpress.XtraEditors.Controls.EditorButton> bts = null;

            if (Properties.ReadOnly)
            {
                _beforeReadOnlyButtons.Clear();

                _beforeReadOnlyButtons.AddRange(
                    this.Properties.Buttons.
                    Cast<DevExpress.XtraEditors.Controls.EditorButton>().Where(p => p.Visible));

                bts = this.Properties.Buttons.
                    Cast<DevExpress.XtraEditors.Controls.EditorButton>();
            }
            else
            {
                bts = _beforeReadOnlyButtons;
            }

            foreach (var b in bts)
                b.Visible = !Properties.ReadOnly;

            base.OnReadOnlyCahnged();
        }
    }
}
