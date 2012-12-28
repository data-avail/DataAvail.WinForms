using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Serialization
{
    public static class SerializationManager
    {
        public static void Serialize(ISerializableObject SerializableObject, string Tag)
        {
            System.IO.Stream stream = SerailizationStream.GetStream(Tag, false);

            SerializationFormatter.Serialize(stream, SerializableObject);

            stream.Close();
        }

        public static void Deserialize(ISerializableObject SerializableObject, string Tag)
        {
            System.IO.Stream stream = SerailizationStream.GetStream(Tag, true);

            if (stream != null)
            {
                SerializationFormatter.Deserialize(stream, SerializableObject);

                stream.Close();
            }
        }

        public static ISerializationStream SerailizationStream = new FileSerializationStream();

        public static ISerializationFormatter SerializationFormatter = new XMLSerializationFormatter();

    }
}
