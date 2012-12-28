using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace HoohlShmoohl
{
    public static class YandexAddressInfo 
    {
        public static AddressInfo Parse(string jSon)
        {
            AddressInfo addr = new AddressInfo(AddressInfoContextType.Yandex);

            JObject jobj = JObject.Parse(jSon);

            IEnumerable<JProperty> props = jobj.Descendants().OfType<JProperty>().Where(p => p.Name == "locations");

            JProperty loc = (JProperty)props.ElementAt(1);

            string text = (string)(loc.Descendants().OfType<JProperty>().First(p => p.Name == "text")).Value;

            string type = (string)(loc.Descendants().OfType<JProperty>().First(p => p.Name == "kind")).Value;

            string req = (string)(props.Descendants().OfType<JProperty>().First(p => p.Name == "request")).Value;//"request": "москва челябинская 10к1";

            Parse(addr, text);

            string[] buildings = loc.Descendants().OfType<JProperty>().Where(p => p.Name == "num").Select(p => (string)p.Value).ToArray();

            if (buildings.Length > 0)
            {
                if (addr.Street == null)
                    addr.Street = new AddressInfo.StreetInfo();

                addr.Street.Buildings = buildings.Select(p => new AddressInfo.BuildingInfo() { FullName = p }).ToArray();

                foreach (AddressInfo.BuildingInfo buildingInfo in addr.Street.Buildings)
                {
                    ParseBuilding(buildingInfo);
                }

                //Get last request part and suppose this is building number
                int i = req.LastIndexOf(" ");

                if (i != -1)
                {
                    string buildingNum = req.Substring(i).Trim();

                    addr.Building = addr.Street.Buildings.FirstOrDefault(p => p.FullName == buildingNum);
                }
            }

            return addr;
        }

        internal static void ParseBuilding(AddressInfo.BuildingInfo BuildingInfo)
        {
            if (!string.IsNullOrEmpty(BuildingInfo.FullName))
            {
                string[] r = new string[] { BuildingInfo.FullName };

                try
                {
                    SeparateBuildingParts(BuildingInfo.FullName);
                }
                catch { }

                AddressInfo.ParseBuilding(BuildingInfo, r[0], r.Skip(1).ToArray());
            }
        }

        static void Parse(AddressInfo AddressInfo, string LocalityText)
        {
            //Россия, республика Башкортостан, Караидельский район, село Юрюзань

            //Россия, Челябинская область, Юрюзань

            //Россия, Челябинская область, станция Юрюзань

            //Россия, река Юрюзань

            //Россия, Свердловская область, Талицкий район, поселок Первухина

            string [] s = LocalityText.Split(',').Select(p=>p.Trim()).ToArray();

            AddressInfo.CountryName = s[0];

            if (s.Length > 1)
            {
                AddressInfo.AdministrativeAreaInfo admAreaInfo = AddressInfo.ParseAdministrativeArea(s[1]);

                if (admAreaInfo.Type != AddressAdministrativeAreaType.Undefined)
                    AddressInfo.AdministrativeArea = admAreaInfo;

                if (s.Length > 2)
                {
                    AddressInfo.SubAdministrativeAreaInfo subAdmAreaInfo = AddressInfo.ParseSubAdministrativeArea(s[2]);

                    if (subAdmAreaInfo.Type != AddressSubAdministrativeAreaType.Undefined)
                        AddressInfo.SubAdministrativeArea = subAdmAreaInfo;
                }

                int cityPos = -1;

                if (AddressInfo.AdministrativeArea == null)
                {
                    cityPos = 1;
                }
                else
                {
                    if (AddressInfo.SubAdministrativeArea == null)
                        cityPos = 2;
                    else
                        cityPos = 3;
                }

                if (cityPos != -1)
                {
                    AddressInfo.City = AddressInfo.ParseCityStatic(s[cityPos]);

                    if (cityPos + 1 < s.Length)
                        AddressInfo.Street = AddressInfo.ParseStreetStatic(s[cityPos + 1]);
                }
            }            
        }

        static string[] SeparateBuildingParts(string BuildingName)
        {
            string mainNonDigit = null ;

            var chrPos = BuildingName.Select((p, i) => new { chr = p, pos = i });
            
            var digits = chrPos.Where(p => char.IsDigit(p.chr)).Select(p=>p.pos).Where(p => p == 0 || !char.IsDigit(BuildingName[p - 1])).ToArray();

            var nonDigits = chrPos.Where(p => !char.IsDigit(p.chr)).Select(p => p.pos).Where(p => p == 0 || char.IsDigit(BuildingName[p - 1])).ToArray();

            if (nonDigits.Length > 0 && nonDigits[0] == 0)
            {
                digits = digits.Skip(1).ToArray();

                nonDigits = nonDigits.Skip(1).ToArray();

                mainNonDigit = BuildingName.Substring(0, digits.Length > 0 ? digits[0] : BuildingName.Length);
            }

            var digVals = digits.Select((p, i) => BuildingName.Substring(digits[i], i < nonDigits.Length ? nonDigits[i] - digits[i] : BuildingName.Length - digits[i])).ToArray();

            var nonDigVals = nonDigits.Select((p, i) => BuildingName.Substring(nonDigits[i], i < digits.Length - 1 ? digits[i + 1] - nonDigits[i] : BuildingName.Length - nonDigits[i])).ToArray();

            string [] mainPart = null;

            if (mainNonDigit == null)
            {
                mainPart = new string[] { digVals.First() };

                digVals = digVals.Skip(1).ToArray();
            }
            else
            {
                mainPart = new string[] { mainNonDigit };
            }

            return mainPart.Union((digVals.Length > nonDigVals.Length ? digVals : nonDigVals).
                Select((p, i) => string.Format("{0} {1}", 
                    i < nonDigits.Length ? nonDigVals[i] : null, 
                    i < digVals.Length ? digVals[i] : null).Trim())).ToArray();
        }
    }
}
