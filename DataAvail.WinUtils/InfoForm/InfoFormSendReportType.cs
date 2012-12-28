using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.WinUtils.InfoForm
{
    public enum InfoFormSendReportType
    {
        /// <summary>
        /// Report will be sent only when Send button is pressed. Sending message details form will be shown.
        /// </summary>
        Default,

        /// <summary>
        /// Send report functionality is disabled.
        /// </summary>
        Disabled,

        /// <summary>
        /// When user press OK button, Report will be sent silently.
        /// </summary>
        SilentOnOK,

        /// <summary>
        /// When user press Send button, Report will be sent silently.
        /// </summary>
        SilentOnSend,

        /// <summary>
        /// SilentOnOK + Default
        /// </summary>
        SilentOnOKAndDefault
    }
}
