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
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IConfiguration config;
        private IAuthService _authService;
        List<CategoryMast> masterCategory = null;
        private AuditToolContext _auditToolContext;
        public CategoryController(ILogger<CategoryController> logger, IConfiguration configuration, AuditToolContext audittoolc)//, IAuthService authService)
        {
            _auditToolContext = audittoolc;
            _logger = logger;
            config = configuration;
      //      _authService = authService;
        }

        [HttpPost]
        public ActionResult GetCategoryByid(string id)
        {
            object response = "";
            if (string.IsNullOrEmpty(id))
            {
                return Json(response);
            }
            return Json(GetSingleCategoryByid(id));
        }
        Categorys GetSingleCategoryByid(string id)
        {
            var data = (from cat in _auditToolContext.Categories select cat).ToList();
            Categorys objCategorys = data.Find(category => category.CatgID == Convert.ToInt32(id));
            return objCategorys;
        }
        [HttpPost]
        public ActionResult Edit(string id)
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
                        Categorys objCategorys = new Categorys();
                        objCategorys = GetSingleCategoryByid(param[0]);
                        objCategorys.CatgDescription = param[1];
                        _auditToolContext.Categories.Update(objCategorys);
                        _auditToolContext.SaveChanges();
                    }
                }
            }
            return Json(resp);
        }

        [HttpGet]
        public ActionResult Insert(string CategoryName)
        {
            var collection = GetDetails(); object responce = "";
            foreach (var item in collection)
            {
                if (item.CatgDescription.ToLower() == CategoryName.ToLower().Trim())
                { responce = "1"; break; }
            }
            if (string.IsNullOrEmpty(responce.ToString()))
            {
                Categorys objCategorys = new Categorys(); objCategorys.CatgDescription = CategoryName;
                _auditToolContext.Categories.Add(objCategorys);
                _auditToolContext.SaveChanges();
                return RedirectToAction("index");
            }
            return Json(responce);
        }
       
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var data = (from cat in _auditToolContext.Categories select cat).ToList();
            Categorys objCategorys = data.Find(category => category.CatgID == id);
            _auditToolContext.Categories.Remove(objCategorys); _auditToolContext.SaveChanges();
            return RedirectToAction("Index",GetDetails());
        }

        List<Categorys> GetDetails()
        {
            var data = _auditToolContext.Categories.ToList();
            return data;
        }
        [HttpPost]
        public ActionResult HasDeleteAccess(int id)
        {
            object response;
            var data = (from cat in _auditToolContext.SubCategories select cat).ToList();
            SubCategory obj = data.Find(a => a.CatgID ==  id);
            response = obj == null ? "HasecOrds" : "NoRecOrds";
            return Json(response);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("Index", GetDetails());
        }

        [HttpPost]
        public IActionResult Index(CategoryMast objCategoryMast)
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

                var data = (from cat in _auditToolContext.Categories select cat).ToList();
                objCategoryMast = new CategoryMast();
                objCategoryMast._categoryList = new List<Category>();
                foreach (var item in data)
                {
                    Category objCategory = new Category();
                    objCategory.CatgID = item.CatgID; objCategory.CatgDescription = item.CatgDescription;
                    objCategoryMast._categoryList.Add(objCategory);
                }

                // getting all Customer data  
                var customerData = (from tempcustomer in objCategoryMast._categoryList
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
            catch (Exception)
            {
                throw;
            }
        }
    }
   
}

