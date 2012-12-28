using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace HoohlShmoohl
{
    public static class GoogleAddressInfo
    {
        public static AddressInfo Parse(string jSon)
        {
            AddressInfo addr = new AddressInfo(AddressInfoContextType.Google);

            JObject jobj = JObject.Parse(jSon);

            IEnumerable<JProperty> props = jobj.Descendants().OfType<JProperty>();

            string s = GetPropValue(props, "name");

            if (s != null)
                addr.Name = s;

            s = GetPropValue(props, "address");

            if (s != null)
                addr.LatName = s;

            s = GetPropValue(props, "CountryName");

            if (s != null)
                addr.CountryName = s;

            s = GetPropValue(props, "LocalityName");

            if (s != null)
                addr.City = AddressInfo.ParseCityStatic(s);

            s = GetPropValue(props, "AdministrativeAreaName");

            if (s != null)
                addr.AdministrativeArea = AddressInfo.ParseAdministrativeArea(s);

            s = GetPropValue(props, "SubAdministrativeAreaName");

            if (s != null)
                addr.SubAdministrativeArea = AddressInfo.ParseSubAdministrativeArea(s);

            s = GetPropValue(props, "DependentLocalityName");

            if (s != null)
                addr.DependedLocalityName = s;

            s = GetPropValue(props, "ThoroughfareName");

            if (s != null)
                addr.Street = ParseStreet(s);

            if (s != null)
                addr.Building = ParseBuildingByStreet(s);

            if (addr.LatName != null)
                addr.PostalCode = ParsePostalCode(addr.LatName);

            return addr;
        }

        private static string GetPropValue(IEnumerable<JProperty> Props, string PropName)
        {
            JProperty prop = Props.FirstOrDefault(p => p.Name == PropName);

            return prop != null ? (string)prop.Value : null;
        }

        #region Geocode parsing

        static AddressInfo.StreetInfo ParseStreet(string GoogleStreetName)
        {
            string strt = GoogleStreetName.Split(',')[0].Trim();

            return AddressInfo.ParseStreetStatic(strt);
        }

        static AddressInfo.BuildingInfo ParseBuildingByStreet(string GoogleStreetInfo)
        {
            string[] strt = GoogleStreetInfo.Split(',');

            if (strt.Length <= 1)
            {
                return null;
            }
            else
            {
                return ParseBuilding(strt[1]);
            }
        }

        internal static AddressInfo.BuildingInfo ParseBuilding(string GoogleBuildingInfo)
        {
            AddressInfo.BuildingInfo buildingInfo = new AddressInfo.BuildingInfo();

            string buildingStr = GoogleBuildingInfo.Trim();

            buildingInfo.FullName = buildingStr;

            var r = buildingStr.Split(' ');

            AddressInfo.ParseBuilding(buildingInfo, r[0], GetBuildingAuxes(r.Skip(1).ToArray()));

            return buildingInfo;
        }

        static string [] GetBuildingAuxes(string [] Auxes)
        {
            if (Auxes.Length > 1)
            {
                return new string [] {string.Format("{0} {1}", Auxes[0], Auxes[1])};
            }
            else if (Auxes.Length > 0)
            {
                return new string [] {Auxes[0]};
            }
           
            return new string [] {};
        }

        static string ParsePostalCode(string GoogleEngName)
        {
            int lcoma = GoogleEngName.LastIndexOf(',');

            string supposedPostalCode = lcoma != -1 ? GoogleEngName.Substring(lcoma + 1, GoogleEngName.Length - lcoma - 1).Trim() : null;

            bool isPostal = false;

            if (!string.IsNullOrEmpty(supposedPostalCode))
            {
                isPostal = supposedPostalCode.Where(p => !Char.IsNumber(p)).Count() == 0;
            }

            return isPostal ? supposedPostalCode : null;
        }

        #endregion
    }
}
