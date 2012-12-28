using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraMenu
{
    public interface IXtraMenu : IEnumerable<IXtraMenuButton>
    {
        IXtraMenuButton this[XtraMenuButtonType ButtonType] { get; }
    }
}
