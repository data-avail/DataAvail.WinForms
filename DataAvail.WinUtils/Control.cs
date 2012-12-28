using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.WinUtils
{
    public static class Control
    {
        public static IEnumerable<System.Windows.Forms.Control> DescndantControls(System.Windows.Forms.Control Control)
        {
            return Control.Controls.Cast<System.Windows.Forms.Control>().Union(
                    Control.Controls.Cast<System.Windows.Forms.Control>().SelectMany(p => DescndantControls(p)));
        }

        public static System.Windows.Forms.Control GetFocusedDescendant(System.Windows.Forms.Control Control)
        {
            return DescndantControls(Control).FirstOrDefault(p => p.Focused);
        }
    }
}
