using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Serialization
{
    public interface ISerializableObject
    {
        void Serialize(System.Runtime.Serialization.SerializationInfo SerializationInfo);

        void Deserialize(System.Runtime.Serialization.SerializationInfo SerializationInfo);

        SerializationTag SerializationTag { get; }

        ISerializableObject[] ChildrenSerializable { get; }
    }

    public struct SerializationTag
    {
        public SerializationTag(string Type, string Name)
        {
            this.Type = Type;

            this.Name = Name;
        }

        public readonly string Type;

        public readonly string Name;

        public bool IsEmpty { get { return string.IsNullOrEmpty(Type) && string.IsNullOrEmpty(Name); } }

        public override string ToString()
        {
            return string.Format("{0}_{1}", Name, Type);
        }
    }
}
