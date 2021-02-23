﻿using HCAaudit.Service.Portal.AuditUI.Models;
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
        private readonly IConfiguration config;
        private IAuthService _authService;
        private AuditToolContext _auditToolContext;
        private bool isAdmin;
        private IErrorLog _log;
        public CategoryController(ILogger<CategoryController> logger, IErrorLog log, IConfiguration configuration, AuditToolContext audittoolc, IAuthService authService)
        {
            _auditToolContext = audittoolc;
            _logger = logger;
            config = configuration;
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
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "CategoryController_GetCategoryByid", ErrorDiscription = ex.Message });
            }
            _logger.LogInformation($"Returning from GetCategoryByid Action to Home Page {id}");
            return RedirectToAction("Index", "Home");
        }

        private Categorys GetSingleCategoryByid(string id)
        {
            _logger.LogInformation($"Entering GetSingleCategoryByid method with CategoryID: {id}");
            Categorys data = null;
            try
            {
                data = (from cat in _auditToolContext.Categories.Where(
                category => category.CatgID == Convert.ToInt32(id) &&
                category.IsActive == true)
                        select cat).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in getting Category in GetSingleCategoryByid method for CategoryID as {id}");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "CategoryController_GetSingleCategoryByid", ErrorDiscription = ex.Message });
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
                        if (param.Count() > 0)
                        {
                            var collection = GetDetails();
                            foreach (var item in collection)
                            {
                                if (item.CatgDescription.ToLower() == param[1].ToLower().Trim())
                                { resp = "1"; break; }
                            }
                            if (string.IsNullOrEmpty(resp.ToString()))
                            {
                                Categorys objCategorys = GetSingleCategoryByid(param[0]);
                                if (objCategorys != null)
                                {
                                    objCategorys.CatgDescription = param[1];
                                    objCategorys.ModifiedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                                    objCategorys.ModifiedDate = DateTime.Now;
                                    _auditToolContext.Categories.Update(objCategorys);
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
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "CategoryController_Edit", ErrorDiscription = ex.Message });
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
                        Categorys objCategorys = new Categorys();
                        objCategorys.CatgDescription = CategoryName;
                        objCategorys.IsActive = true;
                        objCategorys.CreatedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                        objCategorys.CreatedDate = DateTime.Now;
                        objCategorys.ModifiedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                        objCategorys.ModifiedDate = DateTime.Now;
                        _auditToolContext.Categories.Add(objCategorys);
                        _auditToolContext.SaveChanges();
                        return RedirectToAction("index");
                    }
                }
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "CategoryController_Insert", ErrorDiscription = ex.Message });
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
                    var data = (from cat in _auditToolContext.Categories select cat).ToList();
                    Categorys objCategorys = data.Find(category => category.CatgID == id);
                    objCategorys.IsActive = false;
                    objCategorys.ModifiedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                    objCategorys.ModifiedDate = DateTime.Now;
                    _auditToolContext.Categories.Update(objCategorys);
                    _auditToolContext.SaveChanges();
                    return RedirectToAction("Index", GetDetails());
                }
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "CategoryController_Delete", ErrorDiscription = ex.Message });
            }
            return RedirectToAction("Index", "Home");
        }

        private bool IsCategoryNameExists(string inputcategoryname)
        {
            bool result = false;
            try
            {
                var data = (from cat in _auditToolContext.Categories.Where(x => x.CatgDescription.ToLower() == inputcategoryname.ToLower()
                        && x.IsActive == true)
                            select cat).FirstOrDefault();

                if (data != null) result = true;
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "CategoryController_IsCategoryNameExists", ErrorDiscription = ex.Message });
            }
            return result;
        }

        private List<Categorys> GetDetails()
        {
            var data = new List<Categorys>();
            try
            {
                data = _auditToolContext.Categories.Where(x => x.IsActive == true).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "CategoryController_GetDetails", ErrorDiscription = ex.Message });
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
                    var data = (from cat in _auditToolContext.SubCategories.Where(x => x.IsActive == true) select cat).ToList();
                    SubCategory obj = data.Find(a => a.CatgID == id);
                    response = obj == null ? "HasecOrds" : "NoRecOrds";
                    return Json(response);
                }
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "CategoryController_HasDeleteAccess", ErrorDiscription = ex.Message });
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
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "CategoryController_Index", ErrorDiscription = ex.Message });
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

                    var data = _auditToolContext.Categories.Where(x => x.IsActive == true).ToList();
                    objCategoryMast = new CategoryMast();
                    objCategoryMast.CategoryList = new List<Category>();
                    foreach (var item in data)
                    {
                        Category objCategory = new Category();
                        objCategory.CatgID = item.CatgID; objCategory.CatgDescription = item.CatgDescription;
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
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = jsonData });
                }
                catch (Exception ex)
                {
                    _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "CategoryController_Index_objCategoryMast", ErrorDiscription = ex.Message });
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}