using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ad.Website.Pages
{

    public class IndexModel : PageModel
    {

        public void OnGet()
        {
            ServerService.ServerServiceClient bob = new ServerService.ServerServiceClient();
          //  bob.UploadFileAsync
        }

    }
}
