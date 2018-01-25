using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEngine.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string name, string password)
        {

            return Json(new { ok = true });
        }
    }
}