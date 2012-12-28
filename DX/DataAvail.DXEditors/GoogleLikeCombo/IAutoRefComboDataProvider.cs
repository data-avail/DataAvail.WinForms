using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.DXEditors
{
    public interface IAutoRefComboDataProvider : IGoogleLikeComboDataProvider
    {
        GoogleLikeComboData GetData(object Key);
    }
}
