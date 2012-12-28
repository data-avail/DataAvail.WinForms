using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Registrator;

namespace DataAvail.DXEditors
{
    partial class GoogleLikeCombo
    {
        #region Repository item and all that stuff

        static GoogleLikeCombo()
        {
            RepositoryItemGoogleLikeCombo.Register();
        }

        public override string EditorTypeName
        {
            get { return RepositoryItemGoogleLikeCombo.EditorName; }
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public new RepositoryItemGoogleLikeCombo Properties
        {
            get { return base.Properties as RepositoryItemGoogleLikeCombo; }
        }

        [UserRepositoryItem("Register")]
        public class RepositoryItemGoogleLikeCombo : DX.PopupContainerEdit.RepositoryItemPopupContainerEdit
        {
            static RepositoryItemGoogleLikeCombo()
            {
                Register();
            }

            internal new const string EditorName = "DARepositoryItemGoogleLikeCombo";

            public new static void Register()
            {
                EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(EditorName,
                typeof(GoogleLikeCombo),
                typeof(RepositoryItemGoogleLikeCombo),
                typeof(PopupContainerEditViewInfo),
                new DevExpress.XtraEditors.Drawing.ButtonEditPainter(), true));
            }

            public override string EditorTypeName
            {
                get { return EditorName; }
            }
        }

        #endregion
    }
}
