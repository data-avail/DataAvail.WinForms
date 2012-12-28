using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HoohlShmoohl
{
    partial class AddressInfo
    {
        public void Merge(AddressInfo AddressInfo)
        {
            if (string.IsNullOrEmpty(this.CountryName))            
                this.CountryName = AddressInfo.CountryName;

            if (string.IsNullOrEmpty(this.DependedLocalityName))
                this.DependedLocalityName = AddressInfo.DependedLocalityName;

            if (string.IsNullOrEmpty(this.LatName))
                this.LatName = AddressInfo.LatName;

            if (string.IsNullOrEmpty(this.PostalCode))
                this.PostalCode = AddressInfo.PostalCode;


            if (this.AdministrativeArea == null)
                this.AdministrativeArea = AddressInfo.AdministrativeArea;
            else if (AddressInfo.AdministrativeArea != null)
                this.AdministrativeArea.Merge(AddressInfo.AdministrativeArea);

            if (this.SubAdministrativeArea == null)
                this.SubAdministrativeArea = AddressInfo.SubAdministrativeArea;
            else if (AddressInfo.SubAdministrativeArea != null)
                this.SubAdministrativeArea.Merge(AddressInfo.SubAdministrativeArea);

            if (this.City == null)
                this.City = AddressInfo.City;
            else if (AddressInfo.City != null)
                this.City.Merge(AddressInfo.City);

            if (this.Street == null)
                this.Street = AddressInfo.Street;
            else if (AddressInfo.Street != null)
                this.Street.Merge(AddressInfo.Street);

            if (this.Building == null)
                this.Building = AddressInfo.Building;
            else if (AddressInfo.Building != null)
                this.Building.Merge(AddressInfo.Building);
        }

        partial class AdministrativeAreaInfo
        {
            public void Merge(AdministrativeAreaInfo AdministrativeAreaInfo)
            {
                MergeNameTypeInfo<AddressAdministrativeAreaType>(this, AdministrativeAreaInfo, AddressAdministrativeAreaType.Undefined);
            }
        }

        partial class SubAdministrativeAreaInfo
        {
            public void Merge(SubAdministrativeAreaInfo SubAdministrativeAreaInfo)
            {
                MergeNameTypeInfo<AddressSubAdministrativeAreaType>(this, SubAdministrativeAreaInfo, AddressSubAdministrativeAreaType.Undefined);
            }
        }

        partial class CityInfo 
        {
            public void Merge(CityInfo CityInfo)
            {
                MergeNameTypeInfo<AddressCityType>(this, CityInfo, AddressCityType.Undefined);
            }
        }

        partial class StreetInfo
        {
            public void Merge(StreetInfo StreetInfo)
            {
                MergeNameTypeInfo<AddressStreetType>(this, StreetInfo, AddressStreetType.Undefined);

                MergeBuildings(StreetInfo.Buildings);
            }

            void MergeBuildings(BuildingInfo[] Buildings)
            {         
                if (this.Buildings == null)
                {
                    this.Buildings = Buildings;
                }
                else if (Buildings != null)
                {
                    foreach (var i in this.Buildings)
                    {
                        var s = Buildings.FirstOrDefault(p => p.FullName == i.FullName);

                        if (s != null)
                            i.Merge(s);
                    }

                    //Append new 
                    this.Buildings = this.Buildings.Union(Buildings.Where(p => this.Buildings.FirstOrDefault(s => s.FullName == p.FullName) == null)).ToArray();
                }
            }
             
        }

        partial class BuildingInfo
        {
            public void Merge(BuildingInfo BuildingInfo)
            {
                if (string.IsNullOrEmpty(this.FullName))
                    this.FullName = BuildingInfo.FullName;

                if (string.IsNullOrEmpty(this.HomeName))
                    this.HomeName = BuildingInfo.HomeName;

                if (this.Home == null)
                {
                    this.Home = BuildingInfo.Home;
                }

                MergeAuxes(BuildingInfo.Auxes);
            }

            void MergeAuxes(AuxBuildingInfo[] Auxes)
            {
                if (this.Auxes == null)
                {
                    this.Auxes = Auxes;
                }
                else if (Auxes != null)
                {
                    foreach (var i in this.Auxes)
                    {
                        var s = Auxes.FirstOrDefault(p => p.AuxHomeName == i.AuxHomeName);

                        if (s != null)
                            i.Merge(s);
                    }

                    //Append new 
                    this.Auxes = this.Auxes.Union(Auxes.Where(p => this.Auxes.FirstOrDefault(s => s.AuxHomeName == p.AuxHomeName) == null)).ToArray();
                }
            }

            partial class AuxBuildingInfo
            {
                public void Merge(AuxBuildingInfo AuxBuildingInfo)
                {
                    if (string.IsNullOrEmpty(this.AuxHomeName))
                        this.AuxHomeName = AuxBuildingInfo.AuxHomeName;

                    if (string.IsNullOrEmpty(this.AuxHomeTypeName))
                        this.AuxHomeTypeName = AuxBuildingInfo._auxHomeTypeName;

                    if (this.AuxHome == null)
                        this.AuxHome = AuxBuildingInfo.AuxHome;

                    if (this.AuxHomeType == AddressAuxHomeType.Undefined)
                        this.AuxHomeType = AuxBuildingInfo.AuxHomeType;
                }
            }

        }

        static void MergeNameTypeInfo<T>(INameTypeInfo<T> A, INameTypeInfo<T> B, T TypeUndefined)
        {
            if (string.IsNullOrEmpty(A.FullName))
                A.FullName = B.FullName;

            if (string.IsNullOrEmpty(A.Name))
                A.Name = B.Name;

            if (string.IsNullOrEmpty(A.TypeName))
                A.TypeName = B.TypeName;

            if (A.Type.Equals(TypeUndefined))
            {
                A.Type = B.Type;
            }
        }

    }

}
