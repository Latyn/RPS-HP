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
using System.Text;
using Microsoft.AspNet.Mvc.ViewFeatures;

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

        /*public IActionResult Index()
        {
            return View();
        }*/
        public IActionResult Index(UploadFileViewModel model)
        {
            return View(model);
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
              

                string text = System.IO.File.ReadAllText(filename);

                file.SaveAsAsync(filename);
                List<Tournament> Tournaments = _repository.toTournament(text);

                if (Tournaments.Count>1)
                {
                    model.message = "This document has more than one tournament on this file";
                }
                else
                {
                    model.message = "File has been uploaded";
                }
            }

            return RedirectToAction("Index", model);
        }

        public ActionResult Clear(UploadFileViewModel model)
        {
            _repository.ClearDB();

            model.message = "DB has been deleted";

            return RedirectToAction("Index", model);
        }

        public ActionResult Download(string virtualFilePath)
        {

            var filename = Path.Combine(_environment.WebRootPath,
                "Downloads", virtualFilePath);

            //  var file = File(virtualFilePath, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(virtualFilePath));
            string text = System.IO.File.ReadAllText(filename);
            byte[] array = Encoding.ASCII.GetBytes(text);
            return File(
                array, System.Net.Mime.MediaTypeNames.Application.Octet, "Tournament.txt");
        }
    }
}
