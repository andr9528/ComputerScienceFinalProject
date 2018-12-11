using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Ad.Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IServerService" in both code and config file together.
    [ServiceContract]
    public interface IServerService : IClientService
    {
        [OperationContract]
        bool UploadFile(Stream stream);

        [OperationContract(Name = "SetNextFileNameWithOverride")]
        void SetNextFileName(string name, bool @override);

        [OperationContract(Name = "SetNextFileNameWithoutOverride")]
        void SetNextFileName(string name);
    }
}
