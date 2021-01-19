using HCAaudit.Service.Portal.AuditUI.Models;
using HCAaudit.Service.Portal.AuditUI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCAaudit.Service.Portal.AuditUI.Controllers.Evaluation
{
    public class EvaluationController : Controller
    {
        private readonly ILogger<EvaluationController> _logger;
        private readonly IConfiguration config;
        private IAuthService _authService;
        private AuditToolContext _auditToolContext;
        public EvaluationController(ILogger<EvaluationController> logger, IConfiguration configuration, AuditToolContext audittoolc)//, IAuthService authService)
        {
            _auditToolContext = audittoolc;
            _logger = logger;
            config = configuration;
            //      _authService = authService;
        }

        //[Route("")]
        //[Route("home/showcategory")]
        //public IActionResult ShowCategory()
        //{
        //    var categoryList = CategoryList.GetCategory();

        //    return View("category",categoryList);
        //}

        //[Route("")]
        //[Route("home/showsubcategory")]
        //public IActionResult ShowSubCategory()
        //{
        //    var filteredSubCategoryList = SubCategoryList.GetSubCategory();
        //    return View("subcategory", filteredSubCategoryList);
        //}

        public IActionResult BindControls()
        {
            Category objCategory = new Category();
            return View(objCategory);
        }

        [HttpPost]
        public JsonResult BindSubCategory(string categoryID)
        {
            _logger.LogInformation($"Request for SubCategoryList with CategoryID: {categoryID}");
            var filteredSubCategoryList = SubCategoryList.GetSubCategory()
                                         .Where(x => x.CatgID == Convert.ToInt32(categoryID))
                                         .Select(x => new { x.SubCatgID, x.SubCatgDescription }).ToList();
            _logger.LogInformation($"No of SubCategoryListrecords: {filteredSubCategoryList.Count()}");
            return Json(filteredSubCategoryList);
        }

        [HttpPost]
        public JsonResult SaveQuestion(string subcatgid)
        {
            try
            {
                if (subcatgid.Contains('^'))
                {
                    string[] data = subcatgid.Split('^');
                    if (data.Count() > 0)
                    {
                        QuestionMaster objQuestionMaster = new QuestionMaster();
                        //var questionRecords = GetDetails();
                        //int max;
                        //if (questionRecords.Count == 0)
                        //{
                        //    max = 0;
                        //}
                        //else
                        //{
                        //    max = questionRecords.OrderByDescending(x => x.QuestionId).First().QuestionId;
                        //}
                        objQuestionMaster.QuestionId = Guid.NewGuid();
                        //objQuestionMaster.SubCatgID = Convert.ToInt32(data[0]);
                        objQuestionMaster.QuestionText = data[1];
                        _logger.LogInformation($"Request for Adding Question to DB with SubCategoryID: {data[0]} and Question Text as: {data[1]}");
                        _auditToolContext.questionMasters.Add(objQuestionMaster);
                        _auditToolContext.SaveChanges();
                        //if (insertstatus > 0)
                        //{
                        //    _logger.LogInformation($"Inserted Sucessfully.");
                        //}
                        //else _logger.LogInformation($"Inserted failed.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in insert question while typecasting and database insert.");
            }

            return Json("");
        }
        List<tbQuestionMaster> GetDetails()
        {
            var data1 = _auditToolContext.questionMasters.ToList();
            return data1;
        }

        [HttpPost]
        public JsonResult BindQestions(string subCategoryID, string categoryID)
        {
            _logger.LogInformation($"Request for BindQuestions with subCategoryID: {subCategoryID}");
            Category objCategory = new Category();
            var data = CategoryList.GetCategory()
                .Where(x => x.CatgID == Convert.ToInt32(categoryID))
                .Select(x => new { x.CatgID, x.CatgDescription }).SingleOrDefault();

            objCategory.CatgID = data.CatgID; objCategory.CatgDescription = data.CatgDescription;
            objCategory.SubCatID = Convert.ToInt32(subCategoryID);
            objCategory._questionList = QuestionList.GetQestion()
                                         .Where(x => x.SubCatgID == Convert.ToInt32(subCategoryID))
                                         .ToList();
            _logger.LogInformation($"No of QuestionListrecords: {objCategory._questionList.Count()}");
            _logger.LogError("No Error");
            return Json(objCategory);
        }

        public IActionResult Index()
        {
            var categoryList = _auditToolContext.categories.ToList();//CategoryList.GetCategory();
            _logger.LogInformation($"No of records: {categoryList.Count()}");
            categoryList.Insert(0, new Categorys { CatgID = 0, CatgDescription = "Select" });
            ViewBag.ListOfCategory = categoryList;

            Category objCategory = new Category();

            var data = CategoryList.GetCategory()
                .Where(x => x.CatgID == Convert.ToInt32(3))
                .Select(x => new { x.CatgID, x.CatgDescription }).SingleOrDefault();

            objCategory.CatgID = data.CatgID; objCategory.CatgDescription = data.CatgDescription;
            objCategory.SubCatID = Convert.ToInt32(2);
            objCategory._questionList = QuestionList.GetQestion()
                                         .Where(x => x.SubCatgID == Convert.ToInt32(2))
                                         .ToList();

            return View("index", objCategory);
            //_logger.LogInformation("");
            //if (_authService.CheckUserGroups().Result)
            //{
            //    return View("index");
            //}
            //else
            //{
            //    return Error();
            //}
        }
    }
}
