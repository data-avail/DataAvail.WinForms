using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;

namespace DataAvail.Controllers
{
    public interface IControllerProvider
    {
        object GetDataSource(XOTable Table);

        DataAvail.Data.DataAdapter.IDataAdapter GetDataAdapter(XOTable TableContext);

        DataAvail.XtraBindings.Calculator.IObjectCalculator GetObjectCalculator(XOTable TableContext);
    }
}
