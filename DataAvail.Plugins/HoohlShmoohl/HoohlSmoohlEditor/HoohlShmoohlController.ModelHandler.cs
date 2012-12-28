using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HoohlShmoohl;

namespace HoohlShmoohlEditor
{
    partial class HoohlShmoohlController
    {
        private AddressInfo _addressInfo = null;

        private bool _fieldChanging = false;

        public bool FieldChanging
        {
            get { return _fieldChanging; }
            set { _fieldChanging = value; }
        }

        private AddressInfo AddressInfo
        {
            get { return _addressInfo; }
            set { _addressInfo = value; }
        }

        private void HandleModelPropertyChanged(string PropertyName)
        {
            if (!Frozen && !FieldChanging)
            {
                FieldChanging = true;

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

                RefreshAddressString();

                FieldChanging = false;
            }
        }

        private void OnSearchChanged()
        {
            if (string.IsNullOrEmpty(this.Model.Search)) return;

            AddressInfo = HoohlShmoohl.AddressQuery.GetAddressInfo(this.Model.Search);

            if (AddressInfo != null)
            {
                if (!string.IsNullOrEmpty(AddressInfo.CountryName))
                    this.Model.Country = AddressInfo.CountryName;
                else
                    this.Model.Country = null;

                if (AddressInfo.City != null)
                {
                    this.Model.City = !string.IsNullOrEmpty(AddressInfo.City.Name) ? AddressInfo.City.Name : AddressInfo.City.FullName;

                    this.Model.CityType = AddressInfo.City.TypeName;
                }
                else
                {
                    this.Model.City = null;


                    this.Model.CityType = HoohlShmoohlModel.CityTypeDefault;
                }

                if (AddressInfo.Street != null)
                {
                    this.Model.Street = !string.IsNullOrEmpty(AddressInfo.Street.Name) ? AddressInfo.Street.Name : AddressInfo.Street.FullName;

                    this.Model.StreetType = AddressInfo.Street.TypeName;
                }
                else
                {
                    this.Model.Street = null;

                    this.Model.StreetType = HoohlShmoohlModel.StreetTypeDefault;
                }

                this.Model.DependedLocality = AddressInfo.DependedLocalityName;

                this.Model.PostalCode = AddressInfo.PostalCode;

                if (AddressInfo.AdministrativeArea != null)
                    this.Model.AdministrativeArea = AddressInfo.AdministrativeArea.FullName;
                else
                    this.Model.AdministrativeArea = null;

                if (AddressInfo.SubAdministrativeArea != null)
                    this.Model.SubAdministrativeArea = AddressInfo.SubAdministrativeArea.FullName;
                else
                    this.Model.SubAdministrativeArea = null;

            }

            ResetBuilding();
        }

        private void OnCountryChanged()
        {
            ResetAdministrativeArea();

            if (!string.IsNullOrEmpty(this.Model.Country))
                this.AddressInfo = AddressQuery.GetAddressInfo(this.Model.Country);
        }

        private void OnAdministrativeAreaChanged()
        {
            if (!string.IsNullOrEmpty(this.Model.AdministrativeArea))
                this.AddressInfo.AdministrativeArea = AddressInfo.ParseAdministrativeArea(this.Model.AdministrativeArea);
        }

        private void OnSubAdministrativeAreaChanged()
        {
            if (!string.IsNullOrEmpty(this.Model.SubAdministrativeArea))
                this.AddressInfo.SubAdministrativeArea = AddressInfo.ParseSubAdministrativeArea(this.Model.SubAdministrativeArea);
        }

        private void OnDependedLocalityChanged()
        {
            this.AddressInfo.DependedLocalityName = this.Model.DependedLocality;
        }

        private void OnPostalCodeChanged()
        {
            this.AddressInfo.PostalCode = this.Model.PostalCode;
        }

        private void OnCityChanged()
        {
            this.ResetSubAdministrativeArea(false);

            ResetStreet();

            if (string.IsNullOrEmpty(this.Model.City))
                return;

            if (AddressInfo == null)
                this.AddressInfo = AddressQuery.GetAddressInfo(this.Model.City);
            else
                this.AddressInfo.ParseCity(this.Model.City);

            this.Model.City = !string.IsNullOrEmpty(AddressInfo.City.Name) ? AddressInfo.City.Name : AddressInfo.City.FullName;

            this.Model.CityType = !string.IsNullOrEmpty(AddressInfo.City.TypeName) ? AddressInfo.City.TypeName : HoohlShmoohlModel.CityTypeDefault;

            if (AddressInfo.AdministrativeArea != null)
                this.Model.AdministrativeArea = AddressInfo.AdministrativeArea.FullName;
            else
                this.Model.AdministrativeArea = null;

            if (!string.IsNullOrEmpty(this.Model.City))
            {
                AddressInfo addr = AddressQuery.GetAddressInfo(GetSerachString());

                if (addr.AdministrativeArea != null)
                {
                    this.AddressInfo.AdministrativeArea = addr.AdministrativeArea;
                    this.Model.AdministrativeArea = addr.AdministrativeArea.FullName;
                }
                else
                {
                    this.AddressInfo.AdministrativeArea = null;
                    this.Model.AdministrativeArea = null;
                }

                if (addr.SubAdministrativeArea != null)
                {
                    this.AddressInfo.SubAdministrativeArea = addr.SubAdministrativeArea;
                    this.Model.SubAdministrativeArea = addr.SubAdministrativeArea.FullName;
                }
                else
                {
                    this.AddressInfo.SubAdministrativeArea = null;
                    this.Model.SubAdministrativeArea = null;
                }

                this.AddressInfo.DependedLocalityName = addr.DependedLocalityName;
                this.Model.DependedLocality = addr.DependedLocalityName;

                this.Model.PostalCode = addr.PostalCode;
                this.AddressInfo.PostalCode = addr.PostalCode;

                this.RefreshSearch();
            }
        }

