using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraContainer
{
    partial class XtraContainer : DataAvail.Serialization.ISerializableObject
    {
        
        #region ISerializableObject Members

        public virtual void Serialize(System.Runtime.Serialization.SerializationInfo SerializationInfo)
        {
        }

        public virtual void Deserialize(System.Runtime.Serialization.SerializationInfo SerializationInfo)
        {
        }

        public DataAvail.Serialization.SerializationTag SerializationTag
        {
            get { return _serializationTag; }
        }

        public DataAvail.Serialization.ISerializableObject [] ChildrenSerializable
        {
            get
            {
                return this.FieldControls.Where(p => p is DataAvail.Serialization.ISerializableObject).Cast<DataAvail.Serialization.ISerializableObject>().ToArray();
            }
        }

        #endregion

        public virtual string SeraializationType { get { return "Layout"; } }
    }
}
