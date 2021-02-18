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
        private bool isAdmin = false;
        private IErrorLog _log;
        
        public HRRosterController(ILogger<HRRosterController> logger, IErrorLog log, IConfiguration configuration, AuditToolContext audittoolc, IAuthService authService)
        {
            _auditToolContext = audittoolc;
            _logger = logger;
            config = configuration;
            _authService = authService;
            isAdmin = _authService.CheckAdminUserGroup().Result;
            _log = log;
        }


        [HttpPost]
        public JsonResult GetDetailsById(EmployeeIdViewModel employeeIdViewModel)
        {
            try
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
                                                x.EmployeeStatusDesc,
                                                x.CreatedDate,
                                                x.CreatedBy,
                                                x.ModifiedBy,
                                                x.ModifiedDate
                                            }).ToList();

                    return Json(objclstbHROCRoster);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in GetDetailsById method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "HRRosterController_GetDetailsById", ErrorDiscription = ex.Message });
            }
            
            return Json(new { Success = "False", responseText = "Authorization Error" });
        }

        [HttpGet]
        public IActionResult Details()
        {
            try
            {
                if (isAdmin)
                {
                    return View("Details");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in Details method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "HRRosterController_Details", ErrorDiscription = ex.Message });
            }

            return RedirectToAction("Index", "Home");
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
                        HROCRoster objHROCRoster = _auditToolContext.HROCRoster.Where
                            (x => x.HROCRosterId == hROCRosterViewModel.HROCRosterId).FirstOrDefault();
                        if (objHROCRoster != null)
                        {
                            //objCategorys = GetSingleCategoryByid(param[0]);
                            objHROCRoster.EmployeeNumber = hROCRosterViewModel.EmployeeNumber;
                            objHROCRoster.EmployeeFullName = hROCRosterViewModel.EmployeeFullName;
                            objHROCRoster.EmployeeLastName = hROCRosterViewModel.EmployeeLastName;
                            objHROCRoster.EmployeeFirstName = hROCRosterViewModel.EmployeeFirstName;
                            objHROCRoster.SupervisorLastName = hROCRosterViewModel.SupervisorLastName;
                            objHROCRoster.SupervisorFirstName = hROCRosterViewModel.SupervisorFirstName;
                            objHROCRoster.EmployeethreefourID = hROCRosterViewModel.EmployeethreefourID.ToLower();
                            objHROCRoster.EmployeeStatus = hROCRosterViewModel.EmployeeStatus;
                            objHROCRoster.PositionDesc = hROCRosterViewModel.PositionDesc;
                            objHROCRoster.JobCDDesc = hROCRosterViewModel.JobCDDesc;
                            objHROCRoster.EmployeeStatusDesc = hROCRosterViewModel.EmployeeStatusDesc;
                            objHROCRoster.DateHired = hROCRosterViewModel.DateHired;
                            objHROCRoster.ModifiedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                            objHROCRoster.ModifiedDate = DateTime.Now;
                            _auditToolContext.HROCRoster.Update(objHROCRoster);
                            _auditToolContext.SaveChanges();

                            resp = "Success";
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Exception in Edit method");
                    _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "HRRosterController_Edit", ErrorDiscription = ex.Message });
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
                            DateHired = hROCRosterViewModel.DateHired,
                            CreatedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName,
                            CreatedDate = DateTime.Now,
                            ModifiedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName,
                            ModifiedDate = DateTime.Now
                        };
                        _auditToolContext.HROCRoster.Add(objHROCRoster);
                        _auditToolContext.SaveChanges();

                        resp = "Success";
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Exception in Insert method");
                    _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "HRRosterController_Insert", ErrorDiscription = ex.Message });
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

