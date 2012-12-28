using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraObjectProperties.XOPCreator
{
    public interface IXOPCreatorProvider : IXOPFieldCreator
    {
        XtraObjectProperties NextObject();

        XtraFieldProperties NextField();

        void FinalizeCreation(IEnumerable<XtraObjectProperties> XtraObjects);
    }

    
}
