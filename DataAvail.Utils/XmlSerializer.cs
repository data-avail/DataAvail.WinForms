using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Utils
{
    public static class XmlSerializer<T> where T : class
    {
        public static bool Serialize(T SerializedObject, string FileName)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

                System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(FileName);

                xmlSerializer.Serialize(streamWriter, SerializedObject);

                streamWriter.Close();
            }
            catch (System.Exception)
            {
                return false;
            }

            return true;
        }


        public static T DeSerialize(string FileName)
        {
            T desirializedObject = null;

            try
            {
                System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

                System.IO.StreamReader streamReder = new System.IO.StreamReader(FileName);

                desirializedObject = (T)xmlSerializer.Deserialize(streamReder);

                streamReder.Close();
            }
            catch (System.Exception)
            {
                return null;
            }

            return desirializedObject;
        }
    }
}
