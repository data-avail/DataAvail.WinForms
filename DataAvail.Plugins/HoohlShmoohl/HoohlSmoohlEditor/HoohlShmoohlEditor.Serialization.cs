using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace HoohlShmoohlEditor
{
    partial class HoohlShmoohlEditor
    {
        static string SERIALIZATION_NAME = "HoohlShmoohlEditor.xml";

        public void Serialize()
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(SerializationData));

                StreamWriter streamWriter = new StreamWriter(SERIALIZATION_NAME);

                SerializationData szData = new SerializationData();

                szData.Country = Model.Country;
                szData.CityType = Model.CityType;
                szData.City = Model.City;
                szData.IsShowGoolgeMap = IsGoogleMapActive;

                xmlSerializer.Serialize(streamWriter, szData);

                streamWriter.Close();
            }
            catch
            {
            }

        }

        public void Deserialize()
        {
            SerializationData desirializedObject = null;

            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(SerializationData));

                StreamReader streamReder = new StreamReader(SERIALIZATION_NAME);

                desirializedObject = (SerializationData)xmlSerializer.Deserialize(streamReder);

                streamReder.Close();

                Model.Country = desirializedObject.Country;
                Model.CityType = desirializedObject.CityType;
                Model.City = desirializedObject.City;
                IsGoogleMapActive = desirializedObject.IsShowGoolgeMap;
            }
            catch (System.Exception)
            {
            }

         
        }

        [Serializable]
        public class SerializationData
        {
            public SerializationData()
            {
            }

            private string _country;
            private string _cityType;
            private string _city;
            private bool _isShowGoolgeMap;

            public string Country
            {
                get { return _country; }
                set { _country = value; }
            }

            public string CityType
            {
                get { return _cityType; }
                set { _cityType = value; }
            }

            public string City
            {
                get { return _city; }
                set { _city = value; }
            }

            public bool IsShowGoolgeMap
            {
                get { return _isShowGoolgeMap; }
                set { _isShowGoolgeMap = value; }
            }
        }
    }
}
