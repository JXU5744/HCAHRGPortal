using HCAaudit.Service.Portal.AuditUI.Models;
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
    public class QuestionsController : Controller
    {
        private readonly ILogger<QuestionsController> _logger;
        private readonly IConfiguration config;
        private readonly IAuthService _authService;
        private readonly AuditToolContext _auditToolContext;
        private readonly bool isAdmin;
        private readonly IErrorLog _log;
        public QuestionsController(ILogger<QuestionsController> logger, IErrorLog log, IConfiguration configuration, AuditToolContext audittoolc, IAuthService authService)
        {
            _auditToolContext = audittoolc;
            _logger = logger;
            config = configuration;
            _authService = authService;
            isAdmin = _authService.CheckAdminUserGroup().Result;
            _log = log;
        }

        [HttpPost]
        public ActionResult GetQuestionByid(string id)
        {
            try
            {
                if (isAdmin)
                {
                    object response = "";
                    if (string.IsNullOrEmpty(id))
                    {
                        return Json(response);
                    }
                    return Json(GetSingleQuestionByid(id));
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in GetQuestionByid method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "QuestionsController_GetQuestionByid", ErrorDiscription = ex.Message });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult EditQuestionSequence(int newquestionId, int newsequenceno, int cquestionid, int csequenceno)
        {
            if (isAdmin)
            {
                try
                {
                    var objQuestion = _auditToolContext.QuestionMapping
                    .Where(a => a.QuestionMappingId == cquestionid && a.IsActive == true).FirstOrDefault();

                    if (objQuestion != null)
                    {
                        objQuestion.SeqNumber = newsequenceno;
                        objQuestion.ModifiedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
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
                        objQuestion.ModifiedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
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
                    _logger.LogInformation($"Exception in EditQuestionSequence method");
                    _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "QuestionsController_EditQuestionSequence", ErrorDiscription = ex.Message });
                    return Json("Error");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult GetDesbyQuesText(string questionText)
        {
            try
            {
                if (isAdmin)
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
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in GetDesbyQuesText method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "QuestionsController_GetDesbyQuesText", ErrorDiscription = ex.Message });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult GetQuestionSeqByid(int id, int subcatid, int actionQid)
        {
            try
            {
                if (isAdmin)
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
                              QuestionMasterId = x.QuestionMasterId,
                              QuestionMappingId = x.QuestionMasterId,
                              IsActive = (bool)x.IsActive
                          }
                         )
                          .Where(a => a.SubCatID == subcatid && a.IsActive)
                         .OrderBy(a => a.SequenceNo)
                         .ToList();
                    return Json(query);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in GetQuestionSeqByid method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "QuestionsController_GetQuestionSeqByid", ErrorDiscription = ex.Message });
            }
            return RedirectToAction("Index", "Home");
        }

        private QuestionBank GetSingleQuestionByid(string id)
        {
            QuestionBank data = null;

            try
            {
                data = _auditToolContext.QuestionBank.Where(a => a.QuestionId == Convert.ToInt32(id) && a.IsActive == true).FirstOrDefault();

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in GetSingleQuestionByid method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "QuestionsController_GetSingleQuestionByid", ErrorDiscription = ex.Message });
            }
            return data;
        }

        [HttpPost]
        public ActionResult Edit(string id)
        {
            try
            {
                if (isAdmin)
                {
                    object responce = "";
                    if (!string.IsNullOrEmpty(id))
                    {
                        string[] param = id.Split('$');
                        if (param.Any() && param.Count() == 3)
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
                                    objtblQuestionBank.ModifiedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
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
                                objtblQuestionBank.ModifiedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                                objtblQuestionBank.ModifiedDate = DateTime.Now;
                                _auditToolContext.QuestionBank.Update(objtblQuestionBank);
                                _auditToolContext.SaveChanges();
                                return Json(objtblQuestionBank);
                            }
                        }
                    }
                    return Json(responce);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in Edit method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "QuestionsController_Edit", ErrorDiscription = ex.Message });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public JsonResult BindSubCategory(string categoryID)
        {
            _logger.LogInformation($"Request for SubCategoryList with CategoryID: {categoryID}");
            try
            {
                if (isAdmin)
                {
                    var filteredSubCategoryList = _auditToolContext.SubCategory
                                                 .Where(x => x.CatgId == Convert.ToInt32(categoryID) && x.IsActive == true)
                                                 .Select(x => new { x.SubCatgId, x.SubCatgDescription }).ToList();
                    _logger.LogInformation($"No of SubCategoryListrecords: {filteredSubCategoryList.Count}");
                    return Json(filteredSubCategoryList);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in BindSubCategory method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "QuestionsController_BindSubCategory", ErrorDiscription = ex.Message });
            }
            return Json(new { Success = "False", responseText = "Authorization Error" });
        }

        [HttpPost]
        public JsonResult SaveQuestionForMaster(string subcatgid)
        {
            if (isAdmin)
            {
                try
                {
                    if (subcatgid.Contains('^'))
                    {
                        string[] data = subcatgid.Split('^');
                        if (data.Any() && data[0] != null && data[1] != null && data[2] != null && data[3] != null)
                        {
                            var questionbankdata = _auditToolContext.QuestionBank.Where(x => x.QuestionName.Trim() == data[1].ToString().Trim() &&
                                x.IsActive == true).FirstOrDefault();
                            if (questionbankdata == null)//condition to restrict questions which is not available in questionBank table
                            {
                                QuestionBank objNewQuestion = new QuestionBank();
                                objNewQuestion.QuestionName = data[1].ToString().Trim();
                                objNewQuestion.QuestionDescription = data[2].ToString().Trim();
                                objNewQuestion.IsActive = true;
                                objNewQuestion.CreatedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                                objNewQuestion.CreatedDate = DateTime.Now;
                                objNewQuestion.ModifiedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                                objNewQuestion.ModifiedDate = DateTime.Now;
                                _auditToolContext.QuestionBank.Add(objNewQuestion);
                                _auditToolContext.SaveChanges();
                                questionbankdata = objNewQuestion;
                            }
                            QuestionMapping objtbQuestionMaster = new QuestionMapping();
                            objtbQuestionMaster.QuestionId = questionbankdata.QuestionId;
                            objtbQuestionMaster.SubCatgId = Convert.ToInt32(data[0]);
                            objtbQuestionMaster.SeqNumber = Convert.ToInt32(data[3]);
                            objtbQuestionMaster.CreatedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                            objtbQuestionMaster.CreatedDate = DateTime.Now;
                            objtbQuestionMaster.ModifiedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                            objtbQuestionMaster.ModifiedDate = DateTime.Now;
                            objtbQuestionMaster.IsActive = true;
                            _logger.LogInformation($"Request for Adding Question to DB with SubCategoryID: {data[0]} and Question Text as: {data[1]}");

                            _auditToolContext.QuestionMapping.Add(objtbQuestionMaster);
                            int insertstatus = _auditToolContext.SaveChanges();
                            if (insertstatus > 0)
                            {
                                _logger.LogInformation($"Inserted Sucessfully.");
                            }
                            else
                            {
                                _logger.LogInformation($"Inserted failed.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Exception in SaveQuestionForMaster method");
                    _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "QuestionsController_SaveQuestionForMaster", ErrorDiscription = ex.Message });
                }

                return Json("");
            }
            else
            {
                return Json(new { Success = "False", responseText = "Authorization Error" });
            }
        }
        List<QuestionBank> GetDetails()
        {
            List<QuestionBank> data = new List<QuestionBank>();
            try
            {
                data = _auditToolContext.QuestionBank.Where(a => a.IsActive == true).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in GetDetails method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "QuestionsController_GetDetails", ErrorDiscription = ex.Message });
            }
            return data;
        }

        private bool IsQuestionNameExists(string inputQuestionName)
        {
            bool result = false;
            try
            {
                var data = (from cat in _auditToolContext.QuestionBank.Where(x => x.QuestionName.ToLower() == inputQuestionName.ToLower()
                        && x.IsActive == true)
                            select cat).FirstOrDefault();
                if (data != null) result = true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in IsQuestionNameExists method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "QuestionsController_IsQuestionNameExists", ErrorDiscription = ex.Message });
            }
            return result;
        }

        public IActionResult Index()
        {
            try
            {
                if (isAdmin)
                {
                    var categoryList = _auditToolContext.Category
                                       .Where(x => x.IsActive == true)
                                       .ToList();
                    _logger.LogInformation($"No of records: {categoryList.Count}");
                    categoryList.Insert(0, new Category { CatgId = 0, CatgDescription = "Select" });
                    ViewBag.ListOfCategory = categoryList;
                    return View();
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in Index method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "QuestionsController_Index", ErrorDiscription = ex.Message });
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult BindGrid(string subCategoryID)
        {
            try
            {
                if (isAdmin)
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
                          IsActive = (bool)x.IsActive
                      }
                     )
                      .Where(a => a.SubCatID == Int32.Parse(subCategoryID) && a.IsActive)
                     .OrderBy(a => a.SequenceNo)
                     .ToList();
                    return Json(query);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in BindGrid method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "QuestionsController_BindGrid", ErrorDiscription = ex.Message });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Details()
        {
            if (isAdmin)
            {
                return View("Details", GetDetails());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public JsonResult GetCommaSeperated()
        {
            try
            {
                if (isAdmin)
                {
                    return Json(GetDetails().Select(a => a.QuestionName));
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in GetCommaSeperated method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "QuestionsController_GetCommaSeperated", ErrorDiscription = ex.Message });
            }
            return Json(new { Success = "False", responseText = "Authorization Error" });
        }

        public JsonResult GetIndexCommaSeperated(string subCategoryID)
        {
            try
            {
                if (isAdmin)
                {
                    var dataMaster = _auditToolContext.QuestionMapping
                                     .Where(a => a.SubCatgId == Convert.ToInt32(subCategoryID) && a.IsActive == true)
                                     .ToList();
                    var dataBank = GetDetails();
                    if (dataMaster.Any())
                    {
                        foreach (var item in dataMaster)
                        {
                            var objtblQuestionBank = dataBank.Where(a => a.QuestionId == item.QuestionId).FirstOrDefault();
                            if (objtblQuestionBank != null)
                            {
                                dataBank.Remove(objtblQuestionBank);
                            }
                        }
                    }
                    return Json(dataBank.Select(a => a.QuestionName));
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in GetIndexCommaSeperated method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "QuestionsController_GetIndexCommaSeperated", ErrorDiscription = ex.Message });
            }
            return Json(new { Success = "False", responseText = "Authorization Error" });
        }

        [HttpPost]
        public IActionResult Details(QuestionBank objCategoryMast)
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
                catch (Exception ex)
                {
                    _logger.LogInformation($"Exception in Details method");
                    _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "QuestionsController_Details", ErrorDiscription = ex.Message });
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult DeleteQuestionBank(int id)
        {
            try
            {
                if (isAdmin)
                {
                    QuestionBank objtblQuestionBank = _auditToolContext.QuestionBank
                                                 .Where(a => a.QuestionId == id && a.IsActive == true)
                                                 .FirstOrDefault();
                    objtblQuestionBank.IsActive = false;
                    objtblQuestionBank.ModifiedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                    objtblQuestionBank.ModifiedDate = DateTime.Now;
                    _auditToolContext.QuestionBank.Update(objtblQuestionBank);
                    _auditToolContext.SaveChanges();
                    return View("Details", GetDetails());
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in DeleteQuestionBank method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "QuestionsController_DeleteQuestionBank", ErrorDiscription = ex.Message });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult DeleteQuestionMaster(int id)
        {
            try
            {
                if (isAdmin)
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
                            item.ModifiedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                            item.ModifiedDate = DateTime.Now;
                            seq++;
                            _auditToolContext.QuestionMapping.Update(item);
                        }

                        _auditToolContext.SaveChanges();
                    }
                    return Json("Success");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in DeleteQuestionMaster method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "QuestionsController_DeleteQuestionMaster", ErrorDiscription = ex.Message });
            }
            return Json(new { Success = "False", responseText = "Authorization Error" });
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            try
            {
                if (isAdmin)
                {
                    var data = (from Q in _auditToolContext.QuestionBank.Where(a => a.IsActive == true && a.QuestionId == id) select Q).ToList();
                    return View("Edit", data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in Edit method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "QuestionsController_Edit", ErrorDiscription = ex.Message });
            }
            return View("Index");
        }

        [HttpPost]
        public ActionResult Insert(string questionname, string questiondesc)
        {
            try
            {
                if (isAdmin)
                {
                    object responce = "";

                    if (IsQuestionNameExists(questionname))
                    {
                        responce = "1";
                    }

                    if (string.IsNullOrEmpty(responce.ToString()) && !string.IsNullOrEmpty(questionname) && !string.IsNullOrEmpty(questiondesc))
                    {
                        QuestionBank objtblQuestionBank = new QuestionBank();
                        objtblQuestionBank.QuestionName = questionname;
                        objtblQuestionBank.IsActive = true;
                        objtblQuestionBank.QuestionDescription = questiondesc;
                        objtblQuestionBank.CreatedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                        objtblQuestionBank.CreatedDate = DateTime.Now;
                        objtblQuestionBank.ModifiedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                        objtblQuestionBank.ModifiedDate = DateTime.Now;
                        _auditToolContext.QuestionBank.Add(objtblQuestionBank);
                        _auditToolContext.SaveChanges();
                        return RedirectToAction("Details");
                    }
                    return Json(responce);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in Insert method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "QuestionsController_Insert", ErrorDiscription = ex.Message });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult HasDeleteAccessforQB(int id)
        {
            try
            {
                if (isAdmin)
                {
                    object response;
                    var obj = _auditToolContext.QuestionMapping
                                              .Where(a => a.QuestionId == id
                                               && a.IsActive == true)
                                              .FirstOrDefault();
                    response = obj == null ? "NoRecords" : "HasRecords";
                    return Json(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in HasDeleteAccessforQB method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "QuestionsController_HasDeleteAccessforQB", ErrorDiscription = ex.Message });
            }
            return RedirectToAction("Index", "Home");
        }
    }
}