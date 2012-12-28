using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;
using DataAvail.Controllers;
using System.Data;
using DataAvail.XtraBindings.Calculator;

namespace DataAvail.AppShell
{
    class XtraBindingControllerProvider : IControllerProvider
    {
        internal XtraBindingControllerProvider(DataSet DataSet, IObjectCalculatorManager ObjectCalculatorManager)
        {
            _dataSet = DataSet;

            _objectCalculatorManager = ObjectCalculatorManager;
        }

        private readonly System.Data.DataSet _dataSet;

        private readonly DataAvail.XtraBindings.Calculator.IObjectCalculatorManager _objectCalculatorManager;

        #region IXtraBindingControllerProvider Members

        public object GetDataSource(XOTable AppItem)
        {
            return _dataSet.Tables[AppItem.Name];
        }

        public DataAvail.Data.DataAdapter.IDataAdapter GetDataAdapter(XOTable AppItem)
        {
            return DataAdapter.GetDataAdapter(AppItem);
        }

        public DataAvail.XtraBindings.Calculator.IObjectCalculator GetObjectCalculator(XOTable AppItem)
        {
            return _objectCalculatorManager != null ? _objectCalculatorManager.GetObjectCalculator(AppItem) : new DefaultObjectCalculator();

        }

        #endregion

    }
}
