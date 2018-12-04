using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web;

namespace Ad.Server
{
    [DataContract]
    public class DownloadRequest
    {
        [DataMember]
        public string FileName { get; set; }
    }
}