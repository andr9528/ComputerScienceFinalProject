using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ad.Website.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            bool success = false;
            ServerService.ServerServiceClient serverService = new ServerService.ServerServiceClient();

            long size = files.Sum(f => f.Length);
            var filePath = Path.GetTempFileName();
        
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        //byte[] addBytes = new byte[stream.Length];
                        //await serverService.SetNextFileNameWithoutOverrideAsync(filePath);
                        //stream.Read(addBytes, 0, addBytes.Length);
                        //success = await serverService.UploadFileAsync(addBytes);
                        //await formFile.CopyToAsync(stream);
                    }
                }
            }
            if (success == true)
            {

            }
            else
            {

            }
            //var file = new { count = files.Count, size, filePath };
            return null;
        }
    }
}
