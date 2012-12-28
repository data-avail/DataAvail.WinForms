using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataAvail.WinUtils.MailForm
{
    public partial class MailForm : Form
    {
        public MailForm():this(null, null, null, false)
        {
        }

        public MailForm(string Subject, string Text, string CapturedFileName, bool CapturedFileAutoDelete)
        {
            InitializeComponent();

            this.textBox1.Enabled = MailSender.AllowChangeTo;

            this.textBox1.Text = MailSender.DefaultTo;

            this.textBox2.Text = Subject;

            if (!string.IsNullOrEmpty(Text))
            {
                this.textBox3.Text = string.Format("\r\n{0}", Text);
            }

            _capturedFileAutoDelete = CapturedFileAutoDelete;

            if (string.IsNullOrEmpty(CapturedFileName))
            {
                _captureScreenFileName = "ErrorReportCapturedScrn.jpg";

                try
                {
                    ScreenCapturer.Capture(this.ParentForm, _captureScreenFileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                catch (System.Exception)
                { }
            }
            else
            {
                _captureScreenFileName = CapturedFileName;
            }

            this.textBox1.GotFocus += new EventHandler(textBox1_GotFocus);

            this.textBox1.LostFocus += new EventHandler(textBox1_LostFocus);

            this.textBox3.GotFocus += new EventHandler(textBox3_GotFocus);

        }

        void textBox3_GotFocus(object sender, EventArgs e)
        {
            InputLanguage.SetRus();
        }

        public static DataAvail.WinUtils.Mail.IMailSender MailSender = new DataAvail.WinUtils.Mail.MailSender();

        public static IMailMessageFormater MailMessageForametter = null;//;new SoffitMailMessageFormatter("APP USER COMMENT"); 

        private readonly string _captureScreenFileName = null;

        private readonly bool _capturedFileAutoDelete = true;

        protected override void OnLoad(EventArgs e)
        {
            InputLanguage.Store();

            base.OnLoad(e);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            this.textBox3.Focus();

            this.textBox3.Select(0, 0);
        }

        void textBox1_GotFocus(object sender, EventArgs e)
        {
            InputLanguage.SetEng();
        }

        void textBox1_LostFocus(object sender, EventArgs e)
        {
            InputLanguage.SetRus();
        }

        protected override void OnClosed(EventArgs e)
        {
            InputLanguage.ReStore();

            base.OnClosed(e);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (Send(this.textBox1.Text, this.textBox2.Text, this.textBox3.Text, _captureScreenFileName, _capturedFileAutoDelete))
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.Abort;
            }

            this.Close();
        }

        public static void ShowForm()
        {
            ShowForm(null, null, null, true);
        }

        public static void ShowForm(string Subject, string Text, string CapturedFileName, bool CapturedFileAutoDelete)
        {
            MailForm form = new MailForm(Subject, Text, CapturedFileName, CapturedFileAutoDelete);

            form.ShowDialog();
        }

        public static bool Send(string To, string Subject, string Text, string CapturedFileName, bool CapturedFileAutoDelete)
        {
            try
            {
                string subj = Subject;

                string body = Text;

                if (MailForm.MailMessageForametter != null)
                {
                    subj = MailForm.MailMessageForametter.GetSubject(subj, body, body);

                    body = MailForm.MailMessageForametter.GetBody(body, body);
                }

                DataAvail.WinUtils.Mail.MailSenderQueue.Send(MailSender, To, subj, body, CapturedFileName, CapturedFileAutoDelete);
            }
            catch (System.Exception)
            {
                return false;
            }

            return true;
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string fullPath = DataAvail.WinUtils.Path.GetFilePath(_captureScreenFileName);

            try
            {
                System.Diagnostics.Process.Start(@"C:\WINDOWS\system32\mspaint.exe", fullPath);
            }
            catch (System.Exception)
            {
                System.Diagnostics.Process.Start(fullPath);
            }

        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MailForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.Enter)
            {
                okButton_Click(this.submitButton, EventArgs.Empty);
            }
        }

    }
}
