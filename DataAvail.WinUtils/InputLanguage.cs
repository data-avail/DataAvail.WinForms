using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.WinUtils
{
    public static class InputLanguage
    {
        private static System.Windows.Forms.InputLanguage _storedLang; 

        public static void Store()
        {
            _storedLang = System.Windows.Forms.InputLanguage.CurrentInputLanguage;
        }

        public static void ReStore()
        {
            System.Windows.Forms.InputLanguage.CurrentInputLanguage = _storedLang;
        }

        public static void SetRus()
        {
            System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(System.Globalization.CultureInfo.GetCultureInfo("ru"));
        }

        public static void SetEng()
        {
            System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(System.Globalization.CultureInfo.GetCultureInfo("en"));
        }


    }
}