        private void OnStreetChanged()
        {
            this.ResetSubAdministrativeArea(false);

            this.ResetBuilding();

            this.ResetPostalCode();

            if (string.IsNullOrEmpty(this.Model.Street))
                return;

            if (AddressInfo == null)
                this.AddressInfo = AddressQuery.GetAddressInfo(this.Model.Street);
            else
                this.AddressInfo.ParseStreet(this.Model.Street, AddressQuery.LastQueryContext);

            this.Model.Street = !string.IsNullOrEmpty(AddressInfo.Street.Name) ? AddressInfo.Street.Name : AddressInfo.Street.FullName;

            this.Model.StreetType = !string.IsNullOrEmpty(AddressInfo.Street.TypeName) ? AddressInfo.Street.TypeName : HoohlShmoohlModel.StreetTypeDefault;

            if (AddressInfo.AdministrativeArea != null)
            {
                this.AddressInfo.AdministrativeArea = AddressInfo.AdministrativeArea;
                this.Model.AdministrativeArea = AddressInfo.AdministrativeArea.FullName;
            }

            if (AddressInfo.City != null)
            {
                this.AddressInfo.City = AddressInfo.City;
                this.Model.City = !string.IsNullOrEmpty(AddressInfo.City.Name) ? AddressInfo.City.Name : AddressInfo.City.FullName;
            }

            if (!string.IsNullOrEmpty(this.Model.City) && !string.IsNullOrEmpty(this.Model.Street))
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
                    this.Model.AdministrativeArea = addr.AdministrativeArea.FullName;
                }

                if (addr.SubAdministrativeArea != null)
                {
                    this.AddressInfo.SubAdministrativeArea = addr.SubAdministrativeArea;
                    this.Model.SubAdministrativeArea = addr.SubAdministrativeArea.FullName;
                }

                if (!string.IsNullOrEmpty(addr.DependedLocalityName))
                {
                    this.AddressInfo.DependedLocalityName = addr.DependedLocalityName;
                    this.Model.DependedLocality = addr.DependedLocalityName;
                }

                this.Model.PostalCode = addr.PostalCode;
                this.AddressInfo.PostalCode = addr.PostalCode;

                this.RefreshSearch();
            }
        }

        private void OnBuildingChanged()
        {
            if (AddressInfo != null)
                this.AddressInfo.Building = null;

            if (string.IsNullOrEmpty(this.Model.Building))
                return;

            if (this.AddressInfo != null)
            {
                if (this.AddressInfo.Building == null)
                    this.AddressInfo.Building = new AddressInfo.BuildingInfo();

                if (AddressInfo.Building != null)
                    AddressInfo.Building.HomeName = this.Model.Building;

                AddressInfo.ParseBuilding(this.Model.Building);
            }
        }

        private void ResetAdministrativeArea()
        {
            this.Model.AdministrativeArea = null;
            this.AddressInfo.AdministrativeArea = null;

            this.ResetSubAdministrativeArea(true);
        }

        private void ResetSubAdministrativeArea(bool ResetCity)
        {
            this.Model.SubAdministrativeArea = null;
            this.AddressInfo.SubAdministrativeArea = null;

            this.Model.DependedLocality = null;
            this.AddressInfo.DependedLocalityName = null;

            if (ResetCity)
                this.ResetCity();
        }

        private void ResetCity()
        {
            this.Model.City = null;
            this.Model.CityType = HoohlShmoohlModel.CityTypeDefault;
            this.AddressInfo.City = null;

            ResetStreet();
        }

        private void ResetStreet()
        {
            this.Model.Street = null;

            this.Model.StreetType = HoohlShmoohlModel.StreetTypeDefault;

            this.AddressInfo.Street = null;

            ResetBuilding();

            ResetPostalCode();
        }

        private void ResetBuilding()
        {
            this.Model.Building = null;
            this.AddressInfo.Building = null;
        }


        private void ResetPostalCode()
        {
            this.Model.PostalCode = null;
            this.AddressInfo.PostalCode = null;
        }

        private void RefreshAddressString()
        {
            this.Model.Address = this.AddressInfo.ToString();
        }

        private void RefreshSearch()
        {
            this.Frozen = true;
            this.Model.Search = GetSerachString();
            this.Frozen = false;
        }

        private void UpdateAddressInfo()
        {
            AddressInfo = HoohlShmoohl.AddressQuery.GetAddressInfo(GetSerachString());
        }


        string GetSerachString()
        {
            return Utils.ToString(this.Model.Country, this.Model.City, this.Model.Street);
        }
    }
}
