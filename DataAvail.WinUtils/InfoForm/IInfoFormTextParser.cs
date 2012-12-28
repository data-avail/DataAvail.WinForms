using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.WinUtils.InfoForm
{
    public interface IInfoFormTextParser
    {
        string GetMainPart(string Text);

        string GetAuxPart(string Text);
    }
}
