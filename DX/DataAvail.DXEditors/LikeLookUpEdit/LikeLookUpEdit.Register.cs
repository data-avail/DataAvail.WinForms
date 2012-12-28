using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;

namespace DataAvail.DXEditors
{
    partial class LikeLookUpEdit
    {
        #region Repository item and all that stuff

        static LikeLookUpEdit()
        {
            RepositoryItemLikeLookUpEdit.Register();
        }

        public override string EditorTypeName
        {
            get { return RepositoryItemLikeLookUpEdit.EditorName; }
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public new RepositoryItemLikeLookUpEdit Properties
        {
            get { return base.Properties as RepositoryItemLikeLookUpEdit; }
        }

        [UserRepositoryItem("Register")]
        public class RepositoryItemLikeLookUpEdit : DynamicCombo.RepositoryItemDynamicCombo
        {
            static RepositoryItemLikeLookUpEdit()
            {
                Register();
            }

            internal new const string EditorName = "DARepositoryItemLikeLookUpEdit";

            public new static void Register()
            {
                EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(EditorName,
                typeof(LikeLookUpEdit),
                typeof(RepositoryItemLikeLookUpEdit),
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
