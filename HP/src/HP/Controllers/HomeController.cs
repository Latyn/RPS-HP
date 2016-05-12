using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http;
using System.IO;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNet.Hosting;
using HP.ViewModels.Home;
using HP.Models;

namespace HP.Controllers
{
    public class HomeController : Controller
    {

        private IHostingEnvironment _environment;
        private IGameRepository _repository;

        public HomeController(IHostingEnvironment environment, IGameRepository repository)
        {
            _environment = environment;
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "This challenge consists on creating a solution to a small problem and then making it publicly available in the web.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "HP challenge contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public  ActionResult UploadFile(UploadFileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var file = model.File;
                var parsedContentDisposition =
                    ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                var filename = Path.Combine(_environment.WebRootPath,
                    "Uploads", parsedContentDisposition.FileName.Trim('"'));
                file.SaveAsAsync(filename);
            }

            return View();
        }

        public ActionResult Clear()
        {
            _repository.ClearDB();

            return RedirectToAction("Index");
        }

    }
}
