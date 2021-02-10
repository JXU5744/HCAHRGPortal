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
    public class SubCategoryController : Controller
    {
        private readonly ILogger<SubCategoryController> _logger;
        private readonly IConfiguration config;
        private IAuthService _authService;
        List<CategoryMast> masterCategory = null;
        private AuditToolContext _auditToolContext;
        public SubCategoryController(ILogger<SubCategoryController> logger, IConfiguration configuration, AuditToolContext audittoolc)//, IAuthService authService)
        {
            _auditToolContext = audittoolc;
            _logger = logger;
            config = configuration;
      //      _authService = authService;
        }

        
        //[Route("category/method")]
        public IActionResult GetCategory()
        {
            var categoryList = GetCategoryDetails();
            _logger.LogInformation($"No of records: {categoryList.Count()}");
            //categoryList.Insert(0, new Categorys { CatgID = 0, CatgDescription = "Select" });
            //ViewBag.ListOfCategory = categoryList;
            return Json(categoryList);
        }

        [HttpPost]
        public ActionResult GetCategoryByid(int id)
        {
            object response = "";
            if (id == 0)
            {
                return Json(response);
            }
            var data = GetDetail(id);
            return Json(data);
        }
        SubCategory GetSingleCategoryByid(string id)
        {
            var data = (from cat in _auditToolContext.SubCategories select cat).ToList();
            SubCategory objCategorys = data.Find(category => category.SubCatgID == Convert.ToInt32(id));
            return objCategorys;
        }

        [HttpPost]
        public ActionResult Edit(string id)
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
                        { responce = "1"; break; }
                    }
                    if (string.IsNullOrEmpty(responce.ToString()))
                    {
                        SubCategory objCategorys = GetSingleCategoryByid(param[0]);
                        if (objCategorys != null)
                        {
                            objCategorys.SubCatgDescription = param[1];
                            _auditToolContext.SubCategories.Update(objCategorys);
                            _auditToolContext.SaveChanges();
                        }
                    }
                }
            }
            return Json(responce);
        }

        [HttpPost]
        public ActionResult Insert(string catgID, string subCategoryName)
        {
            var collection = GetDetails(); object responce = "";
            foreach (var item in collection)
            {
          if (item.CatgID == Convert.ToInt32(catgID.Trim()) && item.SubCatgDescription == subCategoryName.Trim())
                { responce = "1"; break; }
            }
            if (string.IsNullOrEmpty(responce.ToString()))
            {
                SubCategory objCategorys = new SubCategory();
                objCategorys.CatgID = Convert.ToInt32(catgID); objCategorys.SubCatgDescription = subCategoryName;
                _auditToolContext.SubCategories.Add(objCategorys);
                _auditToolContext.SaveChanges();
                return RedirectToAction("Details");
            }
            return Json(responce);
        }

        [HttpPost]
        public IActionResult Details(CatSubCatJoinMast objCatSubCatJoinMast)
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

                var query = _auditToolContext.SubCategories

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
        }
        ).ToList();

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
            catch (Exception)
            {
                throw;
            }
        }
        
        [HttpGet]
        public IActionResult Details()
        {
            return View("Details", GetDetails());
        }


        List<CatSubCatJoinMast> GetDetails()
        {
            var query = _auditToolContext.SubCategories
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
         .Select( x => new CatSubCatJoinMast { CatgID = x.CatgID,
         SubCatgID = x.SubCatID, CatgDescription = x.CatgDescription
         ,SubCatgDescription = x.SubCatgDescription
         }
        ).ToList();

            return query;
        }

        CatSubCatJoinMast GetDetail(int subcatid)
        {
            var query = _auditToolContext.SubCategories
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
        ).Where(a => a.SubCatgID == subcatid).SingleOrDefault();
            return query;
        }
        [HttpPost]
        public IActionResult delete(int id)
        {
            var data = GetDetails().Where(a=>a.SubCatgID == id).SingleOrDefault();
            SubCategory objSubCategory = new SubCategory();
            objSubCategory.SubCatgID = id;
            _auditToolContext.SubCategories.Remove(objSubCategory);
            _auditToolContext.SaveChanges();
            return View("Details", GetDetails());
        }

        List<Categorys> GetCategoryDetails()
        {
            var data = (from subCat in _auditToolContext.Categories select subCat).ToList();
            return data;
        }

        [HttpPost]
        public ActionResult HasDeleteAccess(int id)
        {
            object response;
            var data = (from cat in _auditToolContext.QuestionMasters select cat).ToList();
            QuestionMaster obj = data.Find(a => a.SubCatgID == id);
            response = obj == null ? "NoRecords" : "HasRecords";
            return Json(response);
        }

    }

}

