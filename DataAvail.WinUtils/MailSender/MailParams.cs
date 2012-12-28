using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.WinUtils.Mail
{
    public class MailParams
    {
        public MailParams()
        { }

        public MailParams(MailParams MailParams)
        {
            this.ServerName = MailParams.ServerName;

            this.MailTo = MailParams.MailTo;

            this.MailFrom = MailParams.MailFrom;

            this.CopyTo = MailParams.CopyTo;
        }

        public string ServerName = null;//"HARMONIA.new_msk.nomos.bank";

        public string MailTo;

        public string CopyTo;

        public string MailFrom;
    }
}
