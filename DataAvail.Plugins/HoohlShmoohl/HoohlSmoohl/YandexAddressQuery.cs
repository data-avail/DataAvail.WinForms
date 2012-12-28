using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HoohlShmoohl
{
    public class YandexAddressQuery : AddressQueryBase
    {
        private const string REQ_SUGGEST_PATTERN = "http://suggest-maps.yandex.ru/suggest-geo?callback=json&_=1297467439342&ll=37.617671%2C55.755768&spn=1.457062%2C0.39496" +
            "&part={0}&highlight=1&fullpath=1&sep=1&search_type=all";

        private const string REQ_SUGGEST_CITY_PATTERN = "http://suggest-maps.yandex.ru/suggest-geo?callback=json&_=1297467439342&ll=37.617671%2C55.755768&spn=1.457062%2C0.39496" +
            "&part={0},+{1}&highlight=1&fullpath=1&sep=1&search_type=all";

        private const string REQ_SUGGEST_STREET_PATTERN = "http://suggest-maps.yandex.ru/suggest-geo?callback=json&_=1297467439342&ll=37.617671%2C55.755768&spn=1.457062%2C0.39496" +
            "&part={0},+{1},+{2}&highlight=1&fullpath=1&sep=1&search_type=all";

        private const string REQ_GEO_PATTERN = "http://maps.yandex.ru/?text={0}&vrb=1&output=json";

        protected override string SuggestionRequestPattern
        {
            get { return REQ_SUGGEST_PATTERN; }
        }

        protected override string SuggestionCountryRequestPattern
        {
            get { return REQ_SUGGEST_PATTERN; }
        }

        protected override string SuggestionCityRequestPattern
        {
            get { return REQ_SUGGEST_CITY_PATTERN; }
        }

        protected override string SuggestionStreetRequestPattern
        {
            get { return REQ_SUGGEST_STREET_PATTERN; }
        }

        protected override string GeocodeRequestPattern
        {
            get { return REQ_GEO_PATTERN; }
        }

        protected override AddressSuggestion[] ParseSuggestion(string SuggestionRespnse, AddressTargetType TargetType)
        {
            string str = SuggestionRespnse.Replace("maps-sep", "maps");

            //MESS--SMART YANDEX DEVELOPERS--
            var r = str.Split(new string[] { "[\"maps\"," }, StringSplitOptions.RemoveEmptyEntries)
                .Skip(1).Select(p => p.TrimEnd(" ],".ToCharArray()).TrimEnd(" ]], []])".ToCharArray()))
                    .Select(p => new AddressSuggestion() { Suggestion = p })
                    .ToArray();

            foreach (var sug in r)
                SuggestionFinishing(sug);

            return r;
        }

        static void SuggestionFinishing(AddressSuggestion AddressSuggestion)
        {
            List<AddressSuggestion.Marker> markers = new List<AddressSuggestion.Marker>();

            int f = -1;

            if (!AddressSuggestion.Suggestion.StartsWith("[[\""))
                AddressSuggestion.Suggestion = AddressSuggestion.Suggestion.TrimStart("[\"".ToCharArray());

            do
            {
                int rem = 3;
                
                f = AddressSuggestion.Suggestion.IndexOf("[[\"", f + 1);

                if (f == -1)
                {
                    f = AddressSuggestion.Suggestion.IndexOf("\",[\"", f + 1);

                    rem = 4;
                }

                if (f != -1)
                {
                    AddressSuggestion.Suggestion = AddressSuggestion.Suggestion.Remove(f, rem);

                    int l = AddressSuggestion.Suggestion.IndexOf("\"],\"", f);

                    if (l != -1)
                    {
                        AddressSuggestion.Suggestion = AddressSuggestion.Suggestion.Remove(l, 4);
                    }

                    markers.Add(new AddressSuggestion.Marker(f, l));
                }

            } while (f != -1);

            string splitStr = AddressSuggestion.Suggestion.Contains("\"],\"") ? "\"],\"" : "\"]],\"";

                            
            AddressSuggestion.Suggestion = AddressSuggestion.Suggestion.Split(new string[] { splitStr }, StringSplitOptions.RemoveEmptyEntries)[0];

            var a = markers.ToArray();

            for (int i = 0; i < a.Length; i++)
                if (a[i].End == -1)
                    a[i].End = AddressSuggestion.Suggestion.Length;
            
            AddressSuggestion.Markers = a;
        }

        protected override AddressInfo ParseAddressInfo(string GeocodeRespnse)
        {
            return YandexAddressInfo.Parse(GeocodeRespnse);
        }

    }

}
