using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.DXEditors
{
    public interface IGoogleLikeComboDataProvider
    {
        GoogleLikeComboData[] GetData(string Expression, int TopCount);
    }
}
