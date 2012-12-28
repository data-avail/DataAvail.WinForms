using System;
using System.Collections.Generic;
using System.Text;

namespace DataAvail.WinUtils.DbConnectForm
{
    public interface IDbConnectContext
    {
        string Validate();

        string DefaultUserName { get; }
    }
}
