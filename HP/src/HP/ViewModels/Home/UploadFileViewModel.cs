using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using HP.Models;

namespace HP.ViewModels.Home
{
    public class UploadFileViewModel
    {
        public IFormFile File { get; set; }
        public string message { get; set; }
        public string winner{ get; set; }
        public string second { get; set; }
        public int winnerScore { get; set; }
        public int secondScore { get; set; }
        public bool buttons { get; set; } = true;
    }
}
