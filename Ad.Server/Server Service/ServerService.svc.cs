using Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Ad.Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServerService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ServerService.svc or ServerService.svc.cs at the Solution Explorer and start debugging.
    public class ServerService : ClientService, IServerService
    {
        public bool UploadFile(Stream stream)
        {
            if (!File.Exists(Path.Combine(adSaveLocation, "AdName.txt")))
                throw new ArgumentNullException("name", "Call SetNextFileName, before this method." );

            string name = File.ReadAllLines(Path.Combine(adSaveLocation, "AdName.txt")).First();
            string fileSaveLocation = Path.Combine(adSaveLocation, name);
            
            File.Delete(Path.Combine(adSaveLocation, "AdName.txt"));

            SharedCode.SaveFile(fileSaveLocation, stream);

            if (File.Exists(fileSaveLocation))
                return true;
            else 
                return false;
        }

        public void SetNextFileName(string name, bool @override = false)
        {
            if (!File.Exists(Path.Combine(adSaveLocation, "AdName.txt")) || @override)
                using (var writer = new StreamWriter(Path.Combine(adSaveLocation, "AdName.txt")))
                    writer.WriteLine(name);
            else
                throw new Exception("Name has already been set for the next upload");
        }
    }
}
