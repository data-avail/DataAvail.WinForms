using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.WinUtils.Mail
{
    public interface IMailSender
    {
        void Send(string MailTo, string Subject, string Text, string AttachFileName);

        string DefaultTo { get; }

        bool AllowChangeTo { get; }
    }
}
