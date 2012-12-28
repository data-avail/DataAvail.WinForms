using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataAvail.DX.XtraReportForm
{
    public partial class XtraReportForm : DataAvail.XtraReportForm.XtraReportForm 
    {
        public XtraReportForm(DataAvail.Controllers.Controller Controller)
            : base(Controller)
        {
        }

        protected override void InitializeUI()
        {
            InitializeComponent();

            base.InitializeUI();
        }

        protected override void OnXtraContainerCreating(DataAvail.XtraEditors.ObjectCreatingEventArgs args)
        {
            base.OnXtraContainerCreating(args);

            if (args.Object == null)
            {
                args.Object = new DataAvail.DX.XtraContainer.XtraContainer(Controller);
            }
        }

        protected override DataAvail.XtraMenu.IXtraMenu OnCreateFormMenu()
        {
            return new DataAvail.DX.XtraForm.XtraFormMenu(
                new DevExpress.XtraBars.BarButtonItem[] {
                    this.saveChangesBarButtonItem
                    , this.rejectChangesBarButtonItem
                    , this.endEditBarButtonItem
                    , this.cancelEditBarButtonItem
                    , this.movePervBarButtonItem
                    , this.moveNextBarButtonItem
                    , this.cloneBarButtonItem}, 
                 new DataAvail.XtraMenu.XtraMenuButtonType[] 
                    {
                        DataAvail.XtraMenu.XtraMenuButtonType.AcceptChanges,
                        DataAvail.XtraMenu.XtraMenuButtonType.RejectChanges,
                        DataAvail.XtraMenu.XtraMenuButtonType.EndEdit,
                        DataAvail.XtraMenu.XtraMenuButtonType.CancelEdit,
                        DataAvail.XtraMenu.XtraMenuButtonType.MovePerv,
                        DataAvail.XtraMenu.XtraMenuButtonType.MoveNext,
                        DataAvail.XtraMenu.XtraMenuButtonType.Clone
                    }
                 );
        }
    }
}
