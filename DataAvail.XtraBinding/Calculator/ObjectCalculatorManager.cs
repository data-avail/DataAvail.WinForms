using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;

namespace DataAvail.XtraBindings.Calculator
{
    public class ObjectCalculatorManager : IObjectCalculatorManager
    {
        private static readonly DataAvail.XtraBindings.Calculator.IObjectCalculator _defaultObjectCalulator = new DataAvail.XtraBindings.Calculator.DefaultObjectCalculator();

        public static DataAvail.XtraBindings.Calculator.IObjectCalculator DefaultObjectCalculator { get { return _defaultObjectCalulator; } }

        #region IObjectCalculatorManager Members

        public DataAvail.XtraBindings.Calculator.IObjectCalculator GetObjectCalculator(XOTable XOTable)
        {
            return _defaultObjectCalulator;
        }

        #endregion
    }
}
