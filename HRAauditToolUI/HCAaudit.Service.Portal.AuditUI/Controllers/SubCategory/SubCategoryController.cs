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
        public IActionResult Index()
        {
            var categoryList = GetCategoryDetails();
            _logger.LogInformation($"No of records: {categoryList.Count()}");
            categoryList.Insert(0, new Categorys { CatgID = 0, CatgDescription = "Select" });
            ViewBag.ListOfCategory = categoryList;
            return View();
        }

        [HttpPost]
        public ActionResult Insert(SubCategory subCategory)
        {
            SubCategory objSubCategory = new SubCategory();
            var subCategoriesList = GetDetails();
            int max;
            if (subCategoriesList.Count == 0)
            {
                max = 0;
            }
            else
            {
                max = subCategoriesList.OrderByDescending(x => x.SubCatgID).First().SubCatgID;
            }
            objSubCategory.SubCatgDescription = subCategory.SubCatgDescription;
            objSubCategory.CatgID = subCategory.CatgID;
            objSubCategory.SubCatgID= max + 1;
            _auditToolContext.subCategories.Add(objSubCategory);
            _auditToolContext.SaveChanges();

            return View("Details");
        }
        public IActionResult Details()
        {
            var categoryList = CategoryList.GetCategory();
            _logger.LogInformation($"No of records: {categoryList.Count()}");
            SubCategoryMastList objSubCategoryMastList = new SubCategoryMastList();
            objSubCategoryMastList._subCategoryMastList = new List<SubCategoryMast>();
            foreach (var item in categoryList)
            {
                SubCategoryMast objSubCategoryMast = new SubCategoryMast(); objSubCategoryMast._subcategoryList = new List<SubCategory>();
                objSubCategoryMast.CatgID = item.CatgID;
                objSubCategoryMast.CatgDescription = item.CatgDescription;
                var subcategorylist = SubCategoryList.GetSubCategory()
                                         .Where(x => x.CatgID == item.CatgID)
                                         .ToList();
                foreach (var subcategory in subcategorylist)
                {
                    objSubCategoryMast._subcategoryList.Add(subcategory);
                }
                objSubCategoryMastList._subCategoryMastList.Add(objSubCategoryMast);
            }
           return View(objSubCategoryMastList);
        }

        public IActionResult Edit(int CatgID)
        {
            fillInSession();

            CategoryMast objCategory = masterCategory.Find(category => category.CatgID == CatgID);
            masterCategory.Remove(objCategory);
            return View("Edit", objCategory);
        }
        private void fillInSession()
        {
            if (masterCategory == null)
            {
                //pqTestContext db = new pqTestContext();
                //add in session["Products"];
                masterCategory = new List<CategoryMast>();
                var categoryList = CategoryList.GetCategory();//db.Database.SqlQuery<Product>("Select productid, ProductName, QuantityPerUnit, UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued from products").ToList();
                foreach (Category objCategory in categoryList)
                {
                    CategoryMast objCategoryMast = new CategoryMast();
                    objCategoryMast.CatgID = objCategory.CatgID;
                    objCategoryMast.CatgDescription = objCategory.CatgDescription;
                    masterCategory.Add(objCategoryMast);
                }
            }
        }

        public String delete(int CatgID)
        {
            fillInSession();
            List<CategoryMast> CategoryList = masterCategory;// ((List<Category>)Session["Products"]);
            CategoryMast objCategory = CategoryList.Find(category => category.CatgID == CatgID);
            CategoryList.Remove(objCategory);
            return "{\"result\": \"success\"}";
        }

        List<Categorys> GetCategoryDetails()
        {
            var data = (from subCat in _auditToolContext.categories select subCat).ToList();
            return data;
        }
        List<SubCategory> GetDetails()
        {
            var data = (from subCat in _auditToolContext.subCategories select subCat).ToList();
            return data;
        }

        /*
        public String add(String strCategoryName)
        {
            fillInSession();

            List<CategoryMast> categoryList = masterCategory;//((List<Category>)Session["Products"]);
            int max;
            if (categoryList.Count == 0)
            {
                max = 0;
            }
            else
            {
                max = categoryList.OrderByDescending(x => x.CatgID).First().CatgID;
            }

            CategoryMast objcategory = new CategoryMast();
            objcategory.CatgID = max + 1;
            objcategory.CatgDescription = strCategoryName;

            categoryList.Add(objcategory);
            return "{\"recId\": \"" + objcategory.CatgID + "\"}";
        }
        public void clear()
        {
            fillInSession();
            //var category =  CategoryList.GetCategory().Select(x => new { x.CatgID, x.CatgDescription }); //((List<Category>)Session["Products"]);
            masterCategory.Clear();
        }
        public String update(int CatgID, String CategoryName)
        {
            fillInSession();

            List<CategoryMast> CategoryList = masterCategory;// ((List<Category>)Session["Products"]);

            CategoryMast objCategory = CategoryList.Find(category => category.CatgID == CatgID);

            objCategory.CatgDescription = CategoryName;

            return "{\"result\": \"success\"}";
        }
        public String delete(int CatgID)
        {
            fillInSession();

            List<CategoryMast> CategoryList = masterCategory;// ((List<Category>)Session["Products"]);

            CategoryMast objCategory = CategoryList.Find(category => category.CatgID == CatgID);
            CategoryList.Remove(objCategory);

            return "{\"result\": \"success\"}";
        }
        private void fillInSession()
        {
            if (masterCategory == null)
            {
                //pqTestContext db = new pqTestContext();
                //add in session["Products"];
                masterCategory = new List<CategoryMast>();
                var categoryList = CategoryList.GetCategory();//db.Database.SqlQuery<Product>("Select productid, ProductName, QuantityPerUnit, UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued from products").ToList();
                foreach (Category objCategory in categoryList)
                {
                    CategoryMast objCategoryMast = new CategoryMast();
                    objCategoryMast.CatgID = objCategory.CatgID;
                    objCategoryMast.CatgDescription = objCategory.CatgDescription;
                    masterCategory.Add(objCategoryMast);
                }
            }
        }
        //get products with paging
        public ActionResult getP(int pq_curPage, int pq_rPP)
        {
            fillInSession();

            int total_Records = (from order in masterCategory
                                 select order).Count();

            int skip = (pq_rPP * (pq_curPage - 1));
            if (skip >= total_Records)
            {
                pq_curPage = (int)Math.Ceiling(((double)total_Records) / pq_rPP);
                skip = (pq_rPP * (pq_curPage - 1));
            }

            var products2 = (from order in masterCategory
                             orderby order.CatgDescription
                             select order).Skip(skip).Take(pq_rPP);

            StringBuilder sb = new StringBuilder(@"{""totalRecords"":" + total_Records + @",""curPage"":" + pq_curPage + @",""data"":");


            //JavaScriptSerializer js = new JavaScriptSerializer();

            //string json = js.Serialize(products2);
            string json = JsonConvert.SerializeObject(products2);
            sb.Append(json);
            sb.Append("}");

            return this.Content(sb.ToString(), "text/text");
        }*/
    }

}

