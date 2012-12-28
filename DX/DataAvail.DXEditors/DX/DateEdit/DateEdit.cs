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
    public class DateEdit : DevExpress.XtraEditors.DateEdit
    {
        static DateEdit()
        {
            RepositoryItemDateEdit.Register();
        }

        public DateEdit()
        {
            _dgtr = new DXButtonEditDelegator(this);

            this.Properties.Mask.BeginUpdate();
            this.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            this.Properties.Mask.EditMask = "D";
            this.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.Properties.Mask.EndUpdate();
        }

        private readonly DXEditDelegator _dgtr;

        public override string EditorTypeName
        {
            get { return RepositoryItemDateEdit.EditorName; }
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public new RepositoryItemDateEdit Properties
        {
            get { return base.Properties as RepositoryItemDateEdit; }
        }

        [UserRepositoryItem("Register")]
        public class RepositoryItemDateEdit : DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
        {
            static RepositoryItemDateEdit()
            {
                Register();
            }
            public RepositoryItemDateEdit() { }

            internal const string EditorName = "DARepositoryItemDateEdit";

            public static void Register()
            {
                EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(EditorName,
                typeof(DateEdit),
                typeof(RepositoryItemDateEdit),
                typeof(DateEditViewInfo),
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
                    RepositoryItemDateEdit source = item as RepositoryItemDateEdit;
                    if (source == null) return;
                }
                finally
                {
                    EndUpdate();
                }
            }

            public override BaseEditViewInfo CreateViewInfo()
            {
                return new DateEditViewInfo(this);
            }

            public override BaseEditPainter CreatePainter()
            {
                return new DateEditPainter();
            }

            public class DateEditPainter : DevExpress.XtraEditors.Drawing.ButtonEditPainter
            {
                private readonly XtraEditorPainter _painter = new XtraEditorPainter();

                internal XtraEditorPainter Painter
                {
                    get { return _painter; }
                }
            }

            public class DateEditViewInfo : DevExpress.XtraEditors.ViewInfo.DateEditViewInfo
            {
                public DateEditViewInfo(DevExpress.XtraEditors.Repository.RepositoryItem owner) : base(owner) { }


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