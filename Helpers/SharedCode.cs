using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class SharedCode
    {
        public static void SaveFile(string downloadedFileSaveLocation, Stream fileStream)
        {
            if (fileStream.CanRead == false)
                throw new Exception("Unable to read from stream, as it is Closed");

            CreateDirectoryForSaveLocation(downloadedFileSaveLocation);

            using (var file = File.Create(downloadedFileSaveLocation))  
            {  
                fileStream.CopyTo(file);  
            }  
        }  
  
        private static void CreateDirectoryForSaveLocation(string downloadedFileSaveLocation)  
        {  
            var fileInfo = new FileInfo(downloadedFileSaveLocation);  
            if (fileInfo.DirectoryName == null) throw new Exception("Save location directory could not be determined");  
            Directory.CreateDirectory(fileInfo.DirectoryName);  
        }
    }
}
