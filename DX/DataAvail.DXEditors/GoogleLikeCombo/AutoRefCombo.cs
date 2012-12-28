using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.DXEditors
{
    /// <summary>
    ///  Select list of items from provider by text typed in the control.
    ///  If KeyValue of control not equals to currently selected list item, 
    ///  the item is selected by this KeyValue, if there is no such item in the list, 
    ///  trying to get one from DataProvider.
    /// </summary>
    public partial class AutoRefCombo : DynamicCombo
    {
        protected override void OnKeyValueChanged()
        {
            base.OnKeyValueChanged();

            if (KeyValue == System.DBNull.Value)
            {
                this.TextValue = null;

                return;
            }
            GoogleLikeComboData item = DataItems.FirstOrDefault(p => p.Key.Equals(KeyValue));

            if (item == null && DataProvider != null)
            {
                item = ((IAutoRefComboDataProvider)DataProvider).GetData((int)KeyValue);

                this.AddData(item);
            }

            if (item == null)
            {
                CantFindItemForKeyValue(false);
            }
            else
            {
                CantFindItemForKeyValue(true);

                this.TextValue = item.Text;
            }
        }

        protected void CantFindItemForKeyValue(bool Reset)
        {
            if (!Reset)
            {
                this.ErrorText = string.Format("Не найдена запись соответствующая текущему ключу [{0}] !", KeyValue);
            }
            else
            {
                this.ErrorText = null;
            }

        }

        protected override void OnDataProviderChanged()
        {
            if (!(DataProvider is IAutoRefComboDataProvider))
                throw new Exception("Data provider for this editor should be IAutoRefComboDataProvider"); 

            base.OnDataProviderChanged();
        }
    }
}
