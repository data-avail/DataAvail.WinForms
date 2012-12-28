using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HoohlShmoohl
{
    public abstract class AddressQueryBase : IAddressQuery
    {
        public AddressSuggestion[] GetAddressesSuggestions(string Country, string City, string Street, int MaxItemsCount, AddressTargetType TargetType)
        {
            string reqStr = null;

            switch (TargetType)
            {
                case AddressTargetType.Country:
                    if (!string.IsNullOrEmpty(Country))
                        reqStr = string.Format(SuggestionCountryRequestPattern, Country.ToLower(), null, null, MaxItemsCount);
                    break;
                case AddressTargetType.Region:
                    throw new NotImplementedException();
                case AddressTargetType.City:
                    if (!string.IsNullOrEmpty(Country) && !string.IsNullOrEmpty(City))
                        reqStr = string.Format(SuggestionCityRequestPattern, Country.ToLower(), City.ToLower(), null, MaxItemsCount);
                    break;
                case AddressTargetType.Street:
                    if (!string.IsNullOrEmpty(Country) && !string.IsNullOrEmpty(City) && !string.IsNullOrEmpty(Street))
                        reqStr = string.Format(SuggestionStreetRequestPattern, Country.ToLower(), City.ToLower(), Street.ToLower(), MaxItemsCount);
                    break;
                case AddressTargetType.Arbitrary:
                    if (!string.IsNullOrEmpty(Country))
                        reqStr = string.Format(SuggestionRequestPattern, Country.ToLower(), null, null, MaxItemsCount);
                    break;
            }

            if (!string.IsNullOrEmpty(reqStr))
            {
                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(reqStr);

                System.IO.StreamReader reader = new System.IO.StreamReader(req.GetResponse().GetResponseStream());

                string str = reader.ReadToEnd();

                reader.Dispose();

                if (!string.IsNullOrEmpty(str) && str != "{}")
                {
                    try
                    {
                        return ParseSuggestion(str.Trim(), TargetType);
                    }
                    catch
                    {
                        return new AddressSuggestion [] { };
                    }
                }
            }

            return new AddressSuggestion[] { };
        }

        public AddressInfo GetAddressInfo(string AddressString)
        {
            System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(string.Format(GeocodeRequestPattern, AddressString));

            System.IO.StreamReader reader = new System.IO.StreamReader(req.GetResponse().GetResponseStream());

            string str = reader.ReadToEnd();

            reader.Dispose();

            return !string.IsNullOrEmpty(str.Trim()) ? ParseAddressInfo(str) : null;
        }

        protected abstract string SuggestionCountryRequestPattern { get; }

        protected abstract string SuggestionCityRequestPattern { get; }

        protected abstract string SuggestionStreetRequestPattern { get; }

        protected abstract string SuggestionRequestPattern { get; }

        protected abstract string GeocodeRequestPattern { get; }

        protected abstract AddressSuggestion[] ParseSuggestion(string SuggestionRespnse, AddressTargetType TargetType);

        protected abstract AddressInfo ParseAddressInfo(string GeocodeRespnse);
    }
}
