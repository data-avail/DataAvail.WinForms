using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;

namespace DataAvail.XtraBindings.Calculator
{
    public interface IObjectCalculatorManager
    {
        DataAvail.XtraBindings.Calculator.IObjectCalculator GetObjectCalculator(XOTable XOTable);
    }
}
