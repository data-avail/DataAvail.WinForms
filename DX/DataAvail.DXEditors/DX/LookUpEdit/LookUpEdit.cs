using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Registrator;
using System.Drawing;
using DevExpress.XtraEditors.ListControls;

namespace DataAvail.DXEditors.DX
{
    public class LookUpEdit : DevExpress.XtraEditors.LookUpEdit
    {
        static LookUpEdit()
        {
            RepositoryItemLookUpEdit.Register();
        }

        public LookUpEdit()
        {
            _dgtr = new DXButtonEditDelegator(this);
        }

        private readonly DXEditDelegator _dgtr;

        public override string EditorTypeName
        {
            get { return RepositoryItemLookUpEdit.EditorName; }
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public new RepositoryItemLookUpEdit Properties
        {
            get { return base.Properties as RepositoryItemLookUpEdit; }
        }

        protected override void OnPreviewKeyDown(System.Windows.Forms.PreviewKeyDownEventArgs e)
        {
            if (e.Control && 
                (e.KeyCode == System.Windows.Forms.Keys.Left || 
                e.KeyCode == System.Windows.Forms.Keys.Right || 
                e.KeyCode == System.Windows.Forms.Keys.Down || 
                e.KeyCode == System.Windows.Forms.Keys.Up))
            {
                e.IsInputKey = true;
            }

            base.OnPreviewKeyDown(e);
        }

        [UserRepositoryItem("Register")]
        public class RepositoryItemLookUpEdit : DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
        {
            static RepositoryItemLookUpEdit()
            {
                Register();
            }

            public RepositoryItemLookUpEdit() 
            {
                this.HighlightedItemStyle = HighlightStyle.Skinned;
            }

            internal const string EditorName = "DARepositoryItemLookUpEdit";

            public static void Register()
            {
                EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(EditorName,
                typeof(LookUpEdit),
                typeof(RepositoryItemLookUpEdit),
                typeof(LookUpEditViewInfo),
                new DevExpress.XtraEditors.Drawing.ButtonEditPainter(), true));
            }

            public override string EditorTypeName
            {
                get { return EditorName; }
            }

            public override void Assign(RepositoryItem item)
            {
                BeginUpdate();
                try
                {
                    base.Assign(item);
                    RepositoryItemLookUpEdit source = item as RepositoryItemLookUpEdit;
                    if (source == null) return;
                }
                finally
                {
                    EndUpdate();
                }
            }

            public override BaseEditViewInfo CreateViewInfo()
            {
                return new LookUpEditViewInfo(this);
            }

            public override BaseEditPainter CreatePainter()
            {
                return new LookUpEditPainter();
            }

            public class LookUpEditPainter : DevExpress.XtraEditors.Drawing.ButtonEditPainter
            {
                private readonly XtraEditorPainter _painter = new XtraEditorPainter();

                internal XtraEditorPainter Painter
                {
                    get { return _painter; }
                }
            }

            public class LookUpEditViewInfo : DevExpress.XtraEditors.ViewInfo.LookUpEditViewInfo
            {
                public LookUpEditViewInfo(DevExpress.XtraEditors.Repository.RepositoryItem owner) : base(owner) { }

                public override bool DrawFocusRect
                {
                    get
                    {
                        return false;
                    }
                }

                protected override DevExpress.Utils.Drawing.BorderPainter GetBorderPainter()
                {
                    return OwnerEdit.ContainsFocus ? new DXBorderPinter() : base.GetBorderPainter();
                }
           }
        }
    }
}