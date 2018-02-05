using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreEngine.Filter;
using Engine.Core.Kernel;
using Engine.Core.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreEngine.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            var User = UserMgr.GetUserBySId(Request.HttpContext.Session.Id);
            if (User == null)
            {
                return RedirectToAction("login");
            }
            return View(User);
        }


        public IActionResult Login()
        {
            var id = Request.HttpContext.Session.Id;
            return View();
        }

        [HttpPost]
        public IActionResult Login(string name, string password)
        {

            Global.Kernel.AddUser(new LoginUser
            {
                Name = name,
                Password = password,
                SessionId = Request.HttpContext.Session.Id

            });
            return Json(new { ok = true });
        }
    }
}