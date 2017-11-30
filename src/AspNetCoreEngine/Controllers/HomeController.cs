using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using TestServices;
using AspNetCoreEngine.Filter;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreEngine.Controllers
{
    [ActionFilter]
    public class HomeController : Controller
    {
        public ICommonService commonService;

        //注入服务
        public HomeController(ICommonService cms)
        {
            commonService = cms;

        }
        public IActionResult Index()
        {
            var world = commonService.GetWorld();
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
