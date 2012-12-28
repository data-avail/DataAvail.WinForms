using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Serialization
{
    public interface ISerializationStream
    {
        System.IO.Stream GetStream(string Tag, bool Read);
    }
}
