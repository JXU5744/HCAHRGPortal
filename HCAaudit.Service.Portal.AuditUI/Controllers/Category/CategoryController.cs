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
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IAuthService _authService;
        private readonly AuditToolContext _auditToolContext;
        private readonly bool isAdmin;
        private readonly IErrorLog _log;
        public CategoryController(ILogger<CategoryController> logger, IErrorLog log, AuditToolContext audittoolc, IAuthService authService)
        {
            _auditToolContext = audittoolc;
            _logger = logger;
            _authService = authService;
            isAdmin = _authService.CheckAdminUserGroup().Result;
            _log = log;
        }

        [HttpPost]
        public ActionResult GetCategoryByid(string id)
        {
            _logger.LogInformation($"Entering GetCategoryByid Request for Category with CategoryID: {id}");
            try
            {
                if (isAdmin)
                {
                    object response = "";
                    if (string.IsNullOrEmpty(id))
                    {
                        _logger.LogInformation($"Returning Response with CategoryID is null or empty");
                        return Json(response);
                    }
                    _logger.LogInformation($"Requesting Method GetSingleCategoryByid for CategoryID as {id}");
                    return Json(GetSingleCategoryByid(id));
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in getting Category for CategoryID as {id}");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "CategoryController_GetCategoryByid", ErrorDiscription = ex.InnerException.ToString() });
            }
            _logger.LogInformation($"Returning from GetCategoryByid Action to Home Page {id}");
            return RedirectToAction("Index", "Home");
        }

        private Category GetSingleCategoryByid(string id)
        {
            _logger.LogInformation($"Entering GetSingleCategoryByid method with CategoryID: {id}");
            Category data = null;
            try
            {
                data = (from cat in _auditToolContext.Category.Where(
                category => category.CatgId == Convert.ToInt32(id) &&
                category.IsActive == true)
                        select cat).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in getting Category in GetSingleCategoryByid method for CategoryID as {id}");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "CategoryController_GetSingleCategoryByid", ErrorDiscription = ex.InnerException.ToString() });
            }
            _logger.LogInformation($"Exiting GetSingleCategoryByid method with data: {data}");
            return data;
        }

        [HttpPost]
        public ActionResult Edit(string id)
        {
            try
            {
                if (isAdmin)
                {
                    object resp = "";
                    if (!string.IsNullOrEmpty(id))
                    {
                        string[] param = id.Split('$');
                        if (param.Any())
                        {
                            var collection = GetDetails();
                            foreach (var item in collection)
                            {
                                if (item.CatgDescription.ToLower() == param[1].ToLower().Trim())
                                { resp = "1"; break; }
                            }
                            if (string.IsNullOrEmpty(resp.ToString()))
                            {
                                Category objCategorys = GetSingleCategoryByid(param[0]);
                                if (objCategorys != null)
                                {
                                    objCategorys.CatgDescription = param[1];
                                    objCategorys.ModifiedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                                    objCategorys.ModifiedDate = DateTime.Now;
                                    _auditToolContext.Category.Update(objCategorys);
                                    _auditToolContext.SaveChanges();
                                }
                            }
                        }
                    }
                    return Json(resp);
                }
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "CategoryController_Edit", ErrorDiscription = ex.InnerException.ToString() });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Insert(string CategoryName)
        {
            try
            {
                if (isAdmin)
                {
                    if (IsCategoryNameExists(CategoryName))
                    {
                        return Json("1");
                    }
                    else
                    {
                        Category objCategorys = new Category
                        {
                            CatgDescription = CategoryName,
                            IsActive = true,
                            CreatedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName,
                            CreatedDate = DateTime.Now,
                            ModifiedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName,
                            ModifiedDate = DateTime.Now
                        };
                        _auditToolContext.Category.Add(objCategorys);
                        _auditToolContext.SaveChanges();
                        return RedirectToAction("index");
                    }
                }
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "CategoryController_Insert", ErrorDiscription = ex.InnerException.ToString() });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                if (isAdmin)
                {
                    var data = (from cat in _auditToolContext.Category select cat).ToList();
                    Category objCategorys = data.Find(category => category.CatgId == id);
                    objCategorys.IsActive = false;
                    objCategorys.ModifiedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                    objCategorys.ModifiedDate = DateTime.Now;
                    _auditToolContext.Category.Update(objCategorys);
                    _auditToolContext.SaveChanges();
                    return RedirectToAction("Index", GetDetails());
                }
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "CategoryController_Delete", ErrorDiscription = ex.InnerException.ToString() });
            }
            return RedirectToAction("Index", "Home");
        }

        private bool IsCategoryNameExists(string inputcategoryname)
        {
            bool result = false;
            try
            {
                var data = (from cat in _auditToolContext.Category.Where(x => x.CatgDescription.ToLower() == inputcategoryname.ToLower()
                        && x.IsActive == true)
                            select cat).FirstOrDefault();

                if (data != null) result = true;
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "CategoryController_IsCategoryNameExists", ErrorDiscription = ex.InnerException.ToString() });
            }
            return result;
        }

        private List<Category> GetDetails()
        {
            var data = new List<Category>();
            try
            {
                data = _auditToolContext.Category.Where(x => x.IsActive == true).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "CategoryController_GetDetails", ErrorDiscription = ex.InnerException.ToString() });
            }
            return data;
        }

        [HttpPost]
        public ActionResult HasDeleteAccess(int id)
        {
            try
            {
                if (isAdmin)
                {
                    object response;
                    var data = (from cat in _auditToolContext.SubCategory.Where(x => x.IsActive == true) select cat).ToList();
                    SubCategory obj = data.Find(a => a.CatgId == id);
                    response = obj == null ? "HasecOrds" : "NoRecOrds";
                    return Json(response);
                }
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "CategoryController_HasDeleteAccess", ErrorDiscription = ex.InnerException.ToString() });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                if (isAdmin)
                {
                    return View("Index", GetDetails());
                }
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "CategoryController_Index", ErrorDiscription = ex.InnerException.ToString() });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Index(CategoryMast objCategoryMast)
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

                    var data = _auditToolContext.Category.Where(x => x.IsActive == true).ToList();
                    objCategoryMast = new CategoryMast
                    {
                        CategoryList = new List<Category>()
                    };
                    foreach (var item in data)
                    {
                        Category objCategory = new Category
                        {
                            CatgId = item.CatgId,
                            CatgDescription = item.CatgDescription
                        };
                        objCategoryMast.CategoryList.Add(objCategory);
                    }

                    // getting all Customer data  
                    var customerData = (from tempcustomer in objCategoryMast.CategoryList
                                        select tempcustomer);

                    //Sorting  
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                    {
                        switch (sortColumn)
                        {
                            case "CatgDescription":
                                if (sortColumnDirection == "desc")
                                {
                                    customerData = customerData.OrderByDescending(s => s.CatgDescription);
                                }
                                else
                                {
                                    customerData = customerData.OrderBy(s => s.CatgDescription);
                                }
                                break;
                        }
                    }
                    //Search  
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        customerData = customerData.Where(m => m.CatgDescription.ToLower().StartsWith(searchValue.ToLower()));
                    }

                    //total number of rows counts   
                    recordsTotal = customerData.Count();
                    //Paging   
                    var jsonData = customerData.Skip(skip).Take(pageSize).ToList();
                    //Returning Json Data  
                    return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data = jsonData });
                }
                catch (Exception ex)
                {
                    _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "CategoryController_Index_objCategoryMast", ErrorDiscription = ex.InnerException.ToString() });
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}