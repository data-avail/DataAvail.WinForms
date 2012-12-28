using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.WinUtils.InfoForm
{
    public class ORATextParser : IInfoFormTextParser
    {
        #region IInfoFormTextParser Members

        public string GetMainPart(string Text)
        {
            int fI = Text.IndexOf(":");

            if (fI != -1)
            {
                int lI = Text.IndexOf("ORA", fI);

                if (fI < lI)
                {
                    int len = lI == -1 ? Text.Length - fI : lI - fI;

                    return Text.Substring(fI + 1, len - 1).Trim(new char[] { '\r', '\n' });
                }
            }

            return Text;
        }

        public string GetAuxPart(string Text)
        {
            return Text;
        }

        #endregion
    }
}
