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
using DevExpress.Skins;

namespace DataAvail.DXEditors.DX
{
    public class CheckEdit : DevExpress.XtraEditors.CheckEdit
    {
        static CheckEdit()
        {
            RepositoryItemCheckEdit.Register();
        }

        protected override void OnPreviewKeyDown(System.Windows.Forms.PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != System.Windows.Forms.Keys.None && (
                e.KeyCode == System.Windows.Forms.Keys.Left
                || e.KeyCode == System.Windows.Forms.Keys.Right
                || e.KeyCode == System.Windows.Forms.Keys.Down
                || e.KeyCode == System.Windows.Forms.Keys.Up)
                )
            {
                e.IsInputKey = true;
            }

            base.OnPreviewKeyDown(e);
        }

        public override string EditorTypeName
        {
            get { return RepositoryItemCheckEdit.EditorName; }
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public new RepositoryItemCheckEdit Properties
        {
            get { return base.Properties as RepositoryItemCheckEdit; }
        }

        [UserRepositoryItem("Register")]
        public class RepositoryItemCheckEdit : DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
        {
            static RepositoryItemCheckEdit()
            {
                Register();
            }
            public RepositoryItemCheckEdit() { }

            internal const string EditorName = "DARepositoryItemCheckEdit";

            public static void Register()
            {
                EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(EditorName,
                typeof(CheckEdit),
                typeof(RepositoryItemCheckEdit),
                typeof(CheckEditViewInfo),
                new DevExpress.XtraEditors.Drawing.CheckEditPainter(), true));
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
                    RepositoryItemCheckEdit source = item as RepositoryItemCheckEdit;
                    if (source == null) return;
                }
                finally
                {
                    EndUpdate();
                }
            }

            public override BaseEditViewInfo CreateViewInfo()
            {
                return new CheckEditViewInfo(this);
            }

            public override BaseEditPainter CreatePainter()
            {
                return new CheckEditPainter(); 
            }

            public class CheckEditPainter : DevExpress.XtraEditors.Drawing.CheckEditPainter
            {

                protected override void DrawCheck(ControlGraphicsInfoArgs info)
                {
                    /*
                    Skin currentSkin = CommonSkins.GetSkin(info.ViewInfo.LookAndFeel);
                    string elementName = CommonSkins.SkinGroupPanelLeft;
                    SkinElement element = currentSkin[elementName];
                    SkinElementInfo args = new SkinElementInfo(element, info.Bounds);
                    args.Cache = info.Cache;
                    args.State = info.ViewInfo.CalcBorderState();
                    args.ImageIndex = -1;
                    args.CalcRectangle(info.Bounds);
                    SkinElementPainter.Default.DrawObject(args);
                     */

                    base.DrawCheck(info);

                    //info.Cache.FillRectangle(Color.FromArgb(90, Color.Gold), info.CalcRectangle(info.Bounds));
                }

            }

            public class CheckObjectPainter : DevExpress.Utils.Drawing.SkinCheckObjectPainter 
            {
                internal CheckObjectPainter(ISkinProvider SkinProvider)
                    : base(SkinProvider)
                { }

                protected override void DrawCheckImage(DevExpress.Utils.Drawing.CheckObjectInfoArgs e)
                {
                    base.DrawCheckImage(e);

                    e.Cache.FillRectangle(Color.FromArgb(90, Color.Gold), e.GlyphRect);

                    e.Cache.DrawRectangle(new Pen(Color.Gold, 1), e.GlyphRect);
                }
            }

            public class CheckEditViewInfo : DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo
            {
                public CheckEditViewInfo(DevExpress.XtraEditors.Repository.RepositoryItem owner) : base(owner) { }

                public override bool DrawFocusRect
                {
                    get
                    {
                        return false;
                    }
                }

                protected override DevExpress.Utils.Drawing.CheckObjectPainter CreateCheckPainter()
                {

                    return this.OwnerEdit.ContainsFocus ? new CheckObjectPainter(LookAndFeel) : base.CreateCheckPainter();
                }
            }
        }
    }
}