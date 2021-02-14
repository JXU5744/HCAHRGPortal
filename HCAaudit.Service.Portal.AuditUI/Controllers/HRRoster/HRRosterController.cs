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
using HCAaudit.Service.Portal.AuditUI.ViewModel;

namespace HCAaudit.Service.Portal.AuditUI.Controllers
{
    [Authorize]
    public class HRRosterController : Controller
    {
        private readonly ILogger<HRRosterController> _logger;
        private readonly IConfiguration config;
        private readonly IAuthService _authService;
        private readonly AuditToolContext _auditToolContext;
        private bool isAdmin;
        public HRRosterController(ILogger<HRRosterController> logger, IConfiguration configuration, AuditToolContext audittoolc, IAuthService authService)
        {
            _auditToolContext = audittoolc;
            _logger = logger;
            config = configuration;
            _authService = authService;
            isAdmin = _authService.CheckAdminUserGroup().Result;
        }


        [HttpPost]
        public JsonResult GetDetailsById(EmployeeIdViewModel employeeIdViewModel)
        {
            if (isAdmin)
            {
                var data = (from hroc in _auditToolContext.HROCRoster select hroc).ToList();
                var objclstbHROCRoster = data.Where(x => x.EmployeethreefourID.ToLower().Equals(employeeIdViewModel.EmployeeId.ToLower()))
                                        .Select(x => new
                                        {
                                            x.HROCRosterId,
                                            x.EmployeeFullName,
                                            x.EmployeeLastName,
                                            x.EmployeeFirstName,
                                            x.EmployeethreefourID,
                                            x.EmployeeNumber,
                                            x.SupervisorLastName,
                                            x.SupervisorFirstName,
                                            x.DateHired,
                                            x.JobCDDesc,
                                            x.PositionDesc,
                                            x.EmployeeStatus,
                                            x.EmployeeStatusDesc
                                        }).ToList();

                return Json(objclstbHROCRoster);
            }
            else
            {
                return Json(new { Success = "False", responseText = "Authorization Error" });
            }
        }

        [HttpGet]
        public IActionResult Details()
        {
            if (isAdmin)
            {
                return View("Details");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        
        [HttpPost]
        public ActionResult Edit(HROCRosterViewModel hROCRosterViewModel)
        {
            if (isAdmin)
            {
                object resp = "";
                try
                {
                    if (hROCRosterViewModel.HROCRosterId > 0)
                    {
                        if (string.IsNullOrEmpty(resp.ToString()))
                        {
                            HROCRoster objHROCRoster = new HROCRoster
                            {
                                //objCategorys = GetSingleCategoryByid(param[0]);
                                EmployeeNumber = hROCRosterViewModel.EmployeeNumber,
                                EmployeeFullName = hROCRosterViewModel.EmployeeFullName,
                                EmployeeLastName = hROCRosterViewModel.EmployeeLastName,
                                EmployeeFirstName = hROCRosterViewModel.EmployeeFirstName,
                                SupervisorLastName = hROCRosterViewModel.SupervisorLastName,
                                SupervisorFirstName = hROCRosterViewModel.SupervisorFirstName,
                                EmployeethreefourID = hROCRosterViewModel.EmployeethreefourID.ToLower(),
                                EmployeeStatus = hROCRosterViewModel.EmployeeStatus,
                                PositionDesc = hROCRosterViewModel.PositionDesc,
                                JobCDDesc = hROCRosterViewModel.JobCDDesc,
                                EmployeeStatusDesc = hROCRosterViewModel.EmployeeStatusDesc,
                                DateHired = hROCRosterViewModel.DateHired
                            };
                            _auditToolContext.HROCRoster.Update(objHROCRoster);
                            _auditToolContext.SaveChanges();

                            resp = "Success";
                        }
                    }
                }
                catch (Exception ex)
                {
                    resp = "Error : " + ex;
                }
                return Json(resp);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public ActionResult Insert(HROCRosterViewModel hROCRosterViewModel)
        {
            if (isAdmin)
            {
                object resp = "";
                try
                {
                    if (string.IsNullOrEmpty(resp.ToString()))
                    {
                        HROCRoster objHROCRoster = new HROCRoster
                        {
                            HROCRosterId = 0,
                            EmployeeNumber = hROCRosterViewModel.EmployeeNumber,
                            EmployeeFullName = hROCRosterViewModel.EmployeeFullName,
                            EmployeeLastName = hROCRosterViewModel.EmployeeLastName,
                            EmployeeFirstName = hROCRosterViewModel.EmployeeFirstName,
                            SupervisorLastName = hROCRosterViewModel.SupervisorLastName,
                            SupervisorFirstName = hROCRosterViewModel.SupervisorFirstName,
                            EmployeethreefourID = hROCRosterViewModel.EmployeethreefourID.ToLower(),
                            EmployeeStatus = hROCRosterViewModel.EmployeeStatus,
                            PositionDesc = hROCRosterViewModel.PositionDesc,
                            JobCDDesc = hROCRosterViewModel.JobCDDesc,
                            EmployeeStatusDesc = hROCRosterViewModel.EmployeeStatusDesc,
                            DateHired = hROCRosterViewModel.DateHired
                        };
                        _auditToolContext.HROCRoster.Add(objHROCRoster);
                        _auditToolContext.SaveChanges();

                        resp = "Success";
                    }
                }
                catch (Exception ex)
                {
                    resp = "Error : " + ex;
                }
                return Json(resp);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }

}

