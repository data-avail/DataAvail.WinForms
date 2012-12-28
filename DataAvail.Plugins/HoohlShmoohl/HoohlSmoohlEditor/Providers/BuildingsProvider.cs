using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.DXEditors;
using HoohlShmoohl;

namespace HoohlShmoohlEditor.Providers
{
    class BuildingsProvider : AddressInfoProviderContainer, IGoogleLikeComboDataProvider
    {
        internal BuildingsProvider(IAddressInfoProvider AddressInfoProvider)
            : base(AddressInfoProvider)
        { }
     
        private HoohlShmoohl.AddressInfo.BuildingInfo [] Buildings
        {
            get 
            { 
                return AddressInfo != null && AddressInfo.Street != null && AddressInfo.Street.Buildings != null ?
                    AddressInfo.Street.Buildings : new HoohlShmoohl.AddressInfo.BuildingInfo []{}; 
            }
        } 

        #region IGoogleLikeComboDataProvider Members

        public GoogleLikeComboData[] GetData(string Expression, int TopCount)
        {
            var items = Buildings.Select((p,i) => new GoogleLikeComboData()
                {
                    Key = i,
                    Text = p.FullName,
                    DropDownText = p.FullName,
                    Markers = p.FullName.ToUpper().StartsWith(Expression.ToUpper()) ?  new GoogleLikeComboMarker [] {new GoogleLikeComboMarker(0, Expression.Length)} : null
                }).ToArray();

            var marked = items.Where(p => p.Markers != null).OrderBy(p => p.DropDownText);

            var unmarked = items.Except(marked).OrderBy(p => p.DropDownText);

            return marked.Union(unmarked).ToArray();
            
        }

        #endregion
    }
}
