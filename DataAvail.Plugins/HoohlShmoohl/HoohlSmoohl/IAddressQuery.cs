using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HoohlShmoohl
{
    public interface IAddressQuery
    {
        AddressSuggestion [] GetAddressesSuggestions(string Country, string City, string Street, int MaxItemsCount, AddressTargetType TargetType);

        AddressInfo GetAddressInfo(string AddressString);
    }
}
