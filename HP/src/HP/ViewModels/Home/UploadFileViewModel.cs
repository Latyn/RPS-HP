using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HP.ViewModels.Home
{
    public class UploadFileViewModel
    {
        public IFormFile File { get; set; }
        public string message { get; set; }
    }
}
