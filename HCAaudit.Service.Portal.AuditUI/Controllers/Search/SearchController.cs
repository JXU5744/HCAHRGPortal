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
    [Authorize]
    public class SearchController : Controller
    {
        private readonly ILogger<SearchController> _logger;
        private readonly IConfiguration config;
        private IAuthService _authService;
        private AuditToolContext _auditToolContext;
        private bool isAuditor = false;
        private IErrorLog _log;

        public SearchController(ILogger<SearchController> logger, IErrorLog log, IConfiguration configuration, AuditToolContext audittoolc, IAuthService authService)
        {
            _auditToolContext = audittoolc;
            _logger = logger;
            config = configuration;
            _authService = authService;
            isAuditor = _authService.CheckAuditorUserGroup().Result;
            _log = log;
        }


        [HttpGet]
        public IActionResult Details()
        {
            BindSearchGrid objBindSearchGrid = new BindSearchGrid();
            try
            {
                // _log.WriteErrorLog(new LogItem { ErrorType = "Info", ErrorSource = "SearchController_Details", ErrorDiscription = "Test message" });
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SearchController_Details", ErrorDiscription = ex.Message });
            }
            return View("Details", objBindSearchGrid);

        }


        [HttpPost]
        public JsonResult GetCommaSeperated()
        {
            try
            {
                if (isAuditor)
                {
                    var mydata = _auditToolContext.HROCRoster.Select(a => a.EmployeethreefourID);

                    return Json(_auditToolContext.HROCRoster.Select(a => a.EmployeethreefourID));
                }

            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SearchController_GetCommaSeperated", ErrorDiscription = ex.Message });
            }

            return Json(new { Success = "False", responseText = "Authorization Error" });
        }

        [HttpPost]
        public IActionResult GetSearchDetails(SearchViewModel searchparameter)
        {
            if (isAuditor)
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

                        if (searchparameter.CategoryID <= 0 && searchparameter.SubcategoryID > 0)
                        {
                            categoryId = GetCaegoryID(searchparameter.SubcategoryID);
                        }

                        int subCategoryId = searchparameter.SubcategoryID;
                        string resultType = searchparameter.ResultType != null ? searchparameter.ResultType : "Audit";
                        int ticketStatus = searchparameter.TicketStatus;
                        string fromDate = searchparameter.FromDate == null ? DateTime.Today.AddDays(-1).ToString() : searchparameter.FromDate;
                        string toDate = searchparameter.EndDate == null ? Convert.ToDateTime(fromDate).AddDays(-7).ToString() : searchparameter.EndDate;
                        string assignedTo = !String.IsNullOrWhiteSpace(searchparameter.AssignedTo) ? searchparameter.AssignedTo : string.Empty;
                        string ticketSubStatus = !String.IsNullOrWhiteSpace(searchparameter.TicketSubStatus) ? searchparameter.TicketSubStatus : string.Empty;
                        string resultCountCriteria = String.IsNullOrWhiteSpace(searchparameter.ResultCountCriteria) ? "All" : searchparameter.ResultCountCriteria;
                        string TicketId = String.IsNullOrWhiteSpace(searchparameter.TicketId) ? string.Empty : searchparameter.TicketId;

                        objgriddata = GetClosedAuditSearchResult(environmentType, categoryId, subCategoryId, resultType,
                                    ticketStatus, ticketSubStatus, resultCountCriteria, assignedTo, fromDate, toDate, TicketId);

                        // All
                        // 1-100%
                        // X RecCounts
                        int count = 0;

                        if (!resultCountCriteria.ToLower().Equals("all"))
                        {
                            if (int.TryParse(resultCountCriteria, out count))
                            {
                                count = count > 1000 ? 1000 : count;
                                objgriddata = objgriddata.Skip(skip).Take(count).ToList();
                            }
                            else
                            {
                                resultCountCriteria = resultCountCriteria.Replace("%25", "%");
                                if (resultCountCriteria.Contains("%") && int.TryParse(resultCountCriteria.Replace("%", ""), out count))
                                {
                                    count = count > 100 ? 100 : count;
                                    count = (objgriddata.Count() * count) / 100;
                                    objgriddata = objgriddata.Skip(skip).Take(count).ToList();
                                }
                            }
                        }

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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }



        [HttpPost]
        public IActionResult Details(BindSearchGrid objBindSearchGrid)
        {
            if (isAuditor)
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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult Index(BindSearchGrid objBindSearchGrid)
        {
            if (isAuditor)
            {
                return RedirectToAction("Details", objBindSearchGrid);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpGet]
        public IActionResult Index()
        {
            if (isAuditor)
            {
                var categoryList = GetCategoryDetails();
                _logger.LogInformation($"No of records: {categoryList.Count()}");
                categoryList.Insert(0, new Categorys { CatgID = 0, CatgDescription = "Select" });
                ViewBag.ListOfCategory = categoryList;

                var assignedtoList = GetHRList();
                //assignedtoList.Insert(0, new AssignedTo { memberID = 0, membername = "Select Member" });
                ViewBag.ListOfMembers = assignedtoList;

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public JsonResult BindSubCategory(string categoryID)
        {
            try
            {


                _logger.LogInformation($"Request for SubCategoryList with CategoryID: {categoryID}");
                if (isAuditor)
                {
                    var subCategoryList = _auditToolContext.SubCategories.Where(x => x.IsActive == true).ToList();
                    var filteredSubCategoryList = subCategoryList
                                                 .Where(x => x.CatgID == Convert.ToInt32(categoryID))
                                                 .Select(x => new { x.SubCatgID, x.SubCatgDescription }).ToList();
                    _logger.LogInformation($"No of SubCategoryListrecords: {filteredSubCategoryList.Count()}");
                    return Json(filteredSubCategoryList);
                }
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SearchController_BindSubCategory", ErrorDiscription = ex.Message });
            }

            return Json(new { Success = "False", responseText = "Authorization Error" });
        }

        List<Categorys> GetCategoryDetails()
        {
            var data = (from subCat in _auditToolContext.Categories.Where(x => x.IsActive == true) select subCat).ToList();
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

                objgriddata = _auditToolContext.Usp_GetHRAuditSearchResult.FromSqlRaw(query, parms.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error While Getting Data" + ex);
            }


            return objgriddata;
        }


        private int GetCaegoryID(int subcategoryid)
        {
            int categoryid = -1;

            var catgObj = _auditToolContext.SubCategories.Where(y => y.IsActive == true &&
            y.SubCatgID == subcategoryid).FirstOrDefault();

            if (catgObj != null)
            {
                categoryid = catgObj.CatgID;
            }

            return categoryid;
        }


        [HttpPost]
        public JsonResult GetAllSubcategory()
        {

            try
            {

                _logger.LogInformation($"Request for AllSubCategoryList Category Identity");


                if (isAuditor)
                {
                    var query = _auditToolContext.SubCategories.Where(x => x.IsActive == true)
                    .Join(
                    _auditToolContext.Categories.Where(x => x.IsActive == true),
                    subCategories => subCategories.CatgID,
                    categories => categories.CatgID,
                    (subCategories, categories) => new
                    {
                        SubCatID = subCategories.SubCatgID,
                        CatgDescription = categories.CatgDescription,
                        SubCatgDescription = subCategories.SubCatgDescription
                    })
                    .Select(x => new CatSubCatJoinMast
                    {
                        SubCatgID = x.SubCatID,
                        SubCatgDescription = string.Format("{0} ({1})", x.SubCatgDescription, x.CatgDescription)
                    }
                    ).ToList().OrderBy(y => y.SubCatgDescription);

                    _logger.LogInformation($"No of SubCategoryListrecords: {query.Count()}");
                    return Json(query);
                }
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SearchController_GetAllSubcategory", ErrorDiscription = ex.Message });
            }



            return Json(new { Success = "False", responseText = "Authorization Error" });

        }

        public List<BindSearchGrid> GetSearchResult()
        {
            //isActiveFlag

            List<BindSearchGrid> objgriddata = new List<BindSearchGrid>();

            try
            {

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

            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SearchController_GetSearchResult", ErrorDiscription = ex.Message });
            }

            return objgriddata;
        }
    }


}

