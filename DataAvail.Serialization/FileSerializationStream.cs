using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Serialization
{
    public class FileSerializationStream : ISerializationStream
    {
        #region ISerializationStream Members

        public System.IO.Stream GetStream(string Tag, bool Read)
        {
            try
            {
                System.IO.FileStream fs = new System.IO.FileStream(GetFilePath(Tag), Read ? System.IO.FileMode.Open : System.IO.FileMode.Create);

                return fs;
            }
            catch (System.IO.FileNotFoundException)
            {
                return null;
            }
        }

        #endregion

        private static string GetFilePath(string Tag)
        {
            return string.Format("{0}{1}.xml", string.IsNullOrEmpty(DeafaultDirectory) ? null : string.Format(@"{0}/", DeafaultDirectory), Tag);
        }

        public static string DeafaultDirectory;
    }
}
