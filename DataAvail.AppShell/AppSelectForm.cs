using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataAvail.AppShell
{
    public partial class AppSelectForm : Form
    {
        public AppSelectForm()
        {
            InitializeComponent();
        }

        internal AppSelectForm(StartupBunch StartupBunch)
        {
            InitializeComponent();

            this.imageListBoxControl1.Items.AddRange(StartupBunch.AppConfigs);

            this.imageListBoxControl1.SetSelected(0, true);
        }

        private AppConfig _selectedAppConfig;

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _selectedAppConfig = imageListBoxControl1.SelectedValue as AppConfig;

            base.OnFormClosing(e);
        }

        internal static AppConfig ShowForm(StartupBunch StartupBunch)
        {
            AppSelectForm appSelectForm = new AppSelectForm(StartupBunch);

            appSelectForm.ShowDialog();

            return appSelectForm.DialogResult == DialogResult.OK ? appSelectForm._selectedAppConfig : null;
        }

        private void imageListBoxControl1_DoubleClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

            this.Close();
        }
    }
}
