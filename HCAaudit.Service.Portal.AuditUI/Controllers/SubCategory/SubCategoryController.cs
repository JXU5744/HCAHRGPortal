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


        SubCategory GetSubCategoryDetailsByID(int id)
        {
            SubCategory data = (from subcat in _auditToolContext.SubCategories.Where(x => x.SubCatgID == id && x.IsActive == true)
                                select subcat).FirstOrDefault();
            return data;
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
                        SubCategory objCategorys = GetSubCategoryDetailsByID(Int32.Parse(param[0]));
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
            object responce = "";

            if (isSubcategoryNameExists(Int32.Parse(catgID), subCategoryName)) { responce = "1"; }
            else
            {
                SubCategory objCategorys = new SubCategory();
                objCategorys.CatgID = Convert.ToInt32(catgID); objCategorys.SubCatgDescription = subCategoryName; objCategorys.IsActive = true;
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

        bool isSubcategoryNameExists(int categoryid, string subcategoryname)
        {
            bool result = false;
            var data = (from subcat in _auditToolContext.SubCategories.Where(x => x.SubCatgDescription.ToLower() == subcategoryname.ToLower()
                         && x.CatgID == categoryid && x.IsActive == true)
                        select subcat).FirstOrDefault();
            if (data != null) result = true;
            return result;
        }




        List<CatSubCatJoinMast> GetDetails()
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
             CatgDescription = x.CatgDescription
        ,
             SubCatgDescription = x.SubCatgDescription
         }
        ).ToList();

            return query;
        }

        CatSubCatJoinMast GetDetail(int subcatid)
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
        ).Where(a => a.SubCatgID == subcatid).SingleOrDefault();
            return query;
        }
        [HttpPost]
        public IActionResult delete(int id)
        {
            SubCategory objSubCategory = new SubCategory();
            try
            {
                objSubCategory = GetSubCategoryDetailsByID(id);

                objSubCategory.SubCatgID = id; objSubCategory.IsActive = false;
                _auditToolContext.SubCategories.Update(objSubCategory);
                _auditToolContext.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View("Details", GetDetails());

        }

        List<Categorys> GetCategoryDetails()
        {
            return (from subCat in _auditToolContext.Categories.Where(a => a.IsActive == true) select subCat).ToList();
        }

        [HttpPost]
        public ActionResult HasDeleteAccess(int id)
        {
            object response;
            var data = (from cat in _auditToolContext.QuestionMasters.Where(a => a.IsActive == true) select cat).ToList();
            QuestionMaster obj = data.Find(a => a.SubCatgID == id);
            response = obj == null ? "NoRecords" : "HasRecords";
            return Json(response);
        }

    }

}

