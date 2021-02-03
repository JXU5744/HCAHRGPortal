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
using System.Collections;
using HCAaudit.Service.Portal.AuditUI.ViewModel;

namespace HCAaudit.Service.Portal.AuditUI.Controllers
{
    //[Authorize]
    public class AuditController : Controller
    {
        private readonly ILogger<AuditController> _logger;
        private readonly IConfiguration config;
        private IAuthService _authService;
        List<CategoryMast> masterCategory = null;
        private AuditToolContext _auditToolContext;
        public AuditController(ILogger<AuditController> logger, IConfiguration configuration, AuditToolContext audittoolc)//, IAuthService authService)
        {
            _auditToolContext = audittoolc;
            _logger = logger;
            config = configuration;
        }

        [HttpGet]
        public IActionResult Details()
        {
            BindSearchGrid objBindSearchGrid = new BindSearchGrid();
            objBindSearchGrid._dataforGrid = BindSearchGrid.GetGridData();
            return View("Details", objBindSearchGrid);
        }

        [HttpPost]
        public IActionResult Details(BindSearchGrid objBindSearchGrid)
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

                // getting all Customer data  
                var customerData = (from tempcustomer in BindSearchGrid.GetGridData()
                                    select tempcustomer);

                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    switch (sortColumn)
                    {
                        case "TicketNumber":
                            if (sortColumnDirection == "desc")
                            {
                                customerData = customerData.OrderByDescending(s => s.TicketNumber);
                            }
                            else
                            {
                                customerData = customerData.OrderBy(s => s.TicketNumber);
                            }
                            break;
                    }
                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.TicketNumber == searchValue);
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
        public ActionResult Navigate(string id)
        {
            return Redirect("https://www.google.com");
        }

        //[HttpPost]
        //public IActionResult Index(BindSearchGrid objBindSearchGrid)
        //{
        //    return RedirectToAction("Details", objBindSearchGrid);
        //}
        [HttpGet]
        //public IActionResult Index(AuditViewModel auditViewModel)

            public IActionResult Index()
        {
            //if (auditViewModel != null)
            {
                //var ticketId = auditViewModel.TicketId;
                //var ticketStatus = auditViewModel.TicketStatus;
                //var ticketSubStatus = auditViewModel.TicketStatus;
                //var auditTicketDate = auditViewModel.TicketDate;
                //var serviceCategory = auditViewModel.ServiceCatId;
                //var subCategory = auditViewModel.SubCatId;

                //if (ticketStatus == 0)
                //{
                //   // var ticketInfo = _auditToolContext.t
                //}
                //else
                //{

                //}

               

                var query = _auditToolContext.questionBank
                   .Join(
                        _auditToolContext.questionMasters,
                        questionBanks => questionBanks.QuestionID,
                       questionMasters => questionMasters.QuestionId,
                       (questionBanks, questionMasters) => new
                       {
                           QuestionId = questionBanks.QuestionID,
                           QuestionName = questionBanks.QuestionName,
                           SeqNumber = questionMasters.SeqNumber,
                           CatSubCatId = questionMasters.SubCatgID
                       })
                    .Where(a => a.CatSubCatId == 25)
                    .OrderBy(b => b.SeqNumber)
                    .Select(x => new QuestionConfigMappingJoinMast
                    {
                        QuestionId = x.QuestionId,
                        QuestionName = x.QuestionName,
                        QuestionSeqNumber = x.SeqNumber
                    }
                   ).ToList();



                //var data = (from config in _auditToolContext.questionMasters
                //            join question in _auditToolContext.questionBank
                //            on config.QuestionId equals question.QuestionID
                //            select question).Distinct().ToList();
                ViewModel.Action objAction = new ViewModel.Action();
                Impact objImpact = new Impact();
                objImpact.IsHighImpact = false;
                objImpact.IsLowImpact = false;
                objAction.Impact = objImpact;
                objAction.IsCompliance = false;
                objAction.IsNonCompliance = false;
                objAction.IsNotApplicable = false;
                CategoryMast objCategoryMast = new CategoryMast();
                objCategoryMast._categoryList = new List<Category>();
                List<HCAaudit.Service.Portal.AuditUI.ViewModel.Question> lstQuestionList = new List<HCAaudit.Service.Portal.AuditUI.ViewModel.Question>();
                foreach (var item in query)
                {
                    HCAaudit.Service.Portal.AuditUI.ViewModel.Question objQuestion = new HCAaudit.Service.Portal.AuditUI.ViewModel.Question();
                    objQuestion.QuestionId = item.QuestionId;
                    objQuestion.QuestionName = item.QuestionName;
                    objQuestion.Action = objAction;
                    objQuestion.CorrectionRequire = true;
                    lstQuestionList.Add(objQuestion);
                }
                AuditViewModel auditDetail = new AuditViewModel();

                auditDetail.Question = lstQuestionList;

                return View(auditDetail);
            }
            //else
            //{
            //    return View(auditViewModel);
            //}
        }

        [HttpPost]
        //public IActionResult SaveAudit(AuditViewModel audit)
        public IActionResult Index(AuditViewModel audit)
        {
            if (audit != null) {
                AuditMainResponse obj = new AuditMainResponse();
                
                //        obj.QuestionId
                //        obj.QuestionRank
                //        obj.TicketID
                foreach (var item in audit.Question)
                {
                    obj.QuestionId = item.QuestionId;
                    obj.QuestionRank = item.QuestionSequence;
                        obj.TicketID = Guid.NewGuid();
                    obj.isCompliant = item.Action.IsCompliance;
                    obj.isCorrectionRequired = item.CorrectionRequire;
                    obj.isHighNonComplianceImpact = item.Action.Impact.IsHighImpact;
                    obj.isLowNonComplianceImpact = item.Action.Impact.IsLowImpact;
                    obj.isNA = item.Action.IsNotApplicable;
                    obj.isNonCompliant = item.Action.IsNonCompliance;
                    obj.NonComplianceComments = item.Comments;
                    obj.ModifiedDate = DateTime.Now.ToShortDateString();
                    _auditToolContext.AuditMainResponse.Add(obj);
                    _auditToolContext.SaveChanges();
                }
            }
            return View(audit);
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

        List<Categorys> GetCategoryDetails()
        {
            var data = (from subCat in _auditToolContext.categories select subCat).ToList();
            return data;
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

        [HttpPost]
        public ActionResult Insert(string subcatgname)
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
            Categorys objCategorys = new Categorys(); objCategorys.CatgDescription = subcatgname;
            objCategorys.CatgID = max + 1;
            _auditToolContext.categories.Add(objCategorys);
            _auditToolContext.SaveChanges();

            return RedirectToAction("Details");
        }
       
        //[HttpPost]
        //public ActionResult GetTicketDetails()
        //{
        //    var data = (from config in _auditToolContext.questionMasters
        //                join question in _auditToolContext.questionBank
        //                on config.QuestionId equals question.QuestionID
        //                select question).ToList();
        //    CategoryMast objCategoryMast = new CategoryMast();
        //    objCategoryMast._categoryList = new List<Category>();
        //    List<QuestionMapping> lstQuestionList = new List<QuestionMapping>();
        //    foreach (var item in data)
        //    {
        //        QuestionMapping objQUestionmapping = new QuestionMapping();
        //        objQUestionmapping.QuestionId = item.QuestionID;
        //        objQUestionmapping.QuestionDescription = item.QuestionName;
        //        lstQuestionList.Add(objQUestionmapping);
        //    }
        //    return Json(lstQuestionList);
     //   }
    }
}

