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
        
        public List<CategoryMastR> ReturnData(List<CategoryMast> masterCategory)
        {
            List<CategoryMastR> objCategoryMastRList = new List<CategoryMastR>();
            foreach (var item in masterCategory)
            {
                CategoryMastR objCategoryMastR = new CategoryMastR();
                objCategoryMastR.CatgID = item.CatgID.ToString();
                objCategoryMastR.CatgDescription = item.CatgDescription;
                objCategoryMastRList.Add(objCategoryMastR);
            }
            return objCategoryMastRList;
        }

        [HttpGet]
        public string GetCategories(string CatgID,string CatgDescription)
        {
            fillInSession();
            var data = JsonConvert.SerializeObject(ReturnData(masterCategory));
            //var Vinayaka = data.Replace("},{", "}],[{");
            return data;
        }

       
        //[Route("category/method")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details()
        {
            return View("details", GetDetails());
        }
        [HttpGet]
        public IActionResult Edit(int CatgID)
        {
            var data = (from cat in _auditToolContext.categories select cat).ToList();
            Categorys objCategorys = data.Find(category => category.CatgID == CatgID);
            Category objCategory = new Category();objCategory.CatgID = objCategorys.CatgID;objCategory.CatgDescription = objCategorys.CatgDescription;
            return View("Edit", objCategory);
        }

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            Categorys objCategorys = new Categorys();objCategorys.CatgDescription = category.CatgDescription;
            objCategorys.CatgID = category.CatgID;
            _auditToolContext.categories.Update(objCategorys);
            _auditToolContext.SaveChanges();
            
            return View("Details", GetDetails());
        }
        [HttpPost]
        public ActionResult Insert(Category category)
        {
            CategoryMast objCategoryMast = new CategoryMast();
            objCategoryMast = GetDetails();
            int max;
            if (objCategoryMast._categoryList.Count == 0)
            {
                max = 0;
            }
            else
            {
                max = objCategoryMast._categoryList.OrderByDescending(x => x.CatgID).First().CatgID;
            }
            Categorys objCategorys = new Categorys(); objCategorys.CatgDescription = category.CatgDescription;
            objCategorys.CatgID = max + 1;
            _auditToolContext.categories.Add(objCategorys);
            _auditToolContext.SaveChanges();

            return View("Details", GetDetails());
        }
        public string insertCategories(CategoryMast item)
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
            objcategory.CatgDescription = item.CatgDescription;

            masterCategory.Insert(10,objcategory);
            var data = JsonConvert.SerializeObject(ReturnData(masterCategory));
            return data;
        }
        public void clear()
        {
            fillInSession();
            //var category =  CategoryList.GetCategory().Select(x => new { x.CatgID, x.CatgDescription }); //((List<Category>)Session["Products"]);
            masterCategory.Clear();
        }
        
        public String update(CategoryMast item)
        {
            fillInSession();
            List<CategoryMast> CategoryList = masterCategory;// ((List<Category>)Session["Products"]);
            CategoryMast objCategory = CategoryList.Find(category => category.CatgID == item.CatgID);
            masterCategory.Remove(objCategory);
            objCategory.CatgID = item.CatgID;
            objCategory.CatgDescription = item.CatgDescription;
            masterCategory.Add(objCategory);
            var data = JsonConvert.SerializeObject(ReturnData(masterCategory));
            return data;
        }

        [HttpGet]
        public IActionResult Delete(int CatgID)
        {
            var data = (from cat in _auditToolContext.categories select cat).ToList();
            Categorys objCategorys = data.Find(category => category.CatgID == CatgID);
            _auditToolContext.categories.Remove(objCategorys);_auditToolContext.SaveChanges();
            return View("Details", GetDetails());
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
        }
        CategoryMast GetDetails()
        {
            var data = (from cat in _auditToolContext.categories select cat).ToList();
            CategoryMast objCategoryMast = new CategoryMast();
            objCategoryMast._categoryList = new List<Category>();
            foreach (var item in data)
            {
                Category objCategory = new Category();
                objCategory.CatgID = item.CatgID; objCategory.CatgDescription = item.CatgDescription;
                objCategoryMast._categoryList.Add(objCategory);
            }
            return objCategoryMast;
        }
    }
   
}

