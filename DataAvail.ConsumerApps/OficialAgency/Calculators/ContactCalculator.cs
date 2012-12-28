using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XtraBindings.Calculator;

namespace OficialAgency.Calculators
{
    internal class ContactCalculator : DefaultObjectCalculator
    {
        #region IObjectCalculator Members

        public override void Calculate(ObjectProperties Item, string FieldName, ObjectCalulatorCalculateType CalculateType)
        {
            base.Calculate(Item, FieldName, CalculateType);

            if (CalculateType == ObjectCalulatorCalculateType.Initialize)
            {
                TAG(Item);
            }

            if (CalculateType == ObjectCalulatorCalculateType.Calculate)
            {
                switch (FieldName)
                {
                    case "TYPE":
                        TAG(Item);
                        break;
                }
            }
        }

        #endregion

        private void TAG(ObjectProperties Item)
        {
            if (Item.AsInt("TYPE") != 3)
            {
                Item.SM("TAG", "r:phone");
            }
            else
            {
                Item.SM("TAG", null);
            }
        }
    }
}
