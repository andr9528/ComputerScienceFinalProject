using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Repository.Core;
using Repository.EntityFramework;

namespace Ad.Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class ClientService : IClientService
    {
        internal string connectionPath = "..\\ConnectionStringFile.txt";
        internal string adSaveLocation = "..\\Ads";

        public Stream DownloadFile(string path)
        {
            if(WebOperationContext.Current == null) throw new Exception("WebOperationContext not set");  
  
            // As the current service is being used by a windows client, there is no browser interactivity.  
            // In case you are using the code Web, please use the appropriate content type.  
            var fileName = Path.GetFileName(path);  
            WebOperationContext.Current.OutgoingResponse.ContentType= "application/octet-stream";  
            WebOperationContext.Current.OutgoingResponse.Headers.Add("content-disposition", "inline; filename=" + fileName);  
  
            return File.OpenRead(path);  
        }

        public IRepository GetHandler()
        {
            return new EntityRepositoryHandler(connectionString: GetConnectionString());
        }

        private string GetConnectionString()
        {
            return File.ReadLines(connectionPath).First();
        }
    }
}
