using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using HCAaudit.Service.Portal.AuditUI.Services;
using Microsoft.AspNetCore.Http;
using HCAaudit.Service.Portal.AuditUI.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;

namespace HCAaudit.Service.Portal.AuditUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration config;
        private IAuthService _authService;
        private AuditToolContext _auditToolContext;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, AuditToolContext audittoolc, IAuthService authService)
        {
            _auditToolContext = audittoolc;
            _logger = logger;
            config = configuration;
            _authService = authService;
        }



        //[Route("category/method")]
        public IActionResult Index()
        {
            bool isAdmin = _authService.CheckAdminUserGroup().Result;
            bool isAuditor = _authService.CheckAuditorUserGroup().Result;

            if (isAdmin || isAuditor)
            {
                if (isAdmin)
                {
                    return RedirectToAction("Details", "Category");
                }
                else
                {
                    return RedirectToAction("Index", "Search");
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
                if (isAdmin)
                {
                    return RedirectToAction("Details", "Category");
                }
                else
                {
                    return RedirectToAction("Index", "Search");
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

