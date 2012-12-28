using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Linq;

namespace DataAvail.DX.XtraListForm
{
    public partial class XtraListForm : DataAvail.XtraListForm.XtraListForm
    {
        public XtraListForm(DataAvail.Controllers.Controller Controller)
            :base(Controller)
        {
        }

        protected override void InitializeUI()
        {
            InitializeComponent();

            base.InitializeUI();
        }

        protected override DataAvail.XtraMenu.IXtraMenu OnCreateFormMenu()
        {
            return new DataAvail.DX.XtraForm.XtraFormMenu(
                new DevExpress.XtraBars.BarButtonItem[] { 
                    this.updateBarButtonItem,
                    this.rejectBarButtonItem,
                    this.uploadToExcelBarButtonItem,
                    this.refillBarButtonItem,
                    this.barButtonItem1},
                new DataAvail.XtraMenu.XtraMenuButtonType[] { 
                    DataAvail.XtraMenu.XtraMenuButtonType.AcceptChanges,
                    DataAvail.XtraMenu.XtraMenuButtonType.RejectChanges,
                    DataAvail.XtraMenu.XtraMenuButtonType.UploadToExcel,
                    DataAvail.XtraMenu.XtraMenuButtonType.Refill,
                    DataAvail.XtraMenu.XtraMenuButtonType.StopFill}                    
                    );
        }

        #region Children creation

        protected override void OnXtraGridCreating(DataAvail.XtraEditors.ObjectCreatingEventArgs args)
        {
            base.OnXtraGridCreating(args);

            if (args.Object == null)
            {
                args.Object = new DataAvail.DX.XtraGrid.XtraGrid((DataAvail.Controllers.Controller)args.controller);
            }
        }

        protected override void OnXtraSearchContainerCreating(DataAvail.XtraEditors.ObjectCreatingEventArgs args)
        {
            base.OnXtraSearchContainerCreating(args);

            if (args.Object == null)
            {
                args.Object = new DataAvail.DX.XtraSearcherContainer.XtraSearchContainer(Controller);
            }
        }

        #endregion

        #region Serialization

        public override void Serialize(System.Runtime.Serialization.SerializationInfo SerializationInfo)
        {
            base.Serialize(SerializationInfo);

            SerializationInfo.AddValue("SplitterPosition", this.splitContainerControl1.SplitterPosition);
        }

        public override void Deserialize(System.Runtime.Serialization.SerializationInfo SerializationInfo)
        {
            base.Deserialize(SerializationInfo);

            try
            {
                this.splitContainerControl1.SplitterPosition = (int)SerializationInfo.GetValue("SplitterPosition", typeof(int));
            }
            catch (System.Runtime.Serialization.SerializationException)
            { }
        }

        #endregion
    }
}