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
using HCAaudit.Service.Portal.AuditUI.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;


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

        [HttpPost]
        public JsonResult GetCommaSeperated()
        {

            var mydata = _auditToolContext.HROCRoster.Select(a => a.EmployeethreefourID);

            return Json(_auditToolContext.HROCRoster.Select(a => a.EmployeethreefourID));
        }

        [HttpGet]
        public IActionResult Details()
        {
            BindSearchGrid objBindSearchGrid = new BindSearchGrid();
            try
            {
                objBindSearchGrid._dataforGrid = BindSearchGrid.GetGridData();
            }
            catch (Exception ex)
            {
                throw ex;
                // _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SearchController_Details", ErrorDiscription = ex.Message });
            }
            return View("Details", objBindSearchGrid);

        }

        [HttpPost]
        public IActionResult GetSearchDetails(SearchViewModel searchparameter)
        {
            List<Usp_GetHRAuditSearchResult> objgriddata = new List<Usp_GetHRAuditSearchResult>();
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
                if (searchparameter == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    string environmentType = searchparameter.EnvironmentType != null ? searchparameter.EnvironmentType : "Production";
                    int categoryId = searchparameter.CategoryID;
                    int subCategoryId = searchparameter.SubcategoryID;
                    string resultType = searchparameter.ResultType != null ? searchparameter.ResultType : "Audit";
                    int ticketStatus = searchparameter.TicketStatus;
                    string fromDate = searchparameter.FromDate == null ? DateTime.Today.AddDays(-1).ToString() : searchparameter.FromDate;
                    string toDate = searchparameter.EndDate == null ? Convert.ToDateTime(fromDate).AddDays(-7).ToString() : searchparameter.EndDate;
                    string assignedTo = !String.IsNullOrWhiteSpace(searchparameter.AssignedTo) ? searchparameter.AssignedTo : string.Empty;
                    string ticketSubStatus = !String.IsNullOrWhiteSpace(searchparameter.TicketSubStatus) ? searchparameter.TicketSubStatus : string.Empty;
                    string resultCountCriteria = String.IsNullOrWhiteSpace(searchparameter.ResultCountCriteria) ? "All" : searchparameter.ResultCountCriteria;
                    string TicketId = String.IsNullOrWhiteSpace(searchparameter.TicketId) ? string.Empty : searchparameter.TicketId;

                    if (resultType.Equals("Audit"))
                    {
                        //objgriddata = GetClosedAuditSearchResult(environmentType, categoryId, subCategoryId, resultType,
                        //        ticketStatus, ticketSubStatus, resultCountCriteria, assignedTo, fromDate, toDate, TicketId);
                    }
                    else
                    {

                    }

                    objgriddata = GetClosedAuditSearchResult(environmentType, categoryId, subCategoryId, resultType,
                                ticketStatus, ticketSubStatus, resultCountCriteria, assignedTo, fromDate, toDate, TicketId);

                    // All
                    // 1-100%
                    // X RecCounts
                    
                    //objgriddata = objgriddata.OrderBy(x => Guid.NewGuid()).Take(20).ToList();

                    recordsTotal = objgriddata.Count();

                    //Paging   
                    var jsonData = objgriddata.Skip(skip).Take(pageSize).ToList();
                    //Returning Json Data  
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = jsonData });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
            


        [HttpPost]
        public IActionResult Details(BindSearchGrid objBindSearchGrid)
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

                var customerData = GetSearchResult();

                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    switch (sortColumn)
                    {
                        case "TicketNumber":
                            if (sortColumnDirection == "desc")
                            {
                                //customerData = customerData.OrderByDescending(s => s.TicketNumber);
                            }
                            else
                            {
                                //customerData = customerData.OrderBy(s => s.TicketNumber);
                            }
                            break;
                    }
                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    //customerData = customerData.Where(m => m.TicketNumber.ToLower() == searchValue.ToLower());
                }

                //total number of rows counts   
                recordsTotal = customerData.Count();
                //Paging   
                var jsonData = customerData.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = jsonData });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult Navigate(string id)
        {
            return Redirect("https://www.google.com");
        }

        [HttpPost]
        public IActionResult Index(BindSearchGrid objBindSearchGrid)
        {
            return RedirectToAction("Details", objBindSearchGrid);
        }
        [HttpGet]
        public IActionResult Index()
        {
            var categoryList = GetCategoryDetails();
            _logger.LogInformation($"No of records: {categoryList.Count()}");
            categoryList.Insert(0, new Categorys { CatgID = 0, CatgDescription = "Select" });
            ViewBag.ListOfCategory = categoryList;

            var ticketList = Tickets.GetTickets();
            //ticketList.Insert(0, new Tickets { TicketID = 0, Ticket = "Select Ticket" });
            ViewBag.ListOfTicket = ticketList;

            //var assignedtoList = AssignedTo.GetAssignedTo();

            var assignedtoList = GetHRList();
            //assignedtoList.Insert(0, new AssignedTo { memberID = 0, membername = "Select Member" });
            ViewBag.ListOfMembers = assignedtoList;

            var statusList = Status.GetStatus();
            //statusList.Insert(0, new Status { StatusID = 0, Statusname = "Select Status" });
            ViewBag.StatusList = statusList;
            return View();
        }

        [HttpPost]
        public JsonResult 
            BindSubCategory(string categoryID)
        {
            _logger.LogInformation($"Request for SubCategoryList with CategoryID: {categoryID}");
            var subCategoryList = _auditToolContext.SubCategories.ToList();
            var filteredSubCategoryList = subCategoryList
                                         .Where(x => x.CatgID == Convert.ToInt32(categoryID))
                                         .Select(x => new { x.SubCatgID, x.SubCatgDescription }).ToList();
            _logger.LogInformation($"No of SubCategoryListrecords: {filteredSubCategoryList.Count()}");
            return Json(filteredSubCategoryList);
        }

        List<Categorys> GetCategoryDetails()
        {
            var data = (from subCat in _auditToolContext.Categories select subCat).ToList();
            return data;
        }

        public List<AssignedTo> GetHRList()
        {
            var query = (from hrdata in _auditToolContext.HROCRoster
                         select new
                         {
                             HrThreeFourID = hrdata.EmployeethreefourID
                         }).Distinct().ToList();

            int rowno = 1;

            List<AssignedTo> lstAssignedTo = new List<AssignedTo>();
            foreach (var emplist in query)
            {
                AssignedTo tempAssignedto = new AssignedTo();
                tempAssignedto.memberID = rowno;
                tempAssignedto.membername = emplist.HrThreeFourID;
                rowno++;
                lstAssignedTo.Add(tempAssignedto);
            }
            return lstAssignedTo.ToList();
        }

        public List<Usp_GetHRAuditSearchResult> GetClosedAuditSearchResult(String environmentType, int categoryId, int subCategoryId, String resultType,
                            int ticketStatus, string ticketSubStatus, string resultCountCriteria, string assignedTo, string fromDate, string toDate, String TicketId)
        {
            List<Usp_GetHRAuditSearchResult> objgriddata = new List<Usp_GetHRAuditSearchResult>();
            try
            {
                var query = "Exec  [dbo].[usp_GetHRAuditSearchResult] @EnvironmentType, @CategoryId, @SubCategoryId, @ResultType, " +
                            "@TicketStatus, @TicketSubStatus, @ResultCountCriteria," +
                            "@AssignedTo, @FromDate, @ToDate, @TicketId";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters    
                    new SqlParameter { ParameterName = "@EnvironmentType", Value = environmentType },
                    new SqlParameter { ParameterName = "@CategoryId", Value = categoryId },
                    new SqlParameter { ParameterName = "@SubCategoryId", Value = subCategoryId },
                    new SqlParameter { ParameterName = "@ResultType", Value = resultType },
                    new SqlParameter { ParameterName = "@TicketStatus", Value = ticketStatus },
                    new SqlParameter { ParameterName = "@TicketSubStatus", Value = ticketSubStatus },
                    new SqlParameter { ParameterName = "@ResultCountCriteria", Value = resultCountCriteria },
                    new SqlParameter { ParameterName = "@AssignedTo", Value = assignedTo },
                    new SqlParameter { ParameterName = "@FromDate", Value = fromDate.Replace("%2F","/") },
                    new SqlParameter { ParameterName = "@ToDate", Value = toDate.Replace("%2F","/") },
                    new SqlParameter { ParameterName = "@TicketId", Value = TicketId }
                };

                objgriddata = _auditToolContext.Usp_GetHRAuditSearchResult.FromSqlRaw(query,parms.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error While Getting Data" + ex);
            }


            return objgriddata;
        }

         public List<BindSearchGrid> GetSearchResult()
        {
            //isActiveFlag

            var ticketdata = (from ticket in _auditToolContext.SearchTicketDetail
                              join hrdata in _auditToolContext.HROCRoster on ticket.CloseUserId.Substring(0, 7) equals hrdata.EmployeethreefourID
                              join Category in _auditToolContext.Categories on hrdata.JobCDDesc equals Category.CatgDescription 
                              
                              select new
                              {
                                  ticket.TicketCode,
                                  hrdata.PositionDesc,
                                  ticket.Category,
                                  ticket.SubCategory,
                                  ticket.CloseUserId,
                                  hrdata.EmployeeFullName,
                                  ticket.ClosedDateTime,
                                  ticket.Topic
                              }
                              ).ToList();


            List<BindSearchGrid> objgriddata = new List<BindSearchGrid>();
            foreach (var t in ticketdata)
            {
                BindSearchGrid tempItem = new BindSearchGrid();
                tempItem.TicketNumber = t.TicketCode;
                tempItem.ServiceGroup = t.PositionDesc;
                tempItem.Category = t.Category;
                tempItem.Subcategory = t.SubCategory;
                tempItem.UserThreeFourID = t.CloseUserId;
                tempItem.AssignedTo = t.EmployeeFullName;
                tempItem.ClosedDateTime = t.ClosedDateTime;
                tempItem.Topic = t.Topic;
                objgriddata.Add(tempItem);
            }



            return objgriddata;
        }
    }


}

