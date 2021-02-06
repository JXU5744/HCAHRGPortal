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


namespace HCAaudit.Service.Portal.AuditUI.Controllers
{
    //[Authorize]
    public class QuestionsController : Controller
    {
        private readonly ILogger<QuestionsController> _logger;
        private readonly IConfiguration config;
        private IAuthService _authService;
        private AuditToolContext _auditToolContext;
        public QuestionsController(ILogger<QuestionsController> logger, IConfiguration configuration, AuditToolContext audittoolc)//, IAuthService authService)
        {
            _auditToolContext = audittoolc;
            _logger = logger;
            config = configuration;
            //_authService = authService;
        }

        [HttpPost]
        public ActionResult GetQuestionByid(string id)
        {
            object response = "";
            if (string.IsNullOrEmpty(id))
            {
                return Json(response);
            }
            return Json(GetSingleQuestionByid(id));
        }
        tblQuestionBank GetSingleQuestionByid(string id)
        {
            var data = (from cat in _auditToolContext.QuestionBank select cat).ToList();
            tblQuestionBank objQuestionBank = data.Find(category => category.QuestionID == Convert.ToInt32(id));
            return objQuestionBank;
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
                    tblQuestionBank objtblQuestionBank = GetSingleQuestionByid(param[0]);
                    //var data = GetDetails().Where(a => a.QuestionName.ToLower() == param[1].ToLower()).SingleOrDefault(); 

                    if (objtblQuestionBank != null && objtblQuestionBank.QuestionName == param[1].ToLower())
                    { responce = "1"; }
                    if (string.IsNullOrEmpty(responce.ToString()))
                    {
                        objtblQuestionBank.QuestionName = param[1];
                        objtblQuestionBank.QuestionDescription = param[2];
                        _auditToolContext.QuestionBank.Update(objtblQuestionBank);
                        _auditToolContext.SaveChanges();
                        return Json(objtblQuestionBank);
                    }
                }
            }
            return Json(responce);
        }


        public IActionResult BindControls()
        {
            Category objCategory = new Category();
            return View(objCategory);
        }

        [HttpPost]
        public JsonResult BindSubCategory(string categoryID)
        {
            _logger.LogInformation($"Request for SubCategoryList with CategoryID: {categoryID}");
            var subCategoryList = _auditToolContext.SubCategories.ToList();
            var filteredSubCategoryList = subCategoryList
                                         .Where(x => x.CatgID == Convert.ToInt32(categoryID))
                                         .Select(x => new { x.SubCatgID, x.SubCatgDescription }).ToList();
            _logger.LogInformation($"No of SubCategoryListrecords: {filteredSubCategoryList.Count()}");
            return Json(filteredSubCategoryList);
        }

        [HttpPost]
        public JsonResult SaveQuestionForMaster(string subcatgid)
        {
            try
            {
                if (subcatgid.Contains('^'))
                {
                    string[] data = subcatgid.Split('^');
                    if (data.Count() > 0 && data[0] != null && data[1] != null && data[2] != null && data[3] != null)
                    {
                        object response = "";

                        var questionbankdata = _auditToolContext.QuestionBank.Where(x => x.QuestionName.Trim() == data[1].ToString().Trim())
                                  .Select(a => new { a.QuestionID, a.QuestionName }).SingleOrDefault();
                        if (questionbankdata == null)//condition to restrict questions which is not available in questionBank table
                        {
                            return Json(response = "2");
                        }
                        QuestionMaster objtbQuestionMaster = new QuestionMaster();
                        objtbQuestionMaster.QuestionId = questionbankdata.QuestionID;
                        objtbQuestionMaster.SubCatgID = Convert.ToInt32(data[0]);
                        objtbQuestionMaster.QuestionText = questionbankdata.QuestionName;
                        objtbQuestionMaster.QuestionScore = 10;// as Score column is not mandatory now Convert.ToInt32(data[2]);
                        objtbQuestionMaster.SeqNumber = Convert.ToInt32(data[3]);
                        _logger.LogInformation($"Request for Adding Question to DB with SubCategoryID: {data[0]} and Question Text as: {data[1]}");

                        _auditToolContext.QuestionMasters.Add(objtbQuestionMaster);
                        int insertstatus = _auditToolContext.SaveChanges();
                        if (insertstatus > 0)
                        {
                            _logger.LogInformation($"Inserted Sucessfully.");
                        }
                        else _logger.LogInformation($"Inserted failed.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in insert question while typecasting and database insert.");
            }

            return Json("");
        }
        List<tblQuestionBank> GetDetails()
        {
            var data = _auditToolContext.QuestionBank.ToList();
            return data;
        }


        [HttpGet]
        public JsonResult BindQestions(string subCategoryID)
        {
            _logger.LogInformation($"Request for BindQuestions with subCategoryID: {subCategoryID}");

            var questionList = _auditToolContext.QuestionMasters.OrderBy(a=>a.SeqNumber).ToList();
            var data = questionList.Where(x => x.SubCatgID == Convert.ToInt32(subCategoryID))
                                   .Select(x => new { x.QuestionId, x.QuestionText })
                                   .ToList();
            return Json(data);
        }

        
        public IActionResult QuestoinMapping()
        {
            var categoryList = _auditToolContext.Categories.ToList();
            _logger.LogInformation($"No of records: {categoryList.Count()}");
            categoryList.Insert(0, new Categorys { CatgID = 0, CatgDescription = "Select" });
            ViewBag.ListOfCategory = categoryList;
            return View();
        }

        [HttpGet]
        public IActionResult BindGrid(string subCategoryID)
        {
            var data = _auditToolContext.QuestionMasters
                                    .Where(x => x.SubCatgID == Convert.ToInt32(subCategoryID))
                                    .OrderBy(y => y.SeqNumber)
                                    .Select(a => new { a.QuestionId, a.QuestionText ,a.QuestionScore })
                                    .ToList();

            return Json(data);
        }


        [HttpGet]
        public IActionResult QuestionMaster()
        {
            return View("QuestionMaster", GetDetails());
        }

       public JsonResult GetCommaSeperated()
        {
            return Json(GetDetails().Select(a => a.QuestionName));
        }

        public JsonResult GetIndexCommaSeperated(string subCategoryID)
        {
            var dataMaster = _auditToolContext.QuestionMasters
                       .Where(a => a.SubCatgID == Convert.ToInt32(subCategoryID)).ToList();
            var dataBank = GetDetails();
            if (dataMaster.Count()>0)
            {
                foreach (var item in dataMaster)
                {

                    tblQuestionBank objtblQuestionBank = new tblQuestionBank();
                    objtblQuestionBank = dataBank.Where(a => a.QuestionID == item.QuestionId).SingleOrDefault();
                    if (objtblQuestionBank != null)
                    {
                        dataBank.Remove(objtblQuestionBank);
                    }
                }
            }

            return Json(dataBank.Select(a => a.QuestionName));
        }

        [HttpPost]
        public IActionResult QuestionMaster(tblQuestionBank objCategoryMast)
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
                var customerData = (from tempcustomer in GetDetails()
                                    select tempcustomer);

                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    switch (sortColumn)
                    {
                        case "QuestionName":
                            if (sortColumnDirection == "desc")
                            {
                                customerData = customerData.OrderByDescending(s => s.QuestionName);
                            }
                            break;
                    }
                }

                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.QuestionName.ToLower().StartsWith(searchValue.ToLower()));
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

        public IActionResult Delete(int id)
        {
            var data = (from Q in _auditToolContext.QuestionBank select Q).ToList();
            tblQuestionBank objtblQuestionBank = data.Find(a => a.QuestionID == id);
            _auditToolContext.QuestionBank.Remove(objtblQuestionBank); _auditToolContext.SaveChanges();
            return View("QuestionMaster", GetDetails());
        }

        public IActionResult DeleteQuestionMaster(int id)
        {
            var data = _auditToolContext.QuestionMasters.ToList();
            QuestionMaster objtbQuestionMaster = data.Find(a => a.QuestionId == id);
            _auditToolContext.QuestionMasters.Remove(objtbQuestionMaster); _auditToolContext.SaveChanges();
            return View("QuestionMaster", GetDetails());
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            var data = (from Q in _auditToolContext.QuestionBank select Q).ToList();
            var filterData = data.Find(a => a.QuestionID == id);
            return View("Edit", filterData);
        }

        [HttpPost]
        public ActionResult Insert(string questionname, string questiondesc)
        {
            var data = GetDetails().Where(a => a.QuestionName.ToLower() == questionname.ToLower()).SingleOrDefault(); object responce = "";
            
                if (data != null)
                { responce = "1"; }
            
            if (string.IsNullOrEmpty(responce.ToString()) && !string.IsNullOrEmpty(questionname) && !string.IsNullOrEmpty(questiondesc))
            {
                tblQuestionBank objtblQuestionBank = new tblQuestionBank();
                objtblQuestionBank.QuestionName = questionname;
                objtblQuestionBank.QuestionDescription = questiondesc;
                _auditToolContext.QuestionBank.Add(objtblQuestionBank);
                _auditToolContext.SaveChanges();
                return RedirectToAction("QuestionMaster");
            }
            
            return Json(responce);
        }

        [HttpPost]
        public ActionResult HasDeleteAccessforQB(int id)
        {
            object response;
            var data = (from cat in _auditToolContext.QuestionMasters select cat).ToList();
            QuestionMaster obj = data.Find(a => a.QuestionId == id);
            response = obj == null ? "NoRecords" : "HasRecords";
            return Json(response);
        }
    }
}
