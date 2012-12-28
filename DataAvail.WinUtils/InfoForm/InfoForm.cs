using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataAvail.WinUtils.InfoForm
{
    public partial class InfoForm : Form
    {
        public InfoForm()
            : this(InfoFormSendReportType.Default)
        {
        }

        public InfoForm(InfoFormSendReportType SendReportType)
        {
            InitializeComponent();

            this.sendReportType = SendReportType;

            if (sendReportType == InfoFormSendReportType.Disabled)
            {
                this.sendButton.Visible = false;
            }
        }

        private bool _isExpand = false;

        public static IInfoFormTextParser InfoTextParser = null;

        public static string DEFAULT_CAPTION = "Error";

        public static string CS_FILE_NAME = "ErrorReportCapturedScrn.jpg";

        public readonly InfoFormSendReportType sendReportType = InfoFormSendReportType.Default;

        public void ShowForm(System.Windows.Forms.Form ParentForm, string Caption, System.Exception ex)
        {
            ShowForm(ParentForm, Caption, ex.Message, ex.StackTrace);
        }

        public void ShowForm(System.Windows.Forms.Form ParentForm, string Caption, string Text)
        {
            ShowForm(ParentForm, Caption, Text, null);
        }

        public void ShowForm(System.Windows.Forms.Form ParentForm, string Caption, string Text, string Trace)
        {
            string mn = Text;

            string aux = Text;

            if (InfoTextParser != null)
            {
                mn = InfoTextParser.GetMainPart(Text);

                aux = InfoTextParser.GetAuxPart(Text);
            }

            ShowForm(ParentForm, Caption, mn.Replace("\n", "\r\n"), aux.Replace("\n", "\r\n"), Trace);
        }

        public void ShowForm(System.Windows.Forms.Form ParentForm, string Caption, string Text, string AuxText, string Trace)
        {
            this.Text = Caption == null ? DEFAULT_CAPTION : Caption;

            this.textBox1.Text = Text;

            this.textBox2.Text = string.IsNullOrEmpty(Trace) ? AuxText : string.Format("{0}\r\n===========================\r\nStack trace:\r\n{1}", AuxText, Trace);

            if (System.Threading.Thread.CurrentThread.GetApartmentState() != System.Threading.ApartmentState.STA)
            {
                this.copyButton.Visible = false;
            }
            else
            {
                this.copyButton.Visible = true;
            }

            this.ShowDialog(ParentForm);
        }

        #region static Show


        #region with parent form parameter

        public static void Show(System.Windows.Forms.Form ParentForm, string Caption, string Text, InfoFormSendReportType SendReportType)
        {
            Show(ParentForm, Caption, Text, null, SendReportType);
        }

        public static void Show(System.Windows.Forms.Form ParentForm, string Text)
        {
            Show(ParentForm, null, Text);
        }

        public static void Show(System.Windows.Forms.Form ParentForm, string Caption, string Text)
        {
            Show(ParentForm, Caption, Text, InfoFormSendReportType.Default);
        }

        public static void Show(System.Windows.Forms.Form ParentForm, string Text, InfoFormSendReportType SendReportType)
        {
            Show(ParentForm, (string)null, Text, SendReportType);
        }

        public static void Show(System.Windows.Forms.Form ParentForm, string Caption, System.Exception Exception, InfoFormSendReportType SendReportType)
        {
            InfoForm infoForm = new InfoForm(SendReportType);

            infoForm.ShowForm(ParentForm, Caption, Exception);
        }

        public static void Show(System.Windows.Forms.Form ParentForm, string Caption, string Text, string AuxText, InfoFormSendReportType SendReportType)
        {
            InfoForm infoForm = new InfoForm(SendReportType);

            if (AuxText != null)
            {
                infoForm.ShowForm(ParentForm, Caption, Text, AuxText, null);
            }
            else
            {
                infoForm.ShowForm(ParentForm, Caption, Text, null);
            }
        }

        #endregion

        #region without parent form parameter

        public static void Show(string Caption, string Text, InfoFormSendReportType SendReportType)
        {
            Show((System.Windows.Forms.Form)null, Caption, Text, SendReportType);
        }

        public static void Show(string Text)
        {
            Show((System.Windows.Forms.Form)null, Text);
        }

        public static void Show(string Caption, string Text)
        {
            Show((System.Windows.Forms.Form)null, Caption, Text);
        }

        public static void Show(string Text, InfoFormSendReportType SendReportType)
        {
            Show((System.Windows.Forms.Form)null, Text, SendReportType);
        }

        public static void Show(string Caption, System.Exception Exception, InfoFormSendReportType SendReportType)
        {
            Show((System.Windows.Forms.Form)null, Caption, Exception, SendReportType);
        }

        public static void Show(string Caption, string Text, string AuxText, InfoFormSendReportType SendReportType)
        {
            Show((System.Windows.Forms.Form)null, Caption, Text, AuxText, SendReportType);
        }

        #endregion

        #endregion


        private void expandButton_Click(object sender, EventArgs e)
        {
            if (_isExpand)
            {
                _isExpand = false;

                this.expandButton.Text = "Подробнее >>>";

                this.Height = 195;
            }
            else
            {
                _isExpand = true;

                this.expandButton.Text = "Свернуть <<<";

                this.Height = 500;
            }
        }

        private void oKbutton_Click(object sender, EventArgs e)
        {
            SendReport(false);

            this.Close();
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.textBox2.Text);
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            SendReport(true);

            this.Close();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!string.IsNullOrEmpty(Mail.MailSender.MAIL_PARAMS.ServerName) && !string.IsNullOrEmpty(Mail.MailSender.MAIL_PARAMS.MailFrom))
            {
                try
                {
                    ScreenCapturer.Capture(this.ParentForm, CS_FILE_NAME, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                catch (System.Exception)
                { }
            }
            else
            {
                this.sendButton.Visible = false;
            }


            base.OnLoad(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            SendReport(false);

            base.OnClosed(e);
        }

        private void SendReport(bool IsOnSend)
        {
            if (sendReportType == InfoFormSendReportType.Default && !IsOnSend) return;


            if (sendReportType == InfoFormSendReportType.Default || (sendReportType == InfoFormSendReportType.SilentOnOKAndDefault && IsOnSend))
            {

                DataAvail.WinUtils.MailForm.MailForm.ShowForm(DEFAULT_CAPTION, string.Format("============\r\n{0}\r\n============\r\n", this.textBox2.Text), CS_FILE_NAME, true);
            }
            else
            {
                DataAvail.WinUtils.MailForm.MailForm.Send(DataAvail.WinUtils.MailForm.MailForm.MailSender.DefaultTo, DEFAULT_CAPTION, this.textBox2.Text, CS_FILE_NAME, true);

            }
        }


        public void InsertButton(InfoFormButtonType AfterButtonType, System.Windows.Forms.Button Button)
        {
            this.splitContainer3.Panel1.Controls.Add(Button);

            int x;

            int offset;

            List<Button> shiftedButtons = new List<Button>();

            if (AfterButtonType == InfoFormButtonType.OK)
            {
                x = this.oKbutton.Location.X + this.oKbutton.Width + 5;

                offset = 10 + Button.Width;

                shiftedButtons.Add(expandButton);
            }
            else
            {
                throw new NotImplementedException();
            }

            Button.Location = new Point(x, this.oKbutton.Location.Y);

            foreach (Button button in shiftedButtons)
            {
                button.Location = new Point(button.Location.X + offset, button.Location.Y);
            }
        }

        public void InsertButton(InfoFormButtonType AfterButtonType, string Text, System.Windows.Forms.DialogResult DialogResult)
        {
            Button button = new Button() { Name = string.Format("{0}Button", Text), Text = Text, DialogResult = DialogResult, AutoSize = true };

            InsertButton(AfterButtonType, button);
        }

        public void SetButtonText(InfoFormButtonType ButtonType, string Text)
        {
            Button button = null;

            switch (ButtonType)
            {
                case InfoFormButtonType.OK:
                    button = oKbutton;
                    break;
                case InfoFormButtonType.Deatils:
                    button = expandButton;
                    break;
                case InfoFormButtonType.Send:
                    button = sendButton;
                    break;
                case InfoFormButtonType.Copy:
                    button = copyButton;
                    break;
            }

            if (button != null)
            {
                button.Text = Text;
            }
        }

        private void oKbutton_Resize(object sender, EventArgs e)
        {
            expandButton.Left = oKbutton.Right + 5;
        }

        public System.Windows.Forms.Button OkButton
        {
            get
            {
                return oKbutton;
            }
        }

    }

    public enum InfoFormButtonType
    {
        None,

        OK,

        Deatils,

        Send,

        Copy
    }
}
