using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvailable
{
    public class DACProperties
    {
        public string Model;

        public string ModelView;

        public string ModelSecurity;

        public DACDataAdapterType AdapterType;

        public string ConnectionString;

        public string LayoutsFolder;

        public string TempFolder;

        public DataAvail.XtraBinding.Calculator.IObjectCalculatorManager CalculatorManager;
    }

    public enum DACDataAdapterType
    {
        MSSQL,
        Oracle,
        SQLite
    }
}
