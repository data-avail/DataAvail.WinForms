using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.DXEditors;
using HoohlShmoohl;

namespace HoohlShmoohlEditor.Providers
{
    internal class CityProvider : AddressInfoProviderContainer, IGoogleLikeComboDataProvider
    {
        internal CityProvider(IAddressInfoProvider AddressInfoProvider)
            : base(AddressInfoProvider)
        { }

        static GoogleAddressQuery _addrQuery = new GoogleAddressQuery();

        #region IGoogleLikeComboDataProvider Members

        public GoogleLikeComboData[] GetData(string Expression, int TopCount)
        {
            try
            {
                return _addrQuery.GetAddressesSuggestions(AddressInfo.CountryName, Expression, null, 15, AddressTargetType.City)
                    .Select((p, i) => new GoogleLikeComboData()
                    {
                        Key = i,
                        Text = p.Suggestion,
                        DropDownText = p.Suggestion,
                        Markers = p.Markers.Select(s => new GoogleLikeComboMarker(s.Start, s.End)).ToArray()
                    }).ToArray();
            }
            catch
            {
                return new GoogleLikeComboData[] { };
            }

        }

        #endregion
    }
}
