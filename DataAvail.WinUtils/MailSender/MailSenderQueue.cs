using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.WinUtils.Mail
{
    public class MailSenderQueue : DataAvail.WinUtils.Threading.SequentalThreadingPool
    {
        public readonly static MailSenderQueue Inst = new MailSenderQueue();

        protected override void OnDoWork(object Object)
        {
            MailSenderQueueParams param = (MailSenderQueueParams)Object;

            param.mailSender.Send(param.to, param.subject, param.body, param.attachment);
        }

        protected override void OnWorkComplete(object Object)
        {
            try
            {
                MailSenderQueueParams param = ((MailSenderQueueParams)Object);

                if (param.attachmentAutoRemove)
                {
                    if (!string.IsNullOrEmpty(param.attachment))
                        System.IO.File.Delete(param.attachment);
                }
            }
            catch (System.Exception)
            { }


            base.OnWorkComplete(Object);
        }

        protected override bool OnException(object Object, Exception Exception)
        {
            System.Windows.Forms.MessageBox.Show(string.Format("Mail send error : {0}", Exception.Message));

            base.OnException(Object, Exception);

            return true;
        }

        public static void Send(IMailSender MailSender, string MailTo, string Subject, string Text, string AttachFileName, bool AttachmentAutoRemove)
        {
            Inst.Add(new MailSenderQueueParams(MailSender, MailTo, Subject, Text, AttachFileName, AttachmentAutoRemove));
        }

        internal class MailSenderQueueParams
        {
            internal MailSenderQueueParams(IMailSender MailSender, string To, string Subject, string Body, string Attach, bool AttachAutoRemove)
            {
                this.to = To;

                this.subject = Subject;

                this.body = Body;

                this.attachment = Attach;

                this.attachmentAutoRemove = AttachAutoRemove;

                this.mailSender = MailSender;
            }

            internal readonly IMailSender mailSender;

            internal readonly string to;

            internal readonly string subject;

            internal readonly string body;

            internal readonly string attachment;

            internal readonly bool attachmentAutoRemove;
        }
    }
}
