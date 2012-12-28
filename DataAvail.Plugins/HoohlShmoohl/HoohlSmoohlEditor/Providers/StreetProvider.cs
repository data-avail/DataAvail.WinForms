using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.DXEditors;
using HoohlShmoohl;

namespace HoohlShmoohlEditor.Providers
{
    internal class StreetProvider : AddressInfoProviderContainer, IGoogleLikeComboDataProvider
    {
        internal StreetProvider(IAddressInfoProvider AddressInfoProvider)
            : base(AddressInfoProvider)
        { }

        #region IGoogleLikeComboDataProvider Members

        public GoogleLikeComboData[] GetData(string Expression, int TopCount)
        {
            if (AddressInfo != null && AddressInfo.City != null)
            {
                try
                {


                    return AddressQuery.GetAddressesSuggestions(AddressInfo.CountryName, !string.IsNullOrEmpty(AddressInfo.City.Name) ? AddressInfo.City.Name : AddressInfo.City.FullName, Expression, 15, AddressTargetType.Street)
                        .Select((p, i) => new GoogleLikeComboData()
                        {
                            Key = i,
                            Text = p.Suggestion,
                            DropDownText = p.Suggestion,
                            Markers = p.Markers.Select(s => new GoogleLikeComboMarker(s.Start, s.End)).ToArray()
                        }).ToArray();
                }
                catch
                {}
            }

            return new GoogleLikeComboData[] { };

        }

        #endregion
    }
}
