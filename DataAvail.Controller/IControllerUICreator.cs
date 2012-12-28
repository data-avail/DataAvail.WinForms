using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Controllers
{
    public interface IControllerUICreator
    {
        IControllerUI Create(Controller Controller, bool ItemUI);
    }
}
