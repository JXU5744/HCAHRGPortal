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
    //[Authorize]
    public class HRRosterController : Controller
    {
        private readonly ILogger<HRRosterController> _logger;
        private readonly IConfiguration config;
        private IAuthService _authService;
        private AuditToolContext _auditToolContext;
        public HRRosterController(ILogger<HRRosterController> logger, IConfiguration configuration, AuditToolContext audittoolc)//, IAuthService authService)
        {
            _auditToolContext = audittoolc;
            _logger = logger;
            config = configuration;
      //      _authService = authService;
        }
       
        public IActionResult GetDetails()
        {
            clstbHROCRosterList objclstbHROCRosterList = new clstbHROCRosterList();
            objclstbHROCRosterList.HROCRosterList = new List<clstbHROCRoster>();
            objclstbHROCRosterList.HROCRosterList = (from hroc in _auditToolContext.hrocMaster select hroc).ToList();
            
            return View("Details",objclstbHROCRosterList);
        }

        public IActionResult StatusUpdate(string id)
        {
            var data = (from hroc in _auditToolContext.hrocMaster select hroc).ToList();
            clstbHROCRoster objclstbHROCRoster = data.Find(a => a.EmployeethreefourID == id);

            if (objclstbHROCRoster.EmployeeStatusDesc.ToLower() == "active full time")
            {
                objclstbHROCRoster.EmployeeStatusDesc = "Non-Facility Paid";
                objclstbHROCRoster.EmployeeStatus = "99";
            }
            else {
                objclstbHROCRoster.EmployeeStatusDesc = "Active Full Time";
                objclstbHROCRoster.EmployeeStatus = "01";
            }

            _auditToolContext.hrocMaster.Update(objclstbHROCRoster);
            _auditToolContext.SaveChanges();

            return RedirectToAction("GetDetails");
        }
    }
   
}

