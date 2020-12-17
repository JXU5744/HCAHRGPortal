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
using System.Collections;

namespace HCAaudit.Service.Portal.AuditUI.Controllers
{
    //[Authorize]
    public class SearchController : Controller
    {
        private readonly ILogger<SearchController> _logger;
        private readonly IConfiguration config;
        private IAuthService _authService;
        List<CategoryMast> masterCategory = null;
        private AuditToolContext _auditToolContext;
        public SearchController(ILogger<SearchController> logger, IConfiguration configuration, AuditToolContext audittoolc)//, IAuthService authService)
        {
            _auditToolContext = audittoolc;
            _logger = logger;
            config = configuration;
        }

        public IActionResult Details()
        {
            BindSearchGrid objBindSearchGrid = new BindSearchGrid();
            objBindSearchGrid._dataforGrid = BindSearchGrid.GetGridData();
            return View("Details",objBindSearchGrid);
        }

        [HttpGet]
        public ActionResult Navigate(string ticketid)
        {
            return Redirect("https://www.google.com");
        }

        public IActionResult Index()
        {
            var categoryList = GetCategoryDetails();
            _logger.LogInformation($"No of records: {categoryList.Count()}");
            categoryList.Insert(0, new Categorys { CatgID = 0, CatgDescription = "Select" });
            ViewBag.ListOfCategory = categoryList;

            var ticketList = Tickets.GetTickets();
            ticketList.Insert(0, new Tickets { TicketID = 0, Ticket = "Select Ticket" });
            ViewBag.ListOfTicket = ticketList;

            var assignedtoList = AssignedTo.GetAssignedTo();
            assignedtoList.Insert(0, new AssignedTo { memberID = 0, membername = "Select Member" });
            ViewBag.ListOfMembers = assignedtoList;

            var statusList = Status.GetStatus();
            statusList.Insert(0, new Status { StatusID = 0, Statusname = "Select Status" });
            ViewBag.StatusList = statusList;
            return View();
        }

        [HttpPost]
        public JsonResult BindSubCategory(string categoryID)
        {
            _logger.LogInformation($"Request for SubCategoryList with CategoryID: {categoryID}");
            var filteredSubCategoryList = SubCategoryList.GetSubCategory()
                                         .Where(x => x.CatgID == Convert.ToInt32(categoryID))
                                         .Select(x => new { x.SubCatgID, x.SubCatgDescription }).ToList();
            _logger.LogInformation($"No of SubCategoryListrecords: {filteredSubCategoryList.Count()}");
            return Json(filteredSubCategoryList);
        }

        List<Categorys> GetCategoryDetails()
        {
            var data = (from subCat in _auditToolContext.categories select subCat).ToList();
            return data;
        }
    }
}

