using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;


namespace HoohlShmoohl
{
    public class GoogleAddressQuery : AddressQueryBase
    {
        #region consts

        //private const string REQ_SUG_COUNTRY_PATTERN = "http://maps.google.ru/maps/suggest?q=%D1%81%D1%82%D1%80%D0%B0%D0%BD%D0%B0+%D0%BB%D0%B8,+,&cp=9&hl=ru&gl=ru&v=2&clid=1&ll=55.354135,40.297852&spn=24.624122,86.220703&auth=ba5077b2:KJr2aXDnd43lXqTa57qd__Ea9YM&src=1&num=5&numps=5";
        private const string REQ_SUG_COUNTRY_PATTERN = "http://maps.google.com/maps/suggest?q=страна+{0},+,&cp=9&hl=ru&gl=ru&v=2&clid=1&ll=37.160317,-95.712891&spn=52.993,113.818359&auth=fe30d5ff:p9Sb3K5u63TDeGvvSVS2Ffn5S7U&src=1&num={3}&numps={3}&";

        //http://maps.google.ru/maps/suggest?q=%D1%81%D1%82%D1%80%D0%B0%D0%BD%D0%B0+%D1%80%D0%BE%D1%81%D1%81%D0%B8%D1%8F,+%D0%B3%D0%BE%D1%80%D0%BE%D0%B4+%D0%BC%D0%BE%D1%81%D0%BA&cp=25&hl=ru&gl=ru&v=2&clid=1&json=a&ll=55.354135,40.297852&spn=24.624122,86.220703&auth=8de4a4d7:iBDNiGUrGGYeFFKvoaX8W7hnb2g&src=1&num=5&numps=5&callback=_xdc_._ugk8iferr
        private const string REQ_SUG_CITY_PATTERN = "http://maps.google.ru/maps/suggest?q=страна+{0},+город+{1},&cp=23&hl=ru&gl=ru&v=2&clid=1&ll=55.354135,40.297852&spn=24.624122,86.220703&auth=ba5077b2:KJr2aXDnd43lXqTa57qd__Ea9YM&src=1&num={3}&numps={3}";

        //http://maps.google.ru/maps/suggest?q=%D0%A0%D0%BE%D1%81%D1%81%D0%B8%D1%8F,+%D0%A7%D0%B5%D0%BB%D1%8F%D0%B1%D0%B8%D0%BD%D1%81%D0%BA%D0%B0%D1%8F+%D0%BE%D0%B1%D0%BB%D0%B0%D1%81%D1%82%D1%8C,+%D0%B3%D0%BE%D1%80%D0%BE%D0%B4+%D0%A7%D0%B5%D0%BB%D1%8F%D0%B1%D0%B8%D0%BD%D1%81%D0%BA,+%D1%83%D0%BB%D0%B8%D1%86%D0%B0+%D0%B3%D0%BE&cp=54&hl=ru&gl=ru&v=2&clid=1&json=a&ll=55.159889,61.40258&spn=0.383638,1.347198&auth=8de4a4d7:iBDNiGUrGGYeFFKvoaX8W7hnb2g&src=1&num=5&numps=5&callback=_xdc_._4ugk8lsloo
        private const string REQ_SUG_STREET_PATTERN = "http://maps.google.ru/maps/suggest?q={0},+{1},+улица+{2}&cp=54&hl=ru&gl=ru&v=2&clid=1&ll=55.159889,61.40258&spn=0.383638,1.347198&auth=8de4a4d7:iBDNiGUrGGYeFFKvoaX8W7hnb2g&src=1&num={3}&numps={3}";

        //http://maps.google.ru/maps/suggest?q=%D0%BF%D1%80&cp=2&hl=ru&gl=ru&v=2&clid=1&json=a&ll=55.354135,40.297852&spn=24.624122,86.220703&auth=34faf61a:uoUdw0n2ujNlrCC1-HvHTUiY29A&src=1&num=5&numps=5&callback=_xdc_._pgk8ukqeg
        private const string REQ_SUG_PATTERN = "http://maps.google.ru/maps/suggest?q={0}&cp=30&hl=ru&gl=ru&v=2&clid=1&ll=55.354135,40.297852&spn=24.624122,86.220703&auth=34faf61a:uoUdw0n2ujNlrCC1-HvHTUiY29A&src=1&num={3}&numps={3}";

        private const string REQ_GEO_PATTERN = "http://maps.google.com/maps/geo?q={0}&output=json&oe=utf8&sensor=true&hl=ru";

        #endregion

        protected override string SuggestionCountryRequestPattern
        {
            get { return REQ_SUG_COUNTRY_PATTERN; }
        }

        protected override string SuggestionCityRequestPattern
        {
            get { return REQ_SUG_CITY_PATTERN; }
        }

        protected override string SuggestionStreetRequestPattern
        {
            get { return REQ_SUG_STREET_PATTERN; }
        }

        protected override string SuggestionRequestPattern
        {
            get { return REQ_SUG_PATTERN; }
        }

        protected override string GeocodeRequestPattern
        {
            get { return REQ_GEO_PATTERN; }
        }

        protected override AddressInfo ParseAddressInfo(string GeocodeRespnse)
        {
            return GoogleAddressInfo.Parse(GeocodeRespnse);
        }

        protected override AddressSuggestion[] ParseSuggestion(string SuggestionRespnse, AddressTargetType TargetType)
        {
            int markerOffset = 0;

            switch (TargetType)
            { 
                case AddressTargetType.Country:
                    markerOffset = 7;
                    break;
            }

            JObject sugObj = JObject.Parse(SuggestionRespnse);

            return sugObj["suggestion"]
                .Select(p => new AddressSuggestion()
                {
                    Suggestion = (string)p["query"], 
                    Markers = (from s in ((JArray)p["interpretation"]["term"]).Where(s=>s["matched"] != null)
                                  let st = (int)s["start"]
                                  let ed = (int)s["start"] + ((int)s["term_end"] - (int)s["term_start"] - markerOffset)
                                  let ted = (int)s["end"]
                                  let real_ed = ed > 0 ? (ted > ed ? ed : ted) : 0
                                    select new AddressSuggestion.Marker(st, real_ed)).ToArray() 
                }).ToArray();
        }

        public static AddressSuggestion[] GetCountrySuggestions(string Country, int MaxCount)
        {
            GoogleAddressQuery query = new GoogleAddressQuery();

            return query.GetAddressesSuggestions(Country, null, null, MaxCount, AddressTargetType.Country);
        }
    }
}
