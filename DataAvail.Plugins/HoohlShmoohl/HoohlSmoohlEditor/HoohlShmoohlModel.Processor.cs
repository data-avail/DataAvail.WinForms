using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using HoohlShmoohl;
using HoohlShmoohlEditor.Providers;

namespace HoohlShmoohlEditor
{
    partial class HoohlShmoohlModel : IAddressInfoProvider
    {
        private AddressInfo _addressInfo = new AddressInfo();

        private bool _frozen = false;

        private bool Frozen
        {
            get { return _frozen; }
            set { _frozen = value; }
        }

        public AddressInfo AddressInfo
        {
            get { return _addressInfo; }
            set { _addressInfo = value; }
        }

        protected virtual void OnPropertyChanged(string PropertyName)
        {

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));

            if (!Frozen)
            {
                Frozen = true;

                switch (PropertyName)
                {
                    case "Search":
                        OnSearchChanged();
                        break;
                    case "Country":
                        OnCountryChanged();
                        break;
                    case "AdministrativeArea":
                        OnAdministrativeAreaChanged();
                        break;
                    case "SubAdministrativeArea":
                        OnSubAdministrativeAreaChanged();
                        break;
                    case "DependedLocality":
                        OnDependedLocalityChanged();
                        break;
                    case "City":
                        OnCityChanged();
                        break;
                    case "Street":
                        OnStreetChanged();
                        break;
                    case "Building":
                        OnBuildingChanged();
                        break;
                    case "PostalCode":
                        OnPostalCodeChanged();
                        break;
                }

                RefreshAddress();

                Frozen = false;
            }
        }

        private void OnSearchChanged()
        {
            AddressInfo = HoohlShmoohl.AddressQuery.GetAddressInfo(this.Search);

            if (AddressInfo != null)
            {
                if (!string.IsNullOrEmpty(AddressInfo.CountryName))
                    this.Country = AddressInfo.CountryName;
                else
                    this.Country = null;

                if (AddressInfo.City != null)
                {
                    this.City = !string.IsNullOrEmpty(AddressInfo.City.Name) ? AddressInfo.City.Name : AddressInfo.City.FullName;

                    this.CityType = AddressInfo.City.TypeName;
                }
                else
                {
                    this.City = null;

                    this.CityType = CityTypeDefault;
                }

                if (AddressInfo.Street != null)
                {
                    this.Street = !string.IsNullOrEmpty(AddressInfo.Street.Name) ? AddressInfo.Street.Name : AddressInfo.Street.FullName;

                    this.StreetType = AddressInfo.Street.TypeName;
                }
                else
                {
                    this.Street = null;

                    this.StreetType = StreetTypeDefault;
                }

                this.DependedLocality = AddressInfo.DependedLocalityName;

                this.PostalCode = AddressInfo.PostalCode;

                if (AddressInfo.AdministrativeArea != null)
                    this.AdministrativeArea = AddressInfo.AdministrativeArea.FullName;
                else
                    this.AdministrativeArea = null;

                if (AddressInfo.SubAdministrativeArea != null)
                    this.SubAdministrativeArea = AddressInfo.SubAdministrativeArea.FullName;
                else
                    this.SubAdministrativeArea = null;

            }

            ResetBuilding();
        }

        private void OnCountryChanged()
        {
            ResetAdministrativeArea();

            if (!string.IsNullOrEmpty(Country))
                this.AddressInfo = AddressQuery.GetAddressInfo(Country);
        }

        private void OnAdministrativeAreaChanged()
        {
            if (!string.IsNullOrEmpty(this.AdministrativeArea))
                this.AddressInfo.AdministrativeArea = AddressInfo.ParseAdministrativeArea(this.AdministrativeArea);
        }

        private void OnSubAdministrativeAreaChanged()
        {
            if (!string.IsNullOrEmpty(this.SubAdministrativeArea))
                this.AddressInfo.SubAdministrativeArea = AddressInfo.ParseSubAdministrativeArea(this.SubAdministrativeArea);
        }

        private void OnDependedLocalityChanged()
        {
            this.AddressInfo.DependedLocalityName = this.DependedLocality;
        }

        private void OnPostalCodeChanged()
        {
            this.AddressInfo.PostalCode = this.PostalCode;
        }

        private void OnCityChanged()
        {
            this.ResetSubAdministrativeArea(false);

            ResetStreet();

            if (string.IsNullOrEmpty(City))
                return;

            if (AddressInfo == null)
                this.AddressInfo = AddressQuery.GetAddressInfo(City);
            else
                this.AddressInfo.ParseCity(City);

            this.City = !string.IsNullOrEmpty(AddressInfo.City.Name) ? AddressInfo.City.Name : AddressInfo.City.FullName;

            this.CityType = !string.IsNullOrEmpty(AddressInfo.City.TypeName) ? AddressInfo.City.TypeName : CityTypeDefault;

            if (AddressInfo.AdministrativeArea != null)
                this.AdministrativeArea = AddressInfo.AdministrativeArea.FullName;
            else
                this.AdministrativeArea = null;

            if (!string.IsNullOrEmpty(this.City))
            {
                AddressInfo addr = AddressQuery.GetAddressInfo(GetSerachString());

                if (addr.AdministrativeArea != null)
                {
                    this.AddressInfo.AdministrativeArea = addr.AdministrativeArea;
                    this.AdministrativeArea = addr.AdministrativeArea.FullName;
                }
                else
                {
                    this.AddressInfo.AdministrativeArea = null;
                    this.AdministrativeArea = null;
                }

                if (addr.SubAdministrativeArea != null)
                {
                    this.AddressInfo.SubAdministrativeArea = addr.SubAdministrativeArea;
                    this.SubAdministrativeArea = addr.SubAdministrativeArea.FullName;
                }
                else
                {
                    this.AddressInfo.SubAdministrativeArea = null;
                    this.SubAdministrativeArea = null;
                }

                this.AddressInfo.DependedLocalityName = addr.DependedLocalityName;
                this.DependedLocality = addr.DependedLocalityName;

                this.PostalCode = addr.PostalCode;
                this.AddressInfo.PostalCode = addr.PostalCode;

                this.RefreshSearch();
            }
        }

        private void OnStreetChanged()
        {
            this.ResetSubAdministrativeArea(false);

            this.ResetBuilding();

            this.ResetPostalCode();

            if (string.IsNullOrEmpty(Street))
                return;

            if (AddressInfo == null)
                this.AddressInfo = AddressQuery.GetAddressInfo(Street);
            else
                this.AddressInfo.ParseStreet(Street, AddressQuery.LastQueryContext);

            this.Street = !string.IsNullOrEmpty(AddressInfo.Street.Name) ? AddressInfo.Street.Name : AddressInfo.Street.FullName;

            this.StreetType = !string.IsNullOrEmpty(AddressInfo.Street.TypeName) ? AddressInfo.Street.TypeName : StreetTypeDefault;

            if (AddressInfo.AdministrativeArea != null)
            {
                this.AddressInfo.AdministrativeArea = AddressInfo.AdministrativeArea;
                this.AdministrativeArea = AddressInfo.AdministrativeArea.FullName;
            }

            if (AddressInfo.City != null)
            {
                this.AddressInfo.City = AddressInfo.City;
                this.City = !string.IsNullOrEmpty(AddressInfo.City.Name) ? AddressInfo.City.Name : AddressInfo.City.FullName;
            }

            if (!string.IsNullOrEmpty(this.City) && !string.IsNullOrEmpty(this.Street))
            {
                AddressInfo addr = AddressQuery.GetAddressInfo(GetSerachString());

                if (addr.Street != null && addr.Street.Buildings != null)
                {
                    if (this.AddressInfo.Street != null)
                        this.AddressInfo.Street.Buildings = addr.Street.Buildings;
                }


                if (addr.AdministrativeArea != null)
                {
                    this.AddressInfo.AdministrativeArea = addr.AdministrativeArea;
                    this.AdministrativeArea = addr.AdministrativeArea.FullName;
                }

                if (addr.SubAdministrativeArea != null)
                {
                    this.AddressInfo.SubAdministrativeArea = addr.SubAdministrativeArea;
                    this.SubAdministrativeArea = addr.SubAdministrativeArea.FullName;
                }

                if (!string.IsNullOrEmpty(addr.DependedLocalityName))
                {
                    this.AddressInfo.DependedLocalityName = addr.DependedLocalityName;
                    this.DependedLocality = addr.DependedLocalityName;
                }

                this.PostalCode = addr.PostalCode;
                this.AddressInfo.PostalCode = addr.PostalCode;

                this.RefreshSearch();
            }
        }

        private void OnBuildingChanged()
        {
            if (AddressInfo != null)
                this.AddressInfo.Building = null;

            if (string.IsNullOrEmpty(Building))
                return;

            if (this.AddressInfo != null)
            {
                if (this.AddressInfo.Building == null)
                    this.AddressInfo.Building = new AddressInfo.BuildingInfo();

                if (AddressInfo.Building != null)
                    AddressInfo.Building.HomeName = this.Building;

                AddressInfo.ParseBuilding(this.Building);
            }
        }

        private void ResetAdministrativeArea()
        {
            this.AdministrativeArea = null;
            this.AddressInfo.AdministrativeArea = null;

            this.ResetSubAdministrativeArea(true);
        }

        private void ResetSubAdministrativeArea(bool ResetCity)
        {
            this.SubAdministrativeArea = null;
            this.AddressInfo.SubAdministrativeArea = null;

            this.DependedLocality = null;
            this.AddressInfo.DependedLocalityName = null;

            if (ResetCity)
                this.ResetCity();
        }

        private void ResetCity()
        {
            this.City = null;
            this.CityType = CityTypeDefault;
            this.AddressInfo.City = null;

            ResetStreet();
        }

        private void ResetStreet()
        {
            this.Street = null;

            this.StreetType = StreetTypeDefault;

            this.AddressInfo.Street = null;

            ResetBuilding();

            ResetPostalCode();
        }

        private void ResetBuilding()
        {
            this.Building = null;
            this.AddressInfo.Building = null;
        }


        private void ResetPostalCode()
        {
            this.PostalCode = null;
            this.AddressInfo.PostalCode = null;
        }

        private void RefreshAddress()
        {
            this.Address = this.AddressInfo.ToString();
        }

        private void RefreshSearch()
        {
            this.Frozen = true;
            this.Search = GetSerachString();
            this.Frozen = false;
        }

        string GetSerachString()
        {
            return Utils.ToString(this.Country, this.City, this.Street);
        }
    }
}
