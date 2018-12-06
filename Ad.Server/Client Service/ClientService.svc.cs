using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Hosting;
using Repository.Core;
using Repository.EntityFramework;

namespace Ad.Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class ClientService : IClientService
    {
        internal string connectionPath = "..\\ConnectionStringFile.txt";
        internal string adSaveLocation = @"C:\inetpub\wwwroot\AdProgram\Service\Ads";

        public Stream DownloadFile(string fileNameAndExtension)
        {
            try
            {
                FileStream file = File.OpenRead(Path.Combine(adSaveLocation, fileNameAndExtension));
                return file;
            }
            catch (IOException ex)
            {
                Console.WriteLine(
                    string.Format("An exception was thrown while trying to open file {0}", fileNameAndExtension));
                Console.WriteLine("Exception is: ");
                Console.WriteLine(ex.ToString());
                throw ex;
            }
            
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
