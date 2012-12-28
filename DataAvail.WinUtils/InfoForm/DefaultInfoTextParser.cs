using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.WinUtils.InfoForm
{
    public class DefaultInfoTextParser : IInfoFormTextParser
    {
        #region IInfoFormTextParser Members

        public string GetMainPart(string Text)
        {
            return Text;
        }

        public string GetAuxPart(string Text)
        {
            return Text;
        }

        #endregion
    }
}
