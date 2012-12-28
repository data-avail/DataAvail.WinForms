using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Serialization
{
    public interface ISerializationFormatter
    {
        void Serialize(System.IO.Stream Stream, ISerializableObject SerializableObject);

        void Deserialize(System.IO.Stream Stream, ISerializableObject SerializableObject);
    }
}
