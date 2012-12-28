using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;

namespace DataAvail.XtraBindings.Calculator
{
    public interface IObjectCalculator
    {
        void Calculate(ObjectProperties Item, string FieldName, ObjectCalulatorCalculateType CalculateType);

        IObjectCalculatorPersistData PersistData { get; }
    }

    public enum ObjectCalulatorCalculateType
    {
        Create,

        Initialize,

        Uninitilaize,

        InitializeNew,

        Calculate,

        Recalculate,

        Clone,

        AfterSave
    }
}
