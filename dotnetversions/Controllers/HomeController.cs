using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace dotnetversions.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Versions()
        {
            var model = Helpers.DotnetVersions.GetAllDotnetVersions();
            return View(model);
        }
        
    }
}
