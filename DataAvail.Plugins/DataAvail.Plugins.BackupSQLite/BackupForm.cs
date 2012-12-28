using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using Ionic.Zip;
using System.Net.Mail;
using System.Net;
using DataAvail.Attributes;

namespace DataAvail.Plugins.BackupSQLite
{
    [PluginMenuItem("Инструменты/Бэкап")]
    public partial class BackupForm : Form
    {
        public BackupForm()
        {
            InitializeComponent();
        }

        ProcessingForm _processingForm = new ProcessingForm();

        private void bkpCheckBoxCheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            bkpFolderPathSelector.Properties.ReadOnly = !bkpCheckBoxCheckEdit.Checked;
        }

        private void archCheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            archivePasswordTextEdit.Properties.ReadOnly = !archCheckEdit.Checked;
        }

        private void mailCheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            smptpServTextEdit.Properties.ReadOnly =
            smptPortTextEdit.Properties.ReadOnly =
            mailPasswordTextEdit.Properties.ReadOnly =
            loginTextEdit.Properties.ReadOnly = !mailCheckEdit.Checked;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            BackupFormParams prms = DataAvail.Utils.XmlSerializer<BackupFormParams>.DeSerialize("SQLiteBackupFormParams");

            if (prms != null)
            {
                this.dbPathSelector.Text = prms.DbFileName;
                this.bkpCheckBoxCheckEdit.Checked = prms.CopyChecked;
                this.bkpFolderPathSelector.Text = prms.CopyFolder;
                this.archCheckEdit.Checked = prms.ArchChecked;
                this.archivePasswordTextEdit.Text = prms.ArchPassword;
                this.mailCheckEdit.Checked = prms.MailChecked;
                this.smptpServTextEdit.Text = prms.MailServerName;
                this.smptPortTextEdit.Text = prms.MailServerPort;
                this.loginTextEdit.Text = prms.MailLogin;
                this.mailPasswordTextEdit.Text = prms.MailPassword;
            }

        }

        private BackupFormParams GetBackupFormParams()
        {
            BackupFormParams prms = new BackupFormParams()
            {
                DbFileName = this.dbPathSelector.Text,
                CopyChecked = this.bkpCheckBoxCheckEdit.Checked,
                CopyFolder = this.bkpFolderPathSelector.Text,
                ArchChecked = this.archCheckEdit.Checked,
                ArchPassword = this.archivePasswordTextEdit.Text,
                MailChecked = this.mailCheckEdit.Checked,
                MailServerName = this.smptpServTextEdit.Text,
                MailServerPort = this.smptPortTextEdit.Text,
                MailLogin = this.loginTextEdit.Text,
                MailPassword = this.mailPasswordTextEdit.Text
            };



            return prms;
        }

        private void okSimpleButton_Click(object sender, EventArgs e)
        {
            DataAvail.Utils.XmlSerializer<BackupFormParams>.Serialize(GetBackupFormParams(), "SQLiteBackupFormParams");

            if (GetValidationErrors() != null)
            {
                System.Windows.Forms.MessageBox.Show(GetValidationErrors(), "Ошибка!", MessageBoxButtons.OK);

                return;
            }

            this.okSimpleButton.Enabled = this.cancelSimpleButton.Enabled = false;
            _processingForm.Show();

            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
            backgroundWorker.RunWorkerAsync(GetBackupFormParams());
        
        }

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackupFormParams prms = (BackupFormParams)e.Argument;

            DateTime dtNow = DateTime.Now;

            string copyFileName = string.Format(@"{0}\bkp_{1}", prms.CopyFolder, dtNow.ToString("g").Replace(".", "").Replace(" ", "").Replace(":", "")); 
            string sendFileName = prms.DbFileName;

            if (prms.CopyChecked)
            {
                if (prms.ArchChecked)
                {
                    using (ZipFile zip = new ZipFile())
                    {
                        if (!string.IsNullOrEmpty(prms.ArchPassword))
                            zip.Password = prms.ArchPassword;

                        zip.AddFile(prms.DbFileName, "");
                        sendFileName = string.Format("{0}.zip", copyFileName);
                        zip.Save(sendFileName);
                    }
                }
                else
                {
                    sendFileName = string.Format("{0}.db", copyFileName);
                    File.Copy(prms.DbFileName, sendFileName);
                }
            }

            if (prms.MailChecked)
            {
                string mailText = string.Format("Бэкап БД от [{0:F}]", dtNow);

                MailMessage message = new MailMessage(prms.MailLogin, prms.MailLogin, mailText, mailText);
                message.Attachments.Add(new Attachment(sendFileName));

                //smtp. 587
                SmtpClient smtpClient = new SmtpClient(prms.MailServerName, int.Parse(prms.MailServerPort));
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
               
                smtpClient.Credentials = new NetworkCredential(prms.MailLogin, prms.MailPassword);
                smtpClient.Send(message);

            }
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _processingForm.Close();
            this.Close();
        }

        private void cancelSimpleButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string GetValidationErrors()
        {
            if (string.IsNullOrEmpty(this.bkpFolderPathSelector.Text) || File.Exists(this.bkpFolderPathSelector.Text))
                return "Файл БД не выбран или не найден.";

            if (!this.bkpCheckBoxCheckEdit.Checked && !this.mailCheckEdit.Checked)
                return "Для выполнения операции бэкапа необхолимо выбрать опцию либо 1. Копировать в папку и/или 2. Отпралять на почту.";

            return null;
        }


       

    }



    public class BackupFormParams
    {
        public BackupFormParams()
        {
        }

        public string DbFileName { get; set; }
        public bool CopyChecked { get; set; }
        public string CopyFolder { get; set; }
        public bool ArchChecked { get; set; }
        public string ArchPassword { get; set; }
        public bool MailChecked { get; set; }
        public string MailServerName { get; set; }
        public string MailServerPort { get; set; }
        public string MailLogin { get; set; }
        public string MailPassword { get; set; }

        internal void DoBackup()
        { 
        
        }
    }
}
