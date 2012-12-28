using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.DX.XtraContainer
{
    partial class XtraContainer
    {
        public override void Serialize(System.Runtime.Serialization.SerializationInfo SerializationInfo)
        {
            base.Serialize(SerializationInfo);

            System.IO.Stream stream = DataAvail.Serialization.SerializationManager.SerailizationStream.GetStream(SerializationTag.ToString(), false);

            try
            {
                layoutControl1.SaveLayoutToStream(stream);
            }
            finally
            {
                stream.Close();
            }
        }

        public override void Deserialize(System.Runtime.Serialization.SerializationInfo SerializationInfo)
        {
            base.Deserialize(SerializationInfo);

            System.IO.Stream stream = DataAvail.Serialization.SerializationManager.SerailizationStream.GetStream(SerializationTag.ToString(), true);

            if (stream != null)
            {
                try
                {
                    layoutControl1.RestoreLayoutFromStream(stream);
                }
                finally
                {
                    stream.Close();
                }
            }
        }
    }
}
