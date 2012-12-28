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
    public class TextEdit : DevExpress.XtraEditors.TextEdit
    {
        static TextEdit()
        {
            RepositoryItemTextEdit.Register();
        } 

        public TextEdit()
        {
            _dgtr = new DXEditDelegator(this);

        }

        private readonly DXEditDelegator _dgtr;

        public override string EditorTypeName
        {
            get { return RepositoryItemTextEdit.EditorName; }
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public new RepositoryItemTextEdit Properties
        {
            get { return base.Properties as RepositoryItemTextEdit; }
        }

        [UserRepositoryItem("Register")]
        public class RepositoryItemTextEdit : DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
        {
            static RepositoryItemTextEdit()
            {
                Register();
            }

            public RepositoryItemTextEdit() 
            {
                //this.AppearanceFocused.Font = new System.Drawing.Font("Tahoma", 12.25F, FontStyle.Bold);
                //this.AppearanceFocused.Options.UseFont = true;
            }

            internal const string EditorName = "DARepositoryItemTextEdit";

            public static void Register()
            {
                EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(EditorName,
                typeof(TextEdit),
                typeof(RepositoryItemTextEdit),
                typeof(TextEditViewInfo),
                new DevExpress.XtraEditors.Drawing.TextEditPainter(), true));
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
                return new TextEditViewInfo(this);
            }

            public override BaseEditPainter CreatePainter()
            {
                return new TextEditPainter();
            }

            public class TextEditPainter : DevExpress.XtraEditors.Drawing.TextEditPainter
            {
                private readonly XtraEditorPainter _painter = new XtraEditorPainter();

                internal XtraEditorPainter Painter
                {
                    get { return _painter; }
                }
            }

            public class TextEditViewInfo : DevExpress.XtraEditors.ViewInfo.TextEditViewInfo
            {
                public TextEditViewInfo(DevExpress.XtraEditors.Repository.RepositoryItem owner) : base(owner) { }


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