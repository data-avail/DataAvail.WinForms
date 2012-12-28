using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.WinUtils.MailForm
{
    public interface IMailMessageFormater
    {
        string GetSubject(string Subject, string Text, string AuxText);

        string GetBody(string Text, string AuxText);
    }
}
