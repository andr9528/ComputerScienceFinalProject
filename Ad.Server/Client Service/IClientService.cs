﻿using Repository.Core;
using Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Ad.Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IClientService
    {
        [OperationContract]
        string GetHandlerConnectionString();

        [OperationContract]
        Stream DownloadFile(string fileNameAndExtension);
    }

}
