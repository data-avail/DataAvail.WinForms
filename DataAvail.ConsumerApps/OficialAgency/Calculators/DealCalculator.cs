using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XtraBindings.Calculator;
using DataAvail.Data.DbContext;

namespace OficialAgency.Calculators
{
    internal class DealCalculator : DefaultObjectCalculator
    {
        #region IObjectCalculator Members

        public override void Calculate(ObjectProperties Item, string FieldName, ObjectCalulatorCalculateType CalculateType)
        {
            base.Calculate(Item, FieldName, CalculateType);

            if (CalculateType == ObjectCalulatorCalculateType.Initialize)
            {
                Item.SF("STATE", "ID <> 1");
                TYPE(Item);
                RATE(Item);
            }

            if (CalculateType == ObjectCalulatorCalculateType.Calculate)
            {
                switch (FieldName)
                {
                    case "PRICEFROMAGENT":
                    case "PRICEFORCLIENT":
                        Item.SVC("PROFIT", Item.AsDbl("PRICEFORCLIENT") - Item.AsDbl("PRICEFROMAGENT"));
                        break;
                    case "TYPE":
                        TYPE(Item);
                        break;
                    case "AGENTCURRENCY":
                        RATE(Item);
                        break;
                    case "PROFIT":
                        Item.SS("PROFIT", CalulatorFieldState.Manual);
                        break;
                }
            }
        }

        #endregion

        private void RATE(ObjectProperties Item)
        {
            if (Item.AsInt("AGENTCURRENCY") != 0)
            {
                Item.SR("RATE", false);
            }
            else
            {
                Item.SV("RATE", 1);
                Item.SR("RATE", true);
            }
        }

        private void TYPE(ObjectProperties Item)
        {
            if (DbContext.GetScalarInt("SELECT TYPE FROM DEAL_TYPE WHERE ID = {0}", Item.GV("TYPE")) == 1)
            {
                Item.SR("VISA_COUNTRY", false);
                Item.SR("VISA_DURATION", false);
            }
            else
            {
                Item.SV("VISA_COUNTRY", System.DBNull.Value);
                Item.SV("VISA_DURATION", System.DBNull.Value);

                Item.SR("VISA_COUNTRY", true);
                Item.SR("VISA_DURATION", true);
            }
        }
    }
}
