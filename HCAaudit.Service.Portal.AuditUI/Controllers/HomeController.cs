using HCAaudit.Service.Portal.AuditUI.Models;
using HCAaudit.Service.Portal.AuditUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HCAaudit.Service.Portal.AuditUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAuthService _authService;
        public HomeController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Index()
        {
            bool isAdmin = _authService.CheckAdminUserGroup().Result;
            bool isAuditor = _authService.CheckAuditorUserGroup().Result;

            if (isAdmin || isAuditor)
            {
                if (isAuditor)
                {
                    return RedirectToAction("Index", "Search");
                }
                else
                {
                    return RedirectToAction("Index", "Category");
                }
            }
            else
            {
                return RedirectToAction("Home");
            }
        }

        public IActionResult Home()
        {
            bool isAdmin = _authService.CheckAdminUserGroup().Result;
            bool isAuditor = _authService.CheckAuditorUserGroup().Result;

            if (isAdmin || isAuditor)
            {
                if (isAuditor)
                {
                    return RedirectToAction("Index", "Search");
                }
                else
                {
                    return RedirectToAction("Index", "Category");
                }
            }
            else
            {
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
