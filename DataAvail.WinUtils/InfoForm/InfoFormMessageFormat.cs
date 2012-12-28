using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.WinUtils.InfoForm
{
    public class InfoFormDisplayInfoFormatEventArgs : System.EventArgs
    {
        public string Info;

        public string AuxInfo;
    }


    public class InfoFormSendInfoFormatEventArgs : InfoFormDisplayInfoFormatEventArgs
    {
        public DataAvail.WinUtils.Mail.MailParams MailParams;

        public string Subject;

        public string Text;
    }

    public delegate void InfoFormDisplayInfoHandler(object sender, InfoFormDisplayInfoFormatEventArgs args);

    public delegate void InfoFormSendInfoHandler(object sender, InfoFormSendInfoFormatEventArgs args);
}
