using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using DevExpress.Utils.Editors;

namespace HoohlShmoohlEditor
{
    //http://maps.google.com/maps?hl=ru&q=%D0%BC%D0%BE%D1%81%D0%BA%D0%B2%D0%B0
    //http://maps.yandex.ru/?text=%D0%A0%D0%BE%D1%81%D1%81%D0%B8%D1%8F,%20%D0%9C%D0%BE%D1%81%D0%BA%D0%B2%D0%B0,%20%D0%A7%D0%B5%D0%BB%D1%8F%D0%B1%D0%B8%D0%BD%D1%81%D0%BA%D0%B0%D1%8F%20%D1%83%D0%BB%D0%B8%D1%86%D0%B0%2015&vrb=1

    //Print
    //http://maps.google.com/maps?hl=ru&q=%D0%BC%D0%BE%D1%81%D0%BA%D0%B2%D0%B0&ie=UTF8&hq=&hnear=%D0%B3%D0%BE%D1%80%D0%BE%D0%B4+%D0%9C%D0%BE%D1%81%D0%BA%D0%B2%D0%B0,+%D0%A0%D0%BE%D1%81%D1%81%D0%B8%D1%8F&z=9&pw=2
    //http://maps.yandex.ru/print/?text=%D0%A0%D0%BE%D1%81%D1%81%D0%B8%D1%8F,%20%D0%9C%D0%BE%D1%81%D0%BA%D0%B2%D0%B0,%20%D0%A7%D0%B5%D0%BB%D1%8F%D0%B1%D0%B8%D0%BD%D1%81%D0%BA%D0%B0%D1%8F%20%D1%83%D0%BB%D0%B8%D1%86%D0%B0&sll=37.829898,55.777788&sspn=0.0189,0.004425&ll=37.829898,55.777788&spn=0.022852,0.006833&z=16&l=map

    public partial class HoohlShmoohlModel : INotifyPropertyChanged
    {
        internal static string CityTypeDefault = "город";

        internal static string StreetTypeDefault = "улица";

        public HoohlShmoohlModel()
        {
        }

        private string _search;
        private string _country;
        private string _city;
        private string _cityType = CityTypeDefault;
        private string _street;
        private string _streetType = StreetTypeDefault;
        private string _building;
        private string _administrativeArea;
        private string _subAdministrativeArea;
        private string _postalCode;
        private string _dependedLocality;
        private string _address;
        private string _room;

        public string Search
        {
            get { return _search; }
            
            set 
            {
                if (Search != value)
                {
                    _search = value;

                    OnPropertyChanged("Search");
                }
            }
        }

        public string Country
        {
            get { return _country; }

            set
            {
                if (Country != value)
                {
                    _country = value;

                    OnPropertyChanged("Country");
                }
            }

        }

        public string City
        {
            get { return _city; }

            set
            {
                if (City != value)
                {
                    _city = value;

                    OnPropertyChanged("City");
                }
            }

        }

        public string CityType
        {
            get { return _cityType; }

            set
            {
                if (CityType != value)
                {
                    _cityType = value;

                    OnPropertyChanged("CityType");
                }
            }

        }

        public string Street
        {
            get { return _street; }

            set
            {
                if (Street != value)
                {
                    _street = value;

                    OnPropertyChanged("Street");
                }
            }

        }

        public string StreetType
        {
            get { return _streetType; }

            set
            {
                if (StreetType != value)
                {
                    _streetType = value;

                    OnPropertyChanged("StreetType");
                }
            }

        }

        public string Building
        {
            get { return _building; }

            set
            {
                if (Building != value)
                {
                    _building = value;

                    OnPropertyChanged("Building");
                }
            }
        }

        public string Address
        {
            get { return _address; }

            set
            {
                if (Address != value)
                {
                    _address = value;

                    OnPropertyChanged("Address");
                }
            }
        }

        public string AdministrativeArea
        {
            get 
            { 
                return _administrativeArea; 
            }

            set 
            {
                if (AdministrativeArea != value)
                {
                    _administrativeArea = value;

                    OnPropertyChanged("AdministrativeArea");
                }
            }
        }

        public string SubAdministrativeArea
        {
            get 
            { 
                return _subAdministrativeArea; 
            }
            
            set 
            {
                if (SubAdministrativeArea != value)
                {
                    _subAdministrativeArea = value;

                    OnPropertyChanged("SubAdministrativeArea");
                }
            }
        }

        public string DependedLocality
        {
            get { return _dependedLocality; }

            set 
            {
                if (DependedLocality != value)
                {
                    _dependedLocality = value;

                    OnPropertyChanged("DependedLocality");
                }
            }
        }

         
        public string PostalCode
        {
            get 
            { 
                return _postalCode; 
            }

            set 
            {
                if (PostalCode != value)
                {
                    _postalCode = value;

                    OnPropertyChanged("PostalCode");
                }
            }
        }

        public string Room
        {
            get { return _room; }
            
            set 
            {
                if (Room != value)
                {
                    _room = value;

                    OnPropertyChanged("Room");
                }
            }
        }

        protected virtual void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

    }
}
