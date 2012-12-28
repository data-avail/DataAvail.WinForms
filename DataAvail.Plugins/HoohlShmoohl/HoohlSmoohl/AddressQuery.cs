using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HoohlShmoohl
{
    public static class AddressQuery
    {
        private static AddressInfoContextType _lastQueryContext;
        
        public static AddressInfoContextType LastQueryContext
        {
            get { return _lastQueryContext; }
        }

        public static AddressSuggestion [] GetAddressesSuggestions(string Country, string City, string Street, int MaxItemsCount, AddressTargetType TargetType)
        {
            _lastQueryContext = IsGoogleKing(Country + City + Street) ? AddressInfoContextType.Google : AddressInfoContextType.Yandex;

            IAddressQuery query = LastQueryContext == AddressInfoContextType.Google ? (IAddressQuery)new GoogleAddressQuery() : (IAddressQuery)new YandexAddressQuery();

            return query.GetAddressesSuggestions(Country, City, Street, MaxItemsCount, TargetType);
        }

        public static AddressInfo GetAddressInfo(string AddressString)
        {
            IAddressQuery gc = new GoogleAddressQuery();

            IAddressQuery yc = new YandexAddressQuery();

            AddressInfo gai = null;

            try
            {
                gai = gc.GetAddressInfo(AddressString);
            }
            catch { }

            AddressInfo yai = null;

            try
            {
                yai = yc.GetAddressInfo(AddressString);
            }
            catch
            { }

            
            bool isGoogleKing = IsGoogleKing(AddressString);

            _lastQueryContext = isGoogleKing ? AddressInfoContextType.Google : AddressInfoContextType.Yandex;

            AddressInfo mainAi = isGoogleKing ? gai : yai;

            AddressInfo subAi = isGoogleKing ? yai : gai;

            if (mainAi != null)
            {
                if (subAi != null)
                    mainAi.Merge(subAi);

                return mainAi;
            }
            else
            {
                return subAi;
            }
        }

        static bool IsGoogleKing(string Req)
        {
            if (Utils.IsLat(Req, true))
                return true;
            else
                return false;
        }


    }
}
