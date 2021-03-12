using HCAaudit.Service.Portal.AuditUI.Models;
using HCAaudit.Service.Portal.AuditUI.Services;
using HCAaudit.Service.Portal.AuditUI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HCAaudit.Service.Portal.AuditUI.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private readonly ILogger<SearchController> _logger;
        private readonly AuditToolContext _auditToolContext;
        private readonly bool isAuditor = false;
        private readonly IAuthService _authService;
        private readonly IErrorLog _log;
        private const string SessionKeyName = "SearchParamObject";


        public SearchController(ILogger<SearchController> logger, IErrorLog log, AuditToolContext audittoolc, IAuthService authService)
        {
            _auditToolContext = audittoolc;
            _logger = logger;
            isAuditor = authService.CheckAuditorUserGroup().Result;
            _log = log;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Details()
        {
            BindSearchGrid objBindSearchGrid = new BindSearchGrid();
            return View("Details", objBindSearchGrid);
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

                    //Paging Size (10, 20, 50,100)  
                    int pageSize = length != null ? Convert.ToInt32(length) : 0;

                    int skip = start != null ? Convert.ToInt32(start) : 0;

                    int recordsTotal = 0;

                    var customerData = GetSearchResult();

                    //total number of rows counts   
                    recordsTotal = customerData.Count;
                    //Paging   
                    var jsonData = customerData.Skip(skip).Take(pageSize).ToList();
                    //Returning Json Data  
                    return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data = jsonData });

                }
                catch (Exception ex)
                {
                    _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SearchController_GetSearchDetails", ErrorDiscription = ex.InnerException != null ? ex.InnerException.ToString() : ex.Message });
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public JsonResult GetCommaSeperated()
        {
            try
            {
                if (isAuditor)
                {
                    var mydata = _auditToolContext.HROCRoster.Select(a => a.Employee34IdLowerCase);
                    return Json(mydata);
                }
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SearchController_GetCommaSeperated", ErrorDiscription = ex.InnerException != null ? ex.InnerException.ToString() : ex.Message });
            }
            return Json(new { Success = "False", responseText = "Authorization Error" });
        }

        [HttpPost]
        public IActionResult GetSearchDetails(SearchViewModel searchparameter)
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


                    if (!string.IsNullOrEmpty(searchparameter.EnvironmentType))
                    {
                        SessionHelper.SetObjectAsJson(HttpContext.Session, SessionKeyName, searchparameter);
                    }
                    else if ((!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName))) && string.IsNullOrEmpty(searchparameter.EnvironmentType))
                    {
                        SearchViewModel tempSearchParam = SessionHelper.GetObjectFromJson<SearchViewModel>(HttpContext.Session, SessionKeyName);
                        if (tempSearchParam == null)
                        {
                            _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SearchController_GetSearchDetails", ErrorDiscription = "Session object corrupted." });
                        }
                        searchparameter = tempSearchParam;
                    }

                    if (searchparameter == null)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        string environmentType = searchparameter.EnvironmentType ?? "Production";
                        int categoryId = searchparameter.CategoryID;

                        if (searchparameter.CategoryID <= 0 && searchparameter.SubcategoryID > 0)
                        {
                            categoryId = GetCategoryID(searchparameter.SubcategoryID);
                        }

                        int subCategoryId = searchparameter.SubcategoryID;
                        string resultType = searchparameter.ResultType ?? "Audit";
                        int ticketStatus = searchparameter.TicketStatus;
                        string fromDate = searchparameter.FromDate ?? DateTime.Today.AddDays(-1).ToString();
                        string toDate = searchparameter.EndDate ?? Convert.ToDateTime(fromDate).AddDays(-7).ToString();
                        string assignedTo = !String.IsNullOrWhiteSpace(searchparameter.AssignedTo) ? searchparameter.AssignedTo : string.Empty;
                        string ticketSubStatus = !String.IsNullOrWhiteSpace(searchparameter.TicketSubStatus) ? searchparameter.TicketSubStatus : string.Empty;
                        string resultCountCriteria = String.IsNullOrWhiteSpace(searchparameter.ResultCountCriteria) ? "All" : searchparameter.ResultCountCriteria;
                        string TicketId = String.IsNullOrWhiteSpace(searchparameter.TicketId) ? string.Empty : searchparameter.TicketId;

                        IEnumerable<UspGetHRAuditSearchResult> objgriddata = GetClosedAuditSearchResult(environmentType, categoryId, subCategoryId, resultType,
                                    ticketStatus, ticketSubStatus, resultCountCriteria, assignedTo, fromDate, toDate, TicketId);
                        // All
                        // 1-100%
                        // X RecCounts
                        int count = 0;

                        if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                        {
                            switch (sortColumn)
                            {
                                case "CreatedDate":
                                    if (sortColumnDirection == "desc")
                                    {
                                        objgriddata = objgriddata.OrderByDescending(s => s.ClosedDate);
                                    }
                                    else
                                    {
                                        objgriddata = objgriddata.OrderBy(s => s.ClosedDate);
                                    }
                                    break;
                                case "Subject":
                                    if (sortColumnDirection == "desc")
                                    {
                                        objgriddata = objgriddata.OrderByDescending(s => s.Topic);
                                    }
                                    else
                                    {
                                        objgriddata = objgriddata.OrderBy(s => s.Topic);
                                    }
                                    break;
                                case "SubCategory":
                                    if (sortColumnDirection == "desc")
                                    {
                                        objgriddata = objgriddata.OrderByDescending(s => s.SubCategory);
                                    }
                                    else
                                    {
                                        objgriddata = objgriddata.OrderBy(s => s.SubCategory);
                                    }
                                    break;
                                case "ServiceGroup":
                                    if (sortColumnDirection == "desc")
                                    {
                                        objgriddata = objgriddata.OrderByDescending(s => s.ServiceDeliveryGroup);
                                    }
                                    else
                                    {
                                        objgriddata = objgriddata.OrderBy(s => s.ServiceDeliveryGroup);
                                    }
                                    break;
                                case "AssignedTo":
                                    if (sortColumnDirection == "desc")
                                    {
                                        objgriddata = objgriddata.OrderByDescending(s => s.Agent34ID);
                                    }
                                    else
                                    {
                                        objgriddata = objgriddata.OrderBy(s => s.Agent34ID);
                                    }
                                    break;
                                default:
                                    if (sortColumnDirection == "desc")
                                    {
                                        objgriddata = objgriddata.OrderByDescending(s => s.TicketCode);
                                    }
                                    else
                                    {
                                        objgriddata = objgriddata.OrderBy(s => s.TicketCode);
                                    }
                                    break;
                            }
                        }

                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            objgriddata = objgriddata.Where(m => m.TicketCode.ToLower().StartsWith(searchValue.ToLower()) ||
                                m.ServiceDeliveryGroup.ToLower().StartsWith(searchValue.ToLower()) ||
                                m.Topic.ToLower().StartsWith(searchValue.ToLower()) ||
                                m.Agent34ID.ToLower().StartsWith(searchValue.ToLower()) ||
                                m.SubCategory.ToLower().StartsWith(searchValue.ToLower())
                                );
                        }

                        if (!resultCountCriteria.ToLower().Equals("all"))
                        {
                            if (int.TryParse(resultCountCriteria, out count))
                            {
                                count = count > 1000 ? 1000 : count;
                                objgriddata = objgriddata.OrderBy(r => Guid.NewGuid()).Take(count).ToList();
                            }
                            else
                            {
                                resultCountCriteria = resultCountCriteria.Replace("%25", "%");
                                if (resultCountCriteria.Contains("%") && int.TryParse(resultCountCriteria.Replace("%", ""), out count))
                                {
                                    double len = objgriddata.ToList().Count;
                                    count = count > 100 ? 100 : count;
                                    count = Convert.ToInt32(Math.Ceiling(len * count / 100));
                                    objgriddata = objgriddata.OrderBy(r => Guid.NewGuid()).Take(count).ToList();
                                }
                            }
                        }
                        else
                        {
                            var len = objgriddata.ToList().Count;
                            count = len > 1000 ? 1000 : objgriddata.ToList().Count;
                            objgriddata = objgriddata.Take(count).ToList();
                        }
                        recordsTotal = objgriddata.ToList().Count;

                        //Paging   
                        var jsonData = objgriddata.Skip(skip).Take(pageSize).ToList();
                        //Returning Json Data  
                        return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data = jsonData });
                    }
                }
                catch (Exception ex)
                {
                    _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SearchController_GetSearchDetails", ErrorDiscription = ex.InnerException != null ? ex.InnerException.ToString() : ex.Message });
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Index(BindSearchGrid objBindSearchGrid)
        {
            try
            {
                if (isAuditor)
                {
                    return RedirectToAction("Details", objBindSearchGrid);
                }
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SearchController_Index", ErrorDiscription = ex.InnerException != null ? ex.InnerException.ToString() : ex.Message });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                if (isAuditor)
                {
                    var categoryList = GetCategoryDetails();
                    _logger.LogInformation($"No of records: {categoryList.Count}");
                    categoryList.Insert(0, new Category { CatgId = 0, CatgDescription = "Select" });
                    ViewBag.ListOfCategory = categoryList;

                    var assignedtoList = GetHRList();
                    ViewBag.ListOfMembers = assignedtoList;

                    if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
                    {
                        HttpContext.Session.Remove(SessionKeyName);
                    }

                    return View();
                }
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SearchController_Index", ErrorDiscription = ex.InnerException != null ? ex.InnerException.ToString() : ex.Message });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public JsonResult BindSubCategory(string categoryID)
        {
            try
            {
                if (isAuditor)
                {
                    if (Convert.ToInt32(categoryID) == 0)
                    {
                        return GetAllSubcategory();
                    }
                    else
                    {
                        var subCategoryList = _auditToolContext.SubCategory.Where(x => x.IsActive == true &&
                        x.CatgId == Convert.ToInt32(categoryID)).Select(x => new CatSubCatJoinMast
                        {
                            SubCatgID = x.SubCatgId,
                            SubCatgDescription = x.SubCatgDescription
                        }).OrderBy(a => a.SubCatgDescription).ToList();

                        _logger.LogInformation($"No of SubCategoryListrecords: {subCategoryList.Count}");
                        return Json(subCategoryList);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SearchController_BindSubCategory", ErrorDiscription = ex.InnerException != null ? ex.InnerException.ToString() : ex.Message });
            }
            return Json(new { Success = "False", responseText = "Authorization Error" });
        }

        List<Category> GetCategoryDetails()
        {
            List<Category> data = null;

            try
            {
                data = (from subCat in _auditToolContext.Category.Where(x => x.IsActive == true) select subCat).OrderBy(a => a.CatgDescription).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SearchController_GetCategoryDetails", ErrorDiscription = ex.InnerException != null ? ex.InnerException.ToString() : ex.Message });
            }
            return data;
        }

        public List<AssignedTo> GetHRList()
        {
            List<AssignedTo> lstAssignedTo = new List<AssignedTo>();

            try
            {
                var query = (from hrdata in _auditToolContext.HROCRoster
                             select new
                             {
                                 HrThreeFourID = hrdata.Employee34IdLowerCase
                             }).Distinct().ToList();

                int rowno = 1;

                foreach (var emplist in query)
                {
                    AssignedTo tempAssignedto = new AssignedTo
                    {
                        MemberID = rowno,
                        MemberName = emplist.HrThreeFourID
                    };
                    rowno++;
                    lstAssignedTo.Add(tempAssignedto);
                }
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SearchController_GetHRList", ErrorDiscription = ex.InnerException != null ? ex.InnerException.ToString() : ex.Message });
            }
            return lstAssignedTo.ToList();
        }

        public List<UspGetHRAuditSearchResult> GetClosedAuditSearchResult(String environmentType, int categoryId, int subCategoryId, String resultType,
                            int ticketStatus, string ticketSubStatus, string resultCountCriteria, string assignedTo, string fromDate, string toDate, String TicketId)
        {
            List<UspGetHRAuditSearchResult> objgriddata = new List<UspGetHRAuditSearchResult>();

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
                objgriddata = _auditToolContext.UspGetHRAuditSearchResult.FromSqlRaw(query, parms.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SearchController_GetClosedAuditSearchResult", ErrorDiscription = ex.InnerException != null ? ex.InnerException.ToString() : ex.Message });
            }
            return objgriddata;
        }


        private int GetCategoryID(int subcategoryid)
        {
            int categoryid = -1;

            try
            {
                var catgObj = _auditToolContext.SubCategory.Where(y => y.IsActive == true &&
           y.SubCatgId == subcategoryid).FirstOrDefault();

                if (catgObj != null)
                {
                    categoryid = catgObj.CatgId;
                }
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SearchController_GetCaegoryID", ErrorDiscription = ex.InnerException != null ? ex.InnerException.ToString() : ex.Message });
            }
            return categoryid;
        }


        [HttpPost]
        public JsonResult GetAllSubcategory()
        {
            try
            {
                if (isAuditor)
                {
                    var query = _auditToolContext.SubCategory
                        .Where(x => x.IsActive == true)
                        .OrderBy(x => x.SubCatgDescription)
                        .Include(subCat => subCat.Catg)
                        .Select(x => new CatSubCatJoinMast
                        {
                            SubCatgID = x.SubCatgId,
                            SubCatgDescription = string.Format("{0} ({1})", x.SubCatgDescription, x.Catg.CatgDescription)
                        }).ToList();

                    query = query.OrderBy(a => a.SubCatgDescription).ToList();

                    _logger.LogInformation($"No of SubCategoryListrecords: {query.Count}");
                    return Json(query);
                }
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SearchController_GetAllSubcategory", ErrorDiscription = ex.InnerException != null ? ex.InnerException.ToString() : ex.Message });
            }
            return Json(new { Success = "False", responseText = "Authorization Error" });
        }

        [HttpPost]
        public JsonResult GetStatistics()
        {
            try
            {
                if (isAuditor)
                {
                    var userName = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                    DateTime startDateofYear = new DateTime(DateTime.Now.Year, 1, 1);
                    DateTime todayDate = DateTime.Now;
                    DateTime monthStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                    var yearToDateAuditCount = _auditToolContext.AuditMain.Where(x =>x.AuditorName == userName && x.SubmitDt >= startDateofYear && x.SubmitDt <= todayDate && x.AuditorQuit != "Quit").Count();
                    var yearToDateDisputeCount = _auditToolContext.AuditDispute.Where(x => x.CreatedBy == userName && x.CreatedDate >= startDateofYear && x.CreatedDate <= todayDate).Count();

                    var monthToDateAuditCount = _auditToolContext.AuditMain.Where(x => x.AuditorName == userName && x.SubmitDt >= monthStartDate && x.SubmitDt <= todayDate && x.AuditorQuit != "Quit").Count();
                    var monthToDateDisputeCount = _auditToolContext.AuditDispute.Where(x => x.CreatedBy == userName && x.CreatedDate >= monthStartDate && x.CreatedDate <= todayDate).Count();

                    AuditorStatistics stats = new AuditorStatistics
                    {
                        YearToDate = string.Format("{0} / {1}", yearToDateAuditCount, yearToDateDisputeCount),
                        MonthToDate = string.Format("{0} / {1}", monthToDateAuditCount, monthToDateDisputeCount)
                    };

                    var auditList = _auditToolContext.AuditMain.Where(x => x.AuditorName == userName && x.AuditorQuit != "Quit").OrderByDescending(ord => ord.CreatedDate).Take(10).ToList();

                    foreach(AuditMain item in auditList)
                    {
                        RecentTicket recentTicket = new RecentTicket
                        {
                            TicketCode = item.TicketId,
                            Agent34Id = item.Agent34Id,
                            AuditDate = (DateTime)item.SubmitDt,
                            Dispute = (bool)item.IsDisputed ? "Yes" : "No"
                        };

                        var auditCompliantResponse = _auditToolContext.AuditMainResponse.Where(x => x.AuditMainId == item.Id && x.IsCompliant == true).Any();
                        var auditNonCompliantResponse = _auditToolContext.AuditMainResponse.Where(x => x.AuditMainId == item.Id && x.IsNonCompliant == true).Any();

                        if (auditNonCompliantResponse || auditCompliantResponse)
                        {
                            recentTicket.CompliantNonCompliant = auditNonCompliantResponse ? "Non Compliant" : "Compliant";
                        }
                        else
                        {
                            recentTicket.CompliantNonCompliant = "Not Applicable";
                        }

                        stats.RecentTicketLists.ToList().Add(recentTicket);
                    }

                    return Json(stats);
                }
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SearchController_GetAllSubcategory", ErrorDiscription = ex.InnerException != null ? ex.InnerException.ToString() : ex.Message });
            }
            return Json(new { Success = "False", responseText = "Authorization Error" });
        }

        public List<BindSearchGrid> GetSearchResult()
        {
            //isActiveFlag

            List<BindSearchGrid> objgriddata = new List<BindSearchGrid>();

            try
            {
                var ticketdata = (from ticket in _auditToolContext.TicketsViaSSIS
                                  join hrdata in _auditToolContext.HROCRoster on ticket.CloseUserId.Substring(0, 7) equals hrdata.Employee34IdLowerCase
                                  join Category in _auditToolContext.Category on hrdata.JobCdDescHomeCurr equals Category.CatgDescription

                                  select new
                                  {
                                      ticket.TicketCode,
                                      hrdata.JobCdDescHomeCurr,
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
                    BindSearchGrid tempItem = new BindSearchGrid
                    {
                        TicketNumber = t.TicketCode,
                        ServiceGroup = t.JobCdDescHomeCurr,
                        Category = t.Category,
                        Subcategory = t.SubCategory,
                        UserThreeFourID = t.CloseUserId,
                        AssignedTo = t.EmployeeFullName,
                        ClosedDateTime = t.ClosedDateTime,
                        Topic = t.Topic
                    };
                    objgriddata.Add(tempItem);
                }
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SearchController_GetSearchResult", ErrorDiscription = ex.InnerException != null ? ex.InnerException.ToString() : ex.Message });
            }
            return objgriddata;
        }
    }
}

