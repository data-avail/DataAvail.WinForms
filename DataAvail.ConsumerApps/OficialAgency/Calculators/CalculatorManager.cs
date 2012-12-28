using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XtraBindings.Calculator;

namespace OficialAgency.Calculators
{
    [DataAvail.Attributes.ClaculatorManagerAttibute]
    public class CalculatorManager : IObjectCalculatorManager
    {
        #region IObjectCalculatorManager Members

        public IObjectCalculator GetObjectCalculator(DataAvail.XObject.XOTable XOTable)
        {
            switch (XOTable.Name)
            {
                case "DEAL":
                    return new DealCalculator();
                case "CONTACT":
                    return new ContactCalculator();
                default:
                    return ObjectCalculatorManager.DefaultObjectCalculator;
            }
        }

        #endregion
    }
}
