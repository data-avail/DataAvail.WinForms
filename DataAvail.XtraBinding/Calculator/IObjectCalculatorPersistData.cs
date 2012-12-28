using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraBindings.Calculator
{
    public interface IObjectCalculatorPersistData
    {
        /// <summary>
        /// Load persitent data to ObjectProperties
        /// </summary>
        void Load(ObjectProperties ObjectProperties);

        /// <summary>
        /// Save persitent data from ObjectProperties
        /// </summary>
        void Save(ObjectProperties ObjectProperties);
    }
}
