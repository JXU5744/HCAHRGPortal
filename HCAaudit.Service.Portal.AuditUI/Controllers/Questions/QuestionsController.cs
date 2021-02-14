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

        [HttpPost]
        public ActionResult EditQuestionSequence(int newquestionId, int newsequenceno, int cquestionid, int csequenceno)
        {
            try
            {
                var objQuestion = _auditToolContext.QuestionMapping
                //.Where(a => a.QuestionMasterId == cquestionid ).FirstOrDefault();
                .Where(a => a.QuestionMappingId == cquestionid && a.IsActive == true).FirstOrDefault();

                if (objQuestion != null)
                {
                    objQuestion.SeqNumber = newsequenceno;
                    objQuestion.ModifiedBy = Environment.UserName;
                    objQuestion.ModifiedDate = DateTime.Now;
                    _auditToolContext.QuestionMapping.Update(objQuestion);
                    _auditToolContext.SaveChanges();
                }
                else
                {
                    return Json("Error Udating new sequence number");
                }

                objQuestion = _auditToolContext.QuestionMapping
                 .Where(a => a.QuestionMappingId == newquestionId && a.IsActive == true).FirstOrDefault();

                if (objQuestion != null)
                {
                    objQuestion.SeqNumber = csequenceno;
                    objQuestion.ModifiedBy = Environment.UserName;
                    objQuestion.ModifiedDate = DateTime.Now;
                    _auditToolContext.QuestionMapping.Update(objQuestion);
                    _auditToolContext.SaveChanges();
                }
                else
                {
                    return Json("Error Udating Current sequence number");
                }

                return Json("Success");

            }
            catch (Exception ex)
            {
                return Json("error");
                throw ex;
            }

        }

        [HttpPost]
        public ActionResult GetDesbyQuesText(string questionText)
        {
            var responce = "";

            responce = _auditToolContext.QuestionBank
                        .Where(a => a.QuestionName == questionText && a.IsActive == true)
                        .Select(a => a.QuestionDescription)
                        .FirstOrDefault();
            if (string.IsNullOrWhiteSpace(responce))
            {
                return Json(responce = "nd");
            }
            return Json(responce);
        }

        [HttpPost]
        public ActionResult GetQuestionSeqByid(int id, int subcatid, int actionQid)
        {
            var query = _auditToolContext.QuestionMapping
             .Join(
                 _auditToolContext.QuestionBank,
                 questionMaster => questionMaster.QuestionId,
                 questionBank => questionBank.QuestionId,
                 (questionMaster, questionBank) => new
                 {
                     QuestionId = questionMaster.QuestionId,
                     SequenceNo = questionMaster.SeqNumber,
                     QuestionText = questionBank.QuestionName,
                     QuestionDes = questionBank.QuestionDescription,
                     SubCatId = questionMaster.SubCatgId,
                     QuestionMasterId = questionMaster.QuestionMappingId,
                     IsActive = questionMaster.IsActive
                 })
                  .Select(x => new QuesBankMasterJoinMast
                  {
                      QuestionId = x.QuestionId,
                      SequenceNo = x.SequenceNo,
                      QuestionText = x.QuestionText,
                      QuestionDesc = x.QuestionDes,
                      SubCatID = x.SubCatId,
                      QuestionMappingId = x.QuestionMasterId,
                      isActive = x.IsActive
                  }
                 )
                  .Where(a => a.SubCatID == subcatid && a.isActive == true)
                 .OrderBy(a => a.SequenceNo)
                 .ToList();
            return Json(query);
        }

        QuestionBank GetSingleQuestionByid(string id)
        {
            var data = _auditToolContext.QuestionBank.Where(a => a.QuestionId == Convert.ToInt32(id) && a.IsActive == true).FirstOrDefault();
            return data;
        }
        [HttpPost]
        public ActionResult Edit(string id)
        {

            object responce = "";
            if (!string.IsNullOrEmpty(id))
            {
                string[] param = id.Split('$');
                if (param.Count() > 0 && param.Count() == 3)
                {
                    QuestionBank objtblQuestionBank = GetSingleQuestionByid(param[0]);

                    var questionlist = _auditToolContext.QuestionBank.Where(x => x.QuestionName.ToLower() == param[1].ToLower() && x.IsActive == true).ToList();
                    if (questionlist.Count > 0)
                    {
                        if (questionlist[0].QuestionId != objtblQuestionBank.QuestionId)
                        {
                            responce = "1";
                        }
                        else
                        {
                            objtblQuestionBank.QuestionName = param[1];
                            objtblQuestionBank.QuestionDescription = param[2];
                            objtblQuestionBank.ModifiedBy = Environment.UserName;
                            objtblQuestionBank.ModifiedDate = DateTime.Now;
                            _auditToolContext.QuestionBank.Update(objtblQuestionBank);
                            _auditToolContext.SaveChanges();
                            return Json(objtblQuestionBank);

                        }
                    }
                    else
                    {
                        objtblQuestionBank.QuestionName = param[1];
                        objtblQuestionBank.QuestionDescription = param[2];
                        objtblQuestionBank.ModifiedBy = Environment.UserName;
                        objtblQuestionBank.ModifiedDate = DateTime.Now;
                        _auditToolContext.QuestionBank.Update(objtblQuestionBank);
                        _auditToolContext.SaveChanges();
                        return Json(objtblQuestionBank);
                    }
                }
            }
            return Json(responce);
        }

        [HttpPost]
        public JsonResult BindSubCategory(string categoryID)
        {
            _logger.LogInformation($"Request for SubCategoryList with CategoryID: {categoryID}");

            var filteredSubCategoryList = _auditToolContext.SubCategories
                                         .Where(x => x.CatgID == Convert.ToInt32(categoryID) && x.IsActive == true)
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

                        var questionbankdata = _auditToolContext.QuestionBank.Where(x => x.QuestionName.Trim() == data[1].ToString().Trim() &&
                            x.IsActive == true).FirstOrDefault();
                        if (questionbankdata == null)//condition to restrict questions which is not available in questionBank table
                        {
                            QuestionBank objNewQuestion = new QuestionBank();
                            objNewQuestion.QuestionName = data[1].ToString().Trim();
                            objNewQuestion.QuestionDescription = data[2].ToString().Trim();
                            objNewQuestion.IsActive = true;
                            objNewQuestion.CreatedBy = Environment.UserName;
                            objNewQuestion.CreatedDate = DateTime.Now;
                            _auditToolContext.QuestionBank.Add(objNewQuestion);
                            _auditToolContext.SaveChanges();
                            questionbankdata = objNewQuestion;
                        }
                        QuestionMapping objtbQuestionMaster = new QuestionMapping();
                        objtbQuestionMaster.QuestionId = questionbankdata.QuestionId;
                        objtbQuestionMaster.SubCatgId = Convert.ToInt32(data[0]);
                        objtbQuestionMaster.CreatedBy = Environment.UserName;
                        objtbQuestionMaster.CreatedDate = DateTime.Now;
                        //objtbQuestionMaster.QuestionText = questionbankdata.QuestionName;
                        // bjtbQuestionMaster.QuestionScore = 10;// as Score column is not mandatory now Convert.ToInt32(data[2]);
                        objtbQuestionMaster.SeqNumber = Convert.ToInt32(data[3]);
                        objtbQuestionMaster.IsActive = true;
                        _logger.LogInformation($"Request for Adding Question to DB with SubCategoryID: {data[0]} and Question Text as: {data[1]}");

                        _auditToolContext.QuestionMapping.Add(objtbQuestionMaster);
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
                throw ex;
            }

            return Json("");
        }
        List<QuestionBank> GetDetails()
        {
            var data = _auditToolContext.QuestionBank.Where(a => a.IsActive == true).ToList();
            return data;
        }


        bool isQuestionNameExists(string inputQuestionName)
        {
            bool result = false;
            var data = (from cat in _auditToolContext.QuestionBank.Where(x => x.QuestionName.ToLower() == inputQuestionName.ToLower()
                        && x.IsActive == true)
                        select cat).FirstOrDefault();
            if (data != null) result = true;
            return result;
        }

        // [HttpGet]
        //public JsonResult BindQestions(string subCategoryID)
        //{
        //    _logger.LogInformation($"Request for BindQuestions with subCategoryID: {subCategoryID}");

        //    var data = _auditToolContext.QuestionMapping.Where(x => x.SubCatgId == Convert.ToInt32(subCategoryID) && x.IsActive == true)
        //                           .OrderBy(a => a.SeqNumber)
        //                           .Select(x => new { x.QuestionId, x.QuestionText })
        //                           .ToList();
        //    return Json(data);
        //}


        public IActionResult Index()
        {
            var categoryList = _auditToolContext.Categories
                               .Where(x => x.IsActive == true)
                               .ToList();
            _logger.LogInformation($"No of records: {categoryList.Count()}");
            categoryList.Insert(0, new Categorys { CatgID = 0, CatgDescription = "Select" });
            ViewBag.ListOfCategory = categoryList;
            return View();
        }

        [HttpGet]
        public IActionResult BindGrid(string subCategoryID)
        {

            var query = _auditToolContext.QuestionMapping.Where(x=>x.SubCatgId == Int32.Parse(subCategoryID) && x.IsActive == true)
             .Join(
                 _auditToolContext.QuestionBank,
                 questionMaster => questionMaster.QuestionId,
                 questionBank => questionBank.QuestionId,
                 (questionMaster, questionBank) => new
                 {
                     QuestionId = questionMaster.QuestionId,
                     SequenceNo = questionMaster.SeqNumber,
                     QuestionText = questionBank.QuestionName,
                     QuestionDes = questionBank.QuestionDescription,
                     SubCatId = questionMaster.SubCatgId,
                     QuestionMasterId = questionMaster.QuestionMappingId,
                     IsActive = questionMaster.IsActive
                 })
                  .Select(x => new QuesBankMasterJoinMast
                  {
                      QuestionId = x.QuestionId,
                      SequenceNo = x.SequenceNo,
                      QuestionText = x.QuestionText,
                      QuestionDesc = x.QuestionDes,
                      SubCatID = x.SubCatId,
                      QuestionMappingId = x.QuestionMasterId,
                      isActive = x.IsActive
                  }
                 )
                  .OrderBy(a => a.SequenceNo)
                 .ToList();
            return Json(query);

            //var query = _auditToolContext.QuestionMapping
            // .Join(
            //     _auditToolContext.QuestionBank,
            //     questionMapping => questionMapping.QuestionId,
            //     questionBank => questionBank.QuestionId,
            //     (questionMapping, questionBank) => new
            //     {
            //         QuestionId = questionMapping.QuestionId,
            //         SequenceNo = questionMapping.SeqNumber,
            //         QuestionText = questionBank.QuestionName,
            //         QuestionDes = questionBank.QuestionDescription,
            //         SubCatId = questionMapping.SubCatgId,
            //         QuestionMappingId = questionMapping.QuestionMappingId,
            //         IsActive = questionMapping.IsActive
            //     })
            //      .Select(x => new QuesBankMasterJoinMast
            //      {
            //          QuestionId = x.QuestionId,
            //          SequenceNo = x.SequenceNo,
            //          QuestionMappingId = x.QuestionMappingId,
            //          QuestionText = x.QuestionText,
            //          QuestionDesc = x.QuestionDes,
            //          SubCatID = x.SubCatId,
            //          //QuestionMappingId = x.QuestionMappingId,
            //          isActive = x.IsActive

            //      }
            //     )
            //      .Where(a => a.SubCatID == Int32.Parse(subCategoryID) && a.isActive == true)
            //     .OrderBy(a => a.SequenceNo)
            //     .ToList();



            //    )

            //            //.Where(x => x.SubCatgId == Convert.ToInt32(subCategoryID) && x.IsActive == true)
            //            //.OrderBy(y => y.SeqNumber)
            //            //.Select(a => new { a.QuestionMappingId, a.SeqNumber, a.QuestionText })
            //            //.ToList();
            //return Json(data);
        }

        [HttpGet]
        public IActionResult Details()
        {
            return View("details", GetDetails());
        }

        public JsonResult GetCommaSeperated()
        {
            return Json(GetDetails().Select(a => a.QuestionName));
        }

        public JsonResult GetIndexCommaSeperated(string subCategoryID)
        {
            var dataMaster = _auditToolContext.QuestionMapping
                             .Where(a => a.SubCatgId == Convert.ToInt32(subCategoryID) && a.IsActive == true)
                             .ToList();
            var dataBank = GetDetails();
            if (dataMaster.Count() > 0)
            {
                foreach (var item in dataMaster)
                {
                    QuestionBank objtblQuestionBank = new QuestionBank();
                    objtblQuestionBank = dataBank.Where(a => a.QuestionId == item.QuestionId).SingleOrDefault();
                    if (objtblQuestionBank != null)
                    {
                        dataBank.Remove(objtblQuestionBank);
                    }
                }
            }

            return Json(dataBank.Select(a => a.QuestionName));
        }

        [HttpPost]
        public IActionResult Details(QuestionBank objCategoryMast)
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

        public IActionResult DeleteQuestionBank(int id)
        {
            QuestionBank objtblQuestionBank = _auditToolContext.QuestionBank
                                         .Where(a => a.QuestionId == id && a.IsActive == true)
                                         .FirstOrDefault();
            objtblQuestionBank.ModifiedDate = DateTime.Now;
            objtblQuestionBank.ModifiedBy = Environment.UserName;
            objtblQuestionBank.IsActive = false;
            _auditToolContext.QuestionBank.Update(objtblQuestionBank); _auditToolContext.SaveChanges();
            return View("Details", GetDetails());
        }

        public IActionResult DeleteQuestionMaster(int id, int SubCatId)
        {
            var data = _auditToolContext.QuestionMapping
                        .Where(a => a.QuestionMappingId == id && a.IsActive == true)
                        .FirstOrDefault();
            if (data != null)
            {
                data.IsActive = false;
                var seq = data.SeqNumber;
                _auditToolContext.QuestionMapping.Update(data);
                var questionRemaingQues = _auditToolContext.QuestionMapping.Where
                        (b => b.SubCatgId == data.SubCatgId &&
                        b.SeqNumber > data.SeqNumber &&
                        b.IsActive == true).ToList();
                foreach (var item in questionRemaingQues)
                {
                    item.SeqNumber = seq;
                    item.ModifiedBy = Environment.UserName;
                    item.ModifiedDate = DateTime.Now;
                    seq++;
                    _auditToolContext.QuestionMapping.Update(item);
                }

                _auditToolContext.SaveChanges();
            }

            return View("index", GetDetails());
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            var data = (from Q in _auditToolContext.QuestionBank.Where(a => a.IsActive == true) select Q).ToList();
            var filterData = data.Find(a => a.QuestionId == id);
            return View("Edit", filterData);
        }

        [HttpPost]
        public ActionResult Insert(string questionname, string questiondesc)
        {
            object responce = "";

            if (isQuestionNameExists(questionname)) { responce = "1"; }

            if (string.IsNullOrEmpty(responce.ToString()) && !string.IsNullOrEmpty(questionname) && !string.IsNullOrEmpty(questiondesc))
            {
                QuestionBank objtblQuestionBank = new QuestionBank();
                objtblQuestionBank.QuestionName = questionname;
                objtblQuestionBank.IsActive = true;
                objtblQuestionBank.CreatedBy = Environment.UserName;
                objtblQuestionBank.CreatedDate = DateTime.Now; ;
                objtblQuestionBank.QuestionDescription = questiondesc;
                _auditToolContext.QuestionBank.Add(objtblQuestionBank);
                _auditToolContext.SaveChanges();
                return RedirectToAction("Details");
            }

            return Json(responce);
        }

        [HttpPost]
        public ActionResult HasDeleteAccessforQB(int id)
        {
            object response;
            //var obj = _auditToolContext.QuestionMasters
            //                          .Where(a => a.QuestionId == id
            //                           && a.IsActive == true)
            //                          .FirstOrDefault();

            var obj = _auditToolContext.QuestionMapping
                                      .Where(a => a.QuestionId == id
                                       && a.IsActive == true)
                                      .FirstOrDefault();

            response = obj == null ? "NoRecords" : "HasRecords";
            return Json(response);
        }
    }
}
