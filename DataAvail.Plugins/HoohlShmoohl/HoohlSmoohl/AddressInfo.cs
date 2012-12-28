using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HoohlShmoohl
{
    public partial class AddressInfo
    {
        public AddressInfo()
        {
        }

        internal AddressInfo(AddressInfoContextType ContextType)
        {
            _contextType = ContextType;
        }

        private string _countryName;

        private AdministrativeAreaInfo _administrativeArea;

        private SubAdministrativeAreaInfo _subAdministrativeArea;

        private CityInfo _city;

        private StreetInfo _street;

        private BuildingInfo _building;

        private string _postalCode;

        private string _name;

        private string _latName;

        private string _fullInfo;

        private string _dependedLocalityName;

        private AddressInfoContextType _contextType;

        public AddressInfoContextType ContextType
        {
            get { return _contextType; }
        }

        public string CountryName
        {
            get { return _countryName; }
            set { _countryName = value; }
        }

        public AdministrativeAreaInfo AdministrativeArea
        {
            get { return _administrativeArea; }
            set { _administrativeArea = value; }
        }

        public SubAdministrativeAreaInfo SubAdministrativeArea
        {
            get { return _subAdministrativeArea; }
            set { _subAdministrativeArea = value; }
        }

        public CityInfo City
        {
            get { return _city; }
            set { _city = value; }
        }

        public StreetInfo Street
        {
            get { return _street; }
            set { _street = value; }
        }

        public BuildingInfo Building
        {
            get { return _building; }
            set { _building = value; }
        }

        public string PostalCode
        {
            get { return _postalCode; }
            set { _postalCode = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string LatName
        {
            get { return _latName; }
            set { _latName = value; }
        }

        public string FullInfo
        {
            get { return _fullInfo; }
            set { _fullInfo = value; }
        }

        public string DependedLocalityName
        {
            get { return _dependedLocalityName; }
            set { _dependedLocalityName = value; }
        }

        public override string ToString()
        {
            return Utils.ToString(this.Street, this.Building, this.PostalCode, this.SubAdministrativeArea, this.DependedLocalityName, this.AdministrativeArea, this.City, this.CountryName);
        }

        public partial class AdministrativeAreaInfo : NameTypeInfo<AddressAdministrativeAreaType>
        {
        }

        public partial class SubAdministrativeAreaInfo : NameTypeInfo<AddressSubAdministrativeAreaType>
        {
        }

        public partial class CityInfo : NameTypeInfo<AddressCityType>
        {
        }

        public partial class StreetInfo : NameTypeInfo<AddressStreetType>
        {
            private BuildingInfo[] _buildings;

            public BuildingInfo[] Buildings
            {
                get { return _buildings; }

                set { _buildings = value; }
            }


        }

        public partial class BuildingInfo
        {
            private string _fullName;

            private string _homeName;

            private int? _home;

            private AuxBuildingInfo[] _auxes;

            public string FullName
            {
                get { return _fullName; }
                set { _fullName = value; }
            }

            public string HomeName
            {
                get { return _homeName; }
                set { _homeName = value; }
            }

            public int? Home
            {
                get { return _home; }
                set { _home = value; }
            }

            public AuxBuildingInfo[] Auxes
            {
                get { return _auxes; }
                set { _auxes = value; }
            }

            public override string ToString()
            {
                return this.FullName;
            }

            public partial class AuxBuildingInfo
            {
                private string _auxHomeName;

                private int? _auxHome;

                private string _auxHomeTypeName;

                private AddressAuxHomeType _auxHomeType;

                public string AuxHomeName
                {
                    get { return _auxHomeName; }
                    set { _auxHomeName = value; }
                }

                public AddressAuxHomeType AuxHomeType
                {
                    get { return _auxHomeType; }
                    set { _auxHomeType = value; }
                }

                public int? AuxHome
                {
                    get { return _auxHome; }
                    set { _auxHome = value; }
                }

                public string AuxHomeTypeName
                {
                    get { return _auxHomeTypeName; }
                    set { _auxHomeTypeName = value; }
                }
            }
        }

        public class NameTypeInfo<T> : INameTypeInfo<T>
        {
            public NameTypeInfo()
            { }

            private string _fullName;

            private string _name;

            private string _typeName;

            private T _type;

            public string FullName
            {
                get
                {
                    return _fullName;
                }
                set
                {
                    _fullName = value;
                }
            }

            public string Name
            {
                get
                {
                    return _name;
                }
                set
                {
                    _name = value;
                }
            }

            public string TypeName
            {
                get
                {
                    return _typeName;
                }
                set
                {
                    _typeName = value;
                }
            }

            public T Type
            {
                get
                {
                    return _type;
                }
                set
                {
                    _type = value;
                }
            }

            public override string ToString()
            {
                return FullName;
            }
        }

        public interface INameTypeInfo<T>
        {
            string FullName { get; set; }

            string Name { get; set; }

            string TypeName { get; set; }

            T Type { get; set; }
        }
    }

    public enum AddressCityType
    {
        Undefined,
        ///Город
        City,
        ///Деревня
        Country,
        ///Село
        Village,
        ///Поселок
        Habitat,
        ///Поселение
        Settelment,
        ///Станция
        Station

    }

    public enum AddressStreetType
    {
        Undefined,
        //Улица
        Street,
        //переулок
        Lane,
        //проезд
        Driveway,
        //площадь
        Square,
        //набережная
        Seafront,
        //тупик
        Deadend,
        //бульвар
        Broadway,
        //Проспект
        Avenu,
        //Шоссе
        Highway
    }

    public enum AddressAuxHomeType
    {
        Undefined,
        ///Строение
        Building,
        ///корпус
        Pavilion,
        ///дробь
        Shot,
        ///владение
        Premise
    }

    public enum AddressTargetType
    {
        Arbitrary = 0,
        Country = 33,
        Region = 545,
        City = 37,
        Street = 17
    }

    public enum AddressAdministrativeAreaType
    {
        Undefined,

        //Город
        City,

        //Облать
        Province
    }

    public enum AddressSubAdministrativeAreaType
    {
        Undefined,

        //Район
        Region,

        //ГОРОДСКОЙ ОКРУГ
        CityDistrict,

        //АО
        AdministrativeDistrict
    }
}
