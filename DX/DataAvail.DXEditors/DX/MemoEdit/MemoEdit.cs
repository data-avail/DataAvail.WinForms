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

namespace DataAvail.DXEditors.DX
{
    public class MemoEdit : DevExpress.XtraEditors.MemoEdit
    {
        static MemoEdit()
        {
            RepositoryItemMemoEdit.Register();
        }

        public MemoEdit()
        {
            _dgtr = new DXEditDelegator(this);
        }

        private readonly DXEditDelegator _dgtr;

        public override string EditorTypeName
        {
            get { return RepositoryItemMemoEdit.EditorName; }
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public new RepositoryItemTextEdit Properties
        {
            get { return base.Properties as RepositoryItemMemoEdit; }
        }

        [UserRepositoryItem("Register")]
        public class RepositoryItemMemoEdit : DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit
        {
            static RepositoryItemMemoEdit()
            {
                Register();
            }
            public RepositoryItemMemoEdit() { }

            internal const string EditorName = "DARepositoryItemMemoEdit";

            public static void Register()
            {
                EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(EditorName,
                typeof(MemoEdit),
                typeof(RepositoryItemMemoEdit),
                typeof(MemoEditViewInfo),
                new DevExpress.XtraEditors.Drawing.MemoEditPainter(), true));
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
                    RepositoryItemTextEdit source = item as RepositoryItemTextEdit;
                    if (source == null) return;
                }
                finally
                {
                    EndUpdate();
                }
            }

            public override BaseEditViewInfo CreateViewInfo()
            {
                return new MemoEditViewInfo(this);
            }

            public override BaseEditPainter CreatePainter()
            {
                return new MemoEditPainter();
            }

            public class MemoEditPainter : DevExpress.XtraEditors.Drawing.MemoEditPainter
            {
                private readonly XtraEditorPainter _painter = new XtraEditorPainter();

                internal XtraEditorPainter Painter
                {
                    get { return _painter; }
                }
            }

            public class MemoEditViewInfo : DevExpress.XtraEditors.ViewInfo.MemoEditViewInfo
            {
                public MemoEditViewInfo(DevExpress.XtraEditors.Repository.RepositoryItem owner) : base(owner) { }


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