namespace DataAvail.Plugins.BackupSQLite
{
    partial class BackupForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.archCheckEdit = new DevExpress.XtraEditors.CheckEdit();
            this.mailCheckEdit = new DevExpress.XtraEditors.CheckEdit();
            this.bkpCheckBoxCheckEdit = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.archivePasswordTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.loginTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.smptPortTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.smptpServTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.bkpFolderPathSelector = new DataAvail.DXEditors.DX.PathSelector();
            this.okSimpleButton = new DevExpress.XtraEditors.SimpleButton();
            this.cancelSimpleButton = new DevExpress.XtraEditors.SimpleButton();
            this.dbPathSelector = new DataAvail.DXEditors.DX.PathSelector();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.mailPasswordTextEdit = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.archCheckEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mailCheckEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bkpCheckBoxCheckEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.archivePasswordTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.loginTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.smptPortTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.smptpServTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bkpFolderPathSelector.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbPathSelector.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mailPasswordTextEdit.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(7, 97);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(61, 13);
            this.labelControl4.TabIndex = 3;
            this.labelControl4.Text = "SMTP Server";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(5, 50);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(28, 13);
            this.labelControl5.TabIndex = 4;
            this.labelControl5.Text = "E-mail";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(93, 13);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "Путь до файла БД";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(7, 50);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(76, 13);
            this.labelControl2.TabIndex = 7;
            this.labelControl2.Text = "Пароль архива";
            // 
            // archCheckEdit
            // 
            this.archCheckEdit.Location = new System.Drawing.Point(5, 25);
            this.archCheckEdit.Name = "archCheckEdit";
            this.archCheckEdit.Properties.Caption = "Архивировать";
            this.archCheckEdit.Size = new System.Drawing.Size(105, 19);
            this.archCheckEdit.TabIndex = 8;
            this.archCheckEdit.CheckedChanged += new System.EventHandler(this.archCheckEdit_CheckedChanged);
            // 
            // mailCheckEdit
            // 
            this.mailCheckEdit.Location = new System.Drawing.Point(5, 25);
            this.mailCheckEdit.Name = "mailCheckEdit";
            this.mailCheckEdit.Properties.Caption = "Отправлять на почту";
            this.mailCheckEdit.Size = new System.Drawing.Size(131, 19);
            this.mailCheckEdit.TabIndex = 9;
            this.mailCheckEdit.CheckedChanged += new System.EventHandler(this.mailCheckEdit_CheckedChanged);
            // 
            // bkpCheckBoxCheckEdit
            // 
            this.bkpCheckBoxCheckEdit.EditValue = true;
            this.bkpCheckBoxCheckEdit.Location = new System.Drawing.Point(5, 29);
            this.bkpCheckBoxCheckEdit.Name = "bkpCheckBoxCheckEdit";
            this.bkpCheckBoxCheckEdit.Properties.Caption = "Копировать";
            this.bkpCheckBoxCheckEdit.Size = new System.Drawing.Size(131, 19);
            this.bkpCheckBoxCheckEdit.TabIndex = 10;
            this.bkpCheckBoxCheckEdit.CheckedChanged += new System.EventHandler(this.bkpCheckBoxCheckEdit_CheckedChanged);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(7, 55);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(39, 13);
            this.labelControl3.TabIndex = 11;
            this.labelControl3.Text = "В папку";
            // 
            // archivePasswordTextEdit
            // 
            this.archivePasswordTextEdit.Location = new System.Drawing.Point(5, 69);
            this.archivePasswordTextEdit.Name = "archivePasswordTextEdit";
            this.archivePasswordTextEdit.Properties.ReadOnly = true;
            this.archivePasswordTextEdit.Size = new System.Drawing.Size(255, 20);
            this.archivePasswordTextEdit.TabIndex = 12;
            // 
            // loginTextEdit
            // 
            this.loginTextEdit.Location = new System.Drawing.Point(74, 48);
            this.loginTextEdit.Name = "loginTextEdit";
            this.loginTextEdit.Properties.ReadOnly = true;
            this.loginTextEdit.Size = new System.Drawing.Size(185, 20);
            this.loginTextEdit.TabIndex = 13;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.archCheckEdit);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.archivePasswordTextEdit);
            this.groupControl1.Location = new System.Drawing.Point(12, 163);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(265, 98);
            this.groupControl1.TabIndex = 17;
            this.groupControl1.Text = "Архивация";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.labelControl6);
            this.groupControl2.Controls.Add(this.mailPasswordTextEdit);
            this.groupControl2.Controls.Add(this.smptPortTextEdit);
            this.groupControl2.Controls.Add(this.labelControl7);
            this.groupControl2.Controls.Add(this.smptpServTextEdit);
            this.groupControl2.Controls.Add(this.mailCheckEdit);
            this.groupControl2.Controls.Add(this.labelControl4);
            this.groupControl2.Controls.Add(this.labelControl5);
            this.groupControl2.Controls.Add(this.loginTextEdit);
            this.groupControl2.Location = new System.Drawing.Point(283, 57);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(265, 150);
            this.groupControl2.TabIndex = 18;
            this.groupControl2.Text = "Почта";
            // 
            // smptPortTextEdit
            // 
            this.smptPortTextEdit.EditValue = "";
            this.smptPortTextEdit.Location = new System.Drawing.Point(74, 120);
            this.smptPortTextEdit.Name = "smptPortTextEdit";
            this.smptPortTextEdit.Properties.ReadOnly = true;
            this.smptPortTextEdit.Size = new System.Drawing.Size(185, 20);
            this.smptPortTextEdit.TabIndex = 17;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(7, 123);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(55, 13);
            this.labelControl7.TabIndex = 16;
            this.labelControl7.Text = "Server Port";
            // 
            // smptpServTextEdit
            // 
            this.smptpServTextEdit.Location = new System.Drawing.Point(74, 95);
            this.smptpServTextEdit.Name = "smptpServTextEdit";
            this.smptpServTextEdit.Properties.ReadOnly = true;
            this.smptpServTextEdit.Size = new System.Drawing.Size(185, 20);
            this.smptpServTextEdit.TabIndex = 15;
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.bkpFolderPathSelector);
            this.groupControl3.Controls.Add(this.bkpCheckBoxCheckEdit);
            this.groupControl3.Controls.Add(this.labelControl3);
            this.groupControl3.Location = new System.Drawing.Point(12, 57);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(265, 100);
            this.groupControl3.TabIndex = 19;
            this.groupControl3.Text = "Копирование";
            // 
            // bkpFolderPathSelector
            // 
            this.bkpFolderPathSelector.Location = new System.Drawing.Point(5, 72);
            this.bkpFolderPathSelector.Name = "bkpFolderPathSelector";
            this.bkpFolderPathSelector.Properties.Appearance.Options.UseTextOptions = true;
            this.bkpFolderPathSelector.Properties.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisPath;
            this.bkpFolderPathSelector.Properties.AppearanceDisabled.Options.UseTextOptions = true;
            this.bkpFolderPathSelector.Properties.AppearanceDisabled.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisPath;
            this.bkpFolderPathSelector.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.bkpFolderPathSelector.Properties.ReadOnly = false;
            this.bkpFolderPathSelector.Properties.SelectorParams.DefaultPath = null;
            this.bkpFolderPathSelector.Properties.SelectorParams.Filter = null;
            this.bkpFolderPathSelector.Properties.SelectorParams.IsFolder = true;
            this.bkpFolderPathSelector.Properties.SelectorParams.IsOpen = false;
            this.bkpFolderPathSelector.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.bkpFolderPathSelector.Size = new System.Drawing.Size(255, 20);
            this.bkpFolderPathSelector.TabIndex = 24;
            // 
            // okSimpleButton
            // 
            this.okSimpleButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okSimpleButton.Location = new System.Drawing.Point(343, 231);
            this.okSimpleButton.Name = "okSimpleButton";
            this.okSimpleButton.Size = new System.Drawing.Size(75, 23);
            this.okSimpleButton.TabIndex = 21;
            this.okSimpleButton.Text = "OK";
            this.okSimpleButton.Click += new System.EventHandler(this.okSimpleButton_Click);
            // 
            // cancelSimpleButton
            // 
            this.cancelSimpleButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelSimpleButton.Location = new System.Drawing.Point(436, 231);
            this.cancelSimpleButton.Name = "cancelSimpleButton";
            this.cancelSimpleButton.Size = new System.Drawing.Size(75, 23);
            this.cancelSimpleButton.TabIndex = 22;
            this.cancelSimpleButton.Text = "Cancel";
            this.cancelSimpleButton.Click += new System.EventHandler(this.cancelSimpleButton_Click);
            // 
            // dbPathSelector
            // 
            this.dbPathSelector.Location = new System.Drawing.Point(12, 31);
            this.dbPathSelector.Name = "dbPathSelector";
            this.dbPathSelector.Properties.Appearance.Options.UseTextOptions = true;
            this.dbPathSelector.Properties.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisPath;
            this.dbPathSelector.Properties.AppearanceDisabled.Options.UseTextOptions = true;
            this.dbPathSelector.Properties.AppearanceDisabled.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisPath;
            this.dbPathSelector.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dbPathSelector.Properties.ReadOnly = false;
            this.dbPathSelector.Properties.SelectorParams.DefaultPath = null;
            this.dbPathSelector.Properties.SelectorParams.Filter = "db";
            this.dbPathSelector.Properties.SelectorParams.IsFolder = false;
            this.dbPathSelector.Properties.SelectorParams.IsOpen = true;
            this.dbPathSelector.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.dbPathSelector.Size = new System.Drawing.Size(536, 20);
            this.dbPathSelector.TabIndex = 23;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(5, 73);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(37, 13);
            this.labelControl6.TabIndex = 18;
            this.labelControl6.Text = "Пароль";
            // 
            // mailPasswordTextEdit
            // 
            this.mailPasswordTextEdit.Location = new System.Drawing.Point(74, 71);
            this.mailPasswordTextEdit.Name = "mailPasswordTextEdit";
            this.mailPasswordTextEdit.Properties.PasswordChar = '*';
            this.mailPasswordTextEdit.Properties.ReadOnly = true;
            this.mailPasswordTextEdit.Size = new System.Drawing.Size(185, 20);
            this.mailPasswordTextEdit.TabIndex = 19;
            // 
            // BackupForm
            // 
            this.AcceptButton = this.okSimpleButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 266);
            this.Controls.Add(this.dbPathSelector);
            this.Controls.Add(this.cancelSimpleButton);
            this.Controls.Add(this.okSimpleButton);
            this.Controls.Add(this.groupControl3);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.labelControl1);
            this.Name = "BackupForm";
            this.Text = "Бэкап";
            ((System.ComponentModel.ISupportInitialize)(this.archCheckEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mailCheckEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bkpCheckBoxCheckEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.archivePasswordTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.loginTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.smptPortTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.smptpServTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            this.groupControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bkpFolderPathSelector.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbPathSelector.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mailPasswordTextEdit.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.CheckEdit archCheckEdit;
        private DevExpress.XtraEditors.CheckEdit mailCheckEdit;
        private DevExpress.XtraEditors.CheckEdit bkpCheckBoxCheckEdit;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit archivePasswordTextEdit;
        private DevExpress.XtraEditors.TextEdit loginTextEdit;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.TextEdit smptPortTextEdit;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.TextEdit smptpServTextEdit;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.SimpleButton okSimpleButton;
        private DevExpress.XtraEditors.SimpleButton cancelSimpleButton;
        private DXEditors.DX.PathSelector bkpFolderPathSelector;
        private DXEditors.DX.PathSelector dbPathSelector;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.TextEdit mailPasswordTextEdit;

    }
}