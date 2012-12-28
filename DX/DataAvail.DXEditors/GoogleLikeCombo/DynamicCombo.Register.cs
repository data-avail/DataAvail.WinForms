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
    partial class DynamicCombo
    {
        #region Repository item and all that stuff

        static DynamicCombo()
        {
            RepositoryItemDynamicCombo.Register();
        }

        public override string EditorTypeName
        {
            get { return RepositoryItemDynamicCombo.EditorName; }
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public new RepositoryItemDynamicCombo Properties
        {
            get { return base.Properties as RepositoryItemDynamicCombo; }
        }

        [UserRepositoryItem("Register")]
        public class RepositoryItemDynamicCombo : GoogleLikeCombo.RepositoryItemGoogleLikeCombo
        {
            static RepositoryItemDynamicCombo()
            {
                Register();
            }

            internal new const string EditorName = "DARepositoryItemDynamicCombo";

            public new static void Register()
            {
                EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(EditorName,
                typeof(DynamicCombo),
                typeof(RepositoryItemDynamicCombo),
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
