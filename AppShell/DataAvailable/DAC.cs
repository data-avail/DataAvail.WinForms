using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvailable
{
    public static class DAC
    {
        public static System.Windows.Forms.Form CreateApp(DACProperties Properties)
        {
            DataAvail.AppShell.MainFrame mainFrame = new DataAvail.AppShell.MainFrame();

            mainFrame.Initialize(Properties);

            return mainFrame;
        }
    }
}
