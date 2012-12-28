using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HoohlShmoohl;

namespace HoohlShmoohl
{
    partial class AddressInfo
    {
        internal static AddressInfo.CityInfo ParseCityStatic(string CityName)
        {
            return ParseType<AddressInfo.CityInfo, AddressCityType>(CityName, CityTypes);
           
        }

        static IEnumerable<NameTypePair> CityTypes
        {
            get
            {
                return new NameTypePair[] { 
                new NameTypePair("ГОРОД", (int)AddressCityType.City),
                new NameTypePair("ДЕРЕВНЯ", (int)AddressCityType.Country),
                new NameTypePair("СЕЛО", (int)AddressCityType.Village),
                new NameTypePair("ПОСЕЛОК", (int)AddressCityType.Habitat)};
            }
        }

        public static AddressInfo.AdministrativeAreaInfo ParseAdministrativeArea(string AdministrativeAreaName)
        {
            return ParseType<AddressInfo.AdministrativeAreaInfo, AddressAdministrativeAreaType>(AdministrativeAreaName, AdministrativeAreaTypes);
        }

        static IEnumerable<NameTypePair> AdministrativeAreaTypes
        {
            get
            {
                return new NameTypePair[] { 
                new NameTypePair("ГОРОД", (int)AddressAdministrativeAreaType.City),
                new NameTypePair("ОБЛАСТЬ", (int)AddressAdministrativeAreaType.Province)};
            }
        }

        public static AddressInfo.SubAdministrativeAreaInfo ParseSubAdministrativeArea(string SubAdministrativeAreaName)
        {
            return ParseType<AddressInfo.SubAdministrativeAreaInfo, AddressSubAdministrativeAreaType>(SubAdministrativeAreaName, SubAdministrativeAreaTypes);
        }

        static IEnumerable<NameTypePair> SubAdministrativeAreaTypes
        {
            get
            {
                return new NameTypePair[] { 
                new NameTypePair("РАЙОН", (int)AddressSubAdministrativeAreaType.Region),
                new NameTypePair("ГОРОДСКОЙ ОКРУГ", (int)AddressSubAdministrativeAreaType.CityDistrict),
                new NameTypePair("АО", (int)AddressSubAdministrativeAreaType.AdministrativeDistrict) };
            }
        }

        internal static AddressInfo.StreetInfo ParseStreetStatic(string StreetName)
        {
            return ParseType<AddressInfo.StreetInfo, AddressStreetType>(StreetName, StreetTypes);
        }

        static IEnumerable<NameTypePair> StreetTypes
        {
            get
            {
                return new NameTypePair[] { 
                new NameTypePair("УЛ.", (int)AddressStreetType.Street),
                new NameTypePair("УЛИЦА", (int)AddressStreetType.Street),
                new NameTypePair("ULITSA", (int)AddressStreetType.Street),
                new NameTypePair("ТУП.", (int)AddressStreetType.Deadend),
                new NameTypePair("ТУПИК", (int)AddressStreetType.Street),
                new NameTypePair("НАБ.", (int)AddressStreetType.Seafront),
                new NameTypePair("НАБЕРЕЖНАЯ", (int)AddressStreetType.Street),
                new NameTypePair("БУЛ.", (int)AddressStreetType.Broadway),
                new NameTypePair("БУЛЬВАР", (int)AddressStreetType.Street),
                new NameTypePair("ПРОСПЕКТ", (int)AddressStreetType.Avenu),
                new NameTypePair("ПРОСП.", (int)AddressStreetType.Avenu),
                new NameTypePair("ПЕРЕУЛОК", (int)AddressStreetType.Lane),
                new NameTypePair("ПЕР.", (int)AddressStreetType.Lane),
                new NameTypePair("ПРОЕЗД", (int)AddressStreetType.Driveway),
                new NameTypePair("ШОССЕ", (int)AddressStreetType.Highway),
                new NameTypePair("Ш.", (int)AddressStreetType.Highway)
                };
            }
        }

