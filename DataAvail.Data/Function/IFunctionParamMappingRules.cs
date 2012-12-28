using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Data.Function
{
    public interface IFunctionParamMappingRules<T>
    {
        System.Data.IDbDataParameter GetParam(T Field);
    }
}
