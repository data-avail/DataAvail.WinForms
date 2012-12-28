using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baio.XtraBinding
{
    public interface IObjectCalculator
    {
        ObjectCalculatorData Calculate(object Object, string PropertyName, ObjectCalculatorAction Action);
    }

    public enum ObjectCalculatorAction
    { 
        Intiailze,

        Uninitailize,

        Calculate
    }
}
