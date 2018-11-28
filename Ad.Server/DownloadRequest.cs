using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace Ad.Server
{
    [MessageContract]
    public class DownloadRequest
    {
        [MessageBodyMember]
        public string FileName { get; set; }
    }
}