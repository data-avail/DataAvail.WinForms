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
    public class PopupContainerEdit : DevExpress.XtraEditors.PopupContainerEdit
    {
        static PopupContainerEdit()
        {
            RepositoryItemPopupContainerEdit.Register();
        }

        public PopupContainerEdit()
        {

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

        public void SelectText(string Text, System.Drawing.Color Color)
        {
            ((RepositoryItemPopupContainerEdit.PopupContainerEditPainter)this.Painter).Painter.
                SetTextSelection(this.Text, Text, Color);

            this.Invalidate();

        }

        public void SelectText(int FirstIndex, int Length, System.Drawing.Color Color)
        {
            ((RepositoryItemPopupContainerEdit.PopupContainerEditPainter)this.Painter).Painter.
                SetTextSelection(FirstIndex, Length, Color);

            this.Invalidate();
        }

        public override string EditorTypeName
        {
            get { return RepositoryItemPopupContainerEdit.EditorName; }
        }

         [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public new RepositoryItemPopupContainerEdit Properties
        {
            get { return base.Properties as RepositoryItemPopupContainerEdit; }
        }

        [UserRepositoryItem("Register")]
        public class RepositoryItemPopupContainerEdit : DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit
        {
            static RepositoryItemPopupContainerEdit()
            {
                Register();
            }
            public RepositoryItemPopupContainerEdit() 
            {
            }

            internal const string EditorName = "DARepositoryItemPopupContainerEdit";

            public static void Register()
            {
                EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(EditorName,
                typeof(PopupContainerEdit),
                typeof(RepositoryItemPopupContainerEdit),
                typeof(PopupContainerEditViewInfo),
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
                    RepositoryItemPopupContainerEdit source = item as RepositoryItemPopupContainerEdit;
                    if (source == null) return;
                }
                finally
                {
                    EndUpdate();
                }
            }

            public override BaseEditViewInfo CreateViewInfo()
            {
                return new PopupContainerEditViewInfo(this);
            }

            public override BaseEditPainter CreatePainter()
            {
                return new PopupContainerEditPainter();
            }

            public class PopupContainerEditPainter : DevExpress.XtraEditors.Drawing.ButtonEditPainter
            {
                private readonly XtraEditorPainter _painter = new XtraEditorPainter();

                internal XtraEditorPainter Painter
                {
                    get { return _painter; }
                }

                protected override void DrawText(ControlGraphicsInfoArgs info)
                {
                    base.DrawText(info);

                    Painter.DrawTextSelection(info);
                }

                
                protected override void DrawButton(ButtonEditViewInfo viewInfo, EditorButtonObjectInfoArgs info)
                {
                    base.DrawButton(viewInfo, info);
                }
                
            }
      
            public class PopupContainerEditViewInfo : DevExpress.XtraEditors.ViewInfo.PopupContainerEditViewInfo
            {
                public PopupContainerEditViewInfo(DevExpress.XtraEditors.Repository.RepositoryItem owner) : base(owner) { }


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
