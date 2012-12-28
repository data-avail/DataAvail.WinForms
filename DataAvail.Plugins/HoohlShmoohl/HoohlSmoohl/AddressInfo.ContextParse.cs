using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HoohlShmoohl
{
    partial class AddressInfo
    {
        /// <summary>
        /// Parse city from string 
        /// </summary>
        /// <param name="CityName">Format : region, city name or just city name or city name, region, country</param>
        public void ParseCity(string CityName)
        {
            string city = CityName.Trim();

            string[] strs = CityName.Split(',');

            if (strs.Length > 2)
            {
                this.AdministrativeArea = ParseAdministrativeArea(strs[1].Trim());

                city = strs[0].Trim();

            }
            else if (strs.Length > 1)
            {
                this.AdministrativeArea = ParseAdministrativeArea(strs[0].Trim());

                city = strs[1].Trim();
            }

            this.City = AddressInfo.ParseCityStatic(city);
        }

        public void ParseBuilding(string BuildingFullName)
        {
            this.Building = new BuildingInfo();

            this.Building.FullName = BuildingFullName;

            YandexAddressInfo.ParseBuilding(this.Building);
        }

        public void ParseStreet(string StreetName, AddressInfoContextType ContextType)
        {
            this.Street = new StreetInfo();

            string region = null;

            string city = null;

            string street = null;

            string[] strs = StreetName.Split(',');

            if (ContextType == AddressInfoContextType.Google)
            {
                if (strs.Length >= 3)
                {
                    region = strs[0].Trim();

                    city = strs[1].Trim();

                    street = strs[2].Trim();
                }
                else if (strs.Length == 2)
                {
                    city = strs[0].Trim();

                    street = strs[1].Trim();
                }
                else
                {
                    street = strs[0];
                }
            }
            else if (ContextType == AddressInfoContextType.Yandex)
            {
                if (strs.Length >= 2)
                {
                    city = strs[1].Trim();

                    street = strs[0].Trim();
                }
                else
                {
                    street = strs[0];
                }
            }

            if (!string.IsNullOrEmpty(region))
                this.AdministrativeArea = ParseAdministrativeArea(region);

            if (!string.IsNullOrEmpty(city))
                this.City = ParseCityStatic(city);

            if (!string.IsNullOrEmpty(street))
                this.Street = ParseStreetStatic(street);

            this.Building = null;
        }

    }

    public enum AddressInfoContextType
    {
        Google,

        Yandex
    }
}
