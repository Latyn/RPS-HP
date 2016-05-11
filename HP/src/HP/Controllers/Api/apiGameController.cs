using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HP.Models;

namespace HP.Controllers.Api
{
    public class ApiGameController:Controller
    {
        private IGameRepository  _repository;

        public ApiGameController(IGameRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet ("api/championship/top")]
        public JsonResult Top( int count )
        {
            string[] tops  = _repository.GetTop(count).ToArray();

            //Json now as a part of the father controller class
            return Json(new { players    = tops });
        }
        
        [HttpPost("/api/championship/result")]
        public IActionResult Result()
        {

            //Json now as a part of the father controller class
            return Json(new { name = "Michael"});
        }

        [HttpPost("/api/championship/New")]
        public IActionResult New()
        {

            //Json now as a part of the father controller class
            return Json("test");
        }

    }
    [Newtonsoft.Json.JsonObject(Title = "Players")]
    public class Test
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "name")]
        public List<string> Name { get; set; }

    }
}
