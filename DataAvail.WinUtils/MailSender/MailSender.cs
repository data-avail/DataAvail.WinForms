using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.WinUtils.Mail
{
    public class MailSender : IMailSender
    {
        public static MailParams MAIL_PARAMS = new MailParams();

        public static string MAIL_SUBJECT;

        public void SendMail(string Text)
        {
            SendMail(MAIL_SUBJECT, Text);
        }

        public void SendMail(string Subject, string Text)
        {
            SendMail(Subject, Text, null, false);
        }

        public void SendMail(string Subject, string Text, string AttachFileName, bool AttachmentAutoRemove)
        {
            SendMail(MAIL_PARAMS, Subject, Text, AttachFileName, AttachmentAutoRemove);
        }

        public void SendMail(string MailTo, string Subject, string Text, string AttachFileName, bool AttachmentAutoRemove)
        {
            MailParams mailParams = new MailParams(MAIL_PARAMS);
    
            mailParams.MailTo = MailTo;

            SendMail(mailParams, Subject, Text, AttachFileName, AttachmentAutoRemove);
        }

        public void SendMail(MailParams MailParams, string Subject, string Text, string AttachFileName, bool AttachmentAutoRemove)
        {
            SendMail(MailParams.ServerName, MailParams.MailTo, MailParams.MailFrom, Subject, Text, AttachFileName, AttachmentAutoRemove);
        }

        public void SendMail(string ServerName, string MailTo, string MailFrom, string Subject, string Text, string AttachmentFileName, bool AttachmentAutoRemove)
        {
            MailSenderQueue.Send(this, MailTo, Subject, Text, AttachmentFileName, AttachmentAutoRemove);
        }

        #region IMailSender Members

        public void Send(string MailTo, string Subject, string Text, string AttachFileName)
        {
            string svrName = DataAvail.WinUtils.Mail.MailSender.MAIL_PARAMS.ServerName;

            string mailFrom = DataAvail.WinUtils.Mail.MailSender.MAIL_PARAMS.MailFrom;

            if (!String.IsNullOrEmpty(svrName) && !String.IsNullOrEmpty(MailTo) && !String.IsNullOrEmpty(mailFrom))
            {
                string to = MailTo.Split(new char[] { ' ', ';', ',' }).Where(p => !string.IsNullOrEmpty(p)).Aggregate((p, n) => p + "," + n);

                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(mailFrom, to, Subject, Text);

                if (!string.IsNullOrEmpty(AttachFileName))
                    message.Attachments.Add(new System.Net.Mail.Attachment(AttachFileName));
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(svrName);
                client.UseDefaultCredentials = true;
                client.Send(message);
                message.Dispose();
            }
            else
            {
                throw new System.Exception("ServerName, MailTo, MailFrom parameters must be defined");
            }
        }

        public string DefaultTo
        {
            get { return DataAvail.WinUtils.Mail.MailSender.MAIL_PARAMS.MailTo; }
        }

        public bool AllowChangeTo
        {
            get
            {
                return true;
            }
        }

        #endregion
    }
}
