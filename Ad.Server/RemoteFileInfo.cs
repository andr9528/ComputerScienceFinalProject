using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web;

namespace Ad.Server
{
    [DataContract]
    public class RemoteFileInfo : IDisposable
    {
        [DataMember]
        public string FileName;

        [DataMember]
        public long Length;

        [DataMember]
        public System.IO.Stream FileByteStream;

        public void Dispose()
        {
            if (FileByteStream != null)
            {
                FileByteStream.Close();
                FileByteStream = null;
            }
        }
    }
}