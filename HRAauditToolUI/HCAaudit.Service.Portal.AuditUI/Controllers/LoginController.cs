using HCAaudit.Service.Portal.AuditUI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCAaudit.Service.Portal.AuditUI.Controllers
{
    public class LoginController : Controller
    {
        //[Route("login/index")]
        public IActionResult Index()
        
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login/Validate")]
        public IActionResult Validate(LoginModel loginDetails)
        {
            if (loginDetails != null && (loginDetails.UserName == "Admin" && loginDetails.Password == "Admin@123"))
            {
                return RedirectToAction("Index", "Search");
            }
            return View();
        }
    }
}
