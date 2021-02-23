using HCAaudit.Service.Portal.AuditUI.Models;
using HCAaudit.Service.Portal.AuditUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HCAaudit.Service.Portal.AuditUI.Controllers
{
    [Authorize]
    public class SubCategoryController : Controller
    {
        private readonly ILogger<SubCategoryController> _logger;
        private readonly IConfiguration config;
        private readonly IAuthService _authService;
        private readonly AuditToolContext _auditToolContext;
        private bool isAdmin = false;
        private IErrorLog _log;

        public SubCategoryController(ILogger<SubCategoryController> logger, IErrorLog log, IConfiguration configuration, AuditToolContext audittoolc, IAuthService authService)
        {
            _auditToolContext = audittoolc;
            _logger = logger;
            config = configuration;
            _authService = authService;
            isAdmin = _authService.CheckAdminUserGroup().Result;
            _log = log;
        }

        //[Route("category/method")]
        public IActionResult GetCategory()
        {
            try
            {
                if (isAdmin)
                {
                    var categoryList = GetCategoryDetails();
                    _logger.LogInformation($"No of records: {categoryList.Count()}");
                    return Json(categoryList);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in GetCategory method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SubCategoryController_GetCategory", ErrorDiscription = ex.Message });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult GetCategoryByid(int id)
        {
            try
            {
                if (isAdmin)
                {
                    object response = "";
                    if (id == 0)
                    {
                        return Json(response);
                    }
                    var data = GetDetail(id);
                    return Json(data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in GetCategoryByid method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SubCategoryController_GetCategoryByid", ErrorDiscription = ex.Message });
            }
            return RedirectToAction("Index", "Home");
        }


        SubCategory GetSubCategoryDetailsByID(int id)
        {
            SubCategory data = null;
            try
            {
                data = (from subcat in _auditToolContext.SubCategories.Where(x => x.SubCatgID == id && x.IsActive == true)
                        select subcat).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in GetSubCategoryDetailsByID method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SubCategoryController_GetSubCategoryDetailsByID", ErrorDiscription = ex.Message });
            }
            return data;
        }

        [HttpPost]
        public IActionResult delete(int id)
        {
            SubCategory objSubCategory = new SubCategory();
            try
            {
                objSubCategory = GetSubCategoryDetailsByID(id);
                objSubCategory.SubCatgID = id; objSubCategory.IsActive = false;
                objSubCategory.ModifiedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                objSubCategory.ModifiedDate = DateTime.Now;
                _auditToolContext.SubCategories.Update(objSubCategory);
                _auditToolContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SubCategoryController_delete", ErrorDiscription = ex.Message });
            }
            return View("Details", GetDetails());
        }


        [HttpPost]
        public ActionResult Edit(string id)
        {
            try
            {
                if (isAdmin)
                {
                    object responce = "";
                    if (!string.IsNullOrEmpty(id))
                    {
                        string[] param = id.Split('$');
                        if (param.Count() > 0)
                        {
                            var collection = GetDetails();
                            foreach (var item in collection)
                            {
                                if (item.SubCatgDescription.ToLower() == param[1].ToLower().Trim())
                                {
                                    responce = "1";
                                    break;
                                }
                            }
                            if (string.IsNullOrEmpty(responce.ToString()))
                            {
                                SubCategory objCategorys = GetSubCategoryDetailsByID(Int32.Parse(param[0]));
                                if (objCategorys != null)
                                {
                                    objCategorys.SubCatgDescription = param[1];
                                    objCategorys.ModifiedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                                    objCategorys.ModifiedDate = DateTime.Now;
                                    _auditToolContext.SubCategories.Update(objCategorys);
                                    _auditToolContext.SaveChanges();
                                }
                            }
                        }
                    }
                    return Json(responce);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in Edit method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SubCategoryController_Edit", ErrorDiscription = ex.Message });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Insert(string catgID, string subCategoryName)
        {
            try
            {
                if (isAdmin)
                {
                    object responce = "";

                    if (IsSubcategoryNameExists(Int32.Parse(catgID), subCategoryName)) { responce = "1"; }
                    else
                    {
                        SubCategory objCategorys = new SubCategory();
                        objCategorys.CatgID = Convert.ToInt32(catgID);
                        objCategorys.SubCatgDescription = subCategoryName;
                        objCategorys.IsActive = true;
                        objCategorys.CreatedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                        objCategorys.CreatedDate = DateTime.Now;
                        objCategorys.ModifiedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                        objCategorys.ModifiedDate = DateTime.Now;
                        _auditToolContext.SubCategories.Add(objCategorys);
                        _auditToolContext.SaveChanges();
                        return RedirectToAction("Details");
                    }
                    return Json(responce);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in Insert method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SubCategoryController_Insert", ErrorDiscription = ex.Message });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Details(CatSubCatJoinMast objCatSubCatJoinMast)
        {
            if (isAdmin)
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

                    var query = _auditToolContext.SubCategories.Where(x => x.IsActive == true)
                        .Join(_auditToolContext.Categories, subCategories => subCategories.CatgID,
                                categories => categories.CatgID,
                                (subCategories, categories) => new
                                {
                                    CatgID = categories.CatgID,
                                    SubCatID = subCategories.SubCatgID,
                                    CatgDescription = categories.CatgDescription,
                                    SubCatgDescription = subCategories.SubCatgDescription
                                }).ToList();

                    // getting all Customer data  
                    var customerData = (from tempcustomer in GetDetails()
                                        select tempcustomer);

                    //Sorting  
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                    {
                        switch (sortColumn)
                        {
                            case "SubCatgDescription":
                                if (sortColumnDirection == "desc")
                                {
                                    customerData = customerData.OrderByDescending(s => s.SubCatgDescription);
                                }
                                else
                                {
                                    customerData = customerData.OrderBy(s => s.SubCatgDescription);
                                }
                                break;
                        }
                    }
                    //Search  
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        customerData = customerData.Where(m => m.SubCatgDescription.ToLower().StartsWith(searchValue.ToLower()));
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
                    _logger.LogInformation($"Exception in Details method");
                    _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SubCategoryController_Details", ErrorDiscription = ex.Message });
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Details()
        {
            try
            {
                if (isAdmin)
                {
                    return View("Details", GetDetails());
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in Details method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SubCategoryController_Details", ErrorDiscription = ex.Message });
            }
            return RedirectToAction("Index", "Home");
        }

        private bool IsSubcategoryNameExists(int categoryid, string subcategoryname)
        {
            bool result = false;
            try
            {
                var data = (from subcat in _auditToolContext.SubCategories.Where(x => x.SubCatgDescription.ToLower() == subcategoryname.ToLower()
                         && x.CatgID == categoryid && x.IsActive == true)
                            select subcat).FirstOrDefault();
                if (data != null) result = true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in IsSubcategoryNameExists method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SubCategoryController_IsSubcategoryNameExists", ErrorDiscription = ex.Message });
            }
            return result;
        }

        private List<CatSubCatJoinMast> GetDetails()
        {
            try
            {
                var query = _auditToolContext.SubCategories.Where(x => x.IsActive == true)
                .Join(
                    _auditToolContext.Categories,
                    subCategories => subCategories.CatgID,
                    categories => categories.CatgID,
                    (subCategories, categories) => new
                    {
                        CatgID = categories.CatgID,
                        SubCatID = subCategories.SubCatgID,
                        CatgDescription = categories.CatgDescription,
                        SubCatgDescription = subCategories.SubCatgDescription
                    })
                     .Select(x => new CatSubCatJoinMast
                     {
                         CatgID = x.CatgID,
                         SubCatgID = x.SubCatID,
                         CatgDescription = x.CatgDescription,
                         SubCatgDescription = x.SubCatgDescription
                     }
                    ).ToList();
                return query;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in GetDetails method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SubCategoryController_GetDetails", ErrorDiscription = ex.Message });
            }
            return new List<CatSubCatJoinMast>();
        }

        private CatSubCatJoinMast GetDetail(int subcatid)
        {
            CatSubCatJoinMast query = null;
            try
            {
                query = _auditToolContext.SubCategories.Where(x => x.IsActive == true)
                .Join(
                    _auditToolContext.Categories,
                    subCategories => subCategories.CatgID,
                    categories => categories.CatgID,
                    (subCategories, categories) => new
                    {
                        CatgID = categories.CatgID,
                        SubCatID = subCategories.SubCatgID,
                        CatgDescription = categories.CatgDescription,
                        SubCatgDescription = subCategories.SubCatgDescription
                    })
                     .Select(x => new CatSubCatJoinMast
                     {
                         CatgID = x.CatgID,
                         SubCatgID = x.SubCatID,
                         CatgDescription = x.CatgDescription,
                         SubCatgDescription = x.SubCatgDescription
                     }
                    ).Where(a => a.SubCatgID == subcatid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in GetDetail method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SubCategoryController_GetDetail", ErrorDiscription = ex.Message });
            }
            return query;
        }

        List<Categorys> GetCategoryDetails()
        {
            return (from subCat in _auditToolContext.Categories.Where(a => a.IsActive == true) select subCat).ToList();
        }

        [HttpPost]
        public ActionResult HasDeleteAccess(int id)
        {
            try
            {
                if (isAdmin)
                {
                    object response;
                    var data = (from cat in _auditToolContext.QuestionMapping.Where(a => a.IsActive == true) select cat).ToList();
                    QuestionMapping obj = data.Find(a => a.SubCatgId == id);
                    response = obj == null ? "NoRecords" : "HasRecords";
                    return Json(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in HasDeleteAccess method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "SubCategoryController_HasDeleteAccess", ErrorDiscription = ex.Message });
            }
            return RedirectToAction("Index", "Home");
        }
    }
}