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
    partial class AutoRefCombo
    {
        #region Repository item and all that stuff

        static AutoRefCombo()
        {
            RepositoryItemAutoRefCombo.Register();
        }

        public override string EditorTypeName
        {
            get { return RepositoryItemAutoRefCombo.EditorName; }
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public new RepositoryItemAutoRefCombo Properties
        {
            get { return base.Properties as RepositoryItemAutoRefCombo; }
        }

        [UserRepositoryItem("Register")]
        public class RepositoryItemAutoRefCombo : DynamicCombo.RepositoryItemDynamicCombo
        {
            static RepositoryItemAutoRefCombo()
            {
                Register();
            }

            internal new const string EditorName = "DARepositoryItemAutoRefCombo";

            public new static void Register()
            {
                EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(EditorName,
                typeof(AutoRefCombo),
                typeof(RepositoryItemAutoRefCombo),
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
