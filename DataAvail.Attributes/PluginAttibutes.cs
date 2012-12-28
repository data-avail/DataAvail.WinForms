using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Attributes
{

    [AttributeUsage(AttributeTargets.Class)]
    public class PluginMenuItem : Attribute
    {
        public PluginMenuItem(string Menu)
        {
            this.Menu = Menu;
        }

        /// <summary>
        /// Path for the main fraim's plugin menu item 
        /// Item/SubItem/SubSubItem/MenuItem
        /// </summary>
        public readonly string Menu;
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ClaculatorManagerAttibute : Attribute
    {
    }



}