        public static void ParseBuilding(BuildingInfo BuildingInfo, string MainPart, string [] Auxes)
        {
            int i;

            BuildingInfo.HomeName = MainPart;

            if (int.TryParse(MainPart, out i))
            {
                BuildingInfo.Home = i;
            }

            BuildingInfo.Auxes = Auxes.Select(p=>ParseBuildingAuxesInfo(p)).ToArray();
        }

        static BuildingInfo.AuxBuildingInfo ParseBuildingAuxesInfo(string Aux)
        {
            int i;

            BuildingInfo.AuxBuildingInfo auxBuildingInfo = new BuildingInfo.AuxBuildingInfo();

            auxBuildingInfo.AuxHomeName = Aux;

            ParsedType parsedType = ParseType(auxBuildingInfo.AuxHomeName, AuxBuildingTypes);

            if (parsedType != null)
            {
                auxBuildingInfo.AuxHomeName = parsedType.name;
                auxBuildingInfo.AuxHomeTypeName = parsedType.typeName;
                auxBuildingInfo.AuxHomeType = (AddressAuxHomeType)parsedType.type;

                if (!string.IsNullOrEmpty(auxBuildingInfo.AuxHomeName))
                {
                    if (int.TryParse(auxBuildingInfo.AuxHomeName, out i))
                    {
                        auxBuildingInfo.AuxHome = i;
                    }
                }
            }

            return auxBuildingInfo;
        }

        static IEnumerable<NameTypePair> AuxBuildingTypes
        {
            get
            {
                return new NameTypePair[] { 
                new NameTypePair("КОРПУС", (int)AddressAuxHomeType.Pavilion),
                new NameTypePair("К", (int)AddressAuxHomeType.Pavilion),
                new NameTypePair("СТРОЕНИЕ", (int)AddressAuxHomeType.Building),
                new NameTypePair("С", (int)AddressAuxHomeType.Building),
                new NameTypePair("/", (int)AddressAuxHomeType.Shot)};
            }
        }

        #region Types parsing

        public static T ParseType<T, Y>(string FullName, IEnumerable<NameTypePair> NameTypePairs) where T : class, INameTypeInfo<Y>, new()
        {
            T obj = new T();

            ParseType(FullName, NameTypePairs, obj);

            return obj;
        }

        static bool ParseType<T>(string FullName, IEnumerable<NameTypePair> NameTypePairs, INameTypeInfo<T> NameTypeInfo)
        {
            NameTypeInfo.FullName = FullName;

            ParsedType parsedType = ParseType(FullName, NameTypePairs);

            if (parsedType != null)
            {
                NameTypeInfo.Name = parsedType.name;
                NameTypeInfo.TypeName = parsedType.typeName;
                NameTypeInfo.Type = (T)(object)parsedType.type;

                return true;
            }
            else
            {
                return false;
            }
        }

        static ParsedType ParseType(string FullName, IEnumerable<NameTypePair> NameTypePairs)
        {
            string up = FullName.ToUpper();

            NameTypePair ntp = NameTypePairs.FirstOrDefault(p => up.StartsWith(p.name.ToUpper() + " ") || up.EndsWith(" " + p.name.ToUpper()));

            if (ntp != null)
            {
                int f = up.IndexOf(ntp.name.ToUpper());

                string name = FullName.Remove(f, ntp.name.Length).Trim();

                string typeName = FullName.Substring(f, ntp.name.Length).Trim();

                return new ParsedType(name, ntp.type, typeName);
            }
            else
            {
                return null;
            }
        }

        public class NameTypePair
        {
            internal NameTypePair(string Name, int Type)
            {
                name = Name;

                type = Type;
            }

            internal readonly string name;

            internal readonly int type;

            internal bool IsEmpty { get { return string.IsNullOrEmpty(name); } }
        }

        public class ParsedType : NameTypePair
        {
            internal ParsedType(string Name, int Type, string TypeName)
                : base(Name, Type)
            {
                this.typeName = TypeName;
            }

            internal readonly string typeName;
        }

        #endregion
    }
}
