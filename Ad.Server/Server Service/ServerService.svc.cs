using Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Repository.Core;
using Repository.EntityFramework;
using Exception = System.Exception;
using Domain.Core;

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

            bool result = CreateAdInDatabase(fileSaveLocation);

            return VerifySuccecfullUpload(fileSaveLocation, result, true);
        }

        private bool CreateAdInDatabase(string fileSaveLocation)
        {
            FileInfo info = new FileInfo(fileSaveLocation);
            IRepository handler = new EntityRepositoryHandler(GetHandlerConnectionString());

            IAd ad = new Domain.Concrete.Ad() {Name = info.Name.Split('.').First(), FileExtension = info.Extension};

            bool result = handler.Add(ad, true);

            return result;
        }

        private bool VerifySuccecfullUpload(string path, bool adCreationResult, bool shouldThrow = false)
        {
            List<bool> bools = new List<bool>() {adCreationResult};
            List<Exception> errors = new List<Exception>();

            FileInfo info = new FileInfo(path);

            bools.Add(info.Exists);
            if (!info.Exists)
                errors.Add(new FileNotFoundException("The File was not created."));

            bool size = info.Length > 0;
            bools.Add(size);
            if (!size)
                errors.Add(new Exception("The File has a size of 0 bytes or less."));

            if (shouldThrow && errors.Count > 0)
                ThrowErrors(errors);

            if (bools.Any(x => x == false))
                return false;
            else
                return true;

        }

        private void ThrowErrors(List<Exception> errors)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var error in errors)
            {
                builder.AppendLine(error.Message + " :");
                builder.AppendLine("\t " + error.StackTrace);
                builder.AppendLine();
            }

            throw new Exception(builder.ToString());
        }

        public void SetNextFileName(string name)
        {
            SetNextFileName(name, false);
        }

        public void SetNextFileName(string name, bool @override)
        {
            if (!File.Exists(Path.Combine(adSaveLocation, "AdName.txt")) || @override)
                using (var writer = new StreamWriter(Path.Combine(adSaveLocation, "AdName.txt")))
                    writer.WriteLine(name);
            else
                throw new Exception("Name has already been set for the next upload");
        }
    }
}
