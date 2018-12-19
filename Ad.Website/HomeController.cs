using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ad.Website
{
    public class HomeController : Controller
    {
        public ActionResult UploadFiles()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFiles(List<IFormFile> files)
        {
            Console.WriteLine("Upload initiated");

            bool success = false;
            ServerServiceClient serverService = new ServerServiceClient();

            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                var filePath = formFile.FileName;

                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        serverService.SetNextFileNameWithoutOverride(filePath);
                        success = serverService.UploadFile(stream);
                    }
                }
            }
            if (success == true)
            {
                Console.WriteLine("Success");
            }
            else
            {
                Console.WriteLine("Nope,m8");
            }
            return RedirectToAction("Index");
        }





        private void UploadFileToRemoteLocation()
        {
            ServerServiceClient serverService = new ServerServiceClient();

            try
            {
                FileInfo file = ChoseFile();
                Stream stream = file.OpenRead();
                serverService.SetNextFileNameWithoutOverride(file.Name);
                serverService.UploadFile(stream);
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
    }
}
