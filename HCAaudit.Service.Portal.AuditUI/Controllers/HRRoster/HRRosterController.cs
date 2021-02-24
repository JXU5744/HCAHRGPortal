using HCAaudit.Service.Portal.AuditUI.Models;
using HCAaudit.Service.Portal.AuditUI.Services;
using HCAaudit.Service.Portal.AuditUI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace HCAaudit.Service.Portal.AuditUI.Controllers
{
    [Authorize]
    public class HRRosterController : Controller
    {
        private readonly ILogger<HRRosterController> _logger;
        private readonly IAuthService _authService;
        private readonly AuditToolContext _auditToolContext;
        private readonly bool isAdmin = false;
        private readonly IErrorLog _log;

        public HRRosterController(ILogger<HRRosterController> logger, IErrorLog log, IConfiguration configuration, AuditToolContext audittoolc, IAuthService authService)
        {
            _auditToolContext = audittoolc;
            _logger = logger;
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
                    var objclstbHROCRoster = data.Where(x => x.Employee34IdLowerCase.ToLower().Equals(employeeIdViewModel.EmployeeId.ToLower()))
                                            .Select(x => new
                                            {
                                                x.HROCRosterId,
                                                x.EmployeeFullName,
                                                x.LastName,
                                                x.FirstName,
                                                x.Employee34IdLowerCase,
                                                x.EmployeeNum,
                                                x.SupervisorLastName,
                                                x.SupervisorFirstName,
                                                x.JobCdDescHomeCurr,
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

        public IActionResult GetCategory()
        {
            try
            {
                if (isAdmin)
                {
                    var categoryList = _auditToolContext.Category.Where(a => a.IsActive == true).ToList();
                    _logger.LogInformation($"No of records: {categoryList.Count}");
                    return Json(categoryList);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in GetCategory method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "HRRosterController_GetCategory", ErrorDiscription = ex.Message });
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
                            objHROCRoster.EmployeeNum = hROCRosterViewModel.EmployeeNumber;
                            objHROCRoster.EmployeeFullName = hROCRosterViewModel.EmployeeFullName;
                            objHROCRoster.LastName = hROCRosterViewModel.EmployeeLastName;
                            objHROCRoster.FirstName = hROCRosterViewModel.EmployeeFirstName;
                            objHROCRoster.SupervisorLastName = hROCRosterViewModel.SupervisorLastName;
                            objHROCRoster.SupervisorFirstName = hROCRosterViewModel.SupervisorFirstName;
                            objHROCRoster.Employee34IdLowerCase = hROCRosterViewModel.EmployeethreefourID.ToLower();
                            objHROCRoster.JobCdDescHomeCurr = hROCRosterViewModel.JobCDDesc;
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
                            EmployeeNum = hROCRosterViewModel.EmployeeNumber,
                            EmployeeFullName = hROCRosterViewModel.EmployeeFullName,
                            LastName = hROCRosterViewModel.EmployeeLastName,
                            FirstName = hROCRosterViewModel.EmployeeFirstName,
                            SupervisorLastName = hROCRosterViewModel.SupervisorLastName,
                            SupervisorFirstName = hROCRosterViewModel.SupervisorFirstName,
                            Employee34IdLowerCase = hROCRosterViewModel.EmployeethreefourID.ToLower(),
                            JobCdDescHomeCurr = hROCRosterViewModel.JobCDDesc,
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