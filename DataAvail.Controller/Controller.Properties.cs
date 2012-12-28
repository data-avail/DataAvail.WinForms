using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Controllers
{
    partial class Controller
    {
        public readonly static Properties properties = new Properties();

        public class Properties
        {
            public string TempFolder;
        }
    }
}
