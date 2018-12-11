using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DownloadUploadTesting.ServerService;
using System.Web;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace DownloadUploadTesting
{
    class Program
    {
        ServerServiceClient server = new ServerServiceClient();

        [STAThread]
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private void Run()
        {
            //DownLoadFileFromRemoteLocation("Circle of Life.mp4");

            UploadFileToRemoteLocation();
        }



        #region Download
        private void DownLoadFileFromRemoteLocation(string fileNameAndExtension, string downloadedFileSaveLocation = @"..\Ads\")
        {
            try
            {
                using (var fileStream = server.DownloadFile(fileNameAndExtension))
                {
                    if (fileStream == null)
                    {
                        Console.WriteLine("File not recieved");
                        return;
                    }

                    CreateDirectoryForSaveLocation(downloadedFileSaveLocation);
                    SaveFile(Path.Combine(downloadedFileSaveLocation, fileNameAndExtension), fileStream);
                }
                Console.WriteLine("File downloaded and copied");
            }
            catch (Exception ex)
            {
                Console.WriteLine("File could not be downloaded or saved. Message :" + ex.Message);
            }

        }

        private void SaveFile(string downloadedFileSaveLocation, Stream fileStream)
        {
            using (var file = File.Create(downloadedFileSaveLocation))
            {
                fileStream.CopyTo(file);
            }
        }

        private void CreateDirectoryForSaveLocation(string downloadedFileSaveLocation)
        {
            var fileInfo = new FileInfo(downloadedFileSaveLocation);
            if (fileInfo.DirectoryName == null) throw new Exception("Save location directory could not be determined");
            Directory.CreateDirectory(fileInfo.DirectoryName);
        }
        #endregion

        #region Upload

        private void UploadFileToRemoteLocation()
        {
            try
            {
                FileInfo file = ChoseFile();
                Stream stream = file.OpenRead();
                server.SetNextFileNameWithoutOverride(file.Name);
                server.UploadFile(stream);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private FileInfo ChoseFile()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                InitialDirectory = @"C:\ComputerScienceFinalProject\Testing Solutions\DownloadUploadTesting\bin\Ads",
                Filter = "Media files (*.mp4)|*.mp4;",
                FilterIndex = 0,
                RestoreDirectory = true
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo info = new FileInfo(dialog.FileName);

                return info;
            }

            return null;
        }

        #endregion
    }
}
