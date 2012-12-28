using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;

namespace DataAvail.AppShell
{
    class XtraBindingControllerProvider : DataAvail.XtraBinding.Controllers.IControllerProvider
    {
        internal XtraBindingControllerProvider(System.Data.DataSet DataSet, DataAvail.XtraBinding.Calculator.IObjectCalculatorManager ObjectCalculatorManager)
        {
            _dataSet = DataSet;

            _objectCalculatorManager = ObjectCalculatorManager;
        }

        private readonly System.Data.DataSet _dataSet;

        private readonly DataAvail.XtraBinding.Calculator.IObjectCalculatorManager _objectCalculatorManager;

        #region IXtraBindingControllerProvider Members

        public object GetDataSource(XOTableContext XtraObjectProperties)
        {
            return _dataSet.Tables[XtraObjectProperties.Name];
        }

        public DataAvail.Data.DataAdapter.IDataAdapter GetDataAdapter(XOTableContext XtraObjectProperties)
        {
            return DataAdapter.GetDataAdapter(XtraObjectProperties);
        }

        public DataAvail.XtraBinding.Calculator.IObjectCalculator GetObjectCalculator(XOTableContext XtraObjectProperties)
        {
            return _objectCalculatorManager != null ? _objectCalculatorManager.GetObjectCalculator(XtraObjectProperties) : null;

        }

        #endregion

    }
}
