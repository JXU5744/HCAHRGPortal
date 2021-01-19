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

        private clstbHROCRosterList GetDetails()
        {
            clstbHROCRosterList objclstbHROCRosterList = new clstbHROCRosterList();
            objclstbHROCRosterList._hrocrosterList = new List<clstbHROCRoster>();
            objclstbHROCRosterList._hrocrosterList = (from hroc in _auditToolContext.hrocMaster select hroc).ToList();
            return objclstbHROCRosterList;
        }

        [HttpGet]
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

            return RedirectToAction("details");
        }

        [HttpGet]
        public IActionResult Details()
        {
            return View("details", GetDetails());
        }

        [HttpPost]
        public IActionResult Details(clstbHROCRosterList objclstbHROCRosterList)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();

                // Skip number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();

                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();

                // Sort Column Name  
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

                // Sort Column Direction (asc, desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10, 20, 50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;

                int skip = start != null ? Convert.ToInt32(start) : 0;

                int recordsTotal = 0;

                objclstbHROCRosterList._hrocrosterList = new List<clstbHROCRoster>();
                objclstbHROCRosterList._hrocrosterList = (from hroc in _auditToolContext.hrocMaster select hroc).ToList();

                // getting all Customer data  
                var customerData = (from tempcustomer in objclstbHROCRosterList._hrocrosterList
                                    select tempcustomer);

                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    switch (sortColumn)
                    {
                        case "EmployeeFullName":
                            if (sortColumnDirection == "desc")
                            {
                                customerData = customerData.OrderByDescending(s => s.EmployeeFullName);
                            }
                            else
                            {
                                customerData = customerData.OrderBy(s => s.EmployeeFullName);
                            }
                            break;
                        case "EmployeethreefourID":
                            if (sortColumnDirection == "desc")
                            {
                                customerData = customerData.OrderByDescending(s => s.EmployeethreefourID);
                            }
                            else
                            {
                                customerData = customerData.OrderBy(s => s.EmployeethreefourID);
                            }
                            break;
                    }
                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.EmployeethreefourID.StartsWith(searchValue));
                }

                //total number of rows counts   
                recordsTotal = customerData.Count();
                //Paging   
                var jsonData = customerData.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = jsonData });

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
   
}

